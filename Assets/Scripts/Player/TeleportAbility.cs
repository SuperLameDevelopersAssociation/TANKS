using UnityEngine;
using System.Collections;
using TrueSync;

public class TeleportAbility : AbilitiesBase
{
    public KeyCode activationKey;                   //Allow the designer to set the key press to activate the power
    public int distance;                            //distance to allow the Raycast to find colliders
    [Tooltip("After the teleport, what the coolDown resets to")]
    public FP maxCooldown = 5;                      //after the teleport, what the coolDown resets to
    [AddTracking]
    FP _cooldown;                                   //create a cooldown so the players don't spam the button
    PhysicsWorldManager manager;                    //Instance that handles the Raycast. Should try the PhysicsWorldManager.instance

    public override void OnSyncedStart()            //TrueSync's version of OnStart()
    {
        StateTracker.AddTracking(this);
        manager = PhysicsWorldManager.instance;
    }

    public override void OnSyncedInput()                        //TrueSync uses this as input, rather than in Update()
    {
        byte activationKeyPressed;                              //Variable/Flag to indicate button was pressed
        if (Input.GetKeyDown(activationKey))
            activationKeyPressed = 1;                           //1 is active
        else
            activationKeyPressed = 0;                           //0 is inactive

        TrueSyncInput.SetByte(6, activationKeyPressed);
    }

    public override void OnSyncedUpdate()
    {
        byte activationKeyPressed = TrueSyncInput.GetByte(6);           //Checks for input

        if (activationKeyPressed == 1 && _cooldown <= 0)                //Input is pressed and cooldown is zeroed
            ActivatePower();

        if (_cooldown > 0)                                              //Subtract delta time from the overall time
            _cooldown -= TrueSyncManager.DeltaTime;
    }

    public override void ActivatePower()
    {
        TSRay ray = new TSRay(tsTransform.position + (tsTransform.forward * 3.55f), tsTransform.forward);
        TSRaycastHit hit = manager.Raycast(ray, distance + 5);
        
        if (hit != null)
        {
            //maybe show something on screen saying that you can't teleport

            //How about like a flashing Exclamation point in the corner to indicate not available
        }
        else
        {
            tsTransform.position += tsTransform.forward * distance;
            _cooldown = maxCooldown;
        }
    }
}
