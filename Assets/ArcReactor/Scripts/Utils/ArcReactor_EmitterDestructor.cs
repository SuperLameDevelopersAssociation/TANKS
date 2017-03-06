using UnityEngine;
using System.Collections;

public class ArcReactor_EmitterDestructor : MonoBehaviour {

	public ParticleSystem partSystem;
	public bool onlyDisable;

	// Update is called once per frame
	void Update () 
	{
		if (!partSystem.IsAlive())
		{
			if (onlyDisable)
			{
				gameObject.SetActive(false);
				enabled = false;
			}
			else
				Destroy(gameObject);
		}
	}
}
