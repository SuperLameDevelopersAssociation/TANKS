using UnityEngine;
using System.Collections;
using TrueSync;

public class CheckGameState : TrueSyncBehaviour 
{
	public override void OnSyncedUpdate ()
	{
		if(PlayerReadinessWrangler.ready)
		gameObject.SetActive (false);
	}
}
