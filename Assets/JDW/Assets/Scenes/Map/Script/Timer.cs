using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float totalTime = 900f; //15분
    public float currentTime;
    

    public Transform storeSpawnPoint;
    public TextMeshProUGUI timerText; // TMP 사용
    public FadeManager fadeManager;
    public MapSwitcer mapSwitcer;
    

    private bool _isFading = false;
    private bool _scriptDisabled = false;
    private bool _paused = false;

    private void Start()
    {
        currentTime = totalTime;
    }
    private void Update()
    {
        if (_paused) return;

        if (currentTime > 0f)
        {
            // 현재 남은 시간에서 실시간 감소
            currentTime -= Time.deltaTime;


            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            timerText.text = "00:00"; // 시간 종료시
           
            // 시간 종료시 코루틴을 호출해 페이드 효과
            StartCoroutine(HandleFadeTransition());
        }
    }
    public void PauseTimer()
    {
        _paused = true; // 타임 스탑
    }

    public void ResumeTimer()
    {
        _paused = false; // 타임 온
    }
    private IEnumerator HandleFadeTransition()
    {
        _isFading = true;
        yield return StartCoroutine(fadeManager.FadeOut()); // 패이드 아웃
        mapSwitcer.OnstoreField(); // 상점 온/ 배틀 오프
        // TODO : 씬 변경일 경우 맵 스위처에서 씬이동으로 코드변경

        GameManager.instance.playerMove.PlayerPositionReset();
        yield return StartCoroutine(fadeManager.FadeIn());
        currentTime = totalTime; // 타이머 초기화
        _isFading = false;
    }
   


}
