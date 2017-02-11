using UnityEngine;
using System.Collections;
using TrueSync;

public class TeleportAbility : AbilitiesBase
{
    public KeyCode activationKey;
    public int distance;
    [AddTracking]
    FP _cooldown;
    PhysicsWorldManager manager;

    public override void OnSyncedStart()
    {
        StateTracker.AddTracking(this);
        manager = new PhysicsWorldManager();
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

        if (activationKeyPressed == 1 && _cooldown <= 0)
            ActivatePower();

        if (_cooldown > 0)
            _cooldown -= TrueSyncManager.DeltaTime;
    }

    public override void ActivatePower()
    {
        TSRay ray = new TSRay(tsTransform.position + (tsTransform.forward * 3.55f), tsTransform.forward);
        TSRaycastHit hit = manager.Raycast(ray, distance + 5);
        
        if (hit != null)
        {
            //maybe show something on screen saying that you can't teleport
        }
        else
        {
            tsTransform.position += tsTransform.forward * distance;
            _cooldown = 5;
        }
    }
}
