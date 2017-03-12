using UnityEngine;
using TrueSync;
using System.Collections;

public class BasicLoopingSFX : TrueSyncBehaviour{

    public AudioSource source;
    public AudioClip sfx;


    public override void OnSyncedStart()    // yes, I know you could set all this in Unity, but redunency is nice, no?
    {
        source.loop = true;
        source.clip = sfx;

        source.Play();
    }
}
