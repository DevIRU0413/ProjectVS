using ProjectVS;
using ProjectVS.Util;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : SimpleSingleton<GameManager>
{
    [HideInInspector] public PlayerConfig Player;
    public int CurrentClassIndex { get; private set; }
    private GameObject _currentPlayerInstance;
}

