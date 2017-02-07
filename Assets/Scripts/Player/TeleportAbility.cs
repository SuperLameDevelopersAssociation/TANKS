using UnityEngine;
using System.Collections;
using TrueSync;

public class TeleportAbility : AbilitiesBase
{
    public override void ActivatePower()
    {
        if (Input.GetKey(KeyCode.C))
        {
            tsTransform.position += tsTransform.forward * 20;
            Cooldown = 5;
        }
        base.ActivatePower();
    }
}
