public class VideoActivatable : ComboActivatable
{
    public VideoLoaderV2 video;
    bool videoPlaying = false;

    public double secondsTimeCode = 0;
    public double timeCodeWindow = 0;
    public bool anytimeAfterTimecode = false;
    void Start()
    {
        video.playStateChanged.AddListener(videoStateChanged);
    }

    void videoStateChanged(bool newState)
    {
        if (!newState)
        {
            changeActivationState(false);
        }
        videoPlaying = newState;
    }

    void Update()
    {
        if (videoPlaying)
        {
            double currentTime = GlobalVideoHandler.instancePlayer.time;
            if (currentTime > secondsTimeCode)
            {
                if (anytimeAfterTimecode || currentTime - secondsTimeCode < timeCodeWindow)
                {
                    changeActivationState(true);
                }
                else changeActivationState(false);
            }
            else changeActivationState(false);
        }
    }


}