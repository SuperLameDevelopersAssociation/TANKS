using UnityEngine;
using System.Collections;
using TrueSync;

public class CustomizeTank : TrueSyncBehaviour 
{

	public override void OnSyncedStart()
	{
		GetComponent<CustomizationManager> ().CustomizeMyTank ();
	}
}
