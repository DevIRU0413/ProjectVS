using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float totalTime = 900f; //15분
    public float currentTime;
    public TextMeshProUGUI timerText; // TMP 사용
    public FadeManager fadeManager;
    private bool _paused = false;

    public MapSwitcher mapSwitcer;
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
            // TODO : 신 매니저 추가해서 신이동 코드 추가 요망
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

        yield return StartCoroutine(fadeManager.FadeOut()); // 패이드 아웃
        // 상점 온/ 배틀 오프
        yield return StartCoroutine(fadeManager.FadeIn());
        currentTime = totalTime; // 타이머 초기화

    }
    public void SetMessage(string msg)
    {
        if (timerText != null)
            timerText.text = msg;
    }
}
