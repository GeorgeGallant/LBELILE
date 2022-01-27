using System;

namespace Kage.Copernic360.Configuration
{
    // Handles the configuration data of the current frame, to
    // hide from other classes whether a video or image is being
    // viewed
    internal class FrameConfigurationProvider
    {
        private ContentConfiguration _contentConfiguration;
        public ContentConfiguration ContentConfiguration
        {
            get => _contentConfiguration;
            set
            {
                _contentConfiguration = value;
                RefreshConfiguration();
            }
        }

        private ulong _currentFrame;
        public ulong CurrentFrame
        {
            get => _currentFrame;
            set
            {
                _currentFrame = value;
                RefreshConfiguration();
            }
        }

        public FrameConfiguration FrameConfiguration { get; private set; }

        public event Action FrameConfigurationRefreshed;

        public FrameConfigurationProvider(ContentConfiguration configuration)
        {
            ContentConfiguration = configuration;
        }

        private void RefreshConfiguration()
        {
            if (ContentConfiguration is ImageConfiguration)
                FrameConfiguration = (ContentConfiguration as ImageConfiguration).ImageData;
            else
                FrameConfiguration = (ContentConfiguration as VideoConfiguration).GetFrameData(CurrentFrame);

            FrameConfigurationRefreshed?.Invoke();
        }
    }
}