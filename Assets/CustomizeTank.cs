using UnityEngine;
using System.Collections;
using TrueSync;

public class CustomizeTank : TrueSyncBehaviour 
{
	PhotonView pv;

	public override void OnSyncedStart()
	{
		pv = GetComponent<PhotonView> ();
		if (TrueSyncManager.LocalPlayer == owner) 
		{
			pv.RPC ("CustomizeMyTank", PhotonTargets.All);
		}
	}
}
