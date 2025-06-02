//using SupermarketTogetherKacker.Patches;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using Mirror;
using StarterAssets;
using Steamworks;
using UnityEngine;
using SupermarketTogetherKacker.tools;
using Unity.VisualScripting;
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
        private string menuName = "Dia Mods";
        private string menuTitle;
        private GUIStyle buttonStyle;

        // All the mods
        private bool showGUI = true;
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
        private bool antiTheft;
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
        private bool spamHitNPCs;
        private bool instantCrasher;
        private bool boxLagger;
        private bool antiCrash;
        private bool classicBoxSpam;
        private bool airJump;
        private bool becomeHost;
        private bool speedBoost;
        private bool jumpBoost;
        private bool noJumpDelay;
        private bool messageCrasher;
        private bool ascendAll;
        private bool ascendOthers;
        private bool idBasedBoxSpam;
        private bool idBasedBoxSpamEverywhere;
        private bool theHolding;
        private bool theHoldingOthers;
        private bool checkoutSpaz;
        private bool freeDestruction;
        private bool nothingSpammer;
        private bool priceSpaz;

        private bool finishedSpeedBoost;
        private bool finishedJumpBoost;
        private bool finishedNoJumpDelay;
        private bool finishedAntiCrash;

        private float infoPageDelayTime = 0.1f;
        private float lastInfoPageExecutionTime = -1f;
        private string infoPageText;

        // Anti Crasher Vars
        private Vector3 lastPlayerPosition;
        private float antiCrashGameObjectTime = -1f;
        private float antiCrashGameObjectDelay = 5f;

        private float fov = 90f;
        
        private PlayerNetwork selectedPlayer = null;
        private bool ascentTarget;
        private bool waterSpammerPlayerSelected;
        private bool theHoldingPlayerSelected;
        private bool spamHitSelected;
        private bool disableMovementSelected;
        
        public enum ModCategory
        {
            Home,
            Market,
            Player,
            Stats,
            Map,
            Server,
            Extras,
            NPC,
            PlayerSpecific,
            Info,
            Debug,
        }

        Dictionary<ModCategory, string> ModCategoryNames = new Dictionary<ModCategory, string>
        {
            { ModCategory.Home, "Home" },
            { ModCategory.Market, "Market" },
            { ModCategory.Player, "Player" },
            { ModCategory.Stats, "Stats" },
            { ModCategory.Map, "Map" },
            { ModCategory.Server, "Server" },
            { ModCategory.Extras, "Extras" },
            { ModCategory.NPC, "NPC" },
            { ModCategory.PlayerSpecific, "Player Specific" },
            { ModCategory.Info, "Info" },
            { ModCategory.Debug, "Debug" }
        };


        public ModCategory currentCategory = ModCategory.Home;
        
        void OnGUI()
        {
            GUI.backgroundColor = Color.black;
            GUI.contentColor = Color.white;
            GUI.color = Color.white;

            if (showGUI)
            {
                GUI.BringWindowToFront(0);
                GUI.FocusWindow(0);
                windowRect = GUI.Window(0, windowRect, WindowFunction, GUIContent.none);
            }
        }


        void WindowFunction(int windowID)
        {
            GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.normal.background = Mods.MakeTex(1, 1, Color.black);
            GUI.Box(new Rect(0, 0, windowRect.width, 20), GUIContent.none, boxStyle);

            GUI.DragWindow(new Rect(0, 0, windowRect.width, 20));
            
            GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 24,
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };

            buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter,
                border = new RectOffset(12, 12, 12, 12),
                normal = { textColor = Color.white, background = Mods.MakeTex(2, 2, new Color(0.4f, 0.4f, 0.4f, 1f)) },
                hover = { background = Mods.MakeTex(2, 2, new Color(0.5f, 0.5f, 0.5f, 1f)) },
                active = { background = Mods.MakeTex(2, 2, new Color(0.3f, 0.3f, 0.3f, 1f)) }
            };

            GUILayout.BeginHorizontal();
            
            categoryScrollPos = GUILayout.BeginScrollView(categoryScrollPos, GUILayout.Width(150));
            GUILayout.BeginVertical();
            foreach (ModCategory category in Enum.GetValues(typeof(ModCategory)))
            {
                if (GUILayout.Button(ModCategoryNames[category], buttonStyle, GUILayout.Height(25)))
                {
                    currentCategory = category;
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();

            GUILayout.Space(10);
            
            GUILayout.BeginVertical();

            GUILayout.Label(menuName, titleStyle);
            GUILayout.Space(10);

            modScrollPos = GUILayout.BeginScrollView(modScrollPos);
            GUILayout.BeginVertical();
            DisplayMods();
            GUILayout.EndVertical();
            GUILayout.EndScrollView();

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        void DisplayMods()
        {
            switch (currentCategory)
            {
                case ModCategory.Home:
                    DisplayHomePage();
                    break;
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
                case ModCategory.PlayerSpecific:
                    DisplayPlayerSpecificMods();
                    break;
                case ModCategory.Info:
                    DisplayInfoPage();
                    break;
                case ModCategory.Debug:
                    DisplayDebugMods();
                    break;
            }
        }

        void DisplayHomePage()
        {
            string text = "<size=15><b>";
            text += "<color=#00FFFF>Welcome to the Project Dia menu for Super Market Together.\n";
            text += "This menu was made out of boredom, and because I mod too many unity games.\n";
            text += "You can find the latest version of the menu and the source code on the github\n";
            text += "<a href='https://github.com/TheDiamondOG/DiaSupermarketTogetherMenu'>https://github.com/TheDiamondOG/DiaSupermarketTogetherMenu</a>\n";
            text += "Also to anyone that is using this on stream, hi </color><color=#ff00ffff>Twitch</color> <color=#00FFFF>or</color> <color=red>Youtube</color>.\n";
            text += "<color=#00FFFF>Also don't worry about getting banned since this game has no anticheat or report system.\n";
            text += "Quick shout out to <color=yellow>4bx9/bxware</color> <color=#00FFFF>for helping out with some of the methods.\n";
            text += "Anyways this is the end of the yap session, have fun.\n";
            text += "F1 - Show/Hide Menu\n";
            text += "F2 - Show/Hide Mouse\n";
            text += "Sincerely, TheDiamondOG\n\n";
            
            text += "P.S. If you have any suggestions join the server: <a href='https://discord.gg/n7pbPyTKDU'>https://discord.gg/n7pbPyTKDU</a>\n";
            
            text += "</color></size></b>";
            
            GUILayout.Label(text);
        }
        
        void DisplayMarketMods()
        {
            if (GUILayout.Button("Open Market", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameData gameData = gameDataManager.GetComponent<GameData>();

                if (!gameData.isSupermarketOpen)
                {
                    gameData.CmdOpenSupermarket();
                
                    Notify.Send("Opened Supermarket", Notify.NotificationType.Success);
                }
                else
                {
                    Notify.Send("Supermarket has already been opened", Notify.NotificationType.Error);
                }
            }

            if (GUILayout.Button("Close Market", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameData gameData = gameDataManager.GetComponent<GameData>();
                
                if (gameData.NetworktimeOfDay >= 23f)
                {
                    gameData.CmdEndDayFromButton();
                    Notify.Send("Closed the Supermarket", Notify.NotificationType.Success);
                }
                else
                {
                    Notify.Send("Not time to close yet", Notify.NotificationType.Error);
                }
            }

            if (GUILayout.Button("Free Expansion (NW)", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();
                
                for (int i = 0; i < 250; i++)
                {
                    upgradesManager.CmdAddStorage(i);
                }
            }

            if (GUILayout.Button("Free Storage (NW)", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();

                for (int i = 0; i < 250; i++)
                {
                    upgradesManager.CmdAddStorage(i);
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

            if (GUILayout.Button(waterBoxSpammerText, buttonStyle ,GUILayout.Height(25)))
            {
                if (waterBoxSpammer)
                {
                    waterBoxSpammer = false;
                    Notify.Send("Water Box Spammer Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    waterBoxSpammer = true;
                    Notify.Send("Water Box Spammer Enabled", Notify.NotificationType.Success);
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

            if (GUILayout.Button(everyBoxSpamText, buttonStyle ,GUILayout.Height(25)))
            {
                if (everyBoxSpam)
                {
                    everyBoxSpam = false;
                    Notify.Send("Lots of Everything Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    everyBoxSpam = true;
                    Notify.Send("Lots of Everything Enabled", Notify.NotificationType.Success);
                    if (!antiCrash)
                    {
                        antiCrash = true;
                        Notify.Send("Anti Crash Enabled", Notify.NotificationType.Success);
                    }
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

            if (GUILayout.Button(speedBoostText, buttonStyle ,GUILayout.Height(25)))
            {
                if (speedBoost)
                {
                    speedBoost = false;
                    Notify.Send("Speedboost Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    speedBoost = true;
                    Notify.Send("Speedboost Enabled", Notify.NotificationType.Success);
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

            if (GUILayout.Button(airJumpText, buttonStyle ,GUILayout.Height(25)))
            {
                if (airJump)
                {
                    airJump = false;
                    Notify.Send("Air Jump Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    airJump = true;
                    Notify.Send("Air Jump Enabled", Notify.NotificationType.Success);
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

            if (GUILayout.Button(jumpBoostText, buttonStyle ,GUILayout.Height(25)))
            {
                if (jumpBoost)
                {
                    jumpBoost = false;
                    Notify.Send("Jump Boost Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    jumpBoost = true;
                    Notify.Send("Jump Boost Enabled");
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

            if (GUILayout.Button(noJumpDelayText, buttonStyle ,GUILayout.Height(25)))
            {
                if (noJumpDelay)
                {
                    noJumpDelay = false;
                    Notify.Send("No Jump Delay Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    noJumpDelay = true;
                    Notify.Send("No Jump Delay Enabled");
                }
            }
            fov = GUILayout.HorizontalSlider(fov, 10, 120);
            if (GUILayout.Button("FOV "+Mathf.Round(fov), buttonStyle ,GUILayout.Height(25)))
            {
                foreach (AuxiliarChangeFOV fover in FindObjectsByType<AuxiliarChangeFOV>(FindObjectsSortMode.None))
                {
                    fover.SetFOV(Mathf.Round(fov));
                }
                Notify.Send("FOV Set to "+Mathf.Round(fov), Notify.NotificationType.Success);
            }
        }

        void DisplayStatsMods()
        {
            string moneyAddStringDisplay = "";

            moneyAddString = GUILayout.TextField(moneyAddString, buttonStyle ,GUILayout.Height(25));

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

            if (GUILayout.Button(moneyAddStringDisplay, buttonStyle ,GUILayout.Height(25)))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameData gameData = gameDataManager.GetComponent<GameData>();

                gameData.CmdAlterFundsWithoutExperience(moneyAdd);
                if (moneyAdd > 0)
                {
                    Notify.Send("Giving "+moneyAdd+"$", Notify.NotificationType.Success);
                }
                else
                {
                    Notify.Send("Removing "+-1*moneyAdd+"$", Notify.NotificationType.Success);
                }
                
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

            if (GUILayout.Button(moneySpamText, buttonStyle ,GUILayout.Height(25)))
            {
                if (moneySpam)
                {
                    moneySpam = false;
                    Notify.Send("Money Spam Disabled");
                }
                else
                {
                    moneySpam = true;
                    if (moneyAdd > 0)
                    {
                        Notify.Send("Spam giving "+moneyAdd+"$", Notify.NotificationType.Success);
                    }
                    else
                    {
                        Notify.Send("Spam removing "+-1*moneyAdd+"$", Notify.NotificationType.Success);
                    }
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

            if (GUILayout.Button(moneyAddStringDisplay + " Points", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();

                upgradesManager.CmdAcquirePerk(0, -pointsAdd);
                
                Notify.Send("Giving "+Math.Abs(pointsAdd)+" points", Notify.NotificationType.Success);
            }

        }

        void DisplayMapMods()
        {
            if (GUILayout.Button("Disable Barrier", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject worldBarriers = GameObject.Find("Level_Exterior/Colliders");

                if (worldBarriers != null && worldBarriers.activeSelf)
                {
                    worldBarriers.SetActive(false);
                    Notify.Send("World Borders Disabled", Notify.NotificationType.Success);
                }
                else if (worldBarriers == null)
                {
                    Notify.Send("World Borders Don't Exist", Notify.NotificationType.Error);
                }
                else if (!worldBarriers.activeSelf)
                {
                    Notify.Send("World Borders already got disabled", Notify.NotificationType.Error);
                }
                
            }

            if (GUILayout.Button("Become Cool", buttonStyle ,GUILayout.Height(25)))
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
                
                Builder_Main builderMain = GameObject.Find("GameCanvas").GetComponent<Builder_Main>();
                
                builderMain.playerIsCool = true;
                
                Traverse.Create(builderMain).Field("isCool").SetValue(true);
                Traverse.Create(builderMain).Field("canPlace").SetValue(true);
                
                builderMain.ActivateUIInfo(true);
                
                Notify.Send("You are now cool", Notify.NotificationType.Success);
            } 

            if (GUILayout.Button("No Jail", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject worldBarriers = GameObject.Find("TheCoolRoom/Jail");

                if (worldBarriers != null && worldBarriers.activeSelf)
                {
                    worldBarriers.SetActive(false);
                    Notify.Send("Jail Disabled", Notify.NotificationType.Success);
                }
                else if (worldBarriers == null)
                {
                    Notify.Send("The Jail Doesn't Exist", Notify.NotificationType.Error);
                }
                else if (!worldBarriers.activeSelf)
                {
                    Notify.Send("The Jail already got disabled", Notify.NotificationType.Error);
                }
            }
            
        }

        void DisplayServerMods()
        {
            productIDString = GUILayout.TextField(productIDString, buttonStyle ,GUILayout.Height(25));

            try
            {
                productID = int.Parse(productIDString);
            }
            catch (Exception)
            {
                
            }
            
            if (GUILayout.Button("Spawn by ID", buttonStyle ,GUILayout.Height(25)))
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

                managerBlackboard.CmdSpawnBoxFromPlayer(spawnPosition, productID, 999999999, 1f);
                
                Notify.Send("Spawned box with id "+productID, Notify.NotificationType.Success);
            }
            string idBasedBoxSpamText;

            if (idBasedBoxSpam)
            {
                idBasedBoxSpamText = "<color=green>ON</color>: Spawn by ID Spam";
            }
            else
            {
                idBasedBoxSpamText = "<color=red>OFF</color>: Spawn by ID Spam";
            }

            if (GUILayout.Button(idBasedBoxSpamText, buttonStyle ,GUILayout.Height(25)))
            {
                if (idBasedBoxSpam)
                {
                    idBasedBoxSpam = false;
                    Notify.Send("ID Box Spammer Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    idBasedBoxSpam = true;
                    Notify.Send("ID Box Spammer Enabled", Notify.NotificationType.Success);
                }
            }
            
            string idBasedBoxSpamEverywhereText;

            if (idBasedBoxSpamEverywhere)
            {
                idBasedBoxSpamEverywhereText = "<color=green>ON</color>: Spawn by ID Spam Everywhere";
            }
            else
            {
                idBasedBoxSpamEverywhereText = "<color=red>OFF</color>: Spawn by ID Spam Everywhere";
            }

            if (GUILayout.Button(idBasedBoxSpamEverywhereText, buttonStyle ,GUILayout.Height(25)))
            {
                if (idBasedBoxSpamEverywhere)
                {
                    idBasedBoxSpamEverywhere = false;
                    Notify.Send("ID Box Everywhere Spammer Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    idBasedBoxSpamEverywhere = true;
                    Notify.Send("ID Box Everywhere Spammer Enabled", Notify.NotificationType.Success);
                }
            }
            
            if (GUILayout.Button("Add Random Perks", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");
                
                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();

                int i = 0;
                foreach (bool boolCool in upgradesManager.extraUpgrades)
                {
                    upgradesManager.CmdAcquirePerk(i, 0);
                    i += 1;
                    upgradesManager.extraUpgrades[i] = false;
                }
                Notify.Send("Added Random Perks", Notify.NotificationType.Success);
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

            if (GUILayout.Button(perkSpamText, buttonStyle ,GUILayout.Height(25)))
            {
                if (perkSpam)
                {
                    perkSpam = false;
                    Notify.Send("Perk Spam Enabled", Notify.NotificationType.Success);
                }
                else
                {
                    perkSpam = true;
                    Notify.Send("Perk Spam Disabled", Notify.NotificationType.Success);
                }
            }

            if (GUILayout.Button("Add Employee", buttonStyle ,GUILayout.Height(25)))
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
                
                Notify.Send("Added an Employee", Notify.NotificationType.Success);
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

            if (GUILayout.Button(employeeSpamText, buttonStyle ,GUILayout.Height(25)))
            {
                if (employeeSpam)
                {
                    employeeSpam = false;
                    Notify.Send("Employee Spam Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    employeeSpam = true;
                    Notify.Send("Employee Spam Enabled", Notify.NotificationType.Success);
                }
            }

            if (GUILayout.Button("Push Others", buttonStyle ,GUILayout.Height(25)))
            {
                PlayerNetwork[] allPlayers = FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None);

                foreach (PlayerNetwork player in allPlayers)
                {
                    if (!player.isLocalPlayer)
                    {
                        Vector3 pushDirection = new Vector3(100, 100, 100);

                        Mods.PushPlayer(player, pushDirection);
                    }
                }
                Notify.Send("Pushed Other Players", Notify.NotificationType.Success);
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

            if (GUILayout.Button(spamPushOthersText, buttonStyle ,GUILayout.Height(25)))
            {
                if (spamPushOthers)
                {
                    spamPushOthers = false;
                    Notify.Send("Push Spam Others Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    spamPushOthers = true;
                    Notify.Send("Push Spam Others Enabled", Notify.NotificationType.Success);
                }
            }

            if (GUILayout.Button("Push Everyone", buttonStyle ,GUILayout.Height(25)))
            {
                // Find all objects with the PlayerNetwork component
                PlayerNetwork[] allPlayers = FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None);

                foreach (PlayerNetwork player in allPlayers)
                {
                    Vector3 pushDirection = new Vector3(100, 100, 100);

                    Mods.PushPlayer(player, pushDirection);
                }
                Notify.Send("Push Everyone", Notify.NotificationType.Success);
            }

            string spamPushText;

            if (spamPush)
            {
                spamPushText = "<color=green>ON</color>: Spam Push";
            }
            else
            {
                spamPushText = "<color=red>OFF</color>: Spam Push";
            }

            if (GUILayout.Button(spamPushText, buttonStyle ,GUILayout.Height(25)))
            {
                if (spamPush)
                {
                    spamPush = false;
                    Notify.Send("Push Spam Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    spamPush = true;
                    Notify.Send("Push Spam Enabled", Notify.NotificationType.Success);
                }
            }

            messageString = GUILayout.TextField(messageString, buttonStyle ,GUILayout.Height(25));

            string messageStringText;

            if (messageSpam)
            {
                messageStringText = "<color=green>ON</color>: Message Spammer";
            }
            else
            {
                messageStringText = "<color=red>OFF</color>: Message Spammer";
            }

            if (GUILayout.Button(messageStringText, buttonStyle ,GUILayout.Height(25)))
            {
                if (messageSpam)
                {
                    messageSpam = false;
                    Notify.Send("Message Spammer Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    messageSpam = true;
                    Notify.Send("Message Spammer Enabled", Notify.NotificationType.Success);
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

            if (GUILayout.Button(messageCrasherText, buttonStyle ,GUILayout.Height(25)))
            {
                if (messageCrasher)
                {
                    messageCrasher = false;
                    Notify.Send("Message Crasher Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    messageCrasher = true;
                    Notify.Send("Message Crasher Enabled", Notify.NotificationType.Success);
                    if (!antiCrash)
                    {
                        antiCrash = true;
                        Notify.Send("Anti Crash Enabled", Notify.NotificationType.Success);
                    }
                }
            }

            if (GUILayout.Button("Bright Sign", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject gameDataObject = GameObject.Find("GameDataManager");

                NetworkSpawner networkSpawner = gameDataObject.GetComponent<NetworkSpawner>();

                Color brightColor = new Color(255, 255, 255, 255);

                Color test;

                if (networkSpawner.SuperMarketColor != brightColor)
                {
                    lastColor = networkSpawner.SuperMarketColor;
                    test = new Color(255, 255, 255, 255);
                    Notify.Send("Here comes the sun", Notify.NotificationType.Success);
                }
                else
                {
                    test = lastColor;
                    Notify.Send("nvm it's gone now", Notify.NotificationType.Success);
                }

                networkSpawner.CmdSetSupermarketColor(test);
                
            }

            newSuperMarketName = GUILayout.TextField(newSuperMarketName, buttonStyle ,GUILayout.Height(25));
            if (GUILayout.Button("Change Supermarket Name", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject gameDataObject = GameObject.Find("GameDataManager");

                NetworkSpawner networkSpawner = gameDataObject.GetComponent<NetworkSpawner>();

                networkSpawner.CmdSetSupermarketText(newSuperMarketName);
                Notify.Send("Supermarket name is now "+newSuperMarketName, Notify.NotificationType.Success);
            }

            if (GUILayout.Button("Max Boxes", buttonStyle ,GUILayout.Height(25)))
            {
                BoxData[] allBoxes = FindObjectsByType<BoxData>(FindObjectsSortMode.None);

                foreach (BoxData box in allBoxes)
                {
                    box.numberOfProducts = 999999999;

                    Type type = typeof(BoxData);
                    
                    MethodInfo privateMethod =
                        type.GetMethod("SetBoxData", BindingFlags.NonPublic | BindingFlags.Instance);

                    ParameterInfo[] parameters = privateMethod.GetParameters();

                    try
                    {
                        privateMethod.Invoke(box, null);
                        Notify.Send("Set all boxes to 999999999", Notify.NotificationType.Success);
                    }
                    catch (Exception)
                    {
                        Notify.Send("Failed to set the boxes to 999999999", Notify.NotificationType.Error);   
                    }
                    
                }
            }

            if (GUILayout.Button("Water Infection", buttonStyle ,GUILayout.Height(25)))
            {
                BoxData[] allBoxes = FindObjectsByType<BoxData>(FindObjectsSortMode.None);

                foreach (BoxData box in allBoxes)
                {
                    box.numberOfProducts = 999999999;
                    box.productID = 1;

                    Type type = typeof(BoxData);
                    
                    MethodInfo privateMethod =
                        type.GetMethod("SetBoxData", BindingFlags.NonPublic | BindingFlags.Instance);
                    
                    try
                    {
                        privateMethod.Invoke(box, null);
                        Notify.Send("Set all boxes to water", Notify.NotificationType.Success);
                    }
                    catch (Exception)
                    {
                        Notify.Send("Failed to set the boxes to water", Notify.NotificationType.Error);   
                    }

                }
            }

            if (GUILayout.Button("No Product", buttonStyle ,GUILayout.Height(25)))
            {
                BoxData[] allBoxes = FindObjectsByType<BoxData>(FindObjectsSortMode.None);

                foreach (BoxData box in allBoxes)
                {
                    box.numberOfProducts = 0;

                    Type type = typeof(BoxData);
                    
                    MethodInfo privateMethod =
                        type.GetMethod("SetBoxData", BindingFlags.NonPublic | BindingFlags.Instance);
                    
                    try
                    {
                        privateMethod.Invoke(box, null);
                        Notify.Send("Set all boxes to 0", Notify.NotificationType.Success);
                    }
                    catch (Exception)
                    {
                        Notify.Send("Failed to set the boxes to 0", Notify.NotificationType.Error);   
                    }

                }
            }

            string disableMovementText;

            if (disableMovement)
            {
                disableMovementText = "<color=green>ON</color>: Disable Movement";
            }
            else
            {
                disableMovementText = "<color=red>OFF</color>: Disable Movement";
            }

            if (GUILayout.Button(disableMovementText, buttonStyle ,GUILayout.Height(25)))
            {
                if (disableMovement)
                {
                    disableMovement = false;
                    
                    Notify.Send("Disable Movement Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    disableMovement = true;
                    Notify.Send("Disable Movement Enabled", Notify.NotificationType.Success);
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

            if (GUILayout.Button(disableOthersMovementText, buttonStyle ,GUILayout.Height(25)))
            {
                if (disableOthersMovement)
                {
                    disableOthersMovement = false;
                    Notify.Send("Disable Others Movement Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    disableOthersMovement = true;
                    Notify.Send("Disable Others Movement Enabled", Notify.NotificationType.Success);
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

            if (GUILayout.Button(randomBoxSpamText, buttonStyle ,GUILayout.Height(25)))
            {
                if (randomBoxSpam)
                {
                    randomBoxSpam = false;
                    Notify.Send("Water Everywhere Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    randomBoxSpam = true;
                    Notify.Send("Water Everywhere Enabled", Notify.NotificationType.Success);
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

            if (GUILayout.Button(instantCrasherText, buttonStyle ,GUILayout.Height(25)))
            {
                if (instantCrasher)
                {
                    instantCrasher = false;
                    Notify.Send("Instant Crasher Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    instantCrasher = true;
                    Notify.Send("Instant Crasher Enabled", Notify.NotificationType.Success);
                    if (!antiCrash)
                    {
                        antiCrash = true;
                        Notify.Send("Anti Crash Enabled", Notify.NotificationType.Success);
                    }
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

            if (GUILayout.Button(boxLaggerText, buttonStyle ,GUILayout.Height(25)))
            {
                if (boxLagger)
                {
                    boxLagger = false;
                    Notify.Send("Box Crasher Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    boxLagger = true;
                    Notify.Send("Box Crasher Enabled", Notify.NotificationType.Success);
                    if (!antiCrash)
                    {
                        antiCrash = true;
                        Notify.Send("Anti Crash Enabled", Notify.NotificationType.Success);
                    }
                }
            }

            if (GUILayout.Button("Bring All Players", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject localPlayer = GameObject.Find("LocalGamePlayer");

                Vector3 SpawnPosition = new Vector3(localPlayer.transform.position.x, localPlayer.transform.position.y,
                    localPlayer.transform.position.z + 2);

                PlayerNetwork[] playerNetworks = FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None);

                foreach (PlayerNetwork playerNetwork in playerNetworks)
                {
                    if (!playerNetwork.isLocalPlayer)
                    {
                        Mods.MoveObject(playerNetwork.gameObject, SpawnPosition);
                    }
                }
                Notify.Send("Brought all player", Notify.NotificationType.Success);
            }
            
            if (GUILayout.Button("Break + Freeze All Cameras", buttonStyle ,GUILayout.Height(25)))
            {
                Vector3 SpawnPosition = new Vector3(10000000000000000, 10000000000000000, 10000000000000000);      
                            
                
                foreach (PlayerNetwork playerNetwork in FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None))
                {
                    if (!playerNetwork.isLocalPlayer)
                    {
                        Mods.MoveObject(playerNetwork.gameObject, SpawnPosition);
                    }
                }

                Notify.Send("Broke and froze everyone's camera", Notify.NotificationType.Success);
            }
            
            if (GUILayout.Button("Break All Cameras", buttonStyle ,GUILayout.Height(25)))
            {
                Vector3 SpawnPosition = new Vector3(100000000000, 100000000000, 100000000000);      
                            
                
                foreach (PlayerNetwork playerNetwork in FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None))
                {
                    if (!playerNetwork.isLocalPlayer)
                    {
                        Mods.MoveObject(playerNetwork.gameObject, SpawnPosition);
                    }
                }

                Notify.Send("Broke everyone's camera", Notify.NotificationType.Success);
            }

            if (GUILayout.Button("Fix All Cameras (Not for normal freezer)", buttonStyle ,GUILayout.Height(25)))
            {
                foreach (PlayerNetwork playerNetwork in FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None))
                {
                    Vector3 SpawnPosition = new Vector3(0, 0, 0);

                    GameObject playerObject = playerNetwork.gameObject;
                    
                    if (float.IsNaN(playerObject.transform.position.x) ||
                        float.IsNaN(playerObject.transform.position.y) ||
                        float.IsNaN(playerObject.transform.position.z))
                    {
                        
                    }
                    else
                    {
                        Mods.MoveObject(playerObject, SpawnPosition);
                    }
                }
                
                Notify.Send("Fixed everyone's cameras", Notify.NotificationType.Success);
                
            }
            
            if (GUILayout.Button("Bring All Players Out of Map", buttonStyle ,GUILayout.Height(25)))
            {
                Vector3 SpawnPosition = new Vector3(33, 0,
                    135);

                PlayerNetwork[] playerNetworks = FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None);

                foreach (PlayerNetwork playerNetwork in playerNetworks)
                {
                    Mods.MoveObject(playerNetwork.gameObject, SpawnPosition);
                }
                Notify.Send("Brought all player out of the map", Notify.NotificationType.Success);
            }

            if (GUILayout.Button("Bring All Boxes", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject localPlayer = GameObject.Find("LocalGamePlayer");

                Vector3 SpawnPosition = new Vector3(localPlayer.transform.position.x, localPlayer.transform.position.y,
                    localPlayer.transform.position.z + 2);

                BoxData[] allBoxes = FindObjectsByType<BoxData>(FindObjectsSortMode.None);

                foreach (BoxData box in allBoxes)
                {
                    Mods.MoveObject(box.gameObject, SpawnPosition);
                }
                Notify.Send("Brought all boxes", Notify.NotificationType.Success);
            }

            if (GUILayout.Button("Screen Freezer", buttonStyle ,GUILayout.Height(25)))
            {
                Vector3 SpawnPosition = new Vector3(float.NaN, float.NaN, float.NaN);

                PlayerNetwork[] playerNetworks = FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None);

                foreach (PlayerNetwork playerNetwork in playerNetworks)
                {
                    if (!playerNetwork.isLocalPlayer)
                    {
                        Mods.MoveObject(playerNetwork.gameObject, SpawnPosition);
                    }
                }
                Notify.Send("Froze everyone's screens", Notify.NotificationType.Success);
            }

            if (GUILayout.Button("Boxes To Nothing", buttonStyle ,GUILayout.Height(25)))
            {
                Vector3 SpawnPosition = new Vector3(float.NaN, float.NaN, float.NaN);

                BoxData[] boxes = FindObjectsByType<BoxData>(FindObjectsSortMode.None);

                foreach (BoxData box in boxes)
                {
                    Mods.MoveObject(box.gameObject, SpawnPosition);
                }
                Notify.Send("Brought all boxes to nothing", Notify.NotificationType.Success);
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

            if (GUILayout.Button(classicBoxSpamText, buttonStyle ,GUILayout.Height(25)))
            {
                if (classicBoxSpam)
                {
                    classicBoxSpam = false;
                    Notify.Send("Classic Box Spam Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    classicBoxSpam = true;
                    Notify.Send("Classic Box Spam Enabled", Notify.NotificationType.Success);
                }
            }

            if (GUILayout.Button("Destroy Floor Colliders", buttonStyle ,GUILayout.Height(25)))
            {
                Vector3 SpawnPosition = new Vector3(float.NaN, float.NaN, float.NaN);

                NPC_Info[] npcs = FindObjectsByType<NPC_Info>(FindObjectsSortMode.None);

                foreach (NPC_Info npc in npcs)
                {
                    Mods.MoveObject(npc.gameObject, SpawnPosition);
                }
                Notify.Send("Destroyed Floor Colliders", Notify.NotificationType.Success);
            }
            
            if (GUILayout.Button("Bring all NPCs", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject localPlayer = GameObject.Find("LocalGamePlayer");

                Vector3 SpawnPosition = new Vector3(localPlayer.transform.position.x, localPlayer.transform.position.y,
                    localPlayer.transform.position.z + 2);
                
                NPC_Info[] npcs = FindObjectsByType<NPC_Info>(FindObjectsSortMode.None);

                foreach (NPC_Info npc in npcs)
                {
                    if (npc.GetComponent<NPC_Manager>() == null)
                    {
                        Mods.MoveObject(npc.gameObject, SpawnPosition);
                    }
                }
                Notify.Send("Brought all NPCs", Notify.NotificationType.Success);
            }
            
            if (GUILayout.Button("NPCs to Nothing", buttonStyle ,GUILayout.Height(25)))
            {
                Vector3 SpawnPosition = new Vector3(float.NaN, float.NaN, float.NaN);
                
                NPC_Info[] npcs = FindObjectsByType<NPC_Info>(FindObjectsSortMode.None);

                foreach (NPC_Info npc in npcs)
                {
                    if (npc.GetComponent<NPC_Manager>() == null)
                    {
                        Mods.MoveObject(npc.gameObject, SpawnPosition);
                    }
                }
                Notify.Send("Brought all NPCs to nothing", Notify.NotificationType.Success);
            }
            
            string becomeHostText;

            if (becomeHost)
            {
                becomeHostText = "<color=green>ON</color>: Become Host (NW)";
            }
            else
            {
                becomeHostText = "<color=red>OFF</color>: Become Host (NW)";
            }

            if (GUILayout.Button(becomeHostText, buttonStyle ,GUILayout.Height(25)))
            {
                if (becomeHost)
                {
                    becomeHost = false;
                    Notify.Send("Become Host Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    becomeHost = true;
                    Notify.Send("Become Host Enabled", Notify.NotificationType.Success);
                }
            }
            if (GUILayout.Button("Enable Voicechat", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject gameDataObject = GameObject.Find("GameDataManager");
                
                NetworkGameBehaviors networkGameBehaviors = gameDataObject.GetComponent<NetworkGameBehaviors>();
                
                networkGameBehaviors.CmdServerEnableVoiceChat();
                Notify.Send("Voice Chat Enabled", Notify.NotificationType.Success);
            }
            if (GUILayout.Button("Bring all Debris", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject localPlayer = GameObject.Find("LocalGamePlayer");

                Vector3 SpawnPosition = new Vector3(localPlayer.transform.position.x, localPlayer.transform.position.y,
                    localPlayer.transform.position.z + 2);

                DemolishDebrisControl[] debrises = FindObjectsByType<DemolishDebrisControl>(FindObjectsSortMode.None);

                foreach (DemolishDebrisControl debis in debrises)
                {
                    Mods.MoveObject(debis.gameObject, SpawnPosition);
                }
                Notify.Send("Brought all Debris", Notify.NotificationType.Success);
            }
            if (GUILayout.Button("Debris to Nothing", buttonStyle ,GUILayout.Height(25)))
            {
                Vector3 SpawnPosition = new Vector3(float.NaN, float.NaN, float.NaN);

                DemolishDebrisControl[] debrises = FindObjectsByType<DemolishDebrisControl>(FindObjectsSortMode.None);

                foreach (DemolishDebrisControl debis in debrises)
                {
                    Mods.MoveObject(debis.gameObject, SpawnPosition);
                }
                Notify.Send("Brought all Debris to Nothing", Notify.NotificationType.Success);
            }
            
            if (GUILayout.Button("Bring all Store Items", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject localPlayer = GameObject.Find("LocalGamePlayer");

                Vector3 SpawnPosition = new Vector3(localPlayer.transform.position.x, localPlayer.transform.position.y,
                    localPlayer.transform.position.z + 2);

                Data_Container[] checkouts = FindObjectsByType<Data_Container>(FindObjectsSortMode.None);

                foreach (Data_Container checkout in checkouts)
                {
                    Mods.MoveObject(checkout.gameObject, SpawnPosition);
                }
                
                BuildableInfo[] buildables = FindObjectsByType<BuildableInfo>(FindObjectsSortMode.None);

                foreach (BuildableInfo buildable in buildables)
                {
                    Mods.MoveObject(buildable.gameObject, SpawnPosition);
                }
                Notify.Send("Brought all Store Items", Notify.NotificationType.Success);
            }
            if (GUILayout.Button("Store Items to Nothing", buttonStyle ,GUILayout.Height(25)))
            {
                Vector3 SpawnPosition = new Vector3(float.NaN, float.NaN, float.NaN);

                Data_Container[] checkouts = FindObjectsByType<Data_Container>(FindObjectsSortMode.None);

                foreach (Data_Container checkout in checkouts)
                {
                    Mods.MoveObject(checkout.gameObject, SpawnPosition);
                }
                
                BuildableInfo[] buildables = FindObjectsByType<BuildableInfo>(FindObjectsSortMode.None);

                foreach (BuildableInfo buildable in buildables)
                {
                    Mods.MoveObject(buildable.gameObject, SpawnPosition);
                }
                Notify.Send("Brought all Store Items to Nothing", Notify.NotificationType.Success);
            }
            
            if (GUILayout.Button("Bring all Networked Items", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject localPlayer = GameObject.Find("LocalGamePlayer");

                Vector3 SpawnPosition = new Vector3(localPlayer.transform.position.x, localPlayer.transform.position.y,
                    localPlayer.transform.position.z + 2);

                NetworkIdentity[] allItems = FindObjectsByType<NetworkIdentity>(FindObjectsSortMode.None);

                foreach (NetworkIdentity item in allItems)
                {
                    Mods.MoveObject(item.gameObject, SpawnPosition);
                }
                
                Notify.Send("Brought all Networked Items", Notify.NotificationType.Success);
            }
            if (GUILayout.Button("Networked Items to Nothing (Except Players, breaks collisions)", buttonStyle ,GUILayout.Height(25)))
            {
                Vector3 SpawnPosition = new Vector3(float.NaN, float.NaN, float.NaN);

                NetworkIdentity[] allItems = FindObjectsByType<NetworkIdentity>(FindObjectsSortMode.None);

                foreach (NetworkIdentity item in allItems)
                {
                    if (item.GetComponent<PlayerNetwork>() == null && item.GetComponent<NPC_Manager>() == null)
                    {
                        Mods.MoveObject(item.gameObject, SpawnPosition);
                    }
                }
                Notify.Send("Brought all Networked Items to Nothing", Notify.NotificationType.Success);
            }
            if (GUILayout.Button("Clear Trash", buttonStyle ,GUILayout.Height(25)))
            {
                TrashSpawn trashSpawn = FindObjectOfType<TrashSpawn>();
                
                trashSpawn.CmdClearTrash();
                Notify.Send("Cleared Trash", Notify.NotificationType.Success);
            }
            if (GUILayout.Button("Unlock Lobby", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject onlineNetworkManager = GameObject.Find("OnlineNetworkManager");
                SteamLobby steamLobby = onlineNetworkManager.GetComponent<SteamLobby>();
                
                steamLobby.SetCurrentLobbyJoinable(true);
                Notify.Send("Unlocked Lobby", Notify.NotificationType.Success);
            }
            if (GUILayout.Button("Lock Lobby", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject onlineNetworkManager = GameObject.Find("OnlineNetworkManager");
                SteamLobby steamLobby = onlineNetworkManager.GetComponent<SteamLobby>();
                
                steamLobby.SetCurrentLobbyJoinable(false);
                Notify.Send("Locked Lobby", Notify.NotificationType.Success);
            }

            
            if (GUILayout.Button("NaN Prices", buttonStyle ,GUILayout.Height(25)))
            {
                for (int i = 0; i < 290; i++)
                {
                    try
                    {
                        ProductListing.Instance.CmdUpdateProductPrice(i, float.NaN);
                    }
                    catch (Exception)
                    {
                        
                    }
                    
                }
                Notify.Send("Set all prices to NaN", Notify.NotificationType.Success);
            }
            if (GUILayout.Button("Max Prices", buttonStyle ,GUILayout.Height(25)))
            {
                for (int i = 0; i < 290; i++)
                {
                    try
                    {
                        ProductListing.Instance.CmdUpdateProductPrice(i, float.MaxValue);
                    }
                    catch (Exception)
                    {
                        
                    }
                    
                }
                Notify.Send("Maxed out all prices", Notify.NotificationType.Success);
            }
            if (GUILayout.Button("Negative Prices", buttonStyle ,GUILayout.Height(25)))
            {
                for (int i = 0; i < 290; i++)
                {
                    try
                    {
                        ProductListing.Instance.CmdUpdateProductPrice(i, -9999999999999f);
                    }
                    catch (Exception)
                    {
                        
                    }
                    
                }
                Notify.Send("Negative Prices", Notify.NotificationType.Success);
            }
            
            string priceSpazText;

            if (priceSpaz)
            {
                priceSpazText = "<color=green>ON</color>: Price Spaz";
            }
            else
            {
                priceSpazText = "<color=red>OFF</color>: Price Spaz";
            }

            if (GUILayout.Button(priceSpazText, buttonStyle ,GUILayout.Height(25)))
            {
                if (priceSpaz)
                {
                    priceSpaz = false;
                    Notify.Send("Price Spaz Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    priceSpaz = true;
                    Notify.Send("Price Spaz Enabled", Notify.NotificationType.Success);
                }
            }
            
            if (GUILayout.Button("Network Cube Spawn", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject localPlayer = GameObject.Find("LocalGamePlayer");

                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube); 
                NetworkServer.Spawn(cube, localPlayer);
                
                Notify.Send("Spawned Networked Cube", Notify.NotificationType.Success);
            }
            
            string ascendAllText;

            if (ascendAll)
            {
                ascendAllText = "<color=green>ON</color>: Ascend All";
            }
            else
            {
                ascendAllText = "<color=red>OFF</color>: Ascend All";
            }

            if (GUILayout.Button(ascendAllText, buttonStyle ,GUILayout.Height(25)))
            {
                if (ascendAll)
                {
                    ascendAll = false;
                    Notify.Send("Ascend All Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    ascendAll = true;
                    Notify.Send("Ascend All Enabled", Notify.NotificationType.Success);
                }
            }
            
            string ascendOthersText;

            if (ascendOthers)
            {
                ascendOthersText = "<color=green>ON</color>: Ascend Others";
            }
            else
            {
                ascendOthersText = "<color=red>OFF</color>: Ascend Others";
            }

            if (GUILayout.Button(ascendOthersText, buttonStyle ,GUILayout.Height(25)))
            {
                if (ascendOthers)
                {
                    ascendOthers = false;
                    Notify.Send("Ascend Others Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    ascendOthers = true;
                    Notify.Send("Ascend Others Enabled", Notify.NotificationType.Success);
                }
            }
            if (GUILayout.Button("Give Most Perms", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject localPlayerObject = GameObject.Find("LocalGamePlayer");
                
                PlayerPermissions playerPermissions = localPlayerObject.GetComponent<PlayerPermissions>();
                
                playerPermissions.RequestGP();
                playerPermissions.RequestMP();
                playerPermissions.RequestSP();
                playerPermissions.RequestTP();
                playerPermissions.RequestRP();
                playerPermissions.RequestCP();
                
                Notify.Send("Gave most perms", Notify.NotificationType.Success);
            }
            
            string theHoldingText;

            if (theHolding)
            {
                theHoldingText = "<color=green>ON</color>: The Holding";
            }
            else
            {
                theHoldingText = "<color=red>OFF</color>: The Holding";
            }

            if (GUILayout.Button(theHoldingText, buttonStyle ,GUILayout.Height(25)))
            {
                if (theHolding)
                {
                    theHolding = false;
                    Notify.Send("The Holding Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    theHolding = true;
                    Notify.Send("The Holding Enabled", Notify.NotificationType.Success);
                }
            }
            
            string theHoldingOthersText;

            if (theHoldingOthers)
            {
                theHoldingOthersText = "<color=green>ON</color>: The Holding Others";
            }
            else
            {
                theHoldingOthersText = "<color=red>OFF</color>: The Holding Others";
            }

            if (GUILayout.Button(theHoldingOthersText, buttonStyle ,GUILayout.Height(25)))
            {
                if (theHoldingOthers)
                {
                    theHoldingOthers = false;
                    Notify.Send("The Holding Others Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    theHoldingOthers = true;
                    Notify.Send("The Holding Others Enabled", Notify.NotificationType.Success);
                }
            }
            
            if (GUILayout.Button("Toggle All Checkouts", buttonStyle ,GUILayout.Height(25)))
            {
                foreach (Data_Container checkout in FindObjectsByType<Data_Container>(FindObjectsSortMode.None))
                {
                    try
                    {
                        if (checkout.gameObject.name.ToLower().Contains("checkout") && !checkout.gameObject.name.ToLower().Contains("selfcheckout") )
                        {
                            checkout.CmdCloseCheckout();
                        }
                    } catch (Exception) {}
                }
                
                Notify.Send("Toggled All Checkouts", Notify.NotificationType.Success);
            }
            
            string checkoutSpazText;

            if (checkoutSpaz)
            {
                checkoutSpazText = "<color=green>ON</color>: Checkout Spaz";
            }
            else
            {
                checkoutSpazText = "<color=red>OFF</color>: Checkout Spaz";
            }

            if (GUILayout.Button(checkoutSpazText, buttonStyle ,GUILayout.Height(25)))
            {
                if (checkoutSpaz)
                {
                    checkoutSpaz = false;
                    Notify.Send("Checkout Spaz Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    checkoutSpaz = true;
                    Notify.Send("Checkout Spaz Enabled", Notify.NotificationType.Success);
                }
            }
            if (GUILayout.Button("Force Close Lobby", buttonStyle ,GUILayout.Height(25)))
            {
                foreach (Data_Container dataContainer in FindObjectsByType<Data_Container>(FindObjectsSortMode.None))
                {
                    try
                    {
                        if (dataContainer.gameObject.name.ToLower().Contains("checkout") && !dataContainer.gameObject.name.ToLower().Contains("selfcheckout") )
                        {
                            dataContainer.DebugAdd(0,0,0);
                            dataContainer.DebugAdd(0,0,0);
                        }
                    } catch (Exception) {}
                }
                
                Notify.Send("Force closed lobby servers", Notify.NotificationType.Success);
            }
            
            string freeDestructionText;

            if (freeDestruction)
            {
                freeDestructionText = "<color=green>ON</color>: Free Destruction";
            }
            else
            {
                freeDestructionText = "<color=red>OFF</color>: Free Destruction";
            }

            if (GUILayout.Button(freeDestructionText, buttonStyle ,GUILayout.Height(25)))
            {
                if (freeDestruction)
                {
                    freeDestruction = false;
                    Notify.Send("Free Destruction Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    freeDestruction = true;
                    Notify.Send("Free Destruction Enabled", Notify.NotificationType.Success);
                }
            }
            if (GUILayout.Button("Force Close Lobby", buttonStyle ,GUILayout.Height(25)))
            {
                foreach (Data_Container dataContainer in FindObjectsByType<Data_Container>(FindObjectsSortMode.None))
                {
                    try
                    {
                        if (dataContainer.gameObject.name.ToLower().Contains("checkout") && !dataContainer.gameObject.name.ToLower().Contains("selfcheckout") )
                        {
                            dataContainer.DebugAdd(0,0,0);
                            dataContainer.DebugAdd(0,0,0);
                        }
                    } catch (Exception) {}
                }
                
                Notify.Send("Force closed lobby servers", Notify.NotificationType.Success);
            }
            string nothingSpammerText;

            if (nothingSpammer)
            {
                nothingSpammerText = "<color=green>ON</color>: Nothing Spam";
            }
            else
            {
                nothingSpammerText = "<color=red>OFF</color>: Nothing Spam";
            }

            if (GUILayout.Button(nothingSpammerText, buttonStyle ,GUILayout.Height(25)))
            {
                if (nothingSpammer)
                {
                    nothingSpammer = false;
                    Notify.Send("Nothing Spam Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    nothingSpammer = true;
                    Notify.Send("Nothing Spam Enabled", Notify.NotificationType.Success);
                }
            }
            if (GUILayout.Button("Delete All Boxes", buttonStyle ,GUILayout.Height(25)))
            {
                foreach (BoxData box in FindObjectsByType<BoxData>(FindObjectsSortMode.None))
                {
                    Mods.DeleteObject(box.gameObject);
                }
                Notify.Send("Destroyed All Boxes", Notify.NotificationType.Success);
            }
            if (GUILayout.Button("Delete All Players", buttonStyle ,GUILayout.Height(25)))
            {
                foreach (PlayerNetwork player in Mods.GetAllPlayers())
                {
                    Mods.DeleteObject(player.gameObject);
                }
                Notify.Send("Destroyed All Players", Notify.NotificationType.Success);
            }
            if (GUILayout.Button("Delete All Networked Items", buttonStyle ,GUILayout.Height(25)))
            {
                foreach (NetworkIdentity item in FindObjectsByType<NetworkIdentity>(FindObjectsSortMode.None))
                {
                    Mods.DeleteObject(item.gameObject);
                }
                Notify.Send("Destroyed All Networked Items", Notify.NotificationType.Success);
            }
            if (GUILayout.Button("Delete All Store Items", buttonStyle ,GUILayout.Height(25)))
            {
                foreach (BoxData item in FindObjectsByType<BoxData>(FindObjectsSortMode.None))
                {
                    Mods.DeleteObject(item.gameObject);
                }
                
                foreach (Data_Container item in FindObjectsByType<Data_Container>(FindObjectsSortMode.None))
                {
                    Mods.DeleteObject(item.gameObject);
                }
                
                foreach (Data_Product item in FindObjectsByType<Data_Product>(FindObjectsSortMode.None))
                {
                    Mods.DeleteObject(item.gameObject);
                }
                Notify.Send("Destroyed All Store Items", Notify.NotificationType.Success);
            }
            if (GUILayout.Button("Delete All Networked Items", buttonStyle ,GUILayout.Height(25)))
            {
                foreach (NetworkIdentity item in FindObjectsByType<NetworkIdentity>(FindObjectsSortMode.None))
                {
                    Mods.DeleteObject(item.gameObject);
                }
                Notify.Send("Destroyed All Networked Items", Notify.NotificationType.Success);
            }
            if (GUILayout.Button("Delete UI", buttonStyle ,GUILayout.Height(25)))
            {
                Mods.DeleteObject(GameObject.Find("GameCanvas"));
                Notify.Send("Destroyed UI", Notify.NotificationType.Success);
            }
            
            if (GUILayout.Button("Delete Main Components", buttonStyle ,GUILayout.Height(25)))
            {
                Mods.DeleteObject(GameObject.Find("NPC_Manager"));
                Mods.DeleteObject(GameObject.Find("SceneManager"));
                Mods.DeleteObject(GameObject.Find("Seasonal_MisterGrusch"));
                Mods.DeleteObject(GameObject.Find("ProductCheckoutSpawn"));
                Mods.DeleteObject(GameObject.Find("TrashSpawn"));
                Mods.DeleteObject(GameObject.Find("HalloweenGhost"));
                Mods.DeleteObject(GameObject.Find("GachaponCapsule_0"));
                Mods.DeleteObject(GameObject.Find("GachaponCapsule_0"));
                Mods.DeleteObject(GameObject.Find("A_NPC_Agent"));
                Mods.DeleteObject(GameObject.Find("Seasonal_MisterGift"));
                Mods.DeleteObject(GameObject.Find("Easter_Prefab"));
                Mods.DeleteObject(GameObject.Find("StolenProductSpawn"));
                Mods.DeleteObject(GameObject.Find("GameCanvas"));
                Mods.DeleteObject(GameObject.Find("TheCoolRoom/Props/Slate_t3iq3t (2)/CoolRoomCanvas/TheCoolRoom/DrawContainer"));
                
                Mods.DeleteObject(GameObject.Find("GameDataManager"));
                Notify.Send("Destroyed UI", Notify.NotificationType.Success);
            }
            string gapochanSpammerText;

            if (nothingSpammer)
            {
                nothingSpammerText = "<color=green>ON</color>: Gapochan Spammer";
            }
            else
            {
                nothingSpammerText = "<color=red>OFF</color>: Gapochan Spammer";
            }

            if (GUILayout.Button(nothingSpammerText, buttonStyle ,GUILayout.Height(25)))
            {
                if (nothingSpammer)
                {
                    nothingSpammer = false;
                    Notify.Send("Nothing Spam Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    nothingSpammer = true;
                    Notify.Send("Nothing Spam Enabled", Notify.NotificationType.Success);
                }
            }
        }

        void DisplayExtraMods()
        {
            if (GUILayout.Button("No Tutorial", buttonStyle, GUILayout.Height(25)))
            {
                GameObject tutorialObject = GameObject.Find("GameCanvas/Tutorials");

                tutorialObject.SetActive(false);

                Notify.Send("Hid Tutorial", Notify.NotificationType.Success);
            }

            if (GUILayout.Button("Scan All", buttonStyle, GUILayout.Height(25)))
            {
                ProductCheckoutSpawn[] allProducts = FindObjectsByType<ProductCheckoutSpawn>(FindObjectsSortMode.None);

                foreach (ProductCheckoutSpawn product in allProducts)
                {
                    product.CmdAddProductValueToCheckout();
                }

                Notify.Send("Scanned All Products", Notify.NotificationType.Success);
            }

            if (GUILayout.Button("Auto Checkout", buttonStyle, GUILayout.Height(25)))
            {
                ProductCheckoutSpawn[] allProducts = FindObjectsByType<ProductCheckoutSpawn>(FindObjectsSortMode.None);

                foreach (ProductCheckoutSpawn product in allProducts)
                {
                    product.CmdAddProductValueToCheckout();
                }

                Data_Container[] allCheckouts = FindObjectsByType<Data_Container>(FindObjectsSortMode.None);

                foreach (Data_Container checkout in allCheckouts)
                {
                    //checkout.CmdActivateCashMethod(checkout.);
                    //checkout.CmdActivateCreditCardMethod();
                    try
                    {
                        if (checkout.productsLeft == 0 && checkout.currentNPC != null)
                        {
                            //checkout.CmdActivateCreditCardMethod();
                            checkout.CmdReceivePayment(checkout.currentAmountToReturn);
                        }
                    }
                    catch (Exception)
                    {
                    }

                }

                Notify.Send("Auto Checked Out", Notify.NotificationType.Success);
            }

            string autoCheckoutText;

            if (autoCheckout)
            {
                autoCheckoutText = "<color=green>ON</color>: Break Checkout";
            }
            else
            {
                autoCheckoutText = "<color=red>OFF</color>: Break Checkout";
            }

            if (GUILayout.Button(autoCheckoutText, buttonStyle, GUILayout.Height(25)))
            {
                if (autoCheckout)
                {
                    autoCheckout = false;
                    Notify.Send("Break Checkout Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    autoCheckout = true;
                    Notify.Send("Break Checkout Enabled", Notify.NotificationType.Success);
                }
            }

            newUsername = GUILayout.TextField(newUsername, buttonStyle, GUILayout.Height(25));

            if (GUILayout.Button("Set Name", buttonStyle, GUILayout.Height(25)))
            {
                Mods.SetPlayerName(newUsername);
                Notify.Send("Player name set to " + newUsername, Notify.NotificationType.Success);
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

            if (GUILayout.Button(coolHeckerText, buttonStyle, GUILayout.Height(25)))
            {
                if (coolHeckerButton)
                {
                    coolHeckerButton = false;
                    Notify.Send("Cool Hecker Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    coolHeckerButton = true;
                    Notify.Send("Cool Hecker Enabled", Notify.NotificationType.Success);
                }
            }

            if (GUILayout.Button("Grab All Stollen", buttonStyle, GUILayout.Height(25)))
            {
                StolenProductSpawn[] allCheckouts = FindObjectsByType<StolenProductSpawn>(FindObjectsSortMode.None);

                foreach (StolenProductSpawn checkout in allCheckouts)
                {
                    checkout.CmdRecoverStolenProduct();
                }

                Notify.Send("Grabbed all Stolen Productws", Notify.NotificationType.Success);
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

            if (GUILayout.Button(antiCrashText, buttonStyle, GUILayout.Height(25)))
            {
                if (antiCrash)
                {
                    antiCrash = false;
                    Notify.Send("Anticrash Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    antiCrash = true;
                    Notify.Send("Anticrash Enabled", Notify.NotificationType.Success);
                }
            }

            if (GUILayout.Button("Unlock FPS", buttonStyle, GUILayout.Height(25)))
            {
                Application.targetFrameRate = 999999999;
                QualitySettings.vSyncCount = 0;
                Notify.Send("Unlocked FPS", Notify.NotificationType.Success);
            }

            if (GUILayout.Button("Force Save", buttonStyle, GUILayout.Height(25)))
            {
                GameObject SceneManager = GameObject.Find("SceneManager");

                SaveBehaviour saveBehaviour = SceneManager.gameObject.GetComponent<SaveBehaviour>();

                saveBehaviour.SavePersistentValues();

                Notify.Send("Forced Save", Notify.NotificationType.Success);
            }

            string fpsBoosterText;

            if (FPSBoostCrap.fpsBoost)
            {
                fpsBoosterText = "<color=green>ON</color>: FPS Boost";
            }
            else
            {
                fpsBoosterText = "<color=red>OFF</color>: FPS Boost";
            }

            if (GUILayout.Button(fpsBoosterText, buttonStyle, GUILayout.Height(25)))
            {
                if (FPSBoostCrap.fpsBoost)
                {
                    FPSBoostCrap.FPSBoost();
                    FPSBoostCrap.fpsBoost = false;
                    Notify.Send("FPS Boost Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    FPSBoostCrap.FPSBoost();
                    FPSBoostCrap.fpsBoost = true;
                    Notify.Send("FPS Boost Enabled", Notify.NotificationType.Success);
                }
            }

            if (GUILayout.Button("Clear Notifications", buttonStyle, GUILayout.Height(25)))
            {
                Notify.ClearNotifications();
            }

            string toggleNotificationsText;

            if (Notify.enabled)
            {
                toggleNotificationsText = "<color=green>ON</color>: Notifications";
            }
            else
            {
                toggleNotificationsText = "<color=red>OFF</color>: Notifications";
            }

            if (GUILayout.Button(toggleNotificationsText, buttonStyle, GUILayout.Height(25)))
            {
                if (Notify.enabled)
                {
                    Notify.enabled = false;
                    Notify.ClearNotifications();
                }
                else
                {
                    Notify.enabled = true;
                    Notify.Send("Notifications Enabled", Notify.NotificationType.Success);
                }
            }

            if (GUILayout.Button("Get All Achievements", buttonStyle, GUILayout.Height(25)))
            {
                for (int i = 0; i < 200; i++)
                {
                    Mods.GiveAchievement(i);
                }
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

            if (GUILayout.Button(spamHitNPCsText, buttonStyle ,GUILayout.Height(25)))
            {
                if (spamHitNPCs)
                {
                    spamHitNPCs = false;
                    Notify.Send("Spam Hit NPCs Disabled", Notify.NotificationType.Success);
                }
                else
                {
                    spamHitNPCs = true;
                    Notify.Send("Spam Hit NPCs Enabled", Notify.NotificationType.Success);
                }
            }

            if (GUILayout.Button("Hit NPCs", buttonStyle ,GUILayout.Height(25)))
            {
                NPC_Info[] allNPCs = FindObjectsByType<NPC_Info>(FindObjectsSortMode.None);

                foreach (NPC_Info npc in allNPCs)
                {
                    npc.CmdAnimationPlay(0);
                }
                Notify.Send("Hit All NPCs", Notify.NotificationType.Success);
            }
        }

        void DisplayPlayerSpecificMods()
        {
            if (Mods.InLobby())
            {
                try
                {
                    if (selectedPlayer == null)
                    {
                        foreach (PlayerNetwork player in FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None))
                        {
                            PlayerObjectController playerObjectController = player.GetComponent<PlayerObjectController>();

                            if (!playerObjectController.isLocalPlayer)
                            {
                                if (GUILayout.Button(playerObjectController.NetworkPlayerName, buttonStyle ,GUILayout.Height(25)))
                                {
                                    selectedPlayer = player;
                                    Notify.Send("Targeting "+playerObjectController.NetworkPlayerName, Notify.NotificationType.Success);
                                }
                            }
                            else
                            {
                                if (GUILayout.Button("Your Self", buttonStyle ,GUILayout.Height(25)))
                                {
                                    selectedPlayer = player;
                                    Notify.Send("Targeting your self", Notify.NotificationType.Success);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (selectedPlayer.gameObject == null)
                        {
                            selectedPlayer = null;
                        }
                        
                        GameObject localPlayer = Mods.GetPlayerObject();
                        
                        GameObject playerObject = selectedPlayer.gameObject;
                        PlayerObjectController playerObjectController = selectedPlayer.GetComponent<PlayerObjectController>();
                        PlayerSyncCharacter playerSyncController = selectedPlayer.GetComponent<PlayerSyncCharacter>();

                        if (GUILayout.Button("Go Back", buttonStyle ,GUILayout.Height(25)))
                        {
                            selectedPlayer = null;
                        }
                        
                        if (GUILayout.Button("Open Steam Profile Info", buttonStyle ,GUILayout.Height(25)))
                        {
                            Application.OpenURL("https://steamdb.info/calculator/" + selectedPlayer.GetComponent<PlayerObjectController>().NetworkPlayerSteamID);
                        }
                        
                        if (GUILayout.Button("TP to Player", buttonStyle ,GUILayout.Height(25)))
                        {
                            Vector3 SpawnPosition = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, playerObject.transform.position.z+2);      
                            
                            Mods.MoveObject(localPlayer, SpawnPosition);
                            
                            Notify.Send("TPed to "+playerObjectController.NetworkPlayerName, Notify.NotificationType.Success);
                        }
                        
                        if (GUILayout.Button("Bring Player", buttonStyle ,GUILayout.Height(25)))
                        {
                            Vector3 SpawnPosition = new Vector3(localPlayer.transform.position.x, localPlayer.transform.position.y, localPlayer.transform.position.z+2);      
                            
                            Mods.MoveObject(playerObject, SpawnPosition);
                            
                            Notify.Send("Brought "+playerObjectController.NetworkPlayerName, Notify.NotificationType.Success);
                        } 
                        
                        if (GUILayout.Button("Bring Player Out of Map", buttonStyle ,GUILayout.Height(25)))
                        {
                            Vector3 SpawnPosition = new Vector3(33, 0, 135);      
                            
                            Mods.MoveObject(playerObject, SpawnPosition);
                            
                            Notify.Send("Brought "+playerObjectController.NetworkPlayerName, Notify.NotificationType.Success);
                        }
                        
                        if (GUILayout.Button("Break Camera", buttonStyle ,GUILayout.Height(25)))
                        {
                            Vector3 SpawnPosition = new Vector3(100000000000, 100000000000, 100000000000);      
                            
                            Mods.MoveObject(playerObject, SpawnPosition);
                            
                            Notify.Send("Broke "+playerObjectController.NetworkPlayerName+" camera", Notify.NotificationType.Success);
                        }
                        
                        if (GUILayout.Button("Break + Freeze Camera", buttonStyle ,GUILayout.Height(25)))
                        {
                            Vector3 SpawnPosition = new Vector3(10000000000000000, 10000000000000000, 10000000000000000);      
                            
                            Mods.MoveObject(playerObject, SpawnPosition);
                            
                            Notify.Send("Broke + froze "+playerObjectController.NetworkPlayerName+" camera", Notify.NotificationType.Success);
                        }
                        
                        if (GUILayout.Button("Camera Fixer (Broken for normal freezer)", buttonStyle ,GUILayout.Height(25)))
                        {
                            Vector3 SpawnPosition = new Vector3(0, 0, 0);

                            if (float.IsNaN(playerObject.transform.position.x) ||
                                float.IsNaN(playerObject.transform.position.y) ||
                                float.IsNaN(playerObject.transform.position.z))
                            {
                                Notify.Send(playerObjectController.NetworkPlayerName+"'s camera is stuck broken", Notify.NotificationType.Error);
                            }
                            else
                            {
                                Mods.MoveObject(playerObject, SpawnPosition);
                            
                                Notify.Send("Fixed "+playerObjectController.NetworkPlayerName+"'s camera", Notify.NotificationType.Success);
                            }
                        }
                        
                        if (GUILayout.Button("Freeze Camera", buttonStyle ,GUILayout.Height(25)))
                        {
                            Vector3 SpawnPosition = new Vector3(float.NaN, float.NaN, float.NaN);      
                            
                            Mods.MoveObject(playerObject, SpawnPosition);
                            
                            Notify.Send("Froze "+playerObjectController.NetworkPlayerName+"'s camera", Notify.NotificationType.Success);
                        }
                        
                        if (GUILayout.Button("Destroy Player", buttonStyle ,GUILayout.Height(25)))
                        {
                            Mods.DestroyObject(selectedPlayer.gameObject);
                        }
                        
                        string ascendTargetText;

                        if (ascentTarget)
                        {
                            ascendTargetText = "<color=green>ON</color>: Ascend Player";
                        }
                        else
                        {
                            ascendTargetText = "<color=red>OFF</color>: Ascend Player";
                        }

                        if (GUILayout.Button(ascendTargetText, buttonStyle ,GUILayout.Height(25)))
                        {
                            if (ascentTarget)
                            {
                                ascentTarget = false;
                                Notify.Send("Ascend Player Disabled", Notify.NotificationType.Success);
                            }
                            else
                            {
                                ascentTarget = true;
                                Notify.Send("Ascend Player Enabled", Notify.NotificationType.Success);
                            }
                        }
                        
                        string waterSpammerText;

                        if (waterSpammerPlayerSelected)
                        {
                            waterSpammerText = "<color=green>ON</color>: Water Spammer";
                        }
                        else
                        {
                            waterSpammerText = "<color=red>OFF</color>: Water Spammer";
                        }

                        if (GUILayout.Button(waterSpammerText, buttonStyle ,GUILayout.Height(25)))
                        {
                            if (waterSpammerPlayerSelected)
                            {
                                waterSpammerPlayerSelected = false;
                                Notify.Send("Water Spammer Disabled", Notify.NotificationType.Success);
                            }
                            else
                            {
                                waterSpammerPlayerSelected = true;
                                Notify.Send("Water Spammer Enabled", Notify.NotificationType.Success);
                            }
                        }
                        string theHoldingText;

                        if (theHoldingPlayerSelected)
                        {
                            theHoldingText = "<color=green>ON</color>: The Holding";
                        }
                        else
                        {
                            theHoldingText = "<color=red>OFF</color>: The Holding";
                        }

                        if (GUILayout.Button(theHoldingText, buttonStyle ,GUILayout.Height(25)))
                        {
                            if (theHoldingPlayerSelected)
                            {
                                theHoldingPlayerSelected = false;
                                Notify.Send("The Holding Disabled", Notify.NotificationType.Success);
                            }
                            else
                            {
                                theHoldingPlayerSelected = true;
                                Notify.Send("The Holding Enabled", Notify.NotificationType.Success);
                            }
                        }
                        string spamHitText;

                        if (spamHitSelected)
                        {
                            spamHitText = "<color=green>ON</color>: Spam Hit";
                        }
                        else
                        {
                            spamHitText = "<color=red>OFF</color>: Spam Hit";
                        }

                        if (GUILayout.Button(spamHitText, buttonStyle ,GUILayout.Height(25)))
                        {
                            if (spamHitSelected)
                            {
                                spamHitSelected = false;
                                Notify.Send("Spam Hit Disabled", Notify.NotificationType.Success);
                            }
                            else
                            {
                                spamHitSelected = true;
                                Notify.Send("Spam Hit Enabled", Notify.NotificationType.Success);
                            }
                        }
                        string disableMovementText;

                        if (disableMovementSelected)
                        {
                            disableMovementText = "<color=green>ON</color>: Disable Movement";
                        }
                        else
                        {
                            disableMovementText = "<color=red>OFF</color>: Disable Movement";
                        }

                        if (GUILayout.Button(disableMovementText, buttonStyle ,GUILayout.Height(25)))
                        {
                            if (disableMovementSelected)
                            {
                                disableMovementSelected = false;
                                Notify.Send("Disable Movement Disabled", Notify.NotificationType.Success);
                            }
                            else
                            {
                                disableMovementSelected = true;
                                Notify.Send("Disable Movement Enabled", Notify.NotificationType.Success);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    GUILayout.Label(e.Message);
                }
            }
            else
            {
                ascentTarget = false;
                waterSpammerPlayerSelected = false;
                theHoldingPlayerSelected = false;
                spamHitSelected = false;
                disableMovementSelected = false;
                selectedPlayer = null;
                
                GUILayout.Label("<size=50><b><color=yellow>You are not connected to a server</color><b></size>\n");
            }
        }

        void DisplayInfoPage()
        {
            // Check if the delay has passed
            if (lastInfoPageExecutionTime == -1f || Time.time - lastInfoPageExecutionTime >= infoPageDelayTime)
            {
                lastInfoPageExecutionTime = Time.time;

                infoPageText = "";

                if (Mods.InLobby())
                {
                    infoPageText += "\n<color=#00FFFF>Player Info</color>\n\n";

                    PlayerNetwork[] allPlayers = FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None);

                    foreach (PlayerNetwork player in allPlayers)
                    {
                        try
                        {
                            if (player != null && player.gameObject != null &&
                                player.gameObject.transform.position != new Vector3(0, 0, 0) &&
                                player.gameObject.gameObject.transform.rotation != new Quaternion(0, 0, 0, 0) &&
                                player.netId != null)
                            {
                                PlayerObjectController playerObjectController =
                                    player.GetComponent<PlayerObjectController>();
                                PlayerSyncCharacter playerSyncController = player.GetComponent<PlayerSyncCharacter>();

                                if (!player.isLocalPlayer)
                                {
                                    infoPageText += "<color=yellow>Name: " + playerObjectController.NetworkPlayerName +
                                                    "</color>\n";
                                }

                                infoPageText += "<color=yellow>Position: " + player.gameObject.transform.position +
                                                "</color>\n";
                                infoPageText += "<color=yellow>Velocity: " + playerSyncController.playerVelocity +
                                                "</color>\n";
                                infoPageText += "<color=#00FFFF>Is Host: " + player.authority + "</color>\n";
                                infoPageText += "<color=#00FFFF>Net ID: " + player.netId + "</color>\n";
                                infoPageText += "<color=#00FFFF>Player ID: " + playerObjectController.PlayerIdNumber +
                                                "</color>\n";
                                infoPageText += "<color=#00FFFF>Steam ID: " +
                                                playerObjectController.NetworkPlayerSteamID + "</color>\n";
                                infoPageText += "<color=#00FFFF>Steam URL: https://steamcommunity.com/profiles/" +
                                                playerObjectController.NetworkPlayerSteamID + "</a></color>\n";
                                infoPageText += "<color=yellow>Is Crouching: " + player.isCrouching + "</color>\n";
                                infoPageText += "<color=green>Character ID: " + player.characterID + "</color>\n";
                                infoPageText += "<color=green>Broom ID: " + playerSyncController.broomSkin +
                                                "</color>\n";
                                infoPageText += "<color=green>Hat ID: " + player.hatID + "</color>\n";
                                if (player.isLocalPlayer)
                                {

                                }

                                infoPageText += "==================================\n\n";
                            }
                            else
                            {
                                infoPageText += "Failed to get player info.\n";
                                infoPageText += "==================================\n\n";
                            }
                        }
                        catch (Exception)
                        {
                            
                        }
                    }

                    infoPageText += "</size></b>";
                }
                else
                {
                    infoPageText += "<size=50><b><color=yellow>You are not connected to a server</color><b></size>\n";
                }
            }

            GUILayout.Label(infoPageText);
        }
        
        void DisplayDebugMods()
        {
            if (GUILayout.Button("Dump Lobby Data", buttonStyle ,GUILayout.Height(25)))
            {
                GameObject onlineNetworkManager = GameObject.Find("OnlineNetworkManager");
                SteamLobby steamLobby = onlineNetworkManager.GetComponent<SteamLobby>();

                CSteamID lobbyCode = new CSteamID(steamLobby.CurrentLobbyID);
                
                string text = "      Start of dump      \n---------------------\n";
                
                Filestuff filestuff = new Filestuff();

                for (int i = 0; i < SteamMatchmaking.GetLobbyDataCount(lobbyCode); i++)
                {
                    bool success = SteamMatchmaking.GetLobbyDataByIndex(lobbyCode, i, out string key, 256, out string value, 256);
                    if (success)
                    {
                        text += key + ": " + value;
                        text += "\n";
                    }
                }
                
                filestuff.WriteToFile("lobby_data_dump_"+steamLobby.CurrentLobbyIDStr+".txt", text);
                Notify.Send("Dumped Lobby Data", Notify.NotificationType.Success);
            }
            if (GUILayout.Button("Dump Product IDs", buttonStyle ,GUILayout.Height(25)))
            {
                
                GameObject onlineNetworkManager = GameObject.Find("OnlineNetworkManager");
                SteamLobby steamLobby = onlineNetworkManager.GetComponent<SteamLobby>();

                CSteamID lobbyCode = new CSteamID(steamLobby.CurrentLobbyID);
                
                string text = "      Start of dump      \n---------------------\n";
                
                Filestuff filestuff = new Filestuff();

                for (int i = 0; i < SteamMatchmaking.GetLobbyDataCount(lobbyCode); i++)
                {
                    bool success = SteamMatchmaking.GetLobbyDataByIndex(lobbyCode, i, out string key, 256, out string value, 256);
                    if (success)
                    {
                        text += key + ": " + value;
                        text += "\n";
                    }
                }
                
                filestuff.WriteToFile("lobby_data_dump_"+steamLobby.CurrentLobbyIDStr+".txt", text);
                Notify.Send("Dumped Product IDs", Notify.NotificationType.Success);
            }
        }

        private void OnDestroy()
        {
            SteamAPI.Shutdown();
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
                if (!Cursor.visible)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }

            /*
            if (Mathf.Ceil(1f / Time.unscaledDeltaTime) < 10)
            {
                antiCrash = true;
            }
            */

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

                for (int i = 0; i < 15; i++)
                {
                    managerBlackboard.CmdSpawnBoxFromPlayer(spawnPosition, 1, 999999999, 1f);
                }
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
                PlayerNetwork[] allPlayers = FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None);

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
                PlayerNetwork[] allPlayers = FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None);

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

                for (int i = 0; i < 15; i++)
                {
                    playerObjectController.SendChatMsg(messageString);
                }
            }

            if (autoCheckout)
            {
                // Find all objects with the PlayerNetwork component
                ProductCheckoutSpawn[] allProducts = FindObjectsByType<ProductCheckoutSpawn>(FindObjectsSortMode.None);

                foreach (ProductCheckoutSpawn product in allProducts)
                {
                    product.CmdAddProductValueToCheckout();
                }

                // Find all objects with the PlayerNetwork component
                Data_Container[] allCheckouts = FindObjectsByType<Data_Container>(FindObjectsSortMode.None);

                foreach (Data_Container checkout in allCheckouts)
                {
                    try
                    {
                        if (checkout.productsLeft == 0 && checkout.currentNPC != null)
                        {
                            //checkout.CmdActivateCreditCardMethod();
                            checkout.CmdReceivePayment(checkout.currentAmountToReturn);
                        } 
                    }catch (Exception){}
                    //checkout.CmdActivateCashMethod(checkout.);
                    //checkout.CmdActivateCreditCardMethod();
                    
                }
            }

            if (coolHeckerButton)
            {
                // Find the GameObject
                GameObject localPlayer = GameObject.Find("LocalGamePlayer");

                PlayerNetwork[] allPlayers = FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None);

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
                    NPC_Info[] allNPCs = FindObjectsByType<NPC_Info>(FindObjectsSortMode.None);

                    foreach (NPC_Info npc in allNPCs)
                    {
                        npc.CmdAnimationPlay(0);
                    }
                }

                for (int i = 0; i < 15; i++)
                {
                    playerObjectController.SendChatMsg("GET HACKED DIA MENU ON TOP");
                }

                gameData.CmdAlterFundsWithoutExperience(-10000000000f);
                networkSpawner.CmdSetSupermarketText("HACKEDBOZO");
                networkSpawner.CmdSetSupermarketColor(Color.red);

                for (int i = 0; i < 1000; i++)
                {
                    upgradesManager.CmdAcquirePerk(i, 999999999);
                }

                // Find all objects with the PlayerNetwork component
                ProductCheckoutSpawn[] allProducts = FindObjectsByType<ProductCheckoutSpawn>(FindObjectsSortMode.None);

                foreach (ProductCheckoutSpawn product in allProducts)
                {
                    product.CmdAddProductValueToCheckout();
                }

                // Find all objects with the PlayerNetwork component
                Data_Container[] allCheckouts = FindObjectsByType<Data_Container>(FindObjectsSortMode.None);

                foreach (Data_Container checkout in allCheckouts)
                {
                    //checkout.CmdActivateCashMethod(checkout.);
                    //checkout.CmdActivateCreditCardMethod();
                    if (checkout.productsLeft == 0 && checkout.currentNPC != null)
                    {
                        try
                        {
                            checkout.CmdReceivePayment(checkout.currentAmountToReturn);
                        } catch (Exception){}
                    }
                }

                Vector3 SpawnPosition = new Vector3(float.NaN, float.NaN, float.NaN);

                /*
                Data_Container[] checkouts = FindObjectsByType<Data_Container>();

                foreach (Data_Container checkout in checkouts)
                {
                    Mods.MoveObject(checkout.gameObject, SpawnPosition);
                }
                */

                BuildableInfo[] buildables = FindObjectsByType<BuildableInfo>(FindObjectsSortMode.None);

                foreach (BuildableInfo buildable in buildables)
                {
                    Mods.MoveObject(buildable.gameObject, SpawnPosition);
                }

                NPC_Info[] npcs = FindObjectsByType<NPC_Info>(FindObjectsSortMode.None);

                foreach (NPC_Info npc in npcs)
                {
                    Mods.MoveObject(npc.gameObject, SpawnPosition);
                }
            }

            if (disableOthersMovement)
            {
                PlayerNetwork[] allPlayers = FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None);

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
                PlayerNetwork[] allPlayers = FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None);

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

                PlayerNetwork[] allPlayers = FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None);

                PlayerObjectController playerObjectController = localPlayer.GetComponent<PlayerObjectController>();

                GameObject gameDataManager = GameObject.Find("GameDataManager");

                GameData gameData = gameDataManager.GetComponent<GameData>();
                NetworkSpawner networkSpawner = gameDataManager.GetComponent<NetworkSpawner>();
                UpgradesManager upgradesManager = gameDataManager.GetComponent<UpgradesManager>();
                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();

                foreach (PlayerNetwork player in allPlayers)
                {
                    for (int i = 0; i < 15; i++)
                    {
                        Vector3 playerPosition = player.gameObject.transform.position;

                        Vector3 spawnPosition = new Vector3(playerPosition.x + 2f, playerPosition.y, playerPosition.z);

                        managerBlackboard.CmdSpawnBoxFromPlayer(spawnPosition, 1, 999999999, 1f);
                    }
                }
            }

            if (antiTheft)
            {
                // Find all objects with the PlayerNetwork component
                NPC_Info[] allNPCs = FindObjectsByType<NPC_Info>(FindObjectsSortMode.None);
                StolenProductSpawn[] allCheckouts = FindObjectsByType<StolenProductSpawn>(FindObjectsSortMode.None);

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
                NPC_Info[] allNPCs = FindObjectsByType<NPC_Info>(FindObjectsSortMode.None);

                foreach (NPC_Info npc in allNPCs)
                {
                    npc.CmdAnimationPlay(0);
                }
            }

            if (instantCrasher)
            {
                PlayerNetwork[] allPlayers = FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None);
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
                GameObject playerObject = GameObject.Find("LocalGamePlayer");
                BoxData[] allBoxes = FindObjectsByType<BoxData>(FindObjectsSortMode.None);

                // Box Based Crasher Reducer
                foreach (BoxData box in allBoxes)
                {
                    box.gameObject.SetActive(false);
                }

                // Chat Based Crasher Ruducer
                GameObject chatObject = GameObject.Find("GameCanvas/ChatContainer");

                if (chatObject != null && chatObject.activeSelf)
                {
                    chatObject.SetActive(false);
                }

                // Position Based Crashers Reduced
                if (!float.IsNaN(playerObject.transform.position.x) &&
                    !float.IsNaN(playerObject.transform.position.y) &&
                    !float.IsNaN(playerObject.transform.position.z) && playerObject.transform.position.x < 10000f &&
                    playerObject.transform.position.y < 10000f && playerObject.transform.position.z < 10000f &&
                    !float.IsInfinity(playerObject.transform.position.x) &&
                    !float.IsInfinity(playerObject.transform.position.y) &&
                    !float.IsInfinity(playerObject.transform.position.z))
                {
                    lastPlayerPosition = playerObject.transform.position;
                }
                else
                {
                    Mods.MoveObject(playerObject, lastPlayerPosition);
                }

                // NaN Gameobject based
                GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
                bool nanFound = false;

                if (nanFound && Time.time - antiCrashGameObjectTime >= antiCrashGameObjectDelay)
                {
                    foreach (GameObject coolObject in allObjects)
                    {
                        if (coolObject != null || coolObject.activeInHierarchy &&
                            coolObject.GetComponent<FirstPersonController>())
                        {
                            Vector3 position = coolObject.transform.position;

                            if (float.IsNaN(position.x) || float.IsNaN(position.y) || float.IsNaN(position.z) ||
                                float.IsInfinity(position.x) || float.IsInfinity(position.y) ||
                                float.IsInfinity(position.z))
                            {
                                coolObject.SetActive(false);
                            }
                        }
                    }

                    antiCrashGameObjectTime = -1f;
                }
                else if (!nanFound)
                {
                    antiCrashGameObjectTime = -1f;
                }



                // Debris Crash Reducer
                DemolishDebrisControl[] debrises = FindObjectsByType<DemolishDebrisControl>(FindObjectsSortMode.None);

                foreach (DemolishDebrisControl debis in debrises)
                {
                    GameObject debrisObject = debis.gameObject;

                    debrisObject.SetActive(false);
                }

                if (!finishedAntiCrash)
                {
                    // Boost the FPS with crap graphics
                    //FPSBoostCrap.FPSBoost();
                    //FPSBoostCrap.fpsBoost = false;

                    finishedAntiCrash = true;
                }
            }
            else
            {
                if (finishedAntiCrash)
                {
                    // Turns back on the chat
                    GameObject chatObject = GameObject.Find("GameCanvas/ChatContainer");

                    chatObject.SetActive(true);

                    // Decrapify the graphics
                    //FPSBoostCrap.FPSBoost();
                    //FPSBoostCrap.fpsBoost = true;

                    finishedAntiCrash = false;
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

                FieldInfo activeField =
                    typeof(NetworkServer).GetField("active", BindingFlags.NonPublic | BindingFlags.Static);
                activeField.SetValue(null, true);

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

            if (ascendAll)
            {
                foreach (PlayerNetwork player in FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None))
                {
                    Mods.MoveObject(player.gameObject,
                        new Vector3(player.gameObject.transform.position.x, player.gameObject.transform.position.y + 5f,
                            player.gameObject.transform.position.z));
                }
            }

            if (ascendOthers)
            {
                foreach (PlayerNetwork player in FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None))
                {
                    if (!player.isLocalPlayer)
                    {
                        Mods.MoveObject(player.gameObject,
                            new Vector3(player.gameObject.transform.position.x,
                                player.gameObject.transform.position.y + 5f, player.gameObject.transform.position.z));
                    }
                }
            }

            if (idBasedBoxSpam)
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();

                GameObject playerObject = GameObject.Find("LocalGamePlayer");

                Vector3 playerPosition = playerObject.transform.position;

                Vector3 spawnPosition = new Vector3(playerPosition.x + 2f, playerPosition.y, playerPosition.z);

                for (int i = 0; i < 15; i++)
                {
                    managerBlackboard.CmdSpawnBoxFromPlayer(spawnPosition, productID, 999999999, 1f);
                }
            }

            if (idBasedBoxSpamEverywhere)
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();

                for (int i = 0; i < 15; i++)
                {
                    foreach (PlayerNetwork player in FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None))
                    {
                        Vector3 spawnPosition = new Vector3(player.transform.position.x + 2f,
                            player.transform.position.y, player.transform.position.z);

                        managerBlackboard.CmdSpawnBoxFromPlayer(spawnPosition, productID, 999999999, 1f);
                    }
                }
            }

            if (ascentTarget)
            {
                Mods.MoveObject(selectedPlayer.gameObject,
                    new Vector3(selectedPlayer.gameObject.transform.position.x,
                        selectedPlayer.gameObject.transform.position.y + 5f,
                        selectedPlayer.gameObject.transform.position.z));
            }

            if (waterSpammerPlayerSelected)
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();

                Vector3 playerPosition = selectedPlayer.gameObject.transform.position;

                for (int i = 0; i < 15; i++)
                {
                    Vector3 spawnPosition = new Vector3(playerPosition.x + 2f, playerPosition.y, playerPosition.z);

                    managerBlackboard.CmdSpawnBoxFromPlayer(spawnPosition, 1, 999999999, 1f);
                }
            }
            
            if (theHolding)
            {
                foreach (PlayerNetwork player in FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None))
                {
                    Mods.MoveObject(player.gameObject, new Vector3(-15, 15, -20));
                }
            }

            if (theHoldingOthers)
            {
                foreach (PlayerNetwork player in FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None))
                {
                    if (!player.isLocalPlayer)
                    {
                        Mods.MoveObject(player.gameObject, new Vector3(-15, 15, -20));
                    }
                }
            }

            if (theHoldingPlayerSelected)
            {
                Mods.MoveObject(selectedPlayer.gameObject, new Vector3(-15, 15, -20));
            }

            if (checkoutSpaz)
            {
                foreach (Data_Container checkout in FindObjectsByType<Data_Container>(FindObjectsSortMode.None))
                {
                    try
                    {
                        if (checkout.gameObject.name.ToLower().Contains("checkout") && !checkout.gameObject.name.ToLower().Contains("selfcheckout") )
                        {
                            checkout.CmdCloseCheckout();
                        }
                    } catch (Exception) {}
                }
            }

            if (spamHitSelected)
            {
                Vector3 pushDirection = new Vector3(100, 100, 100);
                
                Mods.PushPlayer(selectedPlayer, pushDirection);
            }
            
            if (disableMovementSelected)
            {
                Vector3 pushDirection = new Vector3(0, 0, 0);
                
                Mods.PushPlayer(selectedPlayer, pushDirection);
            }

            if (freeDestruction)
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                DemolishableManager demolishableManager = gameDataManager.GetComponent<DemolishableManager>();

                for (int i = 0; i < demolishableManager.demolishableValues.Length; i++)
                {
                    demolishableManager.demolishingCosts[i] = 0f;
                }
            }
            
            if (nothingSpammer)
            {
                GameObject gameDataManager = GameObject.Find("GameDataManager");

                ManagerBlackboard managerBlackboard = gameDataManager.GetComponent<ManagerBlackboard>();

                for (int i = 0; i < 15; i++)
                {
                    managerBlackboard.CmdSpawnBoxEmpty();
                }
            }

            if (priceSpaz)
            {
                for (int i = 0; i < 290; i++)
                {
                    try
                    {
                        ProductListing.Instance.CmdUpdateProductPrice(i, (float)(new Random().NextDouble() * (0f - 100000000000000f) + 0f));
                    }
                    catch (Exception)
                    {
                        
                    }
                }
            }
        }
    }
}