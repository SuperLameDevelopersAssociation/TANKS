using UnityEngine;
using System.Collections;
using TrueSync;

public class CloakingAbility : AbilitiesBase
{
    [AddTracking]
    FP _cooldown = 0;
    
    public Material cloakMaterial;                                  //Material to add to give cloaking effect
    public Renderer[] originalChildrenRender;                       //Grab the current children render so they can be reset after ability is done
    Renderer[] cloakedChildrenRender;                               //copy of the current children renders so they can be changed to cloaking ability
    public Material[] mats;                                         //gather all materials setup for the cloaking
    public float duration = 5;                                      //float to hold duration of cloaking ability
    bool activated;                                                 //Flag to indicate power is active
    [AddTracking]
    FP _duration;                                                   //Duration of power

    public KeyCode activationKey;                                   //Allow the designer to set the key press to activate the power

    public override void OnSyncedStart()                            //TrueSync's version of OnStart()
    {
        StateTracker.AddTracking(this);
        mats = new Material[originalChildrenRender.Length];         //Set length of array
        //_cooldown = cooldown;
        cloakedChildrenRender = originalChildrenRender;             //Copy array to second array
        for (int i = 0; i < originalChildrenRender.Length; i++)
            mats[i] = originalChildrenRender[i].material;           //Set mats array to originalChildrenRender
    }

	void Update()
	{
		
	}

    public override void OnSyncedInput()                            //TrueSync uses this as input, rather than in Update()
    {
        byte activationKeyPressed;                                  //Variable/Flag to indicate button was pressed
        if (Input.GetKeyDown(activationKey))
            activationKeyPressed = 1;                               //1 is active
        else
            activationKeyPressed = 0;                               //0 is inactive

        TrueSyncInput.SetByte(6, activationKeyPressed);
    }

    public override void OnSyncedUpdate()
    {
        byte activationKeyPressed = TrueSyncInput.GetByte(6);                       //Checks for input

        if (activationKeyPressed == 1 && _cooldown <= 0 && !activated)              //Input is pressed and cooldown is zeroed and not already turned on
            ActivatePower();

        if (_cooldown > 0)                                                          //Subtract delta time from the overall time
            _cooldown -= TrueSyncManager.DeltaTime;

        if(_duration > 0)                                                           //Subtract delta time from the overall time
            _duration -= TrueSyncManager.DeltaTime;
        else if(_duration <= 0 && activated)
        {
            for (int i = 0; i < cloakedChildrenRender.Length; i++)
            {
                cloakedChildrenRender[i].material = mats[i];                        //Change materials back from the cloaked materials.
            }
            _cooldown = 5;                                                          //Reset the cooldown
            activated = false;                                                      //Turn off the flag
        }
    }

    public override void ActivatePower()
    {
        _duration = duration;                                                       //Set FP to the float
        activated = true;                                                           //Set the flag up
        foreach (Renderer GO in cloakedChildrenRender)
        {
            GO.material = cloakMaterial;                                            //Change materials to cloaked material
        }        
    }
}
