using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;

namespace ThirdParty
{
    public class AzureVoice
    {
        static bool busy = false;
        static public async Task TimedListener(ValueWrapper<bool> continueListening)
        {
            if (busy) return;
            busy = true;
            var config = SpeechConfig.FromSubscription(ConfigManager.SUBSCRIPTION_KEY, ConfigManager.REGION_NAME);
            string utterance = "";
            SpeechRecognitionResult result = null;

            using var recognizer = new SpeechRecognizer(config);
            recognizer.Recognized += resultRecieved;
            await recognizer.StartContinuousRecognitionAsync();

            while (continueListening.Value) await Task.Delay(100);

            UnityEngine.Debug.Log("no longer listening");
            busy = false;

            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);

            void resultRecieved(object sender, SpeechRecognitionEventArgs e)
            {
                result = e.Result;
                utterance = result.Text;
                UnityEngine.Debug.Log($"{utterance}, {result.Reason}");
                recognizer.Recognized -= resultRecieved;
            }
        }


    }
}