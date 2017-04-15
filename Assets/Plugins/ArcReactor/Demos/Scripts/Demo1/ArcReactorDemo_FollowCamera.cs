using UnityEngine;
using System.Collections;

public class ArcReactorDemo_FollowCamera : MonoBehaviour {
	
	public Transform followTransform;
	public Rect rect = new Rect(0.01f,0.01f,0.4f,0.4f);
	
	private Camera currentCamera;

	// Use this for initialization
	void Start () 
	{
		currentCamera = GetComponent<Camera>();
		currentCamera.rect = rect;	
	}

	void LateUpdate () 
	{
		transform.LookAt(followTransform.position);
	}
}
