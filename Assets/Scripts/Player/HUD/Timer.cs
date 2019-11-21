using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public Text[] timers;

    private int matchCount;
	
	// Update is called once per frame
	void Update ()
    {
        if (GameManager.GetInstance == null)
        {
            Debug.Log("The instance is null");
            return;
        }

        matchCount = GameManager.GetInstance.matchTime.Length;

        if (matchCount == 5)
        {
            for (int i = 0; i < matchCount; i++)
            {
                timers[i].text = GameManager.GetInstance.matchTime[i].ToString();
            }
        }
        else if (matchCount == 4)
        {
            timers[0].text = "0";
            for (int i = 1; i < matchCount; i++)
            {
                timers[i].text = GameManager.GetInstance.matchTime[i].ToString();
            }
        }
        else if (matchCount < 4)
        {
            timers[0].text = "0";
            timers[1].text = "0";
            for (int i = 2; i < matchCount; i++)
            {
                timers[i].text = GameManager.GetInstance.matchTime[i].ToString();
            }
        }

    }
}
