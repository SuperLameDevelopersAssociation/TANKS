using UnityEngine;
using TrueSync;
using System.Collections;

public class Health : TrueSyncBehaviour
{
    public int maxHealth;
    [AddTracking]
    private int currHealth;

    public byte deaths;

    //Weapons weapons

    public override void OnSyncedStart()
    {
        currHealth = maxHealth;
        //Weapons = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Weapons>();
    }


    public void TakeDamage(int damage, int playerID)
    {
        //int damage = 5; //find how much damage the weapons type is from Weapons
        currHealth -= damage;

        if (currHealth <= 0)
        {
            tsTransform.position = new TSVector(TSRandom.Range(-50, 50), 0, TSRandom.Range(-50, 50)); //respawn randomly
            deaths++;
            PhotonNetwork.playerList[playerID].AddScore(1);
            print(PhotonNetwork.playerList[playerID].GetScore() + ": the score for player:" + playerID);
            //TrueSyncManager.Players[playerID];
            StartCoroutine(Death());
        }
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
    }
}
