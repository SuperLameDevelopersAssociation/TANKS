using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public Text[] timers;

    private int matchCount;
	
	// Update is called once per frame
	void Update ()
    {
        matchCount = GameManager.instance.matchTime.Length;

        if (matchCount == 5)
        {
            for (int i = 0; i < matchCount; i++)
            {
                timers[i].text = GameManager.instance.matchTime[i].ToString();
            }
        }
        else if (matchCount == 4)
        {
            timers[0].text = "0";
            for (int i = 1; i < matchCount; i++)
            {
                timers[i].text = GameManager.instance.matchTime[i].ToString();
            }
        }
        else if (matchCount < 4)
        {
            timers[0].text = "0";
            timers[1].text = "0";
            for (int i = 2; i < matchCount; i++)
            {
                timers[i].text = GameManager.instance.matchTime[i].ToString();
            }
        }

    }
}
