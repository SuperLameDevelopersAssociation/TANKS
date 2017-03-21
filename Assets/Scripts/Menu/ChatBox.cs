using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon;
using TrueSync;

public class ChatBox : PunBehaviour
{
    public GameObject chatPanel;
    public Text chatText;
    public InputField chatInput;

    private ScrollRect chatScroll;
    private bool isToggled;


    void Start()
    {
        this.chatScroll = this.chatPanel.transform.Find("ChatScroll").GetComponent<ScrollRect>();
    }

    void Update()
    {
        if (chatPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            MultiplayerPanel_ChatSend();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MultiplayerPanel_ChatToggle();
        }
    }

    public void MultiplayerPanel_ChatSend()
    {
        string text = this.chatInput.text;

        if (text != "")
        {
            this.chatInput.text = "";

            int indexPlayer = System.Array.IndexOf(PhotonNetwork.playerList, PhotonNetwork.player);
            MultiplayerPanel_ChatReceived(PhotonNetwork.playerName, text, indexPlayer);
            photonView.RPC("MultiplayerPanel_ChatReceived", PhotonTargets.Others, PhotonNetwork.playerName, text, indexPlayer);

            this.chatInput.ActivateInputField();
        }
    }

    [PunRPC]
    public void MultiplayerPanel_ChatReceived(string name, string text, int spawnIndex)
    {
        if (spawnIndex < 0)
        {
            spawnIndex = 0;
        }
        this.chatText.text += string.Format("{0}: {1}\n", name, text);
        chatPanel.SetActive(true);

        this.chatScroll.normalizedPosition = new Vector2(0, 0);
    }

    public void MultiplayerPanel_ChatToggle()
    {
        isToggled = !isToggled;

        chatPanel.SetActive(isToggled);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int index = 0; index < players.Length; index++)
        {
            PlayerMovement player = players[index].GetComponent<PlayerMovement>(); ;
            print("Get player " + player);

            if (player.owner == TrueSyncManager.LocalPlayer)
            {
                if (chatPanel.activeSelf)
                {
                    player.StopMovement();
                }
                else
                {
                    player.RestartMovement();
                }

                print("Player " + index + " is local");
            }
        }

        if (chatPanel.activeSelf)
        {
            this.chatInput.ActivateInputField();
            this.chatScroll.normalizedPosition = new Vector2(0, 0);
        }
    }
}
