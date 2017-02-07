using UnityEngine;
using System.Collections;
using TrueSync;

public class TeleportAbility : AbilitiesBase {

    //TSVector
    public override void ActivatePower()
    {
        if (Input.GetKey(KeyCode.C))
        {
            tsTransform.position = new TSVector(gameObject.transform.position.x + 20, gameObject.transform.position.y, gameObject.transform.position.z);

        }
        base.ActivatePower();
    }
}
