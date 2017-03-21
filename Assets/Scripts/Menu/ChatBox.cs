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
            print("The name is " + PhotonNetwork.playerName);
            photonView.RPC("MultiplayerPanel_ChatReceived", PhotonTargets.Others, PhotonNetwork.playerName, text, indexPlayer);

            this.chatInput.ActivateInputField();
        }
    }

    [PunRPC]
    public void MultiplayerPanel_ChatReceived(string playerName, string text, int spawnIndex)
    {
        if (spawnIndex < 0)
        {
            spawnIndex = 0;
        }
        this.chatText.text += string.Format("{0}: {1}\n", playerName, text);
        this.chatScroll.normalizedPosition = new Vector2(0, 0);
    }

    public void MultiplayerPanel_ChatToggle()
    {
        chatPanel.SetActive(!chatPanel.activeSelf);

        if (chatPanel.activeSelf)
        {
            this.chatInput.ActivateInputField();
            this.chatScroll.normalizedPosition = new Vector2(0, 0);
        }
    }
}
