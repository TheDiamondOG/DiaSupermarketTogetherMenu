using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using Steamworks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using System.Reflection;
using Random = System.Random;

namespace SupermarketTogetherKacker.menu
{
    internal class Mods : MonoBehaviour
    {
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
        
        public static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
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
    }
}
