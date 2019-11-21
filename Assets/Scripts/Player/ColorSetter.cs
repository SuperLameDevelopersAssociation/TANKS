using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ColorSetter : NetworkBehaviour
{
    public Color32 tankColor;
    Renderer[] originalChildrenRender;                       //Grab the current children render so they can be reset after ability is done
    Renderer[] cloakedChildrenRender;                        //copy of the current children renders so they can be changed to cloaking ability
    Material[] mats;                                         //gather all materials setup for the cloaking

    void Awake()
    {
        //SetColor(tankColor);
    }

    public void SetColor(Color32 tankColor)
    {
        this.tankColor = tankColor;

        originalChildrenRender = GetComponentsInChildren<Renderer>();
        mats = new Material[originalChildrenRender.Length];         //Set length of array
        cloakedChildrenRender = originalChildrenRender;             //Copy array to second array
        for (int i = 0; i < originalChildrenRender.Length; i++)
            mats[i] = originalChildrenRender[i].material;           //Set mats array to originalChildrenRender

        for (int i = 0; i < cloakedChildrenRender.Length; i++)
        {
            cloakedChildrenRender[i].material.SetColor("_Color", tankColor);                     //Change materials to cloaked material
        }

        CmdFindObjects();
        CmdServerColor();
    }


    [Command]
    void CmdFindObjects()
    {
        originalChildrenRender = GetComponentsInChildren<Renderer>();
        cloakedChildrenRender = originalChildrenRender;
        mats = new Material[originalChildrenRender.Length];
        for (int i = 0; i < originalChildrenRender.Length; i++)
            mats[i] = originalChildrenRender[i].material;
    }

    [Command]
    void CmdServerColor()
    {
        RpcSetColor();
    }

    [ClientRpc] //Activates power on all clients
    void RpcSetColor()
    {
        if (cloakedChildrenRender == null) return;
        for (int i = 0; i < cloakedChildrenRender.Length; i++)
        {
            cloakedChildrenRender[i].material.color = tankColor;                      //Change materials to cloaked material
        }
    }
}