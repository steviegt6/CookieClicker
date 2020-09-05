using CookieClicker.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CookieClicker.AchievementsSystem
{
    public class AchievementManager
    {
        private class StoredAchievement
        {
            [JsonProperty]
            public Dictionary<string, JObject> Conditions;
        }

        private readonly string _savePath;
        private readonly Dictionary<string, Achievement> _achievements = new Dictionary<string, Achievement>();
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings();
        private readonly byte[] _cryptoKey;
        private readonly Dictionary<string, int> _achievementIconIndexes = new Dictionary<string, int>();
        private static readonly object _ioLock = new object();

        public event Achievement.AchievementCompleted OnAchievementCompleted;

        public AchievementManager()
        {
            _savePath = Main.SavePath + Path.DirectorySeparatorChar + "cookieachievements.dat";
            _cryptoKey = Encoding.ASCII.GetBytes("RELOGIC-TERRARIA");
        }

        public void Save()
        {
            FileUtils.ProtectedInvoke(delegate
            {
                Save(_savePath);
            });
        }

        private void Save(string path)
        {
            lock (_ioLock)
            {
                try
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, new RijndaelManaged().CreateEncryptor(_cryptoKey, _cryptoKey), CryptoStreamMode.Write))
                        {
                            using (BsonWriter bsonWriter = new BsonWriter(cryptoStream))
                            {
                                JsonSerializer.Create(_serializerSettings).Serialize(bsonWriter, _achievements);
                                bsonWriter.Flush();
                                cryptoStream.FlushFinalBlock();
                                FileUtilities.WriteAllBytes(path, memoryStream.ToArray(), false);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    ModLoader.GetMod("CookieClicker").Logger.Error(SavingFailureError(exception, _savePath));
                }
            }
        }

        public List<Achievement> CreateAchievementsList() => _achievements.Values.ToList();

        public void Load()
        {
            Load(_savePath);
        }

        private void Load(string path)
        {
            lock (_ioLock)
            {
                if (!FileUtilities.Exists(path, false))
                {
                    return;
                }

                byte[] buffer = FileUtilities.ReadAllBytes(path, false);
                Dictionary<string, StoredAchievement> dictionary = null;
                try
                {
                    using (MemoryStream stream = new MemoryStream(buffer))
                    {
                        using (CryptoStream stream2 = new CryptoStream(stream, new RijndaelManaged().CreateDecryptor(_cryptoKey, _cryptoKey), CryptoStreamMode.Read))
                        {
                            using (BsonReader reader = new BsonReader(stream2))
                            {
                                dictionary = JsonSerializer.Create(_serializerSettings).Deserialize<Dictionary<string, StoredAchievement>>(reader);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    FileUtilities.Delete(path, false);
                    return;
                }

                if (dictionary == null)
                {
                    return;
                }

                foreach (KeyValuePair<string, StoredAchievement> item in dictionary)
                {
                    if (_achievements.ContainsKey(item.Key))
                    {
                        _achievements[item.Key].Load(item.Value.Conditions);
                    }
                }
            }
        }

        public void ClearAll()
        {
            foreach (KeyValuePair<string, Achievement> achievement in _achievements)
            {
                achievement.Value.ClearProgress();
            }
        }

        private void AchievementCompleted(Achievement achievement)
        {
            Save();

            OnAchievementCompleted?.Invoke(achievement);
        }

        public void Register(Achievement achievement)
        {
            _achievements.Add(achievement.Name, achievement);

            achievement.OnCompleted += AchievementCompleted;
        }

        public void RegisterIconIndex(string achievementName, int iconIndex)
        {
            _achievementIconIndexes.Add(achievementName, iconIndex);
        }

        public void RegisterAchievementCategory(string achievementName, AchievementCategory category)
        {
            _achievements[achievementName].SetCategory(category);
        }

        public Achievement GetAchievement(string achievementName)
        {
            if (_achievements.TryGetValue(achievementName, out Achievement value))
            {
                return value;
            }

            return null;
        }

        public T GetCondition<T>(string achievementName, string conditionName) where T : AchievementCondition => GetCondition(achievementName, conditionName) as T;

        public AchievementCondition GetCondition(string achievementName, string conditionName)
        {
            if (_achievements.TryGetValue(achievementName, out Achievement value))
            {
                return value.GetCondition(conditionName);
            }

            return null;
        }

        public int GetIconIndex(string achievementName)
        {
            if (_achievementIconIndexes.TryGetValue(achievementName, out int value))
            {
                return value;
            }

            return 0;
        }

        private string SavingFailureError(Exception exception, string filePath)
        {
            bool specialError = false;

            if (exception is UnauthorizedAccessException || exception is FileNotFoundException || exception is DirectoryNotFoundException)
            {
                specialError = true;
            }

            if (specialError)
            {
                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.AppendLine("Failed to create the file: \"" + filePath.Replace("/", "\\") + "\"!");

                List<string> list = new List<string>
                {
                    "If you are using an Anti-virus, please make sure it does not block Terraria in any way.",
                    "Try making sure your `Documents/My Games/Terraria` folder is not set to 'read-only'.",
                    "Try verifying integrity of game files via Steam."
                };

                if (filePath.ToLower().Contains("onedrive"))
                {
                    list.Add("Try updating OneDrive.");
                }

                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Suggestions:");
                for (int i = 0; i < list.Count; i++)
                {
                    string str = list[i];
                    stringBuilder.AppendLine(i + 1 + ". " + str);
                }
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("The original Error below");
                stringBuilder.Append(exception);

                return stringBuilder.ToString();
            }

            return "";
        }
    }
}
