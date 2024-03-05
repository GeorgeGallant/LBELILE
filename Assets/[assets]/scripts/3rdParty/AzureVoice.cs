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
        public static async Task Listener(ValueWrapper<bool> continueListening, string initiator, string relevantScene)
        {
            // if (busy) return;
            busy = true;
            var config = SpeechConfig.FromSubscription(ConfigManager.SUBSCRIPTION_KEY, ConfigManager.REGION_NAME);

            var predictionEndpointUri = "https://p360v2.cognitiveservices.azure.com/";

            var cluModel = new ConversationalLanguageUnderstandingModel(
              ConfigManager.LANGUAGE_RESOURCE_KEY,
              predictionEndpointUri,
              "P360V_1",
              "p3vDev1");

            var collection = new LanguageUnderstandingModelCollection();
            collection.Add(cluModel);

      /****
       * This was added by Stephen on 2024-03-05 to limit the intents
       * Ideally this should be loaded when the conservation scene is loaded (or each scene)
       * 
       *****/
      string[] validSceneIntents = {
          "p3v.dispatch.ackArrive",
          "p3v.dispatch.ackClear",
          "p3v.dispatch.ackCopy",
          "p3v.fishgame.checkCatch",
          "p3v.fishgame.citeRegulations",
          "p3v.fishgame.getPermit",
          "p3v.fishgame.getStatusCard",
          "p3v.fishgame.giveTicket",
          "p3v.fishgame.giveWarning",
          "p3v.fishgame.haveAuthority",
          "p3v.fishgame.identification",
          "p3v.fishgame.idPlusReason",
          "p3v.fishgame.letItGo",
          "p3v.fishgame.giveReason"
      };

            var recognizer = new IntentRecognizer(config);
            recognizer.ApplyLanguageModels(collection);

      /****
       * This was modified by Stephen on 2024-03-05 manage limited intents instead of all 
       ****/
            // recognizer.AddAllIntents(cluModel);
            foreach(string intent in validSceneIntents)
            {
                recognizer.AddAllIntents(cluModel,intent);
            }
      /**** end modification ****/

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
                var json = result.Properties.GetProperty(PropertyId.SpeechServiceResponse_JsonResult);
                if (result.Reason == ResultReason.RecognizedIntent)
                {
                    UnityEngine.Debug.Log($"Speech: {utterance}, Intent: {e.Result.IntentId}");
                    intent = e.Result.IntentId;
                    // await GetIntentFromUtterance(utterance, initiator);}
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    intent = "No speech";
                }
                string destination = "null";
                intentDestinations.TryGetValue(intent, out destination);

                UnityMainThread.AddJob(() =>
                {
                    IntentRecorder.RecordIntent((utterance, intent, initiator, json, destination));
                    intentEvent.Invoke((intent, initiator, relevantScene));
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


        /*
        // old logic, just use above
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
        */

        // also old
        /*
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
        */
    }
}