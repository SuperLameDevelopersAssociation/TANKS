using UnityEngine;
using System.Collections;
using TrueSync;

public class SonicSpeedAbility : AbilitiesBase
{
    public KeyCode abilityKey;
    public float duration = 10;
    PlayerMovement speed;

    [AddTracking]
    FP _cooldown = 0;

    [AddTracking]
    FP _duration;

    public override void OnSyncedStart()
    {
        StateTracker.AddTracking(this);
        speed = gameObject.GetComponent<PlayerMovement>();
    }

    public override void OnSyncedInput()
    {
        byte abilityKeyPressed;

        if (Input.GetKeyDown(abilityKey))
            abilityKeyPressed = 1;      //ability is active
        else
            abilityKeyPressed = 0;      //ability is deactivated

        TrueSyncInput.SetByte(6, abilityKeyPressed);
    }

    public override void OnSyncedUpdate()
    {
        byte abilityKeyPressed = TrueSyncInput.GetByte(6);

        if (abilityKeyPressed == 1 && _cooldown <= 0)              
            ActivatePower();

        if (_cooldown > 0)                                                          
            _cooldown -= TrueSyncManager.DeltaTime;

        _duration = duration;
    }

    public override void ActivatePower()
    {
        GetComponent<PlayerMovement>().speed *= 2;
        if (_duration > 0)                                                           //Subtract delta time from the overall time
        {
            _duration -= TrueSyncManager.DeltaTime;
        }
        else if (_duration <= 0)
        {
            _cooldown = 5;                                                          //Reset the cooldown
        }
    }
}
