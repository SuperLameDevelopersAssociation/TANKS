using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PowerUpVisual : NetworkBehaviour
{
    public AudioSource powerUpAudio;
    public GameObject healthBubble;
    public GameObject damageBubble;
    public GameObject shieldBubble;
    public AudioClip powerUpActivateSFX;
    public AudioClip powerUpDeactivateSFX;

    [Command]
    public void CmdSetBubble(bool isActive, string powerupType)
    {
        //Debug.LogError("Turn tank bubble : " + isActive + " with the type of " + powerupType);

        if (powerupType.Equals("Health"))
            healthBubble.SetActive(isActive);
        else if (powerupType.Equals("Damage"))
            damageBubble.SetActive(isActive);
        else if (powerupType.Equals("Shield"))
            shieldBubble.SetActive(isActive);

        if (isActive)
            powerUpAudio.clip = powerUpActivateSFX;
        else
            powerUpAudio.clip = powerUpDeactivateSFX;


        powerUpAudio.Play();

        RpcSetBubble(isActive, powerupType);
    }

    [ClientRpc]
    void RpcSetBubble(bool isActive, string powerupType)
    {
        if (powerupType.Equals("Health"))
            healthBubble.SetActive(isActive);
        else if (powerupType.Equals("Damage"))
            damageBubble.SetActive(isActive);
        else if (powerupType.Equals("Shield"))
            shieldBubble.SetActive(isActive);

        if (isActive)
            powerUpAudio.clip = powerUpActivateSFX;
        else
            powerUpAudio.clip = powerUpDeactivateSFX;


        powerUpAudio.Play();

    }


}
