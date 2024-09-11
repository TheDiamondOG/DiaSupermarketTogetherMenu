using BepInEx;
//using SupermarketTogetherKacker.Patches;
using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Mirror;
using Mirror.Examples.Chat;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using SupermarketTogetherKacker.tools;
using Color = UnityEngine.Color;

namespace SupermarketTogetherKacker.menu
{
    public class CoolGUI:MonoBehaviour
    {
        // GUI Crap
        private Rect windowRect = new Rect(20, 20, 500, 450); // Initial position and size of the window
        private bool isDragging = false;
        private Vector2 dragStartPos;
        //public ItemManager itemManager;

        // All the mods
        private bool showGUI = true;
        private bool popSpammer = false;
        private float moneyAdd = 0f;
        private string moneyAddString;
        
        private int pointsAdd = 0;
        
        private int productID = 0;
        private string productIDString;
        private bool waterBoxSpammer = false;
        private bool perkSpam = false;
        private bool employeeSpam = false;
        private bool everyBoxSpam = false;
        private bool spamPushOthers = false;
        private bool moneySpam = false;
        private bool spamPush;
        private bool messageSpam = false;
        private string messageString = "";
        private string newSuperMarketName = "";
        private Color lastColor;
        private bool objectSpammer;
        private bool autoCheckout;
        private string newUsername;
        private bool coolHeckerButton;
        private bool disableOthersMovement;
        private bool disableMovement;
        private bool randomBoxSpam;
        private bool antiTheft;
        private bool spamHitNPCs;
        
        // Define categories
        public enum ModCategory
        {
            Market,
            Stats,
            Map,
            Server,
            Extras,
            NPC,
            Debug,
            // Add more categories here
        }

        public ModCategory currentCategory = ModCategory.Market;
        private Vector2 categoryScrollPos; // Scroll position for categories

        void OnGUI()
        {
            GUI.color = UnityEngine.Color.gray;
            if (showGUI)
            {
                // Draw the window
                windowRect = GUI.Window(0, windowRect, WindowFunction, "Dia Mods");
            }
        }

        void WindowFunction(int windowID)
        {
            // Make the window draggable only when clicking on the title bar
            Rect titleBarRect = new Rect(0, 0, windowRect.width, 20);
            GUI.DragWindow(titleBarRect);

            // Check if the mouse is within the title bar area
            if (titleBarRect.Contains(Event.current.mousePosition))
            {
                // Handle dragging
                if (Event.current.type == EventType.MouseDown)
                {
                    isDragging = true;
                    dragStartPos = Event.current.mousePosition - windowRect.position;
                    Event.current.Use();
                }
                else if (Event.current.type == EventType.MouseUp)
                {
                    isDragging = false;
                    Event.current.Use();
                }
            }

            if (isDragging)
            {
                // Update the window's position while dragging
                windowRect.position = Event.current.mousePosition - dragStartPos;
            }

            // Display category buttons with scrolling
            categoryScrollPos = GUILayout.BeginScrollView(categoryScrollPos, GUILayout.Width(100));
            GUILayout.BeginVertical();
            foreach (ModCategory category in Enum.GetValues(typeof(ModCategory)))
            {
                if (GUILayout.Toggle(category == currentCategory, category.ToString(), GUI.skin.button))
                {
                    currentCategory = category;
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();

            // Display mods based on the current category
            switch (currentCategory)
            {
                case ModCategory.Market:
                    DisplayMarketMods();
                    break;
                case ModCategory.Stats:
                    DisplayStatsMods();
                    break;
                case ModCategory.Map:
                    DisplayMapMods();
                    break;
                case ModCategory.Server:
                    DisplayServerMods();
                    break;
                case ModCategory.Extras:
                    DisplayExtraMods();
                    break;
                case ModCategory.NPC:
                    DisplayNPCMods();
                    break;
                case ModCategory.Debug:
                    DisplayDebugMods();
                    break;
            }
        }

        void DisplayMarketMods()
        {
            if (GUI.Button(new Rect(120, 20, 140, 20), "Unlimited Customers"))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameData gameData = gameDataManager.GetComponent<GameData>();
                
                gameData.maxCustomersNPCs = 1000000000;
            }
            
            if (GUI.Button(new Rect(120, 50, 140, 20), "Open Market"))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameData gameData = gameDataManager.GetComponent<GameData>();
                
                gameData.CmdOpenSupermarket();
            }
            if (GUI.Button(new Rect(120, 80, 140, 20), "Close Market"))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameData gameData = gameDataManager.GetComponent<GameData>();
                
                gameData.NetworktimeOfDay = 23f;
                gameData.SaveOBJ.GetComponent<PlayMakerFSM>().SendEvent("Send_Data");
                gameData.CmdEndDayFromButton();
            }
            if (GUI.Button(new Rect(120, 110, 140, 20), "Free Expansion"))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();
                
                GameData gameData = gameDataManager.GetComponent<GameData>();
                
                if (6750 != upgradesManager.spacePrice)
                {
                    gameData.CmdAlterFundsWithoutExperience(upgradesManager.spacePrice);
                }
                
                upgradesManager.CmdAddSpace();
            }
            if (GUI.Button(new Rect(120, 130, 140, 20), "Free Storage"))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();

                GameData gameData = gameDataManager.GetComponent<GameData>();

                if (6000 != upgradesManager.storagePrice)
                {
                    gameData.CmdAlterFundsWithoutExperience(upgradesManager.storagePrice);
                }
                
                upgradesManager.CmdAddStorage();
            }
            waterBoxSpammer = GUI.Toggle(new Rect(120, 150, 140, 20), waterBoxSpammer, "Lots of Water");
            
            productIDString = GUI.TextField(new Rect(120+120+10, 170, 120, 20), productIDString);

            try
            {
                productID = int.Parse(productIDString);
            }
            catch (Exception)
            {
                productID = 1;
            }
            
            if (GUI.Button(new Rect(120, 170, 120, 20), "Spawn with ID"))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();

                GameObject playerObject = GameObject.Find("LocalGamePlayer");

                Vector3 playerPosition = playerObject.transform.position;

                Vector3 spawnPosition = new Vector3(playerPosition.x + 2f, playerPosition.y, playerPosition.z);
                
                managerBlackboard.CmdSpawnBoxFromPlayer(spawnPosition, productID, 999999999, 1f);
            }

            everyBoxSpam = GUI.Toggle(new Rect(120, 190, 140, 20), everyBoxSpam, "Lots of everything");

            if (GUI.Button(new Rect(120, 220, 120, 20), "Everyone Theft"))
            {
                // Find all objects with the PlayerNetwork component
                NPC_Info[] allNPCs = FindObjectsOfType<NPC_Info>();

                foreach (NPC_Info npc in allNPCs)
                {
                    npc.RpcShowThief();
                }
            }
        }

        void DisplayStatsMods()
        {
            string moneyAddStringDisplay = "";
            
            moneyAddString = GUI.TextField(new Rect(120+120+10, 20, 120, 20), moneyAddString);

            try
            {
                moneyAdd = float.Parse(moneyAddString);
                if (moneyAdd >= 0)
                {
                    moneyAddStringDisplay = "+" + moneyAdd + "$";
                }
                else
                {
                    moneyAddStringDisplay = moneyAdd + "$";
                }
            }
            catch (Exception)
            {
                moneyAddStringDisplay = "+10000";
                moneyAdd = 10000f;
            }
            
            if (GUI.Button(new Rect(120, 20, 120, 20), moneyAddStringDisplay))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameData gameData = gameDataManager.GetComponent<GameData>();
                
                gameData.CmdAlterFundsWithoutExperience(moneyAdd);
            }

            moneySpam = GUI.Toggle(new Rect(120, 40, 120, 20), moneySpam, moneyAddStringDisplay);
            
            try
            {
                pointsAdd = int.Parse(moneyAddString);
                if (pointsAdd >= 0)
                {
                    moneyAddStringDisplay = "+" + pointsAdd;
                }
                else
                {
                    moneyAddStringDisplay = pointsAdd+"";
                }
            }
            catch (Exception)
            {
                moneyAddStringDisplay = "+10";
                pointsAdd = 10;
            }
            
            if (GUI.Button(new Rect(120, 70, 120, 20), moneyAddStringDisplay+" Points (NW)"))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameData gameData = gameDataManager.GetComponent<GameData>();
                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();
                NetworkSpawner networkSpawner = gameDataManager.GetComponent<NetworkSpawner>();
                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();
                ProductListing productListing = gameDataManager.GetComponent<ProductListing>();

                gameData.NetworkgameFranchisePoints += pointsAdd;
            }
        }
        
        void DisplayMapMods()
        {
            if (GUI.Button(new Rect(120, 20, 140, 20), "Disable Barrier"))
            {
                GameObject worldBarriers = GameObject.Find("Level_Exterior/Colliders");
                
                worldBarriers.SetActive(false);
            }
            if (GUI.Button(new Rect(120, 50, 140, 20), "Become Cool"))
            {
                GameObject worldBarriers = GameObject.Find("TheCoolRoom/AddonCollider");
                
                GameObject posesAndDancesText = GameObject.Find("TheCoolRoom/Canvas_Skins/Container/PosesAndDancesText");
                
                GameObject characterNumber = GameObject.Find("TheCoolRoom/Canvas_Skins/Container/CharacterNumber");
                
                GameObject hatNumber = GameObject.Find("TheCoolRoom/Canvas_Skins/Container/HatNumber");
                
                GameObject poses = GameObject.Find("TheCoolRoom/Canvas_Skins/Container/Poses");
                
                GameObject tvController = GameObject.Find("TheCoolRoom/Canvas_TVsHook/Container/");
                
                if (posesAndDancesText != null && !posesAndDancesText.activeSelf)
                {
                    posesAndDancesText.SetActive(true);
                }
                
                if (characterNumber != null && !characterNumber.activeSelf)
                {
                    characterNumber.SetActive(true);
                }
                
                if (hatNumber != null && !hatNumber.activeSelf)
                {
                    hatNumber.SetActive(true);
                }
                
                if (poses != null && !poses.activeSelf)
                {
                    poses.SetActive(true);
                }
                
                if (tvController != null && !tvController.activeSelf)
                {
                    tvController.SetActive(true);
                }
                
                worldBarriers.SetActive(false);
            }
            if (GUI.Button(new Rect(120, 80, 140, 20), "No Jail"))
            {
                GameObject worldBarriers = GameObject.Find("Level_Addons/Jail");
                
                worldBarriers.SetActive(false);
            }
        }

        void DisplayServerMods()
        {
            if (GUI.Button(new Rect(120, 20, 140, 20), "Max Level"))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();

                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();

                GameData gameData = gameDataManager.GetComponent<GameData>();

                // Get the type of the GameData class
                Type type = typeof(GameData);

                // Use reflection to get the private method
                MethodInfo privateMethod = type.GetMethod("CalculateFranchiseLevel",
                    BindingFlags.NonPublic | BindingFlags.Instance);

                if (privateMethod != null)
                {
                    // Provide arguments for the method (oldExp, newExp)
                    object[] parameters = { 0, 999999999 };

                    // Invoke the private method
                    privateMethod.Invoke(gameData, parameters);
                }
                else
                {
                    Console.WriteLine("Method not found.");
                }
            }

            if (GUI.Button(new Rect(120, 50, 140, 20), "Add Random Perks"))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();

                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();

                GameData gameData = gameDataManager.GetComponent<GameData>();

                for (int i = 0; i < 10; i++)
                {
                    upgradesManager.CmdAcquirePerk(i);
                }
            }

            perkSpam = GUI.Toggle(new Rect(120, 70, 140, 20), perkSpam, "Perk Spammer");
            if (GUI.Button(new Rect(120, 100, 140, 20), "Add Employee"))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();

                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();

                GameData gameData = gameDataManager.GetComponent<GameData>();

                upgradesManager.CmdAcquirePerk(1);
            }

            employeeSpam = GUI.Toggle(new Rect(120, 120, 140, 20), employeeSpam, "Employee Spammer");

            if (GUI.Button(new Rect(120, 150, 140, 20), "Push Others"))
            {
                // Find all objects with the PlayerNetwork component
                PlayerNetwork[] allPlayers = FindObjectsOfType<PlayerNetwork>();

                List<PlayerNetwork> otherPlayers = new List<PlayerNetwork>();

                foreach (PlayerNetwork player in allPlayers)
                {
                    // Assuming LocalPlayer is tagged or identified somehow
                    if (!player.isLocalPlayer) // Use your condition to exclude the LocalPlayer
                    {
                        // Get the type of the GameData class
                        Type type = typeof(PlayerNetwork);

                        // Use reflection to get the private method
                        MethodInfo privateMethod = type.GetMethod("CmdPushPlayer",
                            BindingFlags.NonPublic | BindingFlags.Instance);

                        if (privateMethod != null)
                        {
                            Vector3 pushDirection = new Vector3(100, 100, 100);

                            // Provide arguments for the method (oldExp, newExp)
                            object[] parameters = { pushDirection };

                            // Invoke the private method
                            privateMethod.Invoke(player, parameters);
                        }
                        else
                        {
                            Console.WriteLine("Method not found.");
                        }
                    }
                }
            }

            spamPushOthers = GUI.Toggle(new Rect(120, 170, 140, 20), spamPushOthers, "Spam Push Others");

            if (GUI.Button(new Rect(120, 200, 140, 20), "Push Everyone"))
            {
                // Find all objects with the PlayerNetwork component
                PlayerNetwork[] allPlayers = FindObjectsOfType<PlayerNetwork>();

                List<PlayerNetwork> otherPlayers = new List<PlayerNetwork>();

                foreach (PlayerNetwork player in allPlayers)
                {
                    // Get the type of the GameData class
                    Type type = typeof(PlayerNetwork);

                    // Use reflection to get the private method
                    MethodInfo privateMethod =
                        type.GetMethod("CmdPushPlayer", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (privateMethod != null)
                    {
                        Vector3 pushDirection = new Vector3(100, 100, 100);

                        // Provide arguments for the method (oldExp, newExp)
                        object[] parameters = { pushDirection };

                        // Invoke the private method
                        privateMethod.Invoke(player, parameters);
                    }
                    else
                    {
                        Console.WriteLine("Method not found.");
                    }
                }

                objectSpammer = GUI.Toggle(new Rect(120, 220, 140, 20), objectSpammer, "Cube Spam");
            }

            spamPush = GUI.Toggle(new Rect(120, 220, 140, 20), spamPush, "Spam Push");
            messageString = GUI.TextField(new Rect(140 + 120 + 10, 240, 120, 20), messageString);
            messageSpam = GUI.Toggle(new Rect(120, 240, 140, 20), messageSpam, "Spam Message");

            if (GUI.Button(new Rect(120, 270, 140, 20), "Bright Sign"))
            {
                GameObject gameDataObject = GameObject.Find("GameDataManager");

                NetworkSpawner networkSpawner = gameDataObject.GetComponent<NetworkSpawner>();

                Color brightColor = new Color(255, 255, 255, 255);

                Color test;

                if (networkSpawner.SuperMarketColor != brightColor)
                {
                    lastColor = networkSpawner.SuperMarketColor;
                    test = new Color(255, 255, 255, 255);
                }
                else
                {
                    test = lastColor;
                }

                networkSpawner.CmdSetSupermarketColor(test);
            }

            newSuperMarketName = GUI.TextField(new Rect(140 + 120 + 10, 300, 120, 20), newSuperMarketName);
            if (GUI.Button(new Rect(120, 300, 140, 20), "Supermarket Name"))
            {
                GameObject gameDataObject = GameObject.Find("GameDataManager");

                NetworkSpawner networkSpawner = gameDataObject.GetComponent<NetworkSpawner>();

                networkSpawner.CmdSetSupermarketText(newSuperMarketName);
            }

            if (GUI.Button(new Rect(120, 330, 140, 20), "Max Boxes"))
            {
                // Find the GameObject
                GameObject gameDataObject = GameObject.Find("GameDataManager");

                if (gameDataObject == null)
                {
                    Debug.LogError("GameDataManager not found!");
                    return;
                }

                NetworkSpawner networkSpawner = gameDataObject.GetComponent<NetworkSpawner>();

                if (networkSpawner == null)
                {
                    Debug.LogError("NetworkSpawner component not found!");
                    return;
                }

                BoxData[] allBoxes = FindObjectsOfType<BoxData>();

                foreach (BoxData box in allBoxes)
                {
                    // Set public property
                    box.numberOfProducts = 999999999;

                    // Get the type of the BoxData class
                    Type type = typeof(BoxData);

                    // Use reflection to get the private method
                    MethodInfo privateMethod =
                        type.GetMethod("SetBoxData", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (privateMethod != null)
                    {
                        try
                        {
                            // Check if the method requires parameters
                            ParameterInfo[] parameters = privateMethod.GetParameters();
                            if (parameters.Length == 0)
                            {
                                // Invoke the private method without parameters
                                privateMethod.Invoke(box, null);
                            }
                            else
                            {
                                Debug.LogWarning("The method requires parameters. Unable to invoke.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"Exception occurred while invoking the method: {ex.Message}");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Method 'SetBoxData' not found.");
                    }
                }
            }
            
            if (GUI.Button(new Rect(120, 360, 140, 20), "Water Infection"))
            {
                // Find the GameObject
                GameObject gameDataObject = GameObject.Find("GameDataManager");

                if (gameDataObject == null)
                {
                    Debug.LogError("GameDataManager not found!");
                    return;
                }

                NetworkSpawner networkSpawner = gameDataObject.GetComponent<NetworkSpawner>();

                if (networkSpawner == null)
                {
                    Debug.LogError("NetworkSpawner component not found!");
                    return;
                }

                BoxData[] allBoxes = FindObjectsOfType<BoxData>();

                foreach (BoxData box in allBoxes)
                {
                    // Set public property
                    box.numberOfProducts = 999999999;
                    box.productID = 1;

                    // Get the type of the BoxData class
                    Type type = typeof(BoxData);

                    // Use reflection to get the private method
                    MethodInfo privateMethod =
                        type.GetMethod("SetBoxData", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (privateMethod != null)
                    {
                        try
                        {
                            // Check if the method requires parameters
                            ParameterInfo[] parameters = privateMethod.GetParameters();
                            if (parameters.Length == 0)
                            {
                                // Invoke the private method without parameters
                                privateMethod.Invoke(box, null);
                            }
                            else
                            {
                                Debug.LogWarning("The method requires parameters. Unable to invoke.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"Exception occurred while invoking the method: {ex.Message}");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Method 'SetBoxData' not found.");
                    }
                }
            }
            
            if (GUI.Button(new Rect(280, 20, 140, 20), "No Product?"))
            {
                // Find the GameObject
                GameObject gameDataObject = GameObject.Find("GameDataManager");

                if (gameDataObject == null)
                {
                    Debug.LogError("GameDataManager not found!");
                    return;
                }

                NetworkSpawner networkSpawner = gameDataObject.GetComponent<NetworkSpawner>();

                if (networkSpawner == null)
                {
                    Debug.LogError("NetworkSpawner component not found!");
                    return;
                }

                BoxData[] allBoxes = FindObjectsOfType<BoxData>();

                foreach (BoxData box in allBoxes)
                {
                    // Set public property
                    box.numberOfProducts = 0;

                    // Get the type of the BoxData class
                    Type type = typeof(BoxData);

                    // Use reflection to get the private method
                    MethodInfo privateMethod =
                        type.GetMethod("SetBoxData", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (privateMethod != null)
                    {
                        try
                        {
                            // Check if the method requires parameters
                            ParameterInfo[] parameters = privateMethod.GetParameters();
                            if (parameters.Length == 0)
                            {
                                // Invoke the private method without parameters
                                privateMethod.Invoke(box, null);
                            }
                            else
                            {
                                Debug.LogWarning("The method requires parameters. Unable to invoke.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"Exception occurred while invoking the method: {ex.Message}");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Method 'SetBoxData' not found.");
                    }
                }
            }
            
            disableOthersMovement = GUI.Toggle(new Rect(280, 40, 160, 20), disableOthersMovement, "Disables Others Movement");
            disableMovement = GUI.Toggle(new Rect(280, 60, 160, 20), disableMovement, "Disables Movement");
            randomBoxSpam = GUI.Toggle(new Rect(280, 80, 160, 20), randomBoxSpam, "Water Everywhere");
        }
        
        void DisplayExtraMods()
        {
            popSpammer = GUI.Toggle(new Rect(120, 20, 140, 20), popSpammer, "Price Spammer");

            if (GUI.Button(new Rect(120, 50, 140, 20), "No Tutorial"))
            {
                GameObject tutorialObject = GameObject.Find("GameCanvas/Tutorials");

                tutorialObject.SetActive(false);
            }
            
            if (GUI.Button(new Rect(120, 80, 140, 20), "Scan All"))
            {
                // Find all objects with the PlayerNetwork component
                ProductCheckoutSpawn[] allProducts = FindObjectsOfType<ProductCheckoutSpawn>();

                foreach (ProductCheckoutSpawn product in allProducts)
                {
                    product.CmdAddProductValueToCheckout();
                }
            }
            
            if (GUI.Button(new Rect(120, 110, 140, 20), "Auto Checkout"))
            {
                // Find all objects with the PlayerNetwork component
                ProductCheckoutSpawn[] allProducts = FindObjectsOfType<ProductCheckoutSpawn>();
                
                foreach (ProductCheckoutSpawn product in allProducts)
                {
                    product.CmdAddProductValueToCheckout();
                }
                
                // Find all objects with the PlayerNetwork component
                Data_Container[] allCheckouts = FindObjectsOfType<Data_Container>();
                
                foreach (Data_Container checkout in allCheckouts)
                {
                    //checkout.CmdActivateCashMethod(checkout.);
                    //checkout.CmdActivateCreditCardMethod();
                    if (checkout.productsLeft == 0 && checkout.currentNPC != null)
                    {
                        //checkout.CmdActivateCreditCardMethod();
                        checkout.CmdReceivePayment(checkout.currentAmountToReturn);
                    }
                }
            }
            
            autoCheckout = GUI.Toggle(new Rect(120, 140, 140, 20), autoCheckout, "Break Checkouts");
            
            newUsername = GUI.TextField(new Rect(120+140+10, 170, 120, 20), newUsername);
            
            if (GUI.Button(new Rect(120, 170, 140, 20), "Set Name"))
            {
                // Find the GameObject
                GameObject localPlayer = GameObject.Find("LocalGamePlayer");

                PlayerObjectController playerObjectController = localPlayer.GetComponent<PlayerObjectController>();

                // Get the type of the GameData class
                Type type = typeof(PlayerObjectController);
                    
                // Use reflection to get the private method
                MethodInfo privateMethod = type.GetMethod("CmdSetPlayerName", BindingFlags.NonPublic | BindingFlags.Instance);

                if (privateMethod != null)
                {
                    // Provide arguments for the method (oldExp, newExp)
                    object[] parameters = { newUsername };

                    // Invoke the private method
                    privateMethod.Invoke(playerObjectController, parameters);
                    playerObjectController.PlayerNameUpdate("jeff", newUsername);
                }
                else
                {
                    Console.WriteLine("Method not found.");
                }
            }
            
            coolHeckerButton = GUI.Toggle(new Rect(120, 190, 140, 20), coolHeckerButton, "Cool Hecker");

            if (GUI.Button(new Rect(120, 220, 140, 20), "Grab all Stolen"))
            {
                StolenProductSpawn[] allCheckouts = FindObjectsOfType<StolenProductSpawn>();
                
                foreach (StolenProductSpawn checkout in allCheckouts)
                {
                    checkout.CmdRecoverStolenProduct();
                }
            }
        }
        
        void DisplayNPCMods()
        {
            if (GUI.Button(new Rect(120, 20, 160, 20), "Everyone Theft (Master)"))
            {
                // Find all objects with the PlayerNetwork component
                NPC_Info[] allNPCs = FindObjectsOfType<NPC_Info>();

                foreach (NPC_Info npc in allNPCs)
                {
                    npc.RpcShowThief();
                }
            }
            spamHitNPCs = GUI.Toggle(new Rect(140+140+20, 50, 140, 20), spamHitNPCs, "Spam Hit Everyone");
            if (GUI.Button(new Rect(120, 50, 160, 20), "Hit Everyone"))
            {
                // Find all objects with the PlayerNetwork component
                NPC_Info[] allNPCs = FindObjectsOfType<NPC_Info>();

                foreach (NPC_Info npc in allNPCs)
                {
                    npc.CmdAnimationPlay(0);
                }
            }
            
            if (GUI.Button(new Rect(120, 80, 160, 20), "Constant Complaints (Master)"))
            {
                // Find all objects with the PlayerNetwork component
                NPC_Info[] allNPCs = FindObjectsOfType<NPC_Info>();

                foreach (NPC_Info npc in allNPCs)
                {
                    npc.ComplainAboutFilth();
                }
            }
            
            if (GUI.Button(new Rect(120, 110, 160, 20), "Astronauts (Master)"))
            {
                // Find all objects with the PlayerNetwork component
                NPC_Info[] allNPCs = FindObjectsOfType<NPC_Info>();

                foreach (NPC_Info npc in allNPCs)
                {
                    npc.ChangeEmployeeHat(76);
                    npc.RPCChangeEmployeeHat(76);
                }
            }

            antiTheft = GUI.Toggle(new Rect(120, 130, 140, 20), antiTheft, "Anti Theft (NW)");
        }
        
        void DisplayDebugMods()
        {
            if (GUI.Button(new Rect(120, 20, 140, 20), "Dump Prefabs"))
            {
                Filestuff filestuff = new Filestuff();
                
                // Load all prefabs in the Resources folder
                var prefabs = Resources.LoadAll<GameObject>("");
                
                filestuff.WriteToFile("prefab_dump.txt", "      Start of dump      \n---------------------");
                
                foreach (var prefab in prefabs)
                {
                    if (prefab != null)
                    {
                        filestuff.AppendFile("prefab_dump.txt", "\nName: "+ prefab.name+"\nInstance ID: "+prefab.GetInstanceID()+"\n--------------------------------");
                    }
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
        
        void Update()
        {
            // Keybinds
            if (Input.GetKeyDown(KeyCode.F1))
            {
                showGUI = !showGUI;
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                if (!Cursor.visible) // If cursor is currently invisible
                {
                    Cursor.visible = true; // Make cursor visible
                    Cursor.lockState = CursorLockMode.None; // Unlock cursor
                }
                else // If cursor is currently visible
                {
                    Cursor.visible = false; // Hide cursor
                    Cursor.lockState = CursorLockMode.Locked; // Lock cursor
                }
            }

            if (popSpammer)
            {
                GameObject worldBarriers = GameObject.Find("LocalGamePlayer");

                PlayerNetwork player = worldBarriers.GetComponent<PlayerNetwork>();
                
                player.CmdPlayPricingSound();
            }

            if (waterBoxSpammer)
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();
                
                /*
                managerBlackboard.AddShoppingListProduct(1, 0.0000000000000000000001f);
                managerBlackboard.BuyCargo();
                */

                GameObject playerObject = GameObject.Find("LocalGamePlayer");

                Vector3 playerPosition = playerObject.transform.position;

                Vector3 spawnPosition = new Vector3(playerPosition.x + 2f, playerPosition.y, playerPosition.z);
                
                managerBlackboard.CmdSpawnBoxFromPlayer(spawnPosition, 1, 999999999, 1f);
            }

            if (perkSpam)
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();

                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();

                GameData gameData = gameDataManager.GetComponent<GameData>();
                
                for (int i = 0; i < 10; i++)
                {
                    upgradesManager.CmdAcquirePerk(i);
                }
            }

            if (employeeSpam)
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();

                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();

                GameData gameData = gameDataManager.GetComponent<GameData>();

                upgradesManager.CmdAcquirePerk(1);
            }
            if (everyBoxSpam)
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();
                
                /*
                managerBlackboard.AddShoppingListProduct(1, 0.0000000000000000000001f);
                managerBlackboard.BuyCargo();
                */

                GameObject playerObject = GameObject.Find("LocalGamePlayer");

                Vector3 playerPosition = playerObject.transform.position;

                Vector3 spawnPosition = new Vector3(playerPosition.x + 2f, playerPosition.y, playerPosition.z);
                
                for (int i = 0; i < 175; i++)
                    if (everyBoxSpam)
                    {
                        managerBlackboard.CmdSpawnBoxFromPlayer(spawnPosition, i, 999999999, 1f);
                    }
                    else
                    {
                        break;
                    }
            }

            if (spamPushOthers)
            {
                // Find all objects with the PlayerNetwork component
                PlayerNetwork[] allPlayers = FindObjectsOfType<PlayerNetwork>();

                List<PlayerNetwork> otherPlayers = new List<PlayerNetwork>();

                foreach (PlayerNetwork player in allPlayers)
                {
                    // Assuming LocalPlayer is tagged or identified somehow
                    if (!player.isLocalPlayer) // Use your condition to exclude the LocalPlayer
                    {
                        // Get the type of the GameData class
                        Type type = typeof(PlayerNetwork);

                        // Use reflection to get the private method
                        MethodInfo privateMethod = type.GetMethod("CmdPushPlayer", BindingFlags.NonPublic | BindingFlags.Instance);

                        if (privateMethod != null)
                        {
                            Vector3 pushDirection = new Vector3(100, 100, 100);
                            
                            // Provide arguments for the method (oldExp, newExp)
                            object[] parameters = { pushDirection };

                            // Invoke the private method
                            privateMethod.Invoke(player, parameters);
                        }
                        else
                        {
                            Console.WriteLine("Method not found.");
                        }
                    }
                }
            }
            if (spamPush)
            {
                // Find all objects with the PlayerNetwork component
                PlayerNetwork[] allPlayers = FindObjectsOfType<PlayerNetwork>();

                List<PlayerNetwork> otherPlayers = new List<PlayerNetwork>();

                foreach (PlayerNetwork player in allPlayers)
                {
                    // Get the type of the GameData class
                    Type type = typeof(PlayerNetwork);
                    
                    // Use reflection to get the private method
                    MethodInfo privateMethod = type.GetMethod("CmdPushPlayer", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (privateMethod != null)
                    {
                        Vector3 pushDirection = new Vector3(100, 100, 100);
                            
                        // Provide arguments for the method (oldExp, newExp)
                        object[] parameters = { pushDirection };

                        // Invoke the private method
                        privateMethod.Invoke(player, parameters);
                    }
                    else
                    {
                        Console.WriteLine("Method not found.");
                    }
                }
            }

            if (moneySpam)
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameData gameData = gameDataManager.GetComponent<GameData>();
                
                gameData.CmdAlterFundsWithoutExperience(moneyAdd);
            }

            if (messageSpam)
            {
                // Find the GameObject
                GameObject localPlayer = GameObject.Find("LocalGamePlayer");

                PlayerObjectController playerObjectController = localPlayer.GetComponent<PlayerObjectController>();

                playerObjectController.SendChatMsg(messageString);
            }

            if (objectSpammer)
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameData gameData = gameDataManager.GetComponent<GameData>();
                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();
                NetworkSpawner networkSpawner = gameDataManager.GetComponent<NetworkSpawner>();
                
                //networkSpawner.CmdSpawn();
            }

            if (autoCheckout)
            {
                // Find all objects with the PlayerNetwork component
                ProductCheckoutSpawn[] allProducts = FindObjectsOfType<ProductCheckoutSpawn>();
                
                foreach (ProductCheckoutSpawn product in allProducts)
                {
                    product.CmdAddProductValueToCheckout();
                }
                
                // Find all objects with the PlayerNetwork component
                Data_Container[] allCheckouts = FindObjectsOfType<Data_Container>();
                
                foreach (Data_Container checkout in allCheckouts)
                {
                    //checkout.CmdActivateCashMethod(checkout.);
                    //checkout.CmdActivateCreditCardMethod();
                    if (checkout.productsLeft == 0 && checkout.currentNPC != null)
                    {
                        //checkout.CmdActivateCreditCardMethod();
                        checkout.CmdReceivePayment(checkout.currentAmountToReturn);
                    }
                }
            }

            if (coolHeckerButton)
            {
                // Find the GameObject
                GameObject localPlayer = GameObject.Find("LocalGamePlayer");
                
                PlayerNetwork[] allPlayers = FindObjectsOfType<PlayerNetwork>();
                
                PlayerObjectController playerObjectController = localPlayer.GetComponent<PlayerObjectController>();
                
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameData gameData = gameDataManager.GetComponent<GameData>();
                NetworkSpawner networkSpawner = gameDataManager.GetComponent<NetworkSpawner>();
                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();
                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();

                if (playerObjectController.PlayerName != "HACKED")
                {
                    // Get the type of the GameData class
                    Type type = typeof(PlayerObjectController);
                    
                    // Use reflection to get the private method
                    MethodInfo privateMethod = type.GetMethod("CmdSetPlayerName", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (privateMethod != null)
                    {
                        // Provide arguments for the method (oldExp, newExp)
                        object[] parameters = { "DIA ON TOP" };

                        // Invoke the private method
                        privateMethod.Invoke(playerObjectController, parameters);
                        playerObjectController.PlayerNameUpdate("jeff", "DIA ON TOP");
                    }
                    else
                    {
                        Console.WriteLine("Method not found.");
                    }
                }
                
                foreach (PlayerNetwork player in allPlayers)
                {
                    Vector3 playerPosition = player.gameObject.transform.position;

                    Vector3 spawnPosition = new Vector3(playerPosition.x + 2f, playerPosition.y, playerPosition.z);
                
                    managerBlackboard.CmdSpawnBoxFromPlayer(spawnPosition, 1, 999999999, 1f);
                    
                    // Get the type of the GameData class
                    Type type = typeof(PlayerNetwork);
                    
                    // Use reflection to get the private method
                    MethodInfo privateMethod = type.GetMethod("CmdPushPlayer", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (privateMethod != null)
                    {
                        Vector3 pushDirection = new Vector3(0, 0, 0);
                            
                        // Provide arguments for the method (oldExp, newExp)
                        object[] parameters = { pushDirection };

                        // Invoke the private method
                        privateMethod.Invoke(player, parameters);
                    }
                    else
                    {
                        Console.WriteLine("Method not found.");
                    }
                    
                    // Find all objects with the PlayerNetwork component
                    NPC_Info[] allNPCs = FindObjectsOfType<NPC_Info>();

                    foreach (NPC_Info npc in allNPCs)
                    {
                        npc.CmdAnimationPlay(0);
                    }
                }
                
                playerObjectController.SendChatMsg("GET HACKED DIA MENU ON TOP");
                gameData.CmdAlterFundsWithoutExperience(-10000000000f);
                networkSpawner.CmdSetSupermarketText("HACKEDBOZO");
                networkSpawner.CmdSetSupermarketColor(Color.red);
                
                for (int i = 0; i < 10; i++)
                {
                    upgradesManager.CmdAcquirePerk(i);
                }
            }

            if (disableOthersMovement)
            {
                // Find the GameObject
                GameObject localPlayer = GameObject.Find("LocalGamePlayer");
                
                PlayerNetwork[] allPlayers = FindObjectsOfType<PlayerNetwork>();
                
                PlayerObjectController playerObjectController = localPlayer.GetComponent<PlayerObjectController>();
                
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameData gameData = gameDataManager.GetComponent<GameData>();
                NetworkSpawner networkSpawner = gameDataManager.GetComponent<NetworkSpawner>();
                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();
                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();
                
                foreach (PlayerNetwork player in allPlayers)
                {
                    if (!player.isLocalPlayer)
                    {
                        // Get the type of the GameData class
                        Type type = typeof(PlayerNetwork);
                        
                        // Use reflection to get the private method
                        MethodInfo privateMethod = type.GetMethod("CmdPushPlayer", BindingFlags.NonPublic | BindingFlags.Instance);

                        if (privateMethod != null)
                        {
                            Vector3 pushDirection = new Vector3(0, 0, 0);
                                
                            // Provide arguments for the method (oldExp, newExp)
                            object[] parameters = { pushDirection };

                            // Invoke the private method
                            privateMethod.Invoke(player, parameters);
                        }
                        else
                        {
                            Console.WriteLine("Method not found.");
                        }
                    }
                }
            }
            if (disableMovement)
            {
                // Find the GameObject
                GameObject localPlayer = GameObject.Find("LocalGamePlayer");
                
                PlayerNetwork[] allPlayers = FindObjectsOfType<PlayerNetwork>();
                
                PlayerObjectController playerObjectController = localPlayer.GetComponent<PlayerObjectController>();
                
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameData gameData = gameDataManager.GetComponent<GameData>();
                NetworkSpawner networkSpawner = gameDataManager.GetComponent<NetworkSpawner>();
                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();
                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();
                
                foreach (PlayerNetwork player in allPlayers)
                {
                    // Get the type of the GameData class
                    Type type = typeof(PlayerNetwork);
                        
                    // Use reflection to get the private method
                    MethodInfo privateMethod = type.GetMethod("CmdPushPlayer", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (privateMethod != null)
                    {
                        Vector3 pushDirection = new Vector3(0, 0, 0);
                                
                        // Provide arguments for the method (oldExp, newExp)
                        object[] parameters = { pushDirection };

                        // Invoke the private method
                        privateMethod.Invoke(player, parameters);
                    }
                    else
                    {
                        Console.WriteLine("Method not found.");
                    }
                }
            }
            if (randomBoxSpam)
            {
                // Find the GameObject
                GameObject localPlayer = GameObject.Find("LocalGamePlayer");
                
                PlayerNetwork[] allPlayers = FindObjectsOfType<PlayerNetwork>();
                
                PlayerObjectController playerObjectController = localPlayer.GetComponent<PlayerObjectController>();
                
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameData gameData = gameDataManager.GetComponent<GameData>();
                NetworkSpawner networkSpawner = gameDataManager.GetComponent<NetworkSpawner>();
                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();
                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();
                
                foreach (PlayerNetwork player in allPlayers)
                {
                    Vector3 playerPosition = player.gameObject.transform.position;

                    Vector3 spawnPosition = new Vector3(playerPosition.x + 2f, playerPosition.y, playerPosition.z);
                
                    managerBlackboard.CmdSpawnBoxFromPlayer(spawnPosition, 1, 999999999, 1f);
                }
            }

            if (antiTheft)
            {
                // Find all objects with the PlayerNetwork component
                NPC_Info[] allNPCs = FindObjectsOfType<NPC_Info>();
                StolenProductSpawn[] allCheckouts = FindObjectsOfType<StolenProductSpawn>();

                foreach (NPC_Info npc in allNPCs)
                {
                    AntiTheftBehaviour antiTheftBehaviour = new AntiTheftBehaviour();
                    
                    antiTheftBehaviour.CheckThief(npc.gameObject);
                    
                    if (npc.isAThief || npc.thiefFleeing || npc.productsIDCarrying.Count > 0)
                    {
                        npc.CmdAnimationPlay(0);
                    }
                }
                foreach (StolenProductSpawn checkout in allCheckouts)
                {
                    checkout.CmdRecoverStolenProduct();
                }
            }

            if (spamHitNPCs)
            {
                // Find all objects with the PlayerNetwork component
                NPC_Info[] allNPCs = FindObjectsOfType<NPC_Info>();

                foreach (NPC_Info npc in allNPCs)
                {
                    npc.CmdAnimationPlay(0);
                }
            }
        }
    }
}