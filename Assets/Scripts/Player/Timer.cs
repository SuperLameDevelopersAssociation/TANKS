using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public Text[] timers;

    private int matchCount;
    private Deathmatch matchTime;

    // Use this for initialization
    void Start () {
        matchTime  = GameObject.Find("Deathmatch").GetComponent<Deathmatch>();          // The Timer is set in Deathmatch script.
	}
	
	// Update is called once per frame
	void Update () {
        matchCount = matchTime.MatchTime.Length;
        if (matchCount == 4)
        {
            for (int i = 0; i < matchCount; i++) {
                timers[i].text = matchTime.MatchTime[i].ToString();
            }
        }
        else if (matchCount < 4)
        {
            timers[0].text = "0";
            for (int i = 0; i < matchCount; i++)
            {
                timers[i+1].text = matchTime.MatchTime[i].ToString();
            }
        }

    }
}
