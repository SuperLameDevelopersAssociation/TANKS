using UnityEngine;
using System.Collections;
using TrueSync;

public class AbilitiesBase : TrueSyncBehaviour {

    private float cooldown;

    public float Cooldown
    {
        get { return cooldown; }
        set { cooldown = value; }
    }

    public virtual void ActivatePower()
    {

    }
}
