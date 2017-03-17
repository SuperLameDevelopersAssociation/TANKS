using UnityEngine;
using System.Collections;
using TrueSync;

public class ActivateCamera : TrueSyncBehaviour 
{
	public GameObject camera;

	void Start ()
	{
		if (TrueSyncManager.LocalPlayer == owner)
			camera.SetActive(true);
	}
}
