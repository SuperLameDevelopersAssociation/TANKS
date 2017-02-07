using UnityEngine;
using System.Collections;
using TrueSync;

public class CloakingAbility : AbilitiesBase
{
    FP _cooldown;
    //Material to add to give cloaking effect
    public Material cloakMaterial;
    //Grab the current children render so they can be reset after ability is done
    public Renderer[] originalChildrenRender;
    //copy of the current children renders so they can be changed to cloaking ability
    Renderer[] cloakedChildrenRender;
    //float to hold duration of cloaking ability
    public float duration;
    [AddTracking]
    FP _duration;

    public KeyCode activationKey;

    public override void OnSyncedStart()
    {
        StateTracker.AddTracking(this);
        _duration = duration;
        _cooldown = cooldown;
        cloakedChildrenRender = originalChildrenRender;
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
        if (activationKeyPressed == 1)
            ActivatePower();

        if (_cooldown > 0)
            _cooldown -= TrueSyncManager.DeltaTime;
    }

    public override void ActivatePower()
    {
        if (_cooldown <= 0)
        {
            while (duration > 0)
            {
                foreach (Renderer GO in cloakedChildrenRender)
                {
                    GO.material = cloakMaterial;
                }

                _duration -= TrueSyncManager.DeltaTime;
            }
            for (int i = 0; i < cloakedChildrenRender.Length; i++)
            {
                cloakedChildrenRender[i].material = originalChildrenRender[i].material;
            }
        }
        _cooldown = 5;
    }
}
