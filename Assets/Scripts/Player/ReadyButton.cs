using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TrueSync;

public class ReadyButton : TrueSyncBehaviour {

    public PlayerReadinessWrangler readinessWrangler;

	// Use this for initialization
	void Start () {
        readinessWrangler = GameObject.Find("GameManager").GetComponent<PlayerReadinessWrangler>();
       // gameObject.GetComponent<Button>().onClick.AddListener(() => { readinessWrangler.CheckReadiness(); });
	}
	
	public void AddScore()
    {
        print("add score");
        PhotonNetwork.player.AddScore(1);
    }
}
