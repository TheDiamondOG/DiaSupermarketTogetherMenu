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
        // Menus GameObject
        private GameObject menuObject = new GameObject();
    
        // Run some on start crap
        private void Awake()
        {
            // Make the object immortal
            DontDestroyOnLoad(menuObject);
            
            // Make some menu crap work
            menuObject.gameObject.AddComponent<Mods>();
            menuObject.gameObject.AddComponent<CoolGUI>();
            
            //HarmonyPatches.ApplyHarmonyPatches();
        }
    }
}