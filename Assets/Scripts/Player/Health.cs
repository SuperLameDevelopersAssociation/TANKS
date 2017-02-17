using UnityEngine;
using TrueSync;
using UnityEngine.UI;
using System.Collections;

public class Health : TrueSyncBehaviour
{
    public int maxHealth;
    [AddTracking]
    private int currHealth;

    private Text text;

    public byte deaths;

    //Weapons weapons

    public override void OnSyncedStart()
    {
        if (owner.Id == 0)
            text = GameObject.Find("TextP1").GetComponent<Text>();
        else if (owner.Id == 1)
            text = GameObject.Find("TextP1").GetComponent<Text>();
        else
            text = GameObject.Find("TextP2").GetComponent<Text>();

        text.text = "Player: " + owner.Id + " is using this.";
        currHealth = maxHealth;
        //Weapons = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Weapons>();
    }


    public bool TakeDamage(int damage) //, int playerID)
    {
        //int damage = 5; //find how much damage the weapons type is from Weapons
        currHealth -= damage;

        /*
            playerID--;
            int thisID = owner.Id;
            thisID--;
        */

        if (currHealth <= 0)
        {
            tsTransform.position = new TSVector(TSRandom.Range(-50, 50), 0, TSRandom.Range(-50, 50)); //respawn randomly
            deaths++;
            StartCoroutine(Death());
            return true;
            /*
            if (thisID != playerID)
            {
                deaths++;
                PhotonNetwork.playerList[playerID].AddScore(1);
                print("Death Has Occured: " + deaths);
                text.text = "Player: " + owner.Id + " score is " + PhotonNetwork.playerList[thisID].GetScore() + " and has died: " + deaths; //who you are  
                text.text += "\nPlayer: " + (playerID + 1) + " score is " + PhotonNetwork.playerList[playerID].GetScore();  //who shot you
            }
            

            //text.text = PhotonNetwork.playerList[playerID].GetScore() + ": the score for player with the ID of :" + playerID;
            //text.text += " Deaths: " + deaths;
            //print(PhotonNetwork.playerList[playerID].GetScore() + ": the score for player:" + playerID);
            //TrueSyncManager.Players[playerID];

            */

        }

        return false;
    }

    IEnumerator Death()
    {
        FP waitTime = .1;
        //send over information about how killed who
        yield return waitTime;
        currHealth = maxHealth;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 100 + 30 * owner.Id, 300, 30), "player: " + owner.Id + ", health: " + currHealth);
        //GUI.Label(new Rect(10, 140 + 30 * owner.Id, 300, 30), "Deaths: " + deaths + ", Kills: " + PhotonNetwork.playerList[owner.Id].GetScore());
    }
}
