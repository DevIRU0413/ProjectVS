using ProjectVS;
using ProjectVS.Util;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : SimpleSingleton<GameManager>
{
    [HideInInspector] public PlayerConfig Player;
    public int CurrentClassIndex { get; private set; }
    private GameObject currentPlayerInstance;
    public PlayerStats SavedStats;
    public void SavePlayerStats(PlayerStats stats)
    {
        SavedStats = stats.Clone(); // 플레이어 데이터 복사
    }
}

