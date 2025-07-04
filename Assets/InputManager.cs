using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public PlayerAction inputActions { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            inputActions = new PlayerAction();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {  
       inputActions.Disable();
    }

    // 필요하면 리스너를 여기서 등록하거나,
    // 다른 스크립트들이 Instance를 통해 등록하도록
}
