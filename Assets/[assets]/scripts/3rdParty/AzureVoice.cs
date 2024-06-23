using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Intent;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

using Azure.Core;
using Azure.AI.Language.Conversations;
using Azure;
using Microsoft.Extensions.Azure;
using Microsoft.CognitiveServices.Speech.Audio;
using System.Diagnostics;

namespace ThirdParty
{
    public class AzureVoice
    {
        public static Dictionary<string, string> intentDestinations = new Dictionary<string, string>();
        static bool busy = false;
        public static UnityEngine.Events.UnityEvent<(string topIntent, string initiator, string scene)> intentEvent = new UnityEngine.Events.UnityEvent<(string topIntent, string initiator, string scene)>();

        static bool flip = false;

        public static async Task Listener(ValueWrapper<bool> continueListening, string initiator, string relevantScene)
        {
            bool useOld = false;
            if (ScenarioManager.instance)
                switch (ScenarioManager.instance.modelMode)
                {
                    case ModelMode.Old: { useOld = true; break; }
                    case ModelMode.FlipFlop: { useOld = flip; flip = !flip; break; }
                    case ModelMode.Random:
                        { useOld = UnityEngine.Random.value > 0.5f; break; }
                    default: break;
                }

            UnityEngine.Debug.Log(useOld ? "Using old" : "Using new");

            if (!useOld)
            {
                await NewModel(continueListening, initiator, relevantScene);
            }
            else await OldModel(continueListening, initiator, relevantScene);


        }

        static async Task NewModel(ValueWrapper<bool> continueListening, string initiator, string relevantScene)
        {
            // if (busy) return;
            busy = true;
            var config = SpeechConfig.FromSubscription(ConfigManager.SUBSCRIPTION_KEY, ConfigManager.REGION_NAME);
            long timeStart = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            var predictionEndpointUri = "https://p360v2.cognitiveservices.azure.com/";

            /*** 
             * This needs to load the language model on a per-scenario basis
             * 
             * ***/
            var cluModel = new ConversationalLanguageUnderstandingModel(
              ConfigManager.LANGUAGE_RESOURCE_KEY,
              predictionEndpointUri,
              ScenarioManager.instance.cluProjectName,
              ScenarioManager.instance.cluDeploymentName);
            // "P360V_1",
            // "p3vDev1"); ;
            //  "P360V_fishgame",
            //  "P360V_fishgame");

            var collection = new LanguageUnderstandingModelCollection();
            collection.Add(cluModel);

            var recognizer = new IntentRecognizer(config);
            recognizer.ApplyLanguageModels(collection);
            recognizer.AddAllIntents(cluModel);

            recognizer.Recognized += resultRecieved;
            recognizer.Canceled += cancelled;
            // UnityEngine.Debug.Log("Azure listening and busy");
            await recognizer.StartContinuousRecognitionAsync();

            while (continueListening != null && continueListening.Value)
            {
                await Task.Delay(1);
            }

            // UnityEngine.Debug.Log("no longer listening");

            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
            // UnityEngine.Debug.Log("Azure no longer busy");
            busy = false;

            void resultRecieved(object sender, IntentRecognitionEventArgs e)
            {
                IntentRecognitionResult result = e.Result;
                string utterance = result.Text;
                UnityEngine.Debug.Log($"{utterance}, {result.Reason}");
                string intent = "No intent";
                object IntentResult = null;
                IntentResult = e.Result.Properties.GetProperty(PropertyId.LanguageUnderstandingServiceResponse_JsonResult);
                if (result.Reason == ResultReason.RecognizedIntent)
                {
                    /* look at pulling this key from the Result for the file-saved log
					* e.Result.LanguageUnderstandingServiceResponse_JsonResult
					*/
                    UnityEngine.Debug.Log($"Speech: {utterance}, Intent: {e.Result.IntentId}, Json: {IntentResult}");
                    intent = e.Result.IntentId;
                    // await GetIntentFromUtterance(utterance, initiator);}
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    intent = "No speech";
                }
                IntentResultStruct conversationResult = JsonConvert.DeserializeObject<IntentResultStruct>(IntentResult.ToString());
                string destination = "null";
                intentDestinations.TryGetValue(intent, out destination);
                long timeEnd = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                if (conversationResult.result.prediction.intents.FindIndex(x => x.category == "None" && x.confidenceScore > 0.6) != -1)
                {
                    UnityEngine.Debug.Log($"Top intent was {intent} but None had a score over 60%");
                    intent = "None";
                }

                UnityMainThread.AddJob(() =>
                {
                    /* look at pulling this key from the Result for the file-saved log
                    * e.Result.LanguageUnderstandingServiceResponse_JsonResult
                    */
                    intentEvent.Invoke((intent, initiator, relevantScene));
                    IntentRecorder.RecordIntent((utterance, intent, initiator, destination, "New", timeStart, timeEnd, IntentResult != null ? IntentResult.ToString() : ""));
                });
            }
            void cancelled(object sender, IntentRecognitionCanceledEventArgs e)
            {
                IntentRecognitionResult result = e.Result;
                string utterance = result.Text;

                UnityEngine.Debug.Log($"Cancelled: {utterance}, {result.Reason}, {e.ErrorDetails}");
                finish();
            }
            void finish()
            {
                recognizer.Recognized -= resultRecieved;
                recognizer.Canceled -= cancelled;
                busy = false;
            }
        }

        static async Task OldModel(ValueWrapper<bool> continueListening, string initiator, string relevantScene)
        {
            var config = SpeechConfig.FromSubscription(ConfigManager.SUBSCRIPTION_KEY, ConfigManager.REGION_NAME);
            long timeStart = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            var recognizer = new SpeechRecognizer(config);

            recognizer.Recognized += resultRecieved;
            recognizer.Canceled += cancelled;

            await recognizer.StartContinuousRecognitionAsync();

            while (continueListening != null && continueListening.Value)
            {
                await Task.Delay(1);
            }

            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);

            async void resultRecieved(object sender, SpeechRecognitionEventArgs e)
            {
                var result = e.Result;
                string utterance = result.Text;
                UnityEngine.Debug.Log($"{utterance}, {result.Reason}");

                var predictionEndpointUri = "https://p360v.cognitiveservices.azure.com/";

                string strPrediction = await GetIntentFromUtterance(ConfigManager.PREDICTION_KEY, predictionEndpointUri, "70c9a26e-877c-4d94-a0a0-ff5197d4a2e9", utterance);

                var predictionResult = JObject.Parse(strPrediction);
                var topIntent = predictionResult["prediction"]["topIntent"];
                var score = predictionResult["prediction"]["intents"][topIntent.ToString()]["score"];

                UnityEngine.Debug.Log($"{topIntent} was the intent");

                string destination = "null";
                intentDestinations.TryGetValue(topIntent.ToString(), out destination);
                long timeEnd = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                UnityMainThread.AddJob(() =>
                {
                    /* look at pulling this key from the Result for the file-saved log
					* e.Result.LanguageUnderstandingServiceResponse_JsonResult
					*/
                    intentEvent.Invoke((topIntent.ToString(), initiator, relevantScene));
                    IntentRecorder.RecordIntent((utterance, topIntent.ToString(), initiator, destination, "Old", timeStart, timeEnd, predictionResult.ToString()));
                });
            }
            void cancelled(object sender, SpeechRecognitionCanceledEventArgs e)
            {
                var result = e.Result;
                string utterance = result.Text;

                UnityEngine.Debug.Log($"Cancelled: {utterance}, {result.Reason}, {e.ErrorDetails}");
                finish();
            }
            void finish()
            {
                recognizer.Recognized -= resultRecieved;
                recognizer.Canceled -= cancelled;
                busy = false;
            }
            async Task<string> GetIntentFromUtterance(string predictionKey,
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
        }


    }
}
public struct Intent
{
    public string category;
    public double confidenceScore;
}

public struct Prediction
{
    public string topIntent;
    public string projectKind;
    public List<Intent> intents;
}

public struct Result
{
    public string query;
    public Prediction prediction;
}

public struct IntentResultStruct
{
    public string kind;
    public Result result;
}
