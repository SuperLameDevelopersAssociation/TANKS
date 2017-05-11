using UnityEngine;
using UnityEngine.UI;
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
    public Text powerUpText;
    public float timeText = 1.5f;

    void Start()
    {
        powerUpText.gameObject.SetActive(false);
    }


    [Command]
    public void CmdSetBubble(bool isActive, string powerupType)
    {
        //Debug.LogError("Turn tank bubble : " + isActive + " with the type of " + powerupType);

        if (powerupType.Equals("Health"))
        {
            healthBubble.SetActive(isActive);

            if (isActive)
                StartCoroutine(DisplayText("Health Restored!"));
        }
        else if (powerupType.Equals("Damage"))
        {
            damageBubble.SetActive(isActive);

            if (isActive)
                StartCoroutine(DisplayText("Damage Boosted!"));
            else
                StartCoroutine(DisplayText("Boost Ended!"));
        }
        else if (powerupType.Equals("Shield"))
        {
            shieldBubble.SetActive(isActive);

            if (isActive)
                StartCoroutine(DisplayText("Shield Activated!"));
            else
                StartCoroutine(DisplayText("Shield Destroyed!"));
        }
            

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
        {
            healthBubble.SetActive(isActive);

            if (isActive)
                StartCoroutine(DisplayText("Health Restored!"));
        }
        else if (powerupType.Equals("Damage"))
        {
            damageBubble.SetActive(isActive);

            if (isActive)
                StartCoroutine(DisplayText("Damage Boosted!"));
            else
                StartCoroutine(DisplayText("Boost Ended!"));
        }
        else if (powerupType.Equals("Shield"))
        {
            shieldBubble.SetActive(isActive);

            if (isActive)
                StartCoroutine(DisplayText("Shield Activated!"));
            else
                StartCoroutine(DisplayText("Shield Destroyed!"));
        }

        if (isActive)
            powerUpAudio.clip = powerUpActivateSFX;
        else
            powerUpAudio.clip = powerUpDeactivateSFX;


        powerUpAudio.Play();

    }

    IEnumerator DisplayText(string message)
    {
        powerUpText.gameObject.SetActive(true);
        powerUpText.text = message;
        yield return new WaitForSeconds(timeText);
        powerUpText.gameObject.SetActive(false);
    }


}
