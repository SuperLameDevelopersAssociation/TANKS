using UnityEngine;
using System.Collections;

public class CloakingAbility : AbilitiesBase {

    public Material cloakMaterial;
    int renderCount = 0;
    Renderer[] originalChildrenRender;
    
    public override void OnSyncedStart()
    {
        originalChildrenRender = GetComponentsInChildren<Renderer>();
        base.OnSyncedStart();
    }
    public override void ActivatePower()
    {
        if (Input.GetKey(KeyCode.C) && )
        {
            
           
        }
        base.ActivatePower();
    }
}
