using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AbilitiesBase : NetworkBehaviour
{
    [SerializeField]
    float cooldown;
   
    public float Cooldown
    {
        get { return cooldown; }
        set { cooldown = value; }
    }

    [Command]
    public virtual void CmdActivatePower(bool activate) { }
}
