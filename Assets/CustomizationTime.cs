using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TrueSync;
using UnityEngine.SceneManagement;

public class CustomizationTime : TrueSyncBehaviour
{
    CustomizationManager customizationManager;
    public GameObject customizationPanel;
    public int timeToCustomize;
    public Text timer;

    public override void OnSyncedStart()
    {
        customizationManager = GameObject.Find("GameManager").GetComponent<CustomizationManager>();
        TrueSyncManager.SyncedStartCoroutine(TimeLeft());
    }

    IEnumerator TimeLeft()
    {
        for (int i = timeToCustomize; i > 0; i--)
        {
            timer.text = "Time Left: " + i;
            yield return 1;
        }

        customizationPanel.SetActive(false);
        customizationManager.CustomizeMyTank();
    }
}