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
    public MapSwitcer mapSwitcer;

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
        StratrTile();
    }
    private void TimeTxet()
    {
        GameObject boss = GameObject.FindWithTag(_bossTag);// 보스 태그를 확인
        if (boss != null) { timer.timerText.text = "Boss!"; timer.PauseTimer(); return; }//시간을 멈추고 그 태그로 표기
        GameObject store = GameObject.FindWithTag(_storeTag);// 상점 태그를 확인
        if (store != null) { timer.timerText.text = "$Store$"; timer.PauseTimer(); return; }//시간을 멈추고 그 태그로 표기
        timer.ResumeTimer();
    }
    private void StopTile()
    {

        if (timer != null && timer.currentTime <= 1f) // 타이머가 널이 아니고 현재 시간이 1초가 되었을 때
            foreach (var r in Tilemap.GetComponentsInChildren<Reposition>()) // 리포지션 스크립트를 비활성화
                r.isActive = false;
    }
    private void StratrTile()
    {
        if (mapSwitcer.IsStoreActive())
        {
            foreach (var r in Tilemap.GetComponentsInChildren<Reposition>())
                r.isActive = true;
        }
    }

}

