using UnityEngine;
using System.Collections;

public class CloakingAbility : AbilitiesBase
{
    //Material to add to give cloaking effect
    public Material cloakMaterial;
    //Grab the current children render so they can be reset after ability is done
    public Renderer[] originalChildrenRender;
    //copy of the current children renders so they can be changed to cloaking ability
    Renderer[] cloakedChildrenRender;
    //float to hold duration of cloaking ability
    public float duration;

    public override void OnSyncedStart()
    {
        cloakedChildrenRender = originalChildrenRender;
        base.OnSyncedStart();
    }
    public override void ActivatePower()
    {
        if ( && Cooldown <= 0)
        {
            while (duration > 0)
            {
                foreach (Renderer GO in cloakedChildrenRender)
                {
                    GO.material = cloakMaterial;
                }
            }
            for (int i = 0; i < cloakedChildrenRender.Length; i++)
            {
                cloakedChildrenRender[i].material = originalChildrenRender[i].material;
            }
        }
        Cooldown = 5;
        base.ActivatePower();
    }
}
