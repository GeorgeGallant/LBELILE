using Kage.Copernic360.Configuration;
using UnityEngine;

namespace Kage.Copernic360
{
    public class SkyboxController
    {
        private FrameConfigurationProvider frameConfigurationProvider;
        private Material skybox;

        public SkyboxController(Texture tex, ContentConfiguration configuration)
        {
            // We duplicate the material so that changes to it don't affect
            // the material asset.
            skybox = new Material(RenderSettings.skybox);
            RenderSettings.skybox = skybox;

            skybox.mainTexture = tex;

            frameConfigurationProvider = new FrameConfigurationProvider(configuration);
            frameConfigurationProvider.FrameConfigurationRefreshed += OnFrameConfigurationRefreshed;
            UpdateSkyboxProperties();
        }

        ~SkyboxController()
        {
            frameConfigurationProvider.FrameConfigurationRefreshed -= OnFrameConfigurationRefreshed;
        }

        // Replaces the current texture.
        public void SetTexture(Texture tex)
        {
            skybox.mainTexture = tex;
        }

        // Replaces the current content configuration.
        public void SetConfiguration(ContentConfiguration configuration)
        {
            frameConfigurationProvider.ContentConfiguration = configuration;
        }

        // Sets the current frame. Only relevant for video content.
        public void SetCurrentFrame(ulong frame)
        {
            frameConfigurationProvider.CurrentFrame = frame;
        }

        // Sets the scene's rotation about the y-axis, in degrees.
        public void SetRotation(float degrees)
        {
            skybox.SetFloat(ShaderProperties.RotationY, degrees * Mathf.Deg2Rad);
        }

        private void OnFrameConfigurationRefreshed()
        {
            UpdateSkyboxProperties();
        }

        private void UpdateSkyboxProperties()
        {
            FrameConfiguration frame = frameConfigurationProvider.FrameConfiguration;

            skybox.SetFloat(ShaderProperties.Height, frame.Height);
            skybox.SetFloat(ShaderProperties.Radius, frame.Radius);
            skybox.SetFloat(ShaderProperties.FloorRadius, frame.FloorRadius);
            skybox.SetFloat(ShaderProperties.MovementRange, frame.MovementRange);
        }
    }
}