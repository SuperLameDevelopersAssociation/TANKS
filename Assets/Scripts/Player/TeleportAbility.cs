using UnityEngine;
using UnityEngine.Networking;

public class TeleportAbility : AbilitiesBase
{
    public KeyCode activationKey;                   //Allow the designer to set the key press to activate the power
    public int distance;                            //distance to allow the Raycast to find colliders
    [Tooltip("After the teleport, what the coolDown resets to")]
    public float maxCooldown = 5;                      //after the teleport, what the coolDown resets to
    float _cooldown;                                   //create a cooldown so the players don't spam the button

    void Update()
    {
        if (Input.GetKeyDown(activationKey) && _cooldown <= 0)              //Input is pressed and cooldown is zeroed
            CmdActivatePower(true);

        if (_cooldown > 0)                                                  //Subtract delta time from the overall time
            _cooldown -= Time.deltaTime;
    }

    [Command]   //Tells server to acvtivate power
    public override void CmdActivatePower(bool activate)    //only requires a bool because commands can't be overloaded
    {
        Ray ray = new Ray(transform.position + (transform.forward * 3.55f), transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray.origin, ray.direction, out hit, distance + 5))
        {
            //maybe show something on screen saying that you can't teleport
        }
        else
            RpcActivatePower();
    }

    [ClientRpc] //Activates power on all clients
    void RpcActivatePower()
    {
        transform.position += transform.forward * distance;
        _cooldown = maxCooldown;
    }
}
