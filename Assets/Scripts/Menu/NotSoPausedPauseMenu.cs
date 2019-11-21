using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using System.Collections;

public class NotSoPausedPauseMenu : MonoBehaviour {

    [SerializeField]
    GameObject pauseMenu;

    public static bool isOn = false;

    NetworkManager networkManager;

    void Start()
    {
        isOn = false;
        networkManager = NetworkManager.singleton;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePauseMenu();
	}

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        isOn = pauseMenu.activeSelf;
    }

    public void LeaveRoom()
    {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }
}
