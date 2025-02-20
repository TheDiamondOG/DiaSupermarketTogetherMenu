using BepInEx;
//using SupermarketTogetherKacker.Patches;
using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Mirror;
using Mirror.Examples.Chat;
using Mirror.Examples.MultipleMatch;
using StarterAssets;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using SupermarketTogetherKacker.tools;
using Color = UnityEngine.Color;
using Random = System.Random;

namespace SupermarketTogetherKacker.menu
{
    public class CoolGUI : MonoBehaviour
    {
        // GUI Crap
        private Rect windowRect = new Rect(20, 20, 600, 500);
        private Vector2 modScrollPos;
        private Vector2 categoryScrollPos;
        private string menuName = "Dia Mods by TheDiamondOG";
        private string menuTitle;

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
        private bool autoCheckout;
        private string newUsername;
        private bool coolHeckerButton;
        private bool disableOthersMovement;
        private bool disableMovement;
        private bool randomBoxSpam;
        private bool antiTheft;
        private bool spamHitNPCs;
        private bool instantCrasher;
        private bool boxLagger;
        private bool pushCrash;
        private bool antiCrash;
        private bool classicBoxSpam;
        private bool airJump;
        private bool becomeHost;
        private bool speedBoost;
        private bool jumpBoost;
        private bool noJumpDelay;
        private bool messageCrasher;

        private bool finishedSpeedBoost;
        private bool finishedJumpBoost;
        private bool finishedNoJumpDelay;

        private float infoPageDelayTime = 0.1f;
        private float lastInfoPageExecutionTime = -1f;
        private string infoPageText;

        
        private Callback<LobbyMatchList_t> lobbyMatchListCallback;
        private List<CSteamID> foundLobbies = new List<CSteamID>();
        private float lobbyTimer = 0f;
        private const float lobbyTimerDelay = 2f;
        
        public enum ModCategory
        {
            Market,
            Player,
            Stats,
            Map,
            Server,
            Extras,
            NPC,
            Info,
            Lobbies,
            Debug
        }

        public ModCategory currentCategory = ModCategory.Market;

        void OnGUI()
        {
            GUI.backgroundColor = Color.black;
            GUI.contentColor = Color.white;
            GUI.color = Color.white;

            //GUI.skin.window.normal.background = Mods.MakeTex(100, 100, Color.black);
            //GUI.skin.box.normal.background = Mods.MakeTex(100, 100, Color.white);
            //GUI.skin.button.normal.background = Mods.MakeTex(1, 1, Color.HSVToRGB(330f, 15f, 5f));
            //GUI.skin.button.hover.background = Mods.MakeTex(1, 1, Color.HSVToRGB(200f, 10f, 5f));
            //GUI.skin.horizontalScrollbar.normal.background = Mods.MakeTex(1, 1, Color.HSVToRGB(330f, 0f, 0f));
            //GUI.skin.verticalScrollbar.normal.background = Mods.MakeTex(1, 1, Color.HSVToRGB(330f, 0f, 0f));
            //GUI.skin.window.border = new RectOffset(5, 5, 5, 5);
            //GUI.skin.window.padding = new RectOffset(10, 10, 10, 10);
            //GUI.skin.window.alignment = TextAnchor.MiddleCenter;

            if (showGUI)
            {
                GUI.BringWindowToFront(0);
                GUI.FocusWindow(0);
                windowRect = GUI.Window(0, windowRect, WindowFunction, menuTitle);
            }
        }

        void WindowFunction(int windowID)
        {
            GUI.DragWindow(new Rect(0, 0, windowRect.width, 20));

            GUILayout.BeginVertical();

            // Horizontal scrolling for categories
            categoryScrollPos = GUILayout.BeginScrollView(categoryScrollPos, GUILayout.Height(50));
            GUILayout.BeginHorizontal();
            foreach (ModCategory category in Enum.GetValues(typeof(ModCategory)))
            {
                if (GUILayout.Button(category.ToString(), GUILayout.Width(100)))
                {
                    currentCategory = category;
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();

            GUILayout.Space(10);

            // Vertical scrolling for mods
            modScrollPos = GUILayout.BeginScrollView(modScrollPos, GUILayout.Height(400));
            GUILayout.BeginVertical();
            DisplayMods();
            GUILayout.EndVertical();
            GUILayout.EndScrollView();

            GUILayout.EndVertical();
        }

        void DisplayMods()
        {
            switch (currentCategory)
            {
                case ModCategory.Market:
                    DisplayMarketMods();
                    break;
                case ModCategory.Player:
                    DisplayPlayerMods();
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
                case ModCategory.Info:
                    DisplayInfoPage();
                    break;
                case ModCategory.Lobbies:
                    DisplayLobbyList();
                    break;
                case ModCategory.Debug:
                    DisplayDebugMods();
                    break;
            }
        }

        void DisplayMarketMods()
        {
            if (GUILayout.Button("Unlimited Customers", GUILayout.Height(30)))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameData gameData = gameDataManager.GetComponent<GameData>();

                gameData.maxCustomersNPCs = 1000000000;
            }

            if (GUILayout.Button("Open Market", GUILayout.Height(30)))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameData gameData = gameDataManager.GetComponent<GameData>();

                gameData.CmdOpenSupermarket();
            }

            if (GUILayout.Button("Close Market", GUILayout.Height(30)))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameData gameData = gameDataManager.GetComponent<GameData>();

                gameData.NetworktimeOfDay = 23f;
                gameData.SaveOBJ.GetComponent<PlayMakerFSM>().SendEvent("Send_Data");
                gameData.CmdEndDayFromButton();
            }

            if (GUILayout.Button("Free Expantion", GUILayout.Height(30)))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();

                int i = 0;
                foreach (bool indexCheck in upgradesManager.storeSpaceUpgrades)
                {
                    upgradesManager.CmdAddStorage(i);
                    i += 1;
                }
            }

            if (GUILayout.Button("Free Storage", GUILayout.Height(30)))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();

                int i = 0;
                foreach (bool indexCheck in upgradesManager.storageSpaceUpgrades)
                {
                    upgradesManager.CmdAddStorage(i);
                    i += 1;
                }
            }

            string waterBoxSpammerText;

            if (waterBoxSpammer)
            {
                waterBoxSpammerText = "<color=green>ON</color>: Lots of Water";
            }
            else
            {
                waterBoxSpammerText = "<color=red>OFF</color>: Lots of Water";
            }

            if (GUILayout.Button(waterBoxSpammerText, GUILayout.Height(30)))
            {
                if (waterBoxSpammer)
                {
                    waterBoxSpammer = false;
                }
                else
                {
                    waterBoxSpammer = true;
                }
            }

            string everyBoxSpamText;

            if (everyBoxSpam)
            {
                everyBoxSpamText = "<color=green>ON</color>: Lots of Everything (Crash)";
            }
            else
            {
                everyBoxSpamText = "<color=red>OFF</color>: Lots of Everything (Crash)";
            }

            if (GUILayout.Button(everyBoxSpamText, GUILayout.Height(30)))
            {
                if (everyBoxSpam)
                {
                    everyBoxSpam = false;
                }
                else
                {
                    everyBoxSpam = true;
                }
            }
        }

        void DisplayPlayerMods()
        {
            string speedBoostText;

            if (speedBoost)
            {
                speedBoostText = "<color=green>ON</color>: Speedboost";
            }
            else
            {
                speedBoostText = "<color=red>OFF</color>: Speedboost";
            }

            if (GUILayout.Button(speedBoostText, GUILayout.Height(30)))
            {
                if (speedBoost)
                {
                    speedBoost = false;
                }
                else
                {
                    speedBoost = true;
                }
            }

            string airJumpText;

            if (airJump)
            {
                airJumpText = "<color=green>ON</color>: Air Jump";
            }
            else
            {
                airJumpText = "<color=red>OFF</color>: Air Jump";
            }

            if (GUILayout.Button(airJumpText, GUILayout.Height(30)))
            {
                if (airJump)
                {
                    airJump = false;
                }
                else
                {
                    airJump = true;
                }
            }

            string jumpBoostText;

            if (jumpBoost)
            {
                jumpBoostText = "<color=green>ON</color>: Jump Boost";
            }
            else
            {
                jumpBoostText = "<color=red>OFF</color>: Jump Boost";
            }

            if (GUILayout.Button(jumpBoostText, GUILayout.Height(30)))
            {
                if (jumpBoost)
                {
                    jumpBoost = false;
                }
                else
                {
                    jumpBoost = true;
                }
            }

            string noJumpDelayText;

            if (noJumpDelay)
            {
                noJumpDelayText = "<color=green>ON</color>: No Jump Delay";
            }
            else
            {
                noJumpDelayText = "<color=red>OFF</color>: No Jump Delay";
            }

            if (GUILayout.Button(noJumpDelayText, GUILayout.Height(30)))
            {
                if (noJumpDelay)
                {
                    noJumpDelay = false;
                }
                else
                {
                    noJumpDelay = true;
                }
            }
        }

        void DisplayStatsMods()
        {
            string moneyAddStringDisplay = "";

            moneyAddString = GUILayout.TextField(moneyAddString, GUILayout.Height(30));

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

            if (GUILayout.Button(moneyAddStringDisplay, GUILayout.Height(30)))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameData gameData = gameDataManager.GetComponent<GameData>();

                gameData.CmdAlterFundsWithoutExperience(moneyAdd);
            }

            string moneySpamText;

            if (moneySpam)
            {
                moneySpamText = "<color=green>ON</color>: " + moneyAddStringDisplay + "$";
            }
            else
            {
                moneySpamText = "<color=red>OFF</color>: " + moneyAddStringDisplay + "$";
            }

            if (GUILayout.Button(moneySpamText, GUILayout.Height(30)))
            {
                if (moneySpam)
                {
                    moneySpam = false;
                }
                else
                {
                    moneySpam = true;
                }
            }

            try
            {
                pointsAdd = int.Parse(moneyAddString);
                if (pointsAdd >= 0)
                {
                    moneyAddStringDisplay = "+" + pointsAdd;
                }
                else
                {
                    moneyAddStringDisplay = pointsAdd + "";
                }
            }
            catch (Exception)
            {
                moneyAddStringDisplay = "+10";
                pointsAdd = 10;
            }

            if (GUILayout.Button(moneyAddStringDisplay + " Points", GUILayout.Height(30)))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();

                upgradesManager.CmdAcquirePerk(0, -pointsAdd);
            }

        }

        void DisplayMapMods()
        {
            if (GUILayout.Button("Disable Barrier", GUILayout.Height(30)))
            {
                GameObject worldBarriers = GameObject.Find("Level_Exterior/Colliders");

                worldBarriers.SetActive(false);
            }

            if (GUILayout.Button("Become Cool", GUILayout.Height(30)))
            {
                GameObject worldBarriers = GameObject.Find("TheCoolRoom/AddonCollider");

                GameObject posesAndDancesText =
                    GameObject.Find("TheCoolRoom/Canvas_Skins/Container/PosesAndDancesText");

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

            if (GUILayout.Button("No Jail", GUILayout.Height(30)))
            {
                GameObject worldBarriers = GameObject.Find("TheCoolRoom/Jail");

                worldBarriers.SetActive(false);
            }
        }

        void DisplayServerMods()
        {
            if (GUILayout.Button("Max Level", GUILayout.Height(30)))
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

            if (GUILayout.Button("Add Random Perks", GUILayout.Height(30)))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();

                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();

                GameData gameData = gameDataManager.GetComponent<GameData>();

                int i = 0;
                foreach (bool boolCool in upgradesManager.extraUpgrades)
                {
                    upgradesManager.CmdAcquirePerk(i, 0);
                    i += 1;
                    upgradesManager.extraUpgrades[i] = false;
                }
            }

            string perkSpamText;

            if (perkSpam)
            {
                perkSpamText = "<color=green>ON</color>: Perk Spam";
            }
            else
            {
                perkSpamText = "<color=red>OFF</color>: Perk Spam";
            }

            if (GUILayout.Button(perkSpamText, GUILayout.Height(30)))
            {
                if (perkSpam)
                {
                    perkSpam = false;
                }
                else
                {
                    perkSpam = true;
                }
            }

            if (GUILayout.Button("Add Employee", GUILayout.Height(30)))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameObject npcManager = GameObject.Find("NPC_Manager");

                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();

                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();

                GameData gameData = gameDataManager.GetComponent<GameData>();

                NPC_Manager npcManagerClass = npcManager.GetComponent<NPC_Manager>();

                EmployeesDataGeneration employeesData = npcManager.GetComponent<EmployeesDataGeneration>();

                Random rnd = new Random();

                upgradesManager.CmdAcquirePerk(1, 0);

                upgradesManager.extraUpgrades[1] = false;

                gameData.employeesCost = 0;

                //employeesData.HireEmployee(rnd.Next(0,upgradesManager.maxEmployees), RandomString(rnd.Next(5,20)));
            }

            string employeeSpamText;

            if (employeeSpam)
            {
                employeeSpamText = "<color=green>ON</color>: Employee Spam";
            }
            else
            {
                employeeSpamText = "<color=red>OFF</color>: Employee Spam";
            }

            if (GUILayout.Button(employeeSpamText, GUILayout.Height(30)))
            {
                if (employeeSpam)
                {
                    employeeSpam = false;
                }
                else
                {
                    employeeSpam = true;
                }
            }

            if (GUILayout.Button("Push Others", GUILayout.Height(30)))
            {
                PlayerNetwork[] allPlayers = FindObjectsOfType<PlayerNetwork>();

                foreach (PlayerNetwork player in allPlayers)
                {
                    if (!player.isLocalPlayer)
                    {
                        Vector3 pushDirection = new Vector3(100, 100, 100);

                        Mods.PushPlayer(player, pushDirection);
                    }
                }
            }

            string spamPushOthersText;

            if (spamPushOthers)
            {
                spamPushOthersText = "<color=green>ON</color>: Spam Push Others";
            }
            else
            {
                spamPushOthersText = "<color=red>OFF</color>: Spam Push Others";
            }

            if (GUILayout.Button(spamPushOthersText, GUILayout.Height(30)))
            {
                if (spamPushOthers)
                {
                    spamPushOthers = false;
                }
                else
                {
                    spamPushOthers = true;
                }
            }

            if (GUILayout.Button("Push Everyone", GUILayout.Height(30)))
            {
                // Find all objects with the PlayerNetwork component
                PlayerNetwork[] allPlayers = FindObjectsOfType<PlayerNetwork>();

                foreach (PlayerNetwork player in allPlayers)
                {
                    Vector3 pushDirection = new Vector3(100, 100, 100);

                    Mods.PushPlayer(player, pushDirection);
                }
            }

            string spamPushText;

            if (spamPushOthers)
            {
                spamPushText = "<color=green>ON</color>: Spam Push";
            }
            else
            {
                spamPushOthersText = "<color=red>OFF</color>: Spam Push";
            }

            if (GUILayout.Button(spamPushOthersText, GUILayout.Height(30)))
            {
                if (spamPushOthers)
                {
                    spamPushOthers = false;
                }
                else
                {
                    spamPushOthers = true;
                }
            }

            messageString = GUILayout.TextField(messageString, GUILayout.Height(30));

            string messageStringText;

            if (messageSpam)
            {
                messageStringText = "<color=green>ON</color>: Message Spammer";
            }
            else
            {
                messageStringText = "<color=red>OFF</color>: Message Spammer";
            }

            if (GUILayout.Button(messageStringText, GUILayout.Height(30)))
            {
                if (messageSpam)
                {
                    messageSpam = false;
                }
                else
                {
                    messageSpam = true;
                }
            }

            string messageCrasherText;

            if (messageCrasher)
            {
                messageCrasherText = "<color=green>ON</color>: Message Crasher";
            }
            else
            {
                messageCrasherText = "<color=red>OFF</color>: Message Crasher";
            }

            if (GUILayout.Button(messageCrasherText, GUILayout.Height(30)))
            {
                if (messageCrasher)
                {
                    messageCrasher = false;
                }
                else
                {
                    messageCrasher = true;
                }
            }

            if (GUILayout.Button("Bright Sign", GUILayout.Height(30)))
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

            newSuperMarketName = GUILayout.TextField(newSuperMarketName, GUILayout.Height(30));
            if (GUILayout.Button("Change Supermarket Name", GUILayout.Height(30)))
            {
                GameObject gameDataObject = GameObject.Find("GameDataManager");

                NetworkSpawner networkSpawner = gameDataObject.GetComponent<NetworkSpawner>();

                networkSpawner.CmdSetSupermarketText(newSuperMarketName);
            }

            if (GUILayout.Button("Max Boxes", GUILayout.Height(30)))
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

            if (GUILayout.Button("Water Infection", GUILayout.Height(30)))
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

            if (GUILayout.Button("No Product", GUILayout.Height(30)))
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

            string disableOthersMovementText;

            if (disableOthersMovement)
            {
                disableOthersMovementText = "<color=green>ON</color>: Disable Others Movement";
            }
            else
            {
                disableOthersMovementText = "<color=red>OFF</color>: Disable Others Movement";
            }

            if (GUILayout.Button(disableOthersMovementText, GUILayout.Height(30)))
            {
                if (disableOthersMovement)
                {
                    disableOthersMovement = false;
                }
                else
                {
                    disableOthersMovement = true;
                }
            }

            string randomBoxSpamText;

            if (randomBoxSpam)
            {
                randomBoxSpamText = "<color=green>ON</color>: Water Everywhere";
            }
            else
            {
                randomBoxSpamText = "<color=red>OFF</color>: Water Everywhere";
            }

            if (GUILayout.Button(randomBoxSpamText, GUILayout.Height(30)))
            {
                if (randomBoxSpam)
                {
                    randomBoxSpam = false;
                }
                else
                {
                    randomBoxSpam = true;
                }
            }

            string instantCrasherText;

            if (instantCrasher)
            {
                instantCrasherText = "<color=green>ON</color>: Instant Crasher";
            }
            else
            {
                instantCrasherText = "<color=red>OFF</color>: Instant Crasher";
            }

            if (GUILayout.Button(instantCrasherText, GUILayout.Height(30)))
            {
                if (instantCrasher)
                {
                    instantCrasher = false;
                }
                else
                {
                    instantCrasher = true;
                }
            }

            string boxLaggerText;

            if (boxLagger)
            {
                boxLaggerText = "<color=green>ON</color>: Box Crasher";
            }
            else
            {
                boxLaggerText = "<color=red>OFF</color>: Box Crasher";
            }

            if (GUILayout.Button(boxLaggerText, GUILayout.Height(30)))
            {
                if (boxLagger)
                {
                    boxLagger = false;
                }
                else
                {
                    boxLagger = true;
                }
            }

            if (GUILayout.Button("Bring All Players", GUILayout.Height(30)))
            {
                GameObject localPlayer = GameObject.Find("LocalGamePlayer");

                Vector3 SpawnPosition = new Vector3(localPlayer.transform.position.x, localPlayer.transform.position.y,
                    localPlayer.transform.position.z + 2);

                PlayerNetwork[] playerNetworks = FindObjectsOfType<PlayerNetwork>();

                foreach (PlayerNetwork playerNetwork in playerNetworks)
                {
                    if (!playerNetwork.isLocalPlayer)
                    {
                        Mods.MoveObject(playerNetwork.gameObject, SpawnPosition);
                    }
                }
            }

            if (GUILayout.Button("Bring All Boxes", GUILayout.Height(30)))
            {
                GameObject localPlayer = GameObject.Find("LocalGamePlayer");

                Vector3 SpawnPosition = new Vector3(localPlayer.transform.position.x, localPlayer.transform.position.y,
                    localPlayer.transform.position.z + 2);

                BoxData[] allBoxes = FindObjectsOfType<BoxData>();

                foreach (BoxData box in allBoxes)
                {
                    Mods.MoveObject(box.gameObject, SpawnPosition);
                }
            }

            if (GUILayout.Button("Players To Nothing", GUILayout.Height(30)))
            {
                Vector3 SpawnPosition = new Vector3(float.NaN, float.NaN, float.NaN);

                PlayerNetwork[] playerNetworks = FindObjectsOfType<PlayerNetwork>();

                foreach (PlayerNetwork playerNetwork in playerNetworks)
                {
                    if (!playerNetwork.isLocalPlayer)
                    {
                        Mods.MoveObject(playerNetwork.gameObject, SpawnPosition);
                    }
                }
            }

            if (GUILayout.Button("Boxes To Nothing", GUILayout.Height(30)))
            {
                Vector3 SpawnPosition = new Vector3(float.NaN, float.NaN, float.NaN);

                BoxData[] boxes = FindObjectsOfType<BoxData>();

                foreach (BoxData box in boxes)
                {
                    Mods.MoveObject(box.gameObject, SpawnPosition);
                }
            }

            string classicBoxSpamText;

            if (classicBoxSpam)
            {
                classicBoxSpamText = "<color=green>ON</color>: Classic Box Spam";
            }
            else
            {
                classicBoxSpamText = "<color=red>OFF</color>: Classic Box Spam";
            }

            if (GUILayout.Button(classicBoxSpamText, GUILayout.Height(30)))
            {
                if (classicBoxSpam)
                {
                    classicBoxSpam = false;
                }
                else
                {
                    classicBoxSpam = true;
                }
            }

            if (GUILayout.Button("Destroy Floor Colliders", GUILayout.Height(30)))
            {
                Vector3 SpawnPosition = new Vector3(float.NaN, float.NaN, float.NaN);

                NPC_Info[] npcs = FindObjectsOfType<NPC_Info>();

                foreach (NPC_Info npc in npcs)
                {
                    Mods.MoveObject(npc.gameObject, SpawnPosition);
                }
            }

            string becomeHostText;

            if (becomeHost)
            {
                becomeHostText = "<color=green>ON</color>: Become Host";
            }
            else
            {
                becomeHostText = "<color=red>OFF</color>: Become Host";
            }

            if (GUILayout.Button(becomeHostText, GUILayout.Height(30)))
            {
                if (becomeHost)
                {
                    becomeHost = false;
                }
                else
                {
                    becomeHost = true;
                }
            }
            if (GUILayout.Button("Enable Voice Chat", GUILayout.Height(30)))
            {
                GameObject gameDataObject = GameObject.Find("GameDataManager");
                
                NetworkGameBehaviors networkGameBehaviors = gameDataObject.GetComponent<NetworkGameBehaviors>();
                
                networkGameBehaviors.CmdServerEnableVoiceChat();
            }
        }

        void DisplayExtraMods()
        {
            if (GUILayout.Button("No Tutorial", GUILayout.Height(30)))
            {
                GameObject tutorialObject = GameObject.Find("GameCanvas/Tutorials");

                tutorialObject.SetActive(false);
            }

            if (GUILayout.Button("Scan All", GUILayout.Height(30)))
            {
                // Find all objects with the PlayerNetwork component
                ProductCheckoutSpawn[] allProducts = FindObjectsOfType<ProductCheckoutSpawn>();

                foreach (ProductCheckoutSpawn product in allProducts)
                {
                    product.CmdAddProductValueToCheckout();
                }
            }

            if (GUILayout.Button("Auto Checkout", GUILayout.Height(30)))
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

            string autoCheckoutText;

            if (autoCheckout)
            {
                autoCheckoutText = "<color=green>ON</color>: Auto Checkout";
            }
            else
            {
                autoCheckoutText = "<color=red>OFF</color>: Auto Checkout";
            }

            if (GUILayout.Button(autoCheckoutText, GUILayout.Height(30)))
            {
                if (autoCheckout)
                {
                    autoCheckout = false;
                }
                else
                {
                    autoCheckout = true;
                }
            }

            newUsername = GUILayout.TextField(newUsername, GUILayout.Height(30));

            if (GUILayout.Button("Set Name", GUILayout.Height(30)))
            {
                Mods.SetPlayerName(newUsername);
            }

            string coolHeckerText;

            if (coolHeckerButton)
            {
                coolHeckerText = "<color=green>ON</color>: Cool Hecker";
            }
            else
            {
                coolHeckerText = "<color=red>OFF</color>: Cool Hecker";
            }

            if (GUILayout.Button(coolHeckerText, GUILayout.Height(30)))
            {
                if (coolHeckerButton)
                {
                    coolHeckerButton = false;
                }
                else
                {
                    coolHeckerButton = true;
                }
            }

            if (GUILayout.Button("Grab All Stollen", GUILayout.Height(30)))
            {
                StolenProductSpawn[] allCheckouts = FindObjectsOfType<StolenProductSpawn>();

                foreach (StolenProductSpawn checkout in allCheckouts)
                {
                    checkout.CmdRecoverStolenProduct();
                }
            }

            string antiCrashText;

            if (antiCrash)
            {
                antiCrashText = "<color=green>ON</color>: Anti Crash";
            }
            else
            {
                antiCrashText = "<color=red>OFF</color>: Anti Crash";
            }

            if (GUILayout.Button(antiCrashText, GUILayout.Height(30)))
            {
                if (antiCrash)
                {
                    antiCrash = false;
                }
                else
                {
                    antiCrash = true;
                }
            }

            if (GUILayout.Button("Unlock FPS", GUILayout.Height(30)))
            {
                Application.targetFrameRate = 999999999;
                QualitySettings.vSyncCount = 0;
            }
        }

        void DisplayNPCMods()
        {
            string spamHitNPCsText;

            if (spamHitNPCs)
            {
                spamHitNPCsText = "<color=green>ON</color>: Spam Hit NPCs";
            }
            else
            {
                spamHitNPCsText = "<color=red>OFF</color>: Spam Hit NPCs";
            }

            if (GUILayout.Button(spamHitNPCsText, GUILayout.Height(30)))
            {
                if (spamHitNPCs)
                {
                    spamHitNPCs = false;
                }
                else
                {
                    spamHitNPCs = true;
                }
            }

            if (GUILayout.Button("Hit NPCs", GUILayout.Height(30)))
            {
                // Find all objects with the PlayerNetwork component
                NPC_Info[] allNPCs = FindObjectsOfType<NPC_Info>();

                foreach (NPC_Info npc in allNPCs)
                {
                    npc.CmdAnimationPlay(0);
                }
            }
        }

        void DisplayInfoPage()
        {
            // Check if the delay has passed
            if (lastInfoPageExecutionTime == -1f || Time.time - lastInfoPageExecutionTime >= infoPageDelayTime)
            {
                lastInfoPageExecutionTime = Time.time;

                infoPageText = "";

                GameObject onlineNetworkManager = GameObject.Find("OnlineNetworkManager");
                GameObject localNetworkManager = GameObject.Find("LocalNetworkManager");

                if (onlineNetworkManager != null)
                {
                    infoPageText = "<size=15><b><color=#00FFFF>Lobby Info</color>\n\n";

                    SteamLobby steamLobby = onlineNetworkManager.GetComponent<SteamLobby>();

                    infoPageText += "<color=yellow>Lobby Type: Online</color>\n";
                    infoPageText += "<color=yellow>Lobby ID: " + steamLobby.CurrentLobbyIDStr + "</color>\n";
                    infoPageText += "<color=yellow>Is Lobby Closed: " + steamLobby.isLobbyClosed + "</color>\n";
                    infoPageText += "==================================\n";

                    infoPageText += "\n<color=#00FFFF>Player Info</color>\n\n";

                    PlayerNetwork[] allPlayers = FindObjectsOfType<PlayerNetwork>();

                    foreach (PlayerNetwork player in allPlayers)
                    {
                        PlayerObjectController playerObjectController = player.GetComponent<PlayerObjectController>();
                        PlayerSyncCharacter playerSyncController = player.GetComponent<PlayerSyncCharacter>();

                        if (!player.isLocalPlayer)
                        {
                            infoPageText += "<color=yellow>Name: " + playerObjectController.NetworkPlayerName +
                                            "</color>\n";
                        }

                        infoPageText += "<color=yellow>Position: " + player.gameObject.transform.position +
                                        "</color>\n";
                        infoPageText += "<color=#00FFFF>Is Host: " + player.authority + "</color>\n";
                        infoPageText += "<color=#00FFFF>Net ID: " + player.netId + "</color>\n";
                        infoPageText += "<color=#00FFFF>Player ID: " + playerObjectController.PlayerIdNumber +
                                        "</color>\n";
                        infoPageText += "<color=#00FFFF>Steam ID: " + playerObjectController.NetworkPlayerSteamID +
                                        "</color>\n";
                        infoPageText += "<color=yellow>Is Crouching: " + player.isCrouching + "</color>\n";
                        infoPageText += "<color=green>Character ID: " + player.characterID + "</color>\n";
                        infoPageText += "<color=green>Broom ID: " + playerSyncController.broomSkin + "</color>\n";
                        infoPageText += "<color=green>Hat ID: " + player.hatID + "</color>\n";
                        if (player.isLocalPlayer)
                        {

                        }

                        infoPageText += "==================================\n\n";
                    }

                    infoPageText += "</size></b>";
                }
                else if (localNetworkManager != null)
                {
                    infoPageText = "<size=15><b><color=#00FFFF>Lobby Info</color>\n\n";

                    infoPageText += "<color=yellow>Lobby Type: Local</color>\n";
                    infoPageText += "==================================\n";

                    infoPageText += "\n<color=#00FFFF>Player Info</color>\n\n";

                    GameObject playerObject = GameObject.Find("LocalGamePlayer");

                    PlayerNetwork playerNetwork = playerObject.GetComponent<PlayerNetwork>();
                    PlayerObjectController playerObjectController = playerObject.GetComponent<PlayerObjectController>();
                    PlayerSyncCharacter playerSyncController = playerObject.GetComponent<PlayerSyncCharacter>();
                    FirstPersonController firstPersonController = playerObject.GetComponent<FirstPersonController>();

                    infoPageText += "<color=yellow>Name: " + playerObjectController.NetworkPlayerName + "</color>\n";
                    infoPageText += "<color=yellow>Position: " + playerNetwork.gameObject.transform.position +
                                    "</color>\n";
                    infoPageText += "<color=#00FFFF>Is Host: " + playerNetwork.isServer + "</color>\n";
                    infoPageText += "<color=#00FFFF>Net ID: " + playerNetwork.netId + "</color>\n";
                    infoPageText += "<color=#00FFFF>Player ID: " + playerObjectController.PlayerIdNumber + "</color>\n";
                    infoPageText += "<color=#00FFFF>Steam ID: " + playerObjectController.NetworkPlayerSteamID +
                                    "</color>\n";
                    infoPageText += "<color=yellow>Is Crouching: " + playerNetwork.isCrouching + "</color>\n";
                    infoPageText += "<color=green>Character ID: " + playerNetwork.characterID + "</color>\n";
                    infoPageText += "<color=green>Broom ID: " + playerSyncController.broomSkin + "</color>\n";
                    infoPageText += "<color=green>Hat ID: " + playerNetwork.hatID + "</color>\n";
                    infoPageText += "<color=#008080ff>Casual Speed: " + firstPersonController.MoveSpeed + "</color>\n";
                    infoPageText += "<color=#008080ff>Sprint Speed: " + firstPersonController.SprintSpeed +
                                    "</color>\n";
                    infoPageText += "<color=#008080ff>Crouch Speed: " + firstPersonController.CrouchSpeed +
                                    "</color>\n";
                    infoPageText += "<color=#008080ff>Jump Height: " + firstPersonController.JumpHeight + "</color>\n";
                    infoPageText += "<color=#008080ff>Jump Delay: " + firstPersonController.JumpTimeout + "</color>\n";
                    infoPageText += "<color=#008080ff>Grounded: " + firstPersonController.Grounded + "</color>\n";
                    infoPageText += "==================================\n\n";
                    infoPageText += "</size></b>";
                }
                else
                {
                    infoPageText += "<size=50><b><color=yellow>You are not connected to a server</color><b></size>\n";
                }
            }

            GUILayout.Label(infoPageText);
        }

        void DisplayLobbyList()
        {
            System.Random random = new System.Random();
            
            string text = "<size=15><b><color=yellow>Lobby List:</color>\n";
            
            lobbyMatchListCallback = Callback<LobbyMatchList_t>.Create(OnLobbyMatchList);
            
            /*
            lobbyTimer += Time.deltaTime;

            if (lobbyTimer >= lobbyTimerDelay)
            {
                SteamMatchmaking.RequestLobbyList();
                lobbyTimer = 0f;
            }
            */
            
            for (int i = 0; i < foundLobbies.Count; i++)
            {
                CSteamID lobbyID = foundLobbies[i];
                string lobbyName = SteamMatchmaking.GetLobbyData(lobbyID, "name");
                int playerCount = SteamMatchmaking.GetNumLobbyMembers(lobbyID);
                int maxPlayers = SteamMatchmaking.GetLobbyMemberLimit(lobbyID);
                CSteamID ownerID = SteamMatchmaking.GetLobbyOwner(lobbyID);
                
                text += "<color=#00FFFF>Name: "+lobbyName;
                text += "\nLobby ID: "+lobbyID;
                text += "\nPlayer Count: " + playerCount.ToString() + "/"+maxPlayers.ToString();
                text += "</color>\n=============================\n";
            }
            
            GUILayout.Label(text);
            if (GUILayout.Button("Refresh Listings", GUILayout.Height(30)))
            {
                SteamMatchmaking.RequestLobbyList();
            }
            /*
            if (GUILayout.Button("Random Lobby", GUILayout.Height(30)))
            {
                if (foundLobbies.Count > 0)
                {
                    CSteamID randomLobby = foundLobbies[random.Next(0, foundLobbies.Count)];
                    SteamLobby.Instance.JoinLobby(randomLobby);
                }
            }
            */
        }

    void DisplayDebugMods()
        {
            if (GUILayout.Button("Dump Prefabs", GUILayout.Height(30)))
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
        
        private void OnLobbyMatchList(LobbyMatchList_t result)
        {
            foundLobbies.Clear();
            for (int i = 0; i < result.m_nLobbiesMatching; i++)
            {
                CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
                foundLobbies.Add(lobbyID);
            }
        }

        private void OnDestroy()
        {
            SteamAPI.Shutdown();
        }
        
        void Start()
        {
            lobbyMatchListCallback = Callback<LobbyMatchList_t>.Create(OnLobbyMatchList);
        }
    
        void Update()
        {
            menuTitle = menuName + " - FPS: " + Mathf.Ceil(1f / Time.unscaledDeltaTime).ToString();
            
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

                int i = 0;
                foreach (bool boolCool in upgradesManager.extraUpgrades)
                {
                    upgradesManager.CmdAcquirePerk(i, 0);
                    i += 1;
                    upgradesManager.extraUpgrades[i] = false;
                }
            }

            if (employeeSpam)
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();

                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();

                GameData gameData = gameDataManager.GetComponent<GameData>();

                upgradesManager.CmdAcquirePerk(1, 0);
                upgradesManager.extraUpgrades[1] = false;
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
                
                for (int i = 0; i < 250; i++)
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
                PlayerNetwork[] allPlayers = FindObjectsOfType<PlayerNetwork>();

                foreach (PlayerNetwork player in allPlayers)
                {
                    if (!player.isLocalPlayer)
                    {
                        Vector3 pushDirection = new Vector3(100, 100, 100);
                    
                        Mods.PushPlayer(player, pushDirection);
                    }
                }
            }
            if (spamPush)
            {
                PlayerNetwork[] allPlayers = FindObjectsOfType<PlayerNetwork>();

                foreach (PlayerNetwork player in allPlayers)
                {
                    Vector3 pushDirection = new Vector3(100, 100, 100);
                    
                    Mods.PushPlayer(player, pushDirection);
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

                for (int i = 0; i < 5; i++)
                {
                    playerObjectController.SendChatMsg(messageString);
                }
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

                Mods.SetPlayerName("HACKED");
                
                foreach (PlayerNetwork player in allPlayers)
                {
                    Vector3 playerPosition = player.gameObject.transform.position;

                    Vector3 spawnPosition = new Vector3(playerPosition.x + 2f, playerPosition.y, playerPosition.z);
                
                    managerBlackboard.CmdSpawnBoxFromPlayer(spawnPosition, 1, 999999999, 1f);
                    
                    Vector3 pushDirection = new Vector3(0, 0, 0);

                    Mods.PushPlayer(player, pushDirection);
                    
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
                
                for (int i = 0; i < 1000; i++)
                {
                    upgradesManager.CmdAcquirePerk(i, 999999999);
                }
                
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
                        Vector3 pushDirection = new Vector3(0, 0, 0);

                        Mods.PushPlayer(player, pushDirection);
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
                    Vector3 pushDirection = new Vector3(0, 0, 0);

                    Mods.PushPlayer(player, pushDirection);
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
            if (instantCrasher)
            {
                PlayerNetwork[] allPlayers = FindObjectsOfType<PlayerNetwork>();
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();
                
                /*
                managerBlackboard.AddShoppingListProduct(1, 0.0000000000000000000001f);
                managerBlackboard.BuyCargo();
                */
                
                foreach (PlayerNetwork player in allPlayers)
                {
                    Vector3 playerPosition = player.gameObject.transform.position;

                    Vector3 spawnPosition = new Vector3(playerPosition.x + 2f, playerPosition.y, playerPosition.z);
                    
                    for (int i = 0; i < 175; i++)
                        if (instantCrasher)
                        {
                            managerBlackboard.CmdSpawnBoxFromPlayer(spawnPosition, i, 999999999, 1f);
                        }
                        else
                        {
                            break;
                        }
                }
            }
            if (boxLagger) // shout out to 4bx9 for setting up the method
            {
                GameObject g = GameObject.Find("GameDataManager");

                ManagerBlackboard m = g.GetComponent<ManagerBlackboard>();

                Vector3 pp = new Vector3(float.NaN, float.NaN, float.NaN);

                for (int i = 0; i < 175; i++)
                {
                    m.CmdSpawnBoxFromPlayer(pp, i, int.MaxValue, float.NaN);
                    m.CmdSpawnBoxFromPlayer(pp, i, int.MaxValue, float.NaN);
                    m.CmdSpawnBoxFromPlayer(pp, i, int.MaxValue, float.NaN);
                }
            }
            if (antiCrash)
            {
                BoxData[] allBoxes = FindObjectsOfType<BoxData>();

                foreach (BoxData box in allBoxes)
                {
                    box.gameObject.SetActive(false);
                }
            }

            if (classicBoxSpam)
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");
                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();
                
                managerBlackboard.AddShoppingListProduct(1, 0.0000000000000000000001f);
                managerBlackboard.BuyCargo();
            }

            if (airJump)
            {
                if (!Input.GetKeyDown(KeyCode.LeftControl))
                {
                    FirstPersonController.Instance.Grounded = true;
                }
            }

            if (becomeHost)
            {
                GameObject localPlayer = GameObject.Find("LocalGamePlayer");

                PlayerNetwork playerNetwork = localPlayer.GetComponent<PlayerNetwork>();

                Traverse.Create(playerNetwork).Field("isOwned").SetValue(true);
                Traverse.Create(playerNetwork).Field("isServer").SetValue(true);
                Traverse.Create(playerNetwork).Field("authority").SetValue(true);
                
                PlayerPermissions playerPermissions = localPlayer.GetComponent<PlayerPermissions>();

                Traverse.Create(playerPermissions).Field("isOwned").SetValue(true);
                Traverse.Create(playerPermissions).Field("isServer").SetValue(true);
                Traverse.Create(playerPermissions).Field("authority").SetValue(true);
            }

            if (speedBoost)
            {
                if (!finishedSpeedBoost)
                {
                    FirstPersonController.Instance.MoveSpeed = 20f;
                    FirstPersonController.Instance.SprintSpeed = 40f;
                    FirstPersonController.Instance.CrouchSpeed = 16f;

                    finishedSpeedBoost = true;
                }
            }
            else
            {
                if (finishedSpeedBoost)
                {
                    FirstPersonController.Instance.MoveSpeed = 5f;
                    FirstPersonController.Instance.SprintSpeed = 10f;
                    FirstPersonController.Instance.CrouchSpeed = 4f;
                    
                    finishedSpeedBoost = false;
                }
            }

            if (jumpBoost)
            {
                if (!finishedJumpBoost)
                {
                    FirstPersonController.Instance.JumpHeight = 5f;
                    
                    finishedJumpBoost = true;
                }
            }
            else
            {
                if (finishedJumpBoost)
                {
                    FirstPersonController.Instance.JumpHeight = 1.2f;
                    
                    finishedJumpBoost = false;
                }
            }

            if (noJumpDelay)
            {
                if (!finishedNoJumpDelay)
                {
                    FirstPersonController.Instance.JumpTimeout = 0f;
                    
                    finishedNoJumpDelay = true;
                }
            }
            else
            {
                if (finishedNoJumpDelay)
                {
                    FirstPersonController.Instance.JumpTimeout = 0.1f;
                }
            }
            
            if (messageCrasher)
            {
                // Find the GameObject
                GameObject localPlayer = GameObject.Find("LocalGamePlayer");

                PlayerObjectController playerObjectController = localPlayer.GetComponent<PlayerObjectController>();

                for (int i = 0; i < 500; i++)
                {
                    playerObjectController.SendChatMsg(messageString);
                }
            }
        }
    }
}
