using Kage.Copernic360;
using Kage.Copernic360.Configuration;
using System.IO;
using UnityEngine;
using UnityEngine.Video;


    public class Copernic360VideoLoader4D: MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;

        private SkyboxController skyboxController;

        private void Awake()
        {
            string configPath = Path.Combine(Application.streamingAssetsPath,"4DV1.6dof");
            using (var fs = new FileStream(configPath,FileMode.Open, FileAccess.Read))
            {
                skyboxController = new SkyboxController(videoPlayer.targetTexture,VideoConfiguration.LoadFromStream(fs)
                );
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