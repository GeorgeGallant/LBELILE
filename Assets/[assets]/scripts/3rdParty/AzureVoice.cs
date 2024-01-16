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
using UnityEditor.PackageManager;

namespace ThirdParty
{
    public class AzureVoice
    {
        static bool busy = false;
        public static UnityEngine.Events.UnityEvent<(Dictionary<string, Intent> intents, string topIntent, string initiator)> intentEvent = new UnityEngine.Events.UnityEvent<(Dictionary<string, Intent> intents, string topIntent, string initiator)>();
        public static async Task Listener(ValueWrapper<bool> continueListening, string initiator, bool passive = false)
        {
            if (busy) return;
            busy = true;
            var config = SpeechConfig.FromSubscription(ConfigManager.SUBSCRIPTION_KEY, ConfigManager.REGION_NAME);


            using var recognizer = new SpeechRecognizer(config);
            recognizer.Recognized += resultRecieved;
            recognizer.Canceled += cancelled;
            await recognizer.StartContinuousRecognitionAsync();

            while (continueListening != null && continueListening.Value) await Task.Delay(100);

            UnityEngine.Debug.Log("no longer listening");

            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);

            async void resultRecieved(object sender, SpeechRecognitionEventArgs e)
            {
                SpeechRecognitionResult result = e.Result;
                string utterance = result.Text;
                UnityEngine.Debug.Log($"{utterance}, {result.Reason}");
                if (result.Reason == ResultReason.RecognizedSpeech)
                    await GetIntentFromUtterance(utterance, initiator);
                finish();
            }
            void cancelled(object sender, SpeechRecognitionCanceledEventArgs e)
            {
                SpeechRecognitionResult result = e.Result;
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
        public static async Task ListenUntil(ValueWrapper<bool> continueListening)
        {
            if (busy) return;
            busy = true;
            var config = SpeechConfig.FromSubscription(ConfigManager.SUBSCRIPTION_KEY, ConfigManager.REGION_NAME);

            using var recognizer = new SpeechRecognizer(config);
            recognizer.Recognized += resultRecieved;
            recognizer.Canceled += cancelled;

            await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

            async void resultRecieved(object sender, SpeechRecognitionEventArgs e)
            {
                if (!continueListening.Value)
                {
                    finish();
                    return;
                }
                SpeechRecognitionResult result = e.Result;
                string utterance = result.Text;
                UnityEngine.Debug.Log($"{utterance}, {result.Reason}");
                if (result.Reason == ResultReason.RecognizedSpeech)
                    await GetIntentFromUtterance(utterance, "once");
                else if (result.Reason == ResultReason.NoMatch && continueListening.Value)
                {
                    UnityEngine.Debug.Log("No utterance, trying again.");
                    busy = false;
                    await ListenUntil(continueListening);
                    return;
                }
                finish();
            }
            void cancelled(object sender, SpeechRecognitionCanceledEventArgs e)
            {
                SpeechRecognitionResult result = e.Result;
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
        public static async Task GetIntentFromUtterance(string utterance, string initiator)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ConfigManager.PREDICTION_KEY);

            // query string preparation
            queryString["query"] = utterance;               // utterance
            queryString["verbose"] = "true";                // verbose, default true
            queryString["show-all-intents"] = "false";      // show all, default "false"
            queryString["staging"] = "true";                // staging, default true?
            queryString["timezoneOffset"] = "0";            // timezoneOffset, 0? //TODO
            queryString["log"] = "true";                    // utterance logging at Azure //added 2022-03-04

            var predictionEndpointUri = string.Format("{0}luis/prediction/v3.0/apps/{1}/slots/staging/predict?{2}",
                                                   "https://p360v.cognitiveservices.azure.com/",
                                                   ConfigManager.APP_ID,
                                                   queryString);
            UnityEngine.Debug.Log("Getting Prediction");
            var response = await client.GetAsync(predictionEndpointUri);

            UnityEngine.Debug.Log("Reading content");
            var strResponseContent = await response.Content.ReadAsStringAsync();

            UnityEngine.Debug.Log("Parsing content");
            try
            {
                var responseContent = JObject.Parse(strResponseContent).ToObject<IntentResponse>();

                UnityEngine.Debug.Log($"INTENT: {responseContent.prediction.topIntent}");
                UnityMainThread.AddJob(() =>
                {
                    intentEvent.Invoke((responseContent.prediction.intents, responseContent.prediction.topIntent, initiator));
                }
                );
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
                UnityEngine.Debug.Log(strResponseContent);
            }
        }
        static private void requestDone((Dictionary<string, AzureVoice.Intent> intents, string topIntent, string initiator) o)
        {
            intentEvent.Invoke(o);
        }
        public class IntentResponse
        {
            public string query { get; set; }
            public Prediction prediction { get; set; }
            public Sentiment sentiment { get; set; }
        }

        public class Prediction
        {
            public string topIntent { get; set; }
            public Dictionary<string, Intent> intents { get; set; }
            public Entities entities { get; set; }
        }

        public class Intent
        {
            public double score { get; set; }
        }

        public class Entities
        {
            public List<string> personName { get; set; }
            [JsonProperty("$instance")]
            public Instance instance { get; set; }
        }

        public class Instance
        {
            public List<PersonName> personName { get; set; }
        }

        public class PersonName
        {
            public string type { get; set; }
            public string text { get; set; }
            public int startIndex { get; set; }
            public int length { get; set; }
            public int modelTypeId { get; set; }
            public string modelType { get; set; }
            public List<string> recognitionSources { get; set; }
        }

        public class Sentiment
        {
            public string label { get; set; }
            public double score { get; set; }
        }

    }
}