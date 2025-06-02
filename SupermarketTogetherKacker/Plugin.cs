using BepInEx;
using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using SupermarketTogetherKacker.menu;
using SupermarketTogetherKacker.tools;

namespace SupermarketTogetherKacker
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static GameObject menuObject = new GameObject();
        
        private void Awake()
        {
            menuObject.name = "DiaObj";
            
            DontDestroyOnLoad(menuObject);
            
            menuObject.gameObject.AddComponent<Mods>();
            menuObject.gameObject.AddComponent<CoolGUI>();
            menuObject.gameObject.AddComponent<CoolSteamServers>();
            menuObject.gameObject.AddComponent<Notify>();
            
            HarmonyPatches.ApplyHarmonyPatches();
        }
    }
}
