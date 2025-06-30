using System.IO;

using ProjectVS.Data;

using UnityEngine;

namespace ProjectVS.Util
{
    public static class SaveSystem
    {
        private static readonly string savePath = Path.Combine(Application.persistentDataPath, "playerdata.json");

        public static void Save(PlayerData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);
            Debug.Log("Game Saved to: " + savePath);
        }

        public static PlayerData Load()
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                return JsonUtility.FromJson<PlayerData>(json);
            }
            else
            {
                Debug.LogWarning("Save file not found");
                return null;
            }
        }
    }
}
