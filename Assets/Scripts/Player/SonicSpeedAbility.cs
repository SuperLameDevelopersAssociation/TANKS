using UnityEngine;
using System.Collections;
using TrueSync;

public class SonicSpeedAbility :TrueSyncBehaviour
{
    public KeyCode abilityKey;
    public float duration;
    PlayerMovement speed;

    public override void OnSyncedStart()
    {
        speed = gameObject.GetComponent<PlayerMovement>();
    }
}
