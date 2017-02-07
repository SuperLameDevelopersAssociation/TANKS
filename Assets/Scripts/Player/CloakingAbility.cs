using UnityEngine;
using System.Collections;
using TrueSync;

public class CloakingAbility : AbilitiesBase
{
    [AddTracking]
    FP _cooldown = 0;
    //Material to add to give cloaking effect
    public Material cloakMaterial;
    //Grab the current children render so they can be reset after ability is done
    public Renderer[] originalChildrenRender;
    //copy of the current children renders so they can be changed to cloaking ability
    Renderer[] cloakedChildrenRender;
    public Material[] mats;
    //float to hold duration of cloaking ability
    public float duration = 5;
    bool activated;
    [AddTracking]
    FP _duration;

    public KeyCode activationKey;

    public override void OnSyncedStart()
    {
        StateTracker.AddTracking(this);
        mats = new Material[originalChildrenRender.Length];
        //_cooldown = cooldown;
        cloakedChildrenRender = originalChildrenRender;
        for (int i = 0; i < originalChildrenRender.Length; i++)
            mats[i] = originalChildrenRender[i].material;
    }

    public override void OnSyncedInput()
    {
        byte activationKeyPressed;
        if (Input.GetKeyDown(activationKey))
            activationKeyPressed = 1;
        else
            activationKeyPressed = 0;

        TrueSyncInput.SetByte(6, activationKeyPressed);
    }

    public override void OnSyncedUpdate()
    {
        byte activationKeyPressed = TrueSyncInput.GetByte(6);
        if (activationKeyPressed == 1 && _cooldown <= 0 && !activated)
            ActivatePower();

        if (_cooldown > 0)
            _cooldown -= TrueSyncManager.DeltaTime;

        if(_duration > 0)
            _duration -= TrueSyncManager.DeltaTime;
        else if(_duration <= 0 && activated)
        {
            for (int i = 0; i < cloakedChildrenRender.Length; i++)
            {
                cloakedChildrenRender[i].material = mats[i];
            }
            _cooldown = 5;
            activated = false;
        }
    }

    public override void ActivatePower()
    {
        _duration = duration;
        activated = true;
        foreach (Renderer GO in cloakedChildrenRender)
        {
            GO.material = cloakMaterial;
        }        
    }
}
