using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BinocularAudioObject : BinocularActivatable
{
    AudioSource audioSource;

    float TargetVolume
    {
        get
        {
            return targetVolume;
        }
        set
        {
            if (targetVolume == value) return;
            targetVolume = value;
            updateVolume = true;
            if (targetVolume > 0) audioSource.Play();
        }
    }
    float targetVolume = 0;
    bool updateVolume = false;
    float fadeTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = targetVolume;
    }

    public override void SubscribeActivatable(BinocularGrabbable binocular)
    {
        base.SubscribeActivatable(binocular);
        transform.parent = null;
    }

    public override void OnHeldUp()
    {
        base.OnHeldUp();
        TargetVolume = 1;
        Debug.Log("should play");
    }
    public override void OnReleased()
    {
        base.OnReleased();
        TargetVolume = 0;
    }

    void Update()
    {
        if (!updateVolume) return;
        float currentVolume = audioSource.volume;

        currentVolume += Time.deltaTime / fadeTime * Mathf.Sign(TargetVolume - currentVolume);

        audioSource.volume = Mathf.Clamp01(currentVolume);
        if (currentVolume == TargetVolume)
        {
            updateVolume = false;
            if (currentVolume == 0) audioSource.Stop();
        }
    }

}
