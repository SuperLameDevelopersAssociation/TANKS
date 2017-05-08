using UnityEngine;
using System.Collections;

public class LobbySoundCheck : MonoBehaviour
{
    public GameObject menuSound;
    public GameObject lobbySound;

	// Update is called once per frame
	void Update ()
    {
        lobbySound.SetActive(!menuSound.activeSelf);
	}
}
