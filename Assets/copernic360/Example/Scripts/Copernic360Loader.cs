using Kage.Copernic360.Configuration;
using System.IO;
using UnityEngine;

#if UNITY_ANDROID
using System.Collections;
using UnityEngine.Networking;
#endif

namespace Kage.Copernic360.Example
{
    public class Copernic360Loader : MonoBehaviour
    {
        [SerializeField] private Texture2D skyboxTexture = null;

        private SkyboxController skyboxController;

#if UNITY_ANDROID
        private void Awake()
        {
            StartCoroutine(LoadConfigurationFile());
        }

        private IEnumerator LoadConfigurationFile()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "copernic360/example-configuration.6dof");

            var webRequest = UnityWebRequest.Get(filePath);
            yield return webRequest.SendWebRequest();

            using (var stream = new MemoryStream(webRequest.downloadHandler.data))
                InstantiateSkyboxController(stream);
        }
#else
        private void Awake()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "copernic360/example-configuration.6dof");

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                InstantiateSkyboxController(stream);
        }
#endif

        private void InstantiateSkyboxController(Stream stream)
        {
            skyboxController = new SkyboxController(
                skyboxTexture,
                ImageConfiguration.LoadFromStream(stream)
            );
        }
    }
}