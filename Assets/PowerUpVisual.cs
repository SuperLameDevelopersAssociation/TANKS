using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PowerUpVisual : NetworkBehaviour
{
    public GameObject tankBubble;
    public Material healthMaterial;
    public Material damageMaterial;
    public Material shieldMaterial;
    public AudioClip powerUpActivateSFX;
    public AudioClip powerUpDeactivateSFX;

    [Command]
    public void CmdSetBubble(bool isActive, string powerupType)
    {
        tankBubble.SetActive(isActive);

        //Debug.LogError("Turn tank bubble : " + isActive + " with the type of " + powerupType);

        if (powerupType.Equals("Health"))
            tankBubble.GetComponent<MeshRenderer>().material = healthMaterial;
        else if (powerupType.Equals("Damage"))
            tankBubble.GetComponent<MeshRenderer>().material = damageMaterial;
        else if (powerupType.Equals("Shield"))
            tankBubble.GetComponent<MeshRenderer>().material = shieldMaterial;

        if (isActive)
            tankBubble.GetComponent<AudioSource>().clip = powerUpActivateSFX;
        else
            tankBubble.GetComponent<AudioSource>().clip = powerUpDeactivateSFX;


        tankBubble.GetComponent<AudioSource>().Play();

        RpcSetBubble(isActive, powerupType);
    }

    [ClientRpc]
    void RpcSetBubble(bool isActive, string powerupType)
    {
        tankBubble.SetActive(isActive);
        //Debug.LogError("Turn client tank bubble : " + isActive + " with the type of " + powerupType);

        if (powerupType.Equals("Health"))
            tankBubble.GetComponent<MeshRenderer>().material = healthMaterial;
        else if (powerupType.Equals("Damage"))
            tankBubble.GetComponent<MeshRenderer>().material = damageMaterial;
        else if (powerupType.Equals("Shield"))
            tankBubble.GetComponent<MeshRenderer>().material = shieldMaterial;

        //Debug.LogError(tankBubble.GetComponent<MeshRenderer>().material.name + " is the base material");

        if (isActive)
            tankBubble.GetComponent<AudioSource>().clip = powerUpActivateSFX;
        else
            tankBubble.GetComponent<AudioSource>().clip = powerUpDeactivateSFX;

        tankBubble.GetComponent<AudioSource>().Play();

    }


}
