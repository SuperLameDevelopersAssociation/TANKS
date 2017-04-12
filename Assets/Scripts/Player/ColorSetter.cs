using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ColorSetter : NetworkBehaviour
{
    public Color32 tankColor;
    public Renderer[] originalChildrenRender;                       //Grab the current children render so they can be reset after ability is done
    Renderer[] cloakedChildrenRender;                               //copy of the current children renders so they can be changed to cloaking ability
    public Material[] mats;                                         //gather all materials setup for the cloaking

    void Awake()
    {
        originalChildrenRender = GetComponentsInChildren<Renderer>();
        mats = new Material[originalChildrenRender.Length];         //Set length of array
        //_cooldown = cooldown;
        cloakedChildrenRender = originalChildrenRender;             //Copy array to second array
        for (int i = 0; i < originalChildrenRender.Length; i++)
            mats[i] = originalChildrenRender[i].material;           //Set mats array to originalChildrenRender

        for (int i = 0; i < cloakedChildrenRender.Length; i++)
        {
            cloakedChildrenRender[i].material.color = tankColor;                      //Change materials to cloaked material
        }

        CmdFindObjects();
        CmdServerColor();
    }

    void Start()
    {
        
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
        for (int i = 0; i < cloakedChildrenRender.Length; i++)
        {
            cloakedChildrenRender[i].material.color = tankColor;                      //Change materials to cloaked material
        }

        Debug.LogError("This has occured");
    }
}
