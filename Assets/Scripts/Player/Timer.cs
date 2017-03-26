using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    Text tankTimer;
    Deathmatch matchTime;

    // Use this for initialization
    void Start () {
        tankTimer = gameObject.GetComponent<Text>();
        matchTime  = GameObject.Find("Deathmatch").GetComponent<Deathmatch>();          // The Timer is set in Deathmatch script.
	}
	
	// Update is called once per frame
	void Update () {
        tankTimer.text = matchTime.MatchTime;
    }
}
