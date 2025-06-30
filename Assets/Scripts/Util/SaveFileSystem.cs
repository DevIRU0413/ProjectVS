using System.IO;

using ProjectVS.Data;

using UnityEngine;

namespace ProjectVS.Util
{
    public static class SaveFileSystem
    {
        public static string GetSavePath(int index) =>
            Path.Combine(Application.persistentDataPath, $"PlayerData_{index}.json");

        public static void Save(PlayerData data, int index)
        {
            string path = GetSavePath(index);
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);

            Debug.Log($"[SaveFileSystem] 저장 파일 저장됨: {path}");
        }

        public static PlayerData Load(int index)
        {
            string path = GetSavePath(index);
            if (!File.Exists(path)) return null;

            string json = File.ReadAllText(path);
            Debug.Log($"[SaveFileSystem] 저장 파일 로드됨: {path}");
            return JsonUtility.FromJson<PlayerData>(json);
        }

        public static void Delete(int index)
        {
            string path = GetSavePath(index);
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"[SaveFileSystem] 저장 파일 삭제됨: {path}");
            }
            else
            {
                Debug.LogWarning($"[SaveFileSystem] 삭제할 저장 파일 없음: {path}");
            }
        }

        public static bool HasSaveData(int index)
        {
            string path = SaveFileSystem.GetSavePath(index);
            return File.Exists(path);
        }

    }

}
