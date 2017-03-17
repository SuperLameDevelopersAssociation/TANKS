using UnityEngine;
using System.Collections;
using TrueSync;

public class PausePanelWrangler : TrueSyncBehaviour
{
    public GameObject readinessPanel;

    public override void OnGamePaused()
    {
        readinessPanel.SetActive(true);
    }

    public override void OnGameUnPaused()
    {
        readinessPanel.SetActive(false);
    }
}
