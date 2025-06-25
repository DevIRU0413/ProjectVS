using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;
    public PlayerMove playerMove;
    public Bullet bullet;
    public Timer timer;

    private string _bossTag = "Boss";
    private string _storeTag = "Store";

    [SerializeField] GameObject Boss;
    [SerializeField] GameObject Store;
    [SerializeField] GameObject Tilemap;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        TimeTxet();
        StopTile();
    }
    private void TimeTxet()
    {
        GameObject boss = GameObject.FindWithTag(_bossTag);// 보스 태그를 확인
        if (boss != null) { timer.timerText.text = "Boss!"; return; }// 보스 태그를 가진 오브젝트가 있으면 타이머가 보스로 표기
        GameObject store = GameObject.FindWithTag(_storeTag);// 상점 태그를 확인
        if (store != null) { timer.timerText.text = "$Store$"; return; }// 상점 태그를 가진 오브젝트가 있으면 타이머가 상점으로 표기
    }
    private void StopTile()
    {

        if (timer != null && timer._currentTime <= 1f) // 타이머가 널이 아니고 현재 시간이 1초가 되었을 때
            foreach (var r in Tilemap.GetComponentsInChildren<Reposition>()) // 리포지션 스크립트를 비활성화
                r.isActive = false;
    }

}

