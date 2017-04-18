using UnityEngine;
using UnityEngine.Networking;

public class SonicAbility : AbilitiesBase 
{
	PlayerMovement speed;

	float _cooldown = 0;
	public float duration = 10;
	float _duration;
	public KeyCode activateKey;
	bool activated;

	// Use this for initialization
	void Start () 
	{
		speed = gameObject.GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(activateKey) && _cooldown <= 0 && !activated)            //Input is pressed and cooldown is zeroed and not already turned on
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

	[Command]
	public override void CmdActivatePower(bool activate)
	{
		RpcActivatePower(activate);
	}

	void RpcActivatePower(bool activate)
	{
		if (activate)
		{
			_duration = duration;                                                       //Set FP to the float
			activated = true;                                                           //Set the flag up
			GetComponent<PlayerMovement>().speed *= 2;
		}
		else
		{
			_cooldown = 5;                                                              //Reset the cooldown
			activated = false;                                                          //Turn off the flag
		}
	}
}
