using UnityEngine;
using System.Collections;
using TrueSync;

public class PausePanelWrangler : TrueSyncBehaviour
{
    public GameObject readinessPanel;
    public override void OnGamePaused()
    {
        print("Game Paused");
        readinessPanel.SetActive(true);
    }

    public override void OnGameUnPaused()
    {

        print("Game UnPaused");
        readinessPanel.SetActive(false);
    }
}
