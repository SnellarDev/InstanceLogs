using HarmonyLib;
using MelonLoader;
using Photon.Pun;
using Photon.Realtime;
using System;
using Il2CppSystem.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib;
using VRC.SDKBase;
using UnityEngine;
using VRC.Core;
using ExitGames.Client.Photon;

namespace InstanceLogs
{
    public class LogMain : MelonMod
    {
        public override void OnApplicationStart()
        {
            Hook();
            if (!Directory.Exists(StellarFile))
            {
                MelonLogger.Msg("Log folder was not found, creating a new one...");
                Directory.CreateDirectory(StellarFile);
                FilesCheck();
            }
            else
                FilesCheck();
        }
        private static void OnJoinedRoom()
        {
            LogExtensions.LogRoomInfo();
            LogExtensions.LogWorld();
            try
            {
                foreach (KeyValuePair<int, Player> keyValuePair in LogExtensions.PhotonRoom.field_Private_Dictionary_2_Int32_Player_0)
                {
                    Num++;
                    LogExtensions.LogPlayer(keyValuePair.Value, true);
                    if(Num == LogExtensions.PhotonRoom.field_Private_Dictionary_2_Int32_Player_0.count)
                    {
                        SearchAvatars = true;
                        Num = 0;
                    }
                }
            }
            catch { }
        }
        private static void OnLeftRoom()
        {
            SearchAvatars = false;
        }

        public static void OnPlayerJoin(IntPtr thisPtr, IntPtr playerJoinPtr, IntPtr _MethodInfo)
        {
            if (playerJoinPtr == IntPtr.Zero)
            {
                oldDelegate(thisPtr, playerJoinPtr, _MethodInfo);
                return;
            }

            var photonplayer = new Player(playerJoinPtr);
            LogExtensions.LogPlayer(photonplayer, false);
        }

        public static unsafe void Hook()
        {
            var patchDelegate1 = new EventDelegate(OnPlayerJoin);
            var originalMethodPtr1 = *(IntPtr*)(IntPtr)UnhollowerUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod(typeof(NetworkManager).GetMethod(nameof(NetworkManager.Method_Public_Virtual_Final_New_Void_Player_1), BindingFlags.Public | BindingFlags.Instance)).GetValue(null);
            MelonUtils.NativeHookAttach((IntPtr)(&originalMethodPtr1), Marshal.GetFunctionPointerForDelegate(patchDelegate1));
            try { Instance.Patch(AccessTools.Method(typeof(NetworkManager), "OnJoinedRoom", null, null), GetPatch("OnJoinedRoom")); } catch (Exception e) { MelonLogger.Error($"Error Patching OnJoinedRoom => {e.Message}"); }
            try { Instance.Patch(AccessTools.Method(typeof(NetworkManager), "OnLeftRoom"), GetPatch("OnLeftRoom")); } catch (Exception e) { MelonLogger.Error($"Error Patching OnLeftRoom => {e.Message}"); }
            try { Instance.Patch(AccessTools.Method(typeof(LoadBalancingClient), "OnEvent", null, null), GetPatch("OnEvent")); } catch (Exception e) { MelonLogger.Error($"Error Patching OnEvent => {e.Message}"); }
        }

        private static bool OnEvent(ref EventData __0)
        {
            try
            {
                if (__0.Code == 253 && SearchAvatars)
                    LogExtensions.LogAvatar(__0);
            }
            catch { }

            return true;
        }

        public static void FilesCheck()
        {
            if (!File.Exists(PlayerLogs))
                File.AppendAllText(PlayerLogs, "[PLAYER LOGS]\nLogs of all Players you've encountered(The Bottom is the most recent log)\nby Stellar\n\n\n");

            if (!File.Exists(AvatarLogs))
                File.AppendAllText(AvatarLogs, "[AVATAR LOGS]\nLogs of all Avatars you've encountered(The Bottom is the most recent log)\nby Stellar\n\n\n");

            if (!File.Exists(WorldLogs))
                File.AppendAllText(WorldLogs, "[WORLD LOGS]\nLogs of all Instances you've joined(The Bottom is the most recent log)\nby Stellar\n\n\n");
        }

        private static HarmonyMethod GetPatch(string name)
        {
            return new HarmonyMethod(typeof(LogMain).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));
        }

        public static string StellarFile = Directory.GetCurrentDirectory() + @"\InstanceLogs";

        public static string PlayerLogs = StellarFile + @"\PlayerLogs.txt";
        
        public static string AvatarLogs = StellarFile + @"\AvatarLogs.txt";
        
        public static string WorldLogs = StellarFile + @"\WorldLogs.txt";

        public static int Num = 0;

        public static bool SearchAvatars;

        public delegate void EventDelegate(IntPtr thisPtr, IntPtr playerjoinptr, IntPtr nativeMethodInfo); // I looked at how requi did his native hook

        public static EventDelegate oldDelegate = null;

        public static HarmonyLib.Harmony Instance = new HarmonyLib.Harmony("Patches");
    }
}
