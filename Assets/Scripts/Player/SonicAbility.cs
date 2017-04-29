using UnityEngine;
using UnityEngine.Networking;

public class SonicAbility : AbilitiesBase
{
    PlayerMovement speed;           //grabs the player's speed from 

    float _cooldown = 0;
    public float duration = 10;     //sets the length of the ability to 10 seconds
    float _duration;
    public KeyCode activateKey;     //sets the key that activates the ability
    bool activated;                 //indicates if the ability is active or not

    // Use this for initialization
    void Awake()
    {
        speed = gameObject.GetComponent<PlayerMovement>();      //sets the speed to the local speed variable
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(activateKey) && _cooldown <= 0 && !activated)            //Input is pressed and cooldown is zeroed and not already turned on
            CmdActivatePower(true);

        if (_cooldown > 0)                                                              //Subtract delta time from the overall time
            _cooldown -= Time.deltaTime;

        if (_duration > 0)                                                               //Subtract delta time from the overall time
            _duration -= Time.deltaTime;
        else if (_duration <= 0 && activated)
        {
            CmdActivatePower(false);
        }
    }

    [Command]                                                                           //Tells the server to activate the ability
    public override void CmdActivatePower(bool activate)
    {
        RpcActivatePower(activate);
    }

    [ClientRpc]                                                                         //Activates the power on all clients
    void RpcActivatePower(bool activate)
    {
        if (activate)
        {
            _duration = duration;                                                       //Set FP to the float
            activated = true;                                                           //Set the flag up
            speed.MultiplySpeed(2);
        }
        else
        {
            _cooldown = 5;                                                              //Reset the cooldown
            activated = false;                                                          //Turn off the flag
            speed.ResetSpeed();
        }
    }
}
