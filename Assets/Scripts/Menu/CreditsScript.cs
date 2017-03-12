using UnityEngine;
using System.Collections;

public class CreditsScript : MonoBehaviour {
    [SerializeField]
    GameObject mainMenu;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	private void Update()
    {
        if (Input.anyKey && gameObject.activeSelf)//this allows us to skip the menu
            gameObject.GetComponent<Animator>().speed = 10.0f;
    }
    public void EndCredits()
    {
        mainMenu.SetActive(true);
        gameObject.GetComponent<Animator>().speed = 1.0f;
        gameObject.SetActive(false);
    }
}
