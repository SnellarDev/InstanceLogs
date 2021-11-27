using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.SDKBase;
using VRC.UI;

namespace InstanceLogs
{
    internal static class PlayerExtensions
    {
        public static VRCPlayer LocalVRCPlayer
        {
            get
            {
                return VRCPlayer.field_Internal_Static_VRCPlayer_0;
            }
        }
        public static Player LocalPlayer
        {
            get
            {
                return Player.prop_Player_0;
            }
        }

        public static APIUser LocalAPIUser
        {
            get
            {
                return APIUser.CurrentUser;
            }
        }

        public static USpeaker LocalUSpeaker
        {
            get
            {
                return LocalVRCPlayer.field_Private_USpeaker_0;
            }
        }

        public static Animator GetAnimator(this VRCPlayer player)
        {
            return player.field_Internal_Animator_0;
        }

        public static VRCPlayerApi LocalVRCPlayerAPI
        {
            get
            {
                return LocalVRCPlayer.field_Private_VRCPlayerApi_0;
            }
        }

        public static PlayerManager PManager
        {
            get
            {
                return PlayerManager.field_Private_Static_PlayerManager_0;
            }
        }

        public static List<Player> AllPlayers
        {
            get
            {
                return PManager.field_Private_List_1_Player_0.ToArray().ToList<Player>();
            }
        }

        public static ApiAvatar GetAPIAvatar(this Player player)
        {
            return player.prop_ApiAvatar_0;
        }

        public static PlayerNet GetPlayerNet(this Player player)
        {
            return player.prop_PlayerNet_0;
        }

        public static USpeaker GetUSpeaker(this Player player)
        {
            return player.prop_USpeaker_0;
        }

        public static VRCPlayerApi GetVRCPlayerApi(this VRC.Player player)
        {
            return player.field_Private_VRCPlayerApi_0;
        }

        public static APIUser GetAPIUser(this Player player)
        {
            return player.prop_APIUser_0;
        }

        public static bool IsQuest(this Player player, bool v)
        {
            return player.GetAPIUser().IsOnMobile;
        }

        private static void SendRPCWithInt(string Name, int NUM)
        {
            RPC.Destination destination = 0;
            GameObject gameObject = LocalVRCPlayer.gameObject;
            Il2CppSystem.Object[] array = new Il2CppSystem.Object[1];
            int num = 0;
            Il2CppSystem.Int32 @int = default(Il2CppSystem.Int32);
            @int.m_value = NUM;
            array[num] = @int.BoxIl2CppObject();
            Networking.RPC(destination, gameObject, Name, array);
        }

        //public static void ToggleBlock(this Player player)
        //{
        //    PageUserInfo pageUserInfo = player.GetPageUserInfo();
        //    bool flag = !player.IsLocalPlayer();
        //    if (flag)
        //    {
        //        pageUserInfo.ToggleBlock();
        //    }
        //}

        //public static void ToggleMute(this Player player)
        //{
        //    PageUserInfo pageUserInfo = player.GetPageUserInfo();
        //    bool flag = !player.IsLocalPlayer();
        //    if (flag)
        //    {
        //        pageUserInfo.ToggleMute();
        //    }
        //}

        //private static PageUserInfo GetPageUserInfo(this Player player)
        //{
        //    PageUserInfo component = GameObject.Find("Screens").transform.Find("UserInfo").GetComponent<PageUserInfo>();
        //    component.field_Public_APIUser_0 = new APIUser
        //    {
        //        id = player.GetAPIUser().id
        //    };
        //    return component;
        //}

        public static void ConfigurePortal(string WorldID, string InstanceID, int Players, Vector3 Position, Quaternion Rotation)
        {
            GameObject gameObject = Networking.Instantiate(0, "Portals/PortalInternalDynamic", Position, Rotation);
            RPC.Destination destination = (RPC.Destination)7;
            GameObject gameObject2 = gameObject;
            string text = "ConfigurePortal";
            Il2CppSystem.Object[] array = new Il2CppSystem.Object[3];
            array[0] = WorldID;
            array[1] = InstanceID;
            int num = 2;
            Il2CppSystem.Int32 @int = default(Il2CppSystem.Int32);
            @int.m_value = Players;
            array[num] = @int.BoxIl2CppObject();
            Networking.RPC(destination, gameObject2, text, array);
        }

        public static void SendVRCEvent(VRC_EventHandler.VrcEvent vrcEvent, VRC_EventHandler.VrcBroadcastType type, GameObject instagator)
        {
            bool flag = handler == null;
            if (flag)
            {
                handler = Resources.FindObjectsOfTypeAll<VRC_EventHandler>().FirstOrDefault<VRC_EventHandler>();
            }
            vrcEvent.ParameterObject = Player.prop_Player_0.prop_USpeaker_0.gameObject;
            handler.TriggerEvent(vrcEvent, type, instagator, 0f);
        }

        public static GameObject InstantiatePrefab(string PrefabNAME, Vector3 position, Quaternion rotation)
        {
            return Networking.Instantiate(0, PrefabNAME, position, rotation);
        }

        public static void Mute(bool toggle)
        {
            DefaultTalkController.field_Private_Static_Boolean_0 = toggle;
        }

        public static void SetVolume(this Player player, float vol)
        {
            player.GetUSpeaker().field_Private_SimpleAudioGain_0.field_Public_Single_0 = vol;
        }

        public static float GetVolume(this Player player)
        {
            return player.GetUSpeaker().field_Private_SimpleAudioGain_0.field_Public_Single_0;
        }

        public static bool IsLocalMuted(this Player player)
        {
            return player.GetVolume() == 0f;
        }

        public static void LocalMute(this Player player)
        {
            player.SetVolume(0f);
        }

        public static void LocalUnMute(this Player player)
        {
            player.SetVolume(1f);
        }

        public static bool IsInVR(this Player player)
        {
            return player.GetVRCPlayerApi().IsUserInVR();
        }

        public static void Teleport(this Player player)
        {
            LocalVRCPlayer.transform.position = player.GetVRCPlayer().transform.position;
        }

        /*public static void ReloadAvatar(this Player player)
        {
            VRCPlayer.Method_Public_Static_Void_APIUser(player.GetAPIUser());
        }*/

        public static void ReloadAllAvatars()
        {
            LocalVRCPlayer.Method_Public_Void_Boolean_0(false);
        }
        public static Player GetPlayerByActor(int id)
        {
            return (from p in PlayerExtensions.AllPlayers
                    where p.GetVRCPlayerApi().playerId == id
                    select p).FirstOrDefault<Player>();
        }

        public static VRCPlayer GetVRCPlayer(this Player player)
        {
            return player.prop_VRCPlayer_0;
        }

        public static VRCAvatarManager GetVRCAvatarManager(this VRCPlayer player)
        {
            return player.prop_VRCAvatarManager_0;
        }

        public static string GetName(this Player player)
        {
            return player.GetAPIUser().displayName;
        }

        public static float LocalGain
        {
            get
            {
                return USpeaker.field_Internal_Static_Single_1;
            }
            set
            {
                USpeaker.field_Internal_Static_Single_1 = value;
            }
        }

        public static float AllGain
        {
            get
            {
                return USpeaker.field_Internal_Static_Single_0;
            }
            set
            {
                USpeaker.field_Internal_Static_Single_0 = value;
            }
        }

        public static float DefaultGain
        {
            get
            {
                return 1f;
            }
        }

        public static float MaxGain
        {
            get
            {
                return float.MaxValue;
            }
        }

        public static bool IsMaster(this Player player)
        {
            return player.GetVRCPlayerApi().isMaster;
        }

        public static int GetActorNumber(this VRC.Player player)
        {
            return player.GetVRCPlayerApi().playerId;
        }

        public static int GetPlayerFrames(this Player player)
        {
            return (player.GetPlayerNet().field_Private_Byte_0 != 0) ? ((int)(1000f / (float)player.GetPlayerNet().field_Private_Byte_0)) : 0;
        }

        public static int GetPlayerPing(this Player player)
        {
            return (int)player.GetPlayerNet().field_Private_Int16_0;
        }

        public static void ChangeAvatar(string AvatarID)
        {
            PageAvatar component = GameObject.Find("Screens").transform.Find("Avatar").GetComponent<PageAvatar>();
            component.field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0 = new ApiAvatar
            {
                id = AvatarID
            };
            component.ChangeToSelectedAvatar();
        }

        public static void SetLocalPlayerWalkSpeed(float speed)
        {
            LocalPlayer.GetVRCPlayerApi().SetWalkSpeed(speed);
        }

        public static void SetLocalPlayerWalkSpeed()
        {
            LocalPlayer.GetVRCPlayerApi().SetWalkSpeed(0f);
        }

        public static void SetLocalPlayerRunSpeed(float speed)
        {
            LocalPlayer.GetVRCPlayerApi().SetRunSpeed(speed);
        }

        public static void SetLocalPlayerRunSpeed()
        {
            LocalPlayer.GetVRCPlayerApi().SetRunSpeed(0f);
        }

        public static void SetLocalPlayerStrafeSpeed(float speed)
        {
            LocalPlayer.GetVRCPlayerApi().SetStrafeSpeed(speed);
        }

        public static void SetLocalPlayerStrafeSpeed()
        {
            LocalPlayer.GetVRCPlayerApi().SetStrafeSpeed(0f);
        }

        public static void FreezeLocalPlayer(bool Enabled)
        {
            bool flag = LocalPlayerCollider == null;
            if (flag)
            {
                LocalPlayerCollider = LocalVRCPlayer.GetComponent<Collider>();
            }
            LocalPlayerCollider.enabled = Enabled;
        }

        public static void FreezeGravity(bool Enabled)
        {
            LocalPlayer.GetComponent<Rigidbody>().gameObject.SetActive(Enabled);
        }

        public static void ToggleBlock(string player)
        {
        }

        public static Player GetPlayer(int ActorNumber)
        {
            return (from p in AllPlayers
                    where p.GetActorNumber() == ActorNumber
                    select p).FirstOrDefault<Player>();
        }

        public static string GetSteamID(this VRCPlayer player)
        {
            return player.field_Private_UInt64_0.ToString();
        }

        public static Player GetPlayer(string Displayname)
        {
            return (from p in AllPlayers
                    where p.GetName() == Displayname
                    select p).FirstOrDefault<Player>();
        }

        public static Player GetPlayer(this VRCPlayer player)
        {
            return player.prop_Player_0;
        }

        public static bool IsLocalPlayer(this Player player)
        {
            return player.GetAPIUser().id == APIUser.CurrentUser.id;
        }

        public static Player GetPlayerByID(string UserID)
        {
            return (from p in AllPlayers
                    where p.GetAPIUser().id == UserID
                    select p).FirstOrDefault<Player>();
        }
        public static Player GetPlayerByAPIAvatar(ApiAvatar apiavatar)
        {
            return (from p in AllPlayers
                    where p.GetAPIAvatar().Pointer == apiavatar.Pointer
                    select p).FirstOrDefault<Player>();
        }
        public static void SetGain(float Gain)
        {
            LocalGain = Gain;
        }

        public static void ResetGain()
        {
            USpeaker.field_Internal_Static_Single_1 = DefaultGain;
        }

        public static Player[] GetAllPlayers()
        {
            return PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.ToArray();
        }
        private static bool JumpIsPressed
        {
            get
            {
                return VRCInputManager.Method_Public_Static_VRCInput_String_0("Jump").prop_Boolean_0;
            }
        }

        public static bool IsInWorld()
        {
            return RoomManager.field_Internal_Static_ApiWorld_0 != null;
        }

        private static VRC_EventHandler handler;

        private static Collider LocalPlayerCollider;
        //private static Collider LocalPlayerCollider;
    }
}