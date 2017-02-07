using UnityEngine;
using System.Collections;
using TrueSync;

public class AbilitiesBase : TrueSyncBehaviour
{
    [AddTracking]
    private FP cooldown;
   
    public FP Cooldown
    {
        get { return cooldown; }
        set { cooldown = value; }
    }

    public virtual void ActivatePower()
    {

    }
}
