using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {
    public bool transitionIn;

	// Use this for initialization
	void OnEnable ()
    {
        if (transitionIn)
            GetComponent<Animator>().Play("LoadingTransitionOut");
        else
            GetComponent<Animator>().Play("LoadingTransition");
    }
}
