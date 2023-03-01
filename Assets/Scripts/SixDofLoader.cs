using Kage.Copernic360;
using Kage.Copernic360.Configuration;
using System.IO;
using UnityEngine;
using UnityEngine.Video;


    public class SixDofLoader : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private VideoLoader videoLoader;

        private SkyboxController skyboxController;

        private void Awake()
        {
            string configPath = Path.Combine(Application.persistentDataPath, videoLoader.fileName + ".6dof");
            Debug.Log("LOADING 6DOF");
            Debug.Log(configPath);
            using (var fs = new FileStream(configPath,FileMode.Open, FileAccess.Read))
            {
                skyboxController = new SkyboxController(videoPlayer.targetTexture,VideoConfiguration.LoadFromStream(fs));
            }
        }
        private void Update()
        {
            long frame = videoPlayer.frame;
            if (frame < 0)
                return;
            skyboxController.SetCurrentFrame((ulong)frame);
        }
    }