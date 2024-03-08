using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BirdSounds : MonoBehaviour
{
    public float MaxVolume = 0.4f;
    public float fadeTime = 1;
    float targetLerp;
    float currentLerp;
    bool updateVolume = false;
    bool startRan = false;
    AudioSource audioSource;
    void Start()
    {
        if (startRan) return;
        MaxVolume = Mathf.Clamp01(MaxVolume);
        audioSource = GetComponent<AudioSource>();
        startRan = true;
    }
    public void PlayBirds()
    {
        Start();
        if (targetLerp == 1) return;
        audioSource.Play();
        targetLerp = 1;
        updateVolume = true;
    }
    public void StopBirds()
    {
        if (targetLerp == 0) return;
        targetLerp = 0;
        updateVolume = true;
    }
    void Update()
    {
        if (!updateVolume) return;

        currentLerp += Time.deltaTime / fadeTime * Mathf.Sign(targetLerp - currentLerp);
        currentLerp = Mathf.Clamp01(currentLerp);
        audioSource.volume = Mathf.Lerp(0, MaxVolume, currentLerp);
        if (currentLerp == targetLerp)
        {
            updateVolume = false;
            if (currentLerp == 0) audioSource.Stop();
        }
    }
}