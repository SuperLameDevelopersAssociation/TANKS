using UnityEngine;
using UnityEngine.Networking;

public class CloakingAbility : AbilitiesBase
{
    float _cooldown = 0;
    
    public Material cloakMaterial;                                  //Material to add to give cloaking effect
    public Renderer[] originalChildrenRender;                       //Grab the current children render so they can be reset after ability is done
    Renderer[] cloakedChildrenRender;                               //copy of the current children renders so they can be changed to cloaking ability
    public Material[] mats;                                         //gather all materials setup for the cloaking
    public float duration = 5;                                      //float to hold duration of cloaking ability
    bool activated;                                                 //Flag to indicate power is active
    float _duration;                                                //Duration of power

    public KeyCode activationKey;                                   //key to activate the power

    void Awake()
    {
        originalChildrenRender = GetComponentsInChildren<Renderer>();
        mats = new Material[originalChildrenRender.Length];         //Set length of array
        //_cooldown = cooldown;
        cloakedChildrenRender = originalChildrenRender;             //Copy array to second array
        for (int i = 0; i < originalChildrenRender.Length; i++)
            mats[i] = originalChildrenRender[i].material;           //Set mats array to originalChildrenRender

        CmdFindObjects();
    }

    [Command]
    void CmdFindObjects()
    {
        originalChildrenRender = GetComponentsInChildren<Renderer>();
        cloakedChildrenRender = originalChildrenRender;
        mats = new Material[originalChildrenRender.Length];
        for (int i = 0; i < originalChildrenRender.Length; i++)
            mats[i] = originalChildrenRender[i].material;
    }

    void Update()
    {
        if (Input.GetKeyDown(activationKey) && _cooldown <= 0 && !activated)            //Input is pressed and cooldown is zeroed and not already turned on
            CmdActivatePower(true);                                                         

        if (_cooldown > 0)                                                              //Subtract delta time from the overall time
            _cooldown -= Time.deltaTime;

        if(_duration > 0)                                                               //Subtract delta time from the overall time
            _duration -= Time.deltaTime;
        else if(_duration <= 0 && activated)
        {
            CmdActivatePower(false);
        }
    }

    [Command]   //Tells server to acvtivate power
    public override void CmdActivatePower(bool activate)
    {
        RpcActivatePower(activate);
    }

    [ClientRpc] //Activates power on all clients
    void RpcActivatePower(bool activate)
    {
        if (activate)
        {
            _duration = duration;                                                       //Set FP to the float
            activated = true;                                                           //Set the flag up
            for (int i = 0; i < cloakedChildrenRender.Length; i++)
            {
                cloakedChildrenRender[i].material = cloakMaterial;                      //Change materials to cloaked material
            }
        }
        else
        {
            for (int i = 0; i < cloakedChildrenRender.Length; i++)
            {
                cloakedChildrenRender[i].material = mats[i];                            //Change materials back from the cloaked materials.
            }
            _cooldown = 5;                                                              //Reset the cooldown
            activated = false;                                                          //Turn off the flag
        }
    }
}
