using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

    public class MatchMaker : NetworkBehaviour
    {
        LobbyManager lobbyManager;

        void Start()
        {
            lobbyManager = GetComponentInParent<LobbyManager>();
            print(lobbyManager);
        }

        public void CheckLobbyMatchCount()
        {
            //for(int i = 0; i < lobbyManager.matches.Count; i++)
            //{
            //    print(i);
            //}
        }
    }

