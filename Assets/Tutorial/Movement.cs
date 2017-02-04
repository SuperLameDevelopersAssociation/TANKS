using UnityEngine;
using TrueSync; //Using the truesync library

public class Movement : TrueSyncBehaviour //Inherit from trueSyncBehavior
{
    [AddTracking]
    public int deaths = 0;

    public override void OnSyncedStart()
    {
        tsTransform.position = new TSVector(TSRandom.Range(-5, 5), 0, TSRandom.Range(-5, 5));   //Spawns players in a random place at the start of the game
    }

    public override void OnSyncedInput()
    {
        print("Player: Syncing Input...");
        FP vertical = Input.GetAxis("Vertical");        //If we decide to ever use InControl for input management, we can simply check the controller in an update call and make it public
        FP horizontal = Input.GetAxis("Horizontal");    //So we can check that controller from this script and then replace the input here with InControls input.

        TrueSyncInput.SetFP(0, vertical);
        TrueSyncInput.SetFP(1, horizontal);
    }

    public override void OnSyncedUpdate()   //This is called by photon when all input each player for a specific frame is available
    {
        print("OnSyncedUpdate");
        FP vertical = TrueSyncInput.GetFP(0);           //We get out synced input values from the OnSyncedInput()
        FP horizontal = TrueSyncInput.GetFP(1);

        vertical *= 10 * TrueSyncManager.DeltaTime;     //We multiply the input values by TrueSync's custom Time.deltaTime variable.
        horizontal *= 250 * TrueSyncManager.DeltaTime;  //The 10 and 250 can be replaced with a speed variable.

        tsTransform.Translate(0, 0, vertical, Space.Self);  //We apply movement to the object.
        tsTransform.Rotate(0, horizontal, 0);               //Pay attention that we are not using regular transform.Translate but rather tsTransform.Translate. This is TrueSync's version of a transform component.
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 100 + 30 * owner.Id, 300, 30), "player: " + owner.Id + ", deaths: " + deaths);   //Display your death count
    }

    public void Respawn()
    {
        tsTransform.position = new TSVector(TSRandom.Range(-5, 5), 0, TSRandom.Range(-5, 5));       //Respawns the player
        deaths++;       //Increase the players death count
    }
}
