﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using Steamworks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using System.Reflection;
using System.Threading.Tasks;
using SupermarketTogetherKacker.tools;
using UnityEngine.Networking;
using Random = System.Random;

namespace SupermarketTogetherKacker.menu
{
    internal class Mods : MonoBehaviour
    {
        public static bool finishedUpdateCheck = false;
        public static bool upToDate = false;
        
        public static void MoveObject(GameObject gameObject, Vector3 position)
        {
            GameObject gameDataObject = GameObject.Find("GameDataManager");
            
            Quaternion rotation = Quaternion.identity;

            NetworkSpawner networkSpawner = gameDataObject.GetComponent<NetworkSpawner>();
            
            Type type = typeof(NetworkSpawner);

            MethodInfo privateMethod = type.GetMethod("CmdObjectMove", BindingFlags.NonPublic | BindingFlags.Instance);

            if (privateMethod != null)
            {
                object[] parameters = { gameObject, position, rotation };

                privateMethod.Invoke(networkSpawner, parameters);
            }
            else
            {
                Console.WriteLine("Method not found.");
            }
        }
        
        public static void DeleteObject(GameObject gameObject, bool safeDelete = true)
        {
            GameObject gameDataObject = GameObject.Find("GameDataManager");
  
            NetworkSpawner networkSpawner = gameDataObject.GetComponent<NetworkSpawner>();

            if (safeDelete)
            {
                if (!gameObject.name.Contains("GameDataManager"))
                {
                    networkSpawner.CmdDestroyBox(gameObject);
                }
            }
            else
            {
                networkSpawner.CmdDestroyBox(gameObject);
            }
        }

        public static PlayerNetwork[] GetAllPlayers()
        {
            return FindObjectsOfType<PlayerNetwork>();
        }

        public static void PushPlayer(PlayerNetwork player, Vector3 direction)
        {
            Type type = typeof(PlayerNetwork);
            
            MethodInfo privateMethod = type.GetMethod("CmdPushPlayer", BindingFlags.NonPublic | BindingFlags.Instance);

            if (privateMethod != null)
            {
                object[] parameters = { direction };
                
                privateMethod.Invoke(player, parameters);
            }
            else
            {
                Console.WriteLine("Method not found.");
            }
        }

        public static void SetPlayerName(string name)
        {
            GameObject localPlayer = GameObject.Find("LocalGamePlayer");
            
            PlayerObjectController playerObjectController = localPlayer.GetComponent<PlayerObjectController>();
            
            
            if (playerObjectController.PlayerName != name)
            {
                Type type = typeof(PlayerObjectController);
            
                MethodInfo privateMethod = type.GetMethod("CmdSetPlayerName", BindingFlags.NonPublic | BindingFlags.Instance);

                if (privateMethod != null)
                {
                    object[] parameters = { name };
                
                    privateMethod.Invoke(playerObjectController, parameters);
                    playerObjectController.PlayerNameUpdate(name, name);
                }
                else
                {
                    Console.WriteLine("Method not found.");
                }
            }
        } 
        
        ulong GenerateRandomUlong()
        {
            System.Random random = new System.Random();
            ulong upper = (ulong)random.Next(int.MinValue, int.MaxValue); // Upper 32 bits
            ulong lower = (ulong)random.Next(int.MinValue, int.MaxValue); // Lower 32 bits
            return (upper << 32) | lower;
        }
        
        public static string RandomString(int length)
        {
            Random rnd = new Random();
            
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        public static bool InLobby()
        {
            if (GameObject.Find("OnlineNetworkManager") != null || GameObject.Find("LocalNetworkManager") != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static GameObject GetPlayerObject()
        {
            GameObject playerObject = GameObject.Find("LocalGamePlayer");

            if (playerObject == null)
            {
                FirstPersonTransform playerTransform = FindAnyObjectByType<FirstPersonTransform>();
                
                playerObject = playerTransform.gameObject;
            }
            
            return playerObject;
        }
        
        public static bool CheckForUpdates()
        {
            try
            {
                using UnityWebRequest webRequest = UnityWebRequest.Get("https://raw.githubusercontent.com/TheDiamondOG/DiaSupermarketTogetherMenu/refs/heads/master/SupermarketTogetherKacker/version.txt");

                if (PluginInfo.Version == webRequest.downloadHandler.text)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}