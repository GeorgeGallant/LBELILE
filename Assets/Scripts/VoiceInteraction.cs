/**
 * \file    VoiceInteraction.cs
 * \author  Stephen Graham
 * \author  MacKenzie Ackles
 * \date    2021-02-13
 * \brief   Voice Interaction handler
 * 
 * \TODO    fix click vs hold on mic button
**/

using UnityEngine;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Web;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.XR.Interaction.Toolkit;

public class VoiceInteraction : MonoBehaviour
{
// private
    private static object threadLocker = new object();  // for thread locking
    private float elapsedTime = 0.0f;         // for timing 
    private bool isMicTriggerSet = false;    // has the mic trigger been set?
    private static bool isRecording = false;        // have we started recording?
    private static bool isRecognized = false;
    private ArrayList selectedIntent = new ArrayList();  // the answer!
    private string KeyFilePath; //the file path to the keys and app
    
    // Replace with your own subscription key and service region (e.g., "westus").
    private string subscription_key; // speech to text
    private string region_name;   // speech to text

    // YOUR-APP-ID: The App ID GUID found on the www.luis.ai Application Settings page.
    private string appId;  // LUIS

    // YOUR-PREDICTION-KEY: 32 character key.
    private string predictionKey; // LUIS

    // YOUR-PREDICTION-ENDPOINT: Example is "https://westus.api.cognitive.microsoft.com/"
    private static string predictionEndpoint = "https://p360v.cognitiveservices.azure.com/"; // LUIS

    // public
    private string FileName = "HiddenText/AppKey.txt"; // Stephen added this so that it will be a default. 2022/03/02
    // VR Related

    public event Action OnRecordMic;

    // Interface for scene direction
    public string GoodSceneIntent;          // code for what the user should say
    public string GoodSceneName;            // where to go when utterance is correct
    public string BadSceneName;             // where to go when utterance is not correct

    // How are we listening?
    public Boolean useMic;              // trigger on mic if true 
    public int StartAfterSeconds;       // don't allow the trigger until after this time
    public int TimeoutAfterSeconds;     // how much time after the listening starts

    public String lastUtterance = "";

    public ButtonHandler buttonPress;

    private bool buttonPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        pullKeyAndId();//pull from text method
        // if this is attached to a button, this would be a good place to add a state listener
        if (useMic && (StartAfterSeconds == 0))
        {
            //RecordMic.AddOnStateDownListener(ButtonClick, handType); // this is a click not a hold!!!
            isMicTriggerSet = true;
            BindXRControllerEvents();
        }
    }
    public void OnDestroy()
    {
        UnbindXRControllerEvents();
    }

    public void BindXRControllerEvents()
    {
        buttonPress.OnButtonDown += ButtonDown;

    }

    public void UnbindXRControllerEvents()
    {
        if (buttonPress)
        {
            buttonPress.OnButtonDown -= ButtonDown;
        }
    }

    public void ButtonDown(XRController controller, InputHelpers.Button button)
    {
        if (!buttonPressed) { 
        ButtonClick();
        buttonPressed = true;
        }
    }

    // Update is called once per frame
    async void Update()
    {
        elapsedTime += Time.deltaTime;
        if (!isMicTriggerSet)
        {
            // start the mic if it is a delayed start
            if (useMic && (StartAfterSeconds > 0) && (elapsedTime >= StartAfterSeconds))
            {
                //RecordMic.AddOnStateDownListener(ButtonClick, handType); // this is a click not a hold?!!
                isMicTriggerSet = true;
                BindXRControllerEvents();
            }
            // start listening at the right time if we are not using the mic/radio
            else if (!useMic && (elapsedTime >= StartAfterSeconds)) {
                isMicTriggerSet = true;
                isRecording = true;
//                Debug.Log($"About to start TimedListener({reps})");
                await TimedListener(true);
//                Debug.Log($"Returned from starting TimedListener({reps})");
                if (!isRecognized)
                {
                    isMicTriggerSet = false;
                }
            }
        }

        if (isRecording && (TimeoutAfterSeconds > 0) && (elapsedTime >= (StartAfterSeconds + TimeoutAfterSeconds)))
        {
            // that's all the time we have
            // stop the recordinng
            // check the utterance
            // take an action
            Debug.Log("Timeout Occurred.");
            selectedIntent.Add("Timeout!");
            selectedIntent.Add(0.000);
            isRecognized = true;
        }

        if (isRecognized)
        {
            // all the listening is done and we have some answer
            try
            {
                //checks the answer against the message.
                if (selectedIntent[0].ToString() == GoodSceneIntent && Convert.ToDouble(selectedIntent[1]) >= 0.167)
                {
                    SceneManager.LoadScene(GoodSceneName);
                }
                //in case it fails.
                else
                {
                    SceneManager.LoadScene(BadSceneName);
                }

                if (useMic)
                {
                    // if we have attached this to the Button Click, we need to specifically destroy it
                    Destroy(this.gameObject);
                }
                else
                {
                    // otherwise, trying to destroy breaks everything!
                }


                //catches out of range exception
            }
            //catches the argument for feedback answers not being fullfilled yet.
            catch (ArgumentOutOfRangeException)
            { }

        }
    }


    public async Task TimedListener(Boolean continualListening = false)
    {
        var config = SpeechConfig.FromSubscription(subscription_key, region_name);
        String utterance = null;
        using (var recognizer = new SpeechRecognizer(config))
        {
            var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

            // Checks result.
            //lock (threadLocker) {
            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                utterance = result.Text;
            }
            else if (result.Reason == ResultReason.NoMatch)
            {
                if (!continualListening)
                {
                    utterance = "NOMATCH: Speech could not be recognized.";
                } else
                {
                    // don't utter a thing... just return!
                    return;
                }
            }
            //if you stop it it stops. //if there are errors
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = CancellationDetails.FromResult(result);
                utterance = $"CANCELED: Reason={cancellation.Reason} ErrorDetails={cancellation.ErrorDetails}";
            }
            else
            {
                utterance = "ALARM! this should never happen!";
            }
//            isRecording = false;
            //}
        }

        if (utterance != null)
        {
            Debug.Log("Finished listening and heard: " + utterance);
            Task<string> strPrediction = GetIntentFromUtterance(predictionKey, predictionEndpoint, appId, utterance);
            var predictionResult = JObject.Parse(strPrediction.Result);
            var topIntent = predictionResult["prediction"]["topIntent"];
            var score = predictionResult["prediction"]["intents"][topIntent.ToString()]["score"];

            // provides topintent and score.
            selectedIntent.Add(topIntent.ToString());
            selectedIntent.Add(score);
            isRecognized = true;
            isRecording = false;
            Debug.Log("Selected Intent: " + selectedIntent[0] + "(" + selectedIntent[1] + ")\n"+
                "Looking for: "+GoodSceneIntent + " to go to " + GoodSceneName);
        }
    }

    public async void ButtonClick()
    {
        await TimedListener();
    }

    // Prediction Task
    static async Task<string> GetIntentFromUtterance(string predictionKey,
                                          string predictionEndpoint, 
                                          string appId,
                                          string utterance)
    {
        var client = new HttpClient();
        var queryString = HttpUtility.ParseQueryString(string.Empty);

        // The request header contains your subscription key
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", predictionKey);

        // query string preparation
        queryString["query"] = utterance;               // utterance
        queryString["verbose"] = "true";                // verbose, default true
        queryString["show-all-intents"] = "false";      // show all, default "false"
        queryString["staging"] = "true";                // staging, default true?
        queryString["timezoneOffset"] = "0";            // timezoneOffset, 0? //TODO
        queryString["log"] = "true";                    // utterance logging at Azure //added 2022-03-04

        var predictionEndpointUri = String.Format("{0}luis/prediction/v3.0/apps/{1}/slots/staging/predict?{2}",
                                                   predictionEndpoint,
                                                   appId,
                                                   queryString);

        // connection
        var response = await client.GetAsync(predictionEndpointUri);

        // response
        var strResponseContent = await response.Content.ReadAsStringAsync();

        // return the JSON
        return strResponseContent.ToString();
    }
    private void pullKeyAndId()
    {
        KeyFilePath = Application.persistentDataPath + "/" + FileName;
        string [] textfile = File.ReadAllLines(KeyFilePath);
        subscription_key = textfile[1];
        region_name = textfile[3];
        appId = textfile[5];
        predictionKey = textfile[7];
    }
}
