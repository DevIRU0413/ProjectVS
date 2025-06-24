using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player _player;
    public PlayerMove _playerMove;
    public Bullet _bullet;
    private void Awake()
    {
        instance = this;
    }
}

