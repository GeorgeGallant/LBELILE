using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinocularVideoActivatable : BinocularActivatable
{
    VideoLoaderV2 loader;
    void Start()
    {
        loader = GetComponent<VideoLoaderV2>();
    }
    public override void OnHeldUp()
    {
        loader.playVideo();
        base.OnHeldUp();
    }
    public override void OnReleased()
    {
        loader.stopVideo();
        base.OnReleased();
    }
}
