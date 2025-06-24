using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float totalTime = 900f; //15분
    private float _currentTime;

    public Transform storeSpawnPoint;

    public TextMeshProUGUI timerText; // TMP 사용
    public FadeManager fadeManager;
    public Tilemap tilemap;

    private bool _isFading = false;
    private bool _scriptDisabled = false;

    public string bossTag = "Boss";
    public string storeTag = "Store";

   
    public GameObject BattleField;
    public GameObject StoreField;
    public GameObject player; // 이동시킬 플레이어
   
    public GameObject Tilemap1;
    public GameObject Tilemap2;
    public GameObject Tilemap3;
    public GameObject Tilemap4;

    private void Start()
    {
        _currentTime = totalTime;
    }
    private void Update()
    {
        // 보스 존재를 태그를 통해 확인
        GameObject boss = GameObject.FindWithTag(bossTag);
        if(boss != null)
        {
            timerText.text = "Boss!";
            return;
        }
        // 상점을 태그를 통해 확인
        GameObject store = GameObject.FindWithTag(storeTag);
        if (store != null)
        {
            timerText.text = "$Store$";
            return;
        }
        if (_currentTime > 0f)
        {
            // 현재 남은 시간에서 실시간 감소
            _currentTime -= Time.deltaTime;

            if (_currentTime <= 1f) // 남은 시간이 1초가되면
            {
                Debug.Log("스크립트가 오프됨");
                Tilemap1.GetComponent<Reposition>().isActive = false; // 무한맵 스크립트를 멈춤
                Tilemap2.GetComponent<Reposition>().isActive = false;
                Tilemap3.GetComponent<Reposition>().isActive = false;
                Tilemap4.GetComponent<Reposition>().isActive = false;


                _scriptDisabled = true;
            }

            int minutes = Mathf.FloorToInt(_currentTime / 60f);
            int seconds = Mathf.FloorToInt(_currentTime % 60f);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            timerText.text = "00:00"; // 시간 종료시
           
            // 시간 종료시 코루틴을 호출해 페이드 효과
            StartCoroutine(HandleFadeTransition());
        }
    }
    private IEnumerator HandleFadeTransition()
    {
        _isFading = true;
        yield return StartCoroutine(fadeManager.FadeOut());
        BattleField.SetActive(false); // 페이드아웃 이후 배틀필드 오프
        StoreField.SetActive(true); // 페이드인 이후 스토어필드 온
        player.transform.position = new Vector3(0f, 0f, -1f); //플레이어의 위치 초기화
        
        yield return StartCoroutine(fadeManager.FadeIn());
        _currentTime = totalTime; // 타이머 초기화
        _isFading = false;
    }
   


}
