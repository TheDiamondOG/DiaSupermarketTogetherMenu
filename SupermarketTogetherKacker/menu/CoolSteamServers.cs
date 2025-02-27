using System.Collections.Generic;
using Steamworks;
using UnityEngine;
 
namespace SupermarketTogetherKacker.menu
{
    public class CoolSteamServers:MonoBehaviour
    {
        public static HashSet<CSteamID> knownLobbies = new HashSet<CSteamID>();

        private void Start()
        {
            if (!SteamAPI.Init())
            {
                return;
            }

            SteamMatchmaking.AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter.k_ELobbyDistanceFilterWorldwide);
            SteamMatchmaking.RequestLobbyList();

            Callback<LobbyMatchList_t>.Create(OnLobbyListReceived);
        }

        private void OnLobbyListReceived(LobbyMatchList_t result)
        {
            for (int i = 0; i < result.m_nLobbiesMatching; i++)
            {
                CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);

                if (!knownLobbies.Contains(lobbyID))
                {
                    knownLobbies.Add(lobbyID);
                }
            }
        }

        private void OnDestroy()
        {
            SteamAPI.Shutdown();
        }
    }
}