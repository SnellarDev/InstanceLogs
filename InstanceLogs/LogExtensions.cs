using Newtonsoft.Json;
using Photon.Pun;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2CppSystem.Collections.Generic;
using VRC.Core;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace InstanceLogs
{
    public class LogExtensions
    {
        public static void LogPlayer(Photon.Realtime.Player player, bool avatars)
        {
            try
            {
                Dictionary<string, Il2CppSystem.Object> playerdict = player.field_Private_Hashtable_0["user"].Cast<Dictionary<string, Il2CppSystem.Object>>();
                Dictionary<string, Il2CppSystem.Object> avatardict = player.field_Private_Hashtable_0["avatarDict"].Cast<Dictionary<string, Il2CppSystem.Object>>();
                File.AppendAllText(LogMain.PlayerLogs, string.Concat(new object[]
                            {
                            $"----------------------------------\n",
                            "Player Name: ",
                            LogToString(playerdict["displayName"]).TrimStart('"').TrimEnd('"') + "\n",
                            "UserID: ",
                            LogToString(playerdict["id"]).TrimStart('"').TrimEnd('"') + "\n",
                            "Platform: ",
                            LogToString(playerdict["last_platform"]).TrimStart('"').TrimEnd('"') + "\n",
                            "Status: ",
                            LogToString(playerdict["statusDescription"]).TrimStart('"').TrimEnd('"') +"\n",
                            "Bio: ",
                            LogToString(playerdict["bio"]).TrimStart('"').TrimEnd('"') + "\n",
                            "UserIcon URL: ",
                            LogToString(playerdict["userIcon"]).TrimStart('"').TrimEnd('"') + "\n",
                            "Tags: ",
                            $"{LogToString(playerdict["tags"]).TrimStart('[').TrimEnd(']')}", $"\nLogged at: {DateTime.Now}\n----------------------------------\n\n",
                            }));
                if(avatars)
                {
                    File.AppendAllText(LogMain.AvatarLogs, string.Concat(new object[]
                                {
                            $"----------------------------------\n",
                            "User: ",
                            LogToString(playerdict["displayName"]).TrimStart('"').TrimEnd('"') + "\n",
                            "Avatar Name: ",
                            LogToString(avatardict["name"]).TrimStart('"').TrimEnd('"') + "\n",
                            "AvatarID: ",
                            LogToString(avatardict["id"]).TrimStart('"').TrimEnd('"') + "\n",
                            "AssetURL: ",
                            LogToString(avatardict["assetUrl"]).TrimStart('"').TrimEnd('"') + "\n",
                            "ImageURL: ",
                            LogToString(avatardict["imageUrl"]).TrimStart('"').TrimEnd('"') +"\n",
                            "ReleaseStatus: ",
                            LogToString(avatardict["releaseStatus"]).TrimStart('"').TrimEnd('"') + "\n",
                            "Version: ",
                            LogToString(avatardict["version"]).TrimStart('"').TrimEnd('"') + "\n",
                            "Last Upload Time: ",
                            $"{LogToString(avatardict["updated_at"]).TrimStart('"').TrimEnd('"')}", $"\nLogged at: {DateTime.Now}\n----------------------------------\n\n",
                                }));
                }
                
            }
            catch { }
        }
        public static void LogAvatar(EventData __0)
        {
            try
            {
                Il2CppSystem.Collections.Hashtable hashtable = __0.Parameters[251].Cast<Il2CppSystem.Collections.Hashtable>();
                Dictionary<string, Il2CppSystem.Object> playerdict = hashtable["user"].Cast<Dictionary<string, Il2CppSystem.Object>>();
                Dictionary<string, Il2CppSystem.Object> avatardict = hashtable["avatarDict"].Cast<Dictionary<string, Il2CppSystem.Object>>();
                if(lastid != LogToString(avatardict["id"]) || lastplayer != LogToString(playerdict["displayName"]))
                {
                    File.AppendAllText(LogMain.AvatarLogs, string.Concat(new object[]
                                {
                            $"----------------------------------\n",
                            "User: ",
                            LogToString(playerdict["displayName"]).TrimStart('"').TrimEnd('"') + "\n",
                            "Avatar Name: ",
                            LogToString(avatardict["name"]).TrimStart('"').TrimEnd('"') + "\n",
                            "AvatarID: ",
                            LogToString(avatardict["id"]).TrimStart('"').TrimEnd('"') + "\n",
                            "AssetURL: ",
                            LogToString(avatardict["assetUrl"]).TrimStart('"').TrimEnd('"') + "\n",
                            "ImageURL: ",
                            LogToString(avatardict["imageUrl"]).TrimStart('"').TrimEnd('"') +"\n",
                            "ReleaseStatus: ",
                            LogToString(avatardict["releaseStatus"]).TrimStart('"').TrimEnd('"') + "\n",
                            "Version: ",
                            LogToString(avatardict["version"]).TrimStart('"').TrimEnd('"') + "\n",
                            "Last Upload Time: ",
                            $"{LogToString(avatardict["updated_at"]).TrimStart('"').TrimEnd('"')}", $"\nLogged at: {DateTime.Now}\n----------------------------------\n\n",
                                }));
                }

                lastplayer = LogToString(playerdict["displayName"]);
                lastid = LogToString(avatardict["id"]);
            }
            catch { }
        }
        public static void LogRoomInfo()
        {
            File.AppendAllText(LogMain.PlayerLogs, $"----------------------------------\nJoined World: {WorldInfo.name}" +
                $"\nPlayers: {PlayerMath}\nInstance: {WrldInstance.id}\n----------------------------------\n\n");
            File.AppendAllText(LogMain.AvatarLogs, $"----------------------------------\nJoined World: {WorldInfo.name}" +
                $"\nPlayers: {PlayerMath}\nInstance: {WrldInstance.id}\n----------------------------------\n\n");
        }

        public static void LogWorld()
        {
            try
            {
                File.AppendAllText(LogMain.WorldLogs, string.Concat(new object[]
                                {
                            $"----------------------------------\n",
                            "World Name: ",
                            WorldInfo.name + "\n",
                            "InstanceID: ",
                            WrldInstance.id + "\n",
                            "AssetURL: ",
                            WorldInfo.assetUrl + "\n",
                            "Author: ",
                            WorldInfo.authorName + "\n",
                            "Retroactive Player Count: ",
                            PlayerMath + "\n",
                            "ImageURL: ",
                            WorldInfo.imageUrl +"\n",
                            "ReleaseStatus: ",
                            WorldInfo.releaseStatus + "\n",
                            "Unity Version: ",
                            WorldInfo.unityVersion + "\n",
                            "Version: ",
                            WorldInfo.version + "\n",
                            "Description: ",
                            WorldInfo.description, $"\nLogged at: {DateTime.Now}\n----------------------------------\n\n",
                                }));
            }
            catch { }
        }

        public static ApiWorldInstance WrldInstance
        {
            get
            {
                return RoomManager.field_Internal_Static_ApiWorldInstance_0;
            }
        }

        public static string PlayerMath
        {
            get
            {
                return PhotonRoom.field_Private_Dictionary_2_Int32_Player_0.Count + "/" + WrldInstance.world.capacity * 2;
            }

        
        }
        public static Room PhotonRoom
        {
            get
            {
                return PhotonNetwork.field_Public_Static_LoadBalancingClient_0.field_Private_Room_0;
            }
        }

        public static ApiWorld WorldInfo
        {
            get
            {
                return RoomManager.field_Internal_Static_ApiWorld_0;
            }
        }

        public static string lastid;
        public static string lastplayer;
        public static string LogToString(Il2CppSystem.Object obj) => JsonConvert.SerializeObject(Serialization.FromIL2CPPToManaged<object>(obj), Formatting.None);
    }
}
