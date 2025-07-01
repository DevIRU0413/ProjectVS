using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float TotalTime = 900f; //15분
    public float CurrentTime;

    public TextMeshProUGUI TimerText; // TMP 사용
    public FadeManager FadeManager;
    public MapSwitcher MapSwitcer;

    private bool _paused = false;

    private void Start()
    {
        CurrentTime = TotalTime;
    }
    private void Update()
    {
        if (_paused) return;

        if (CurrentTime > 0f)
        {
            // 현재 남은 시간에서 실시간 감소
            CurrentTime -= Time.deltaTime;


            int minutes = Mathf.FloorToInt(CurrentTime / 60f);
            int seconds = Mathf.FloorToInt(CurrentTime % 60f);

            TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            TimerText.text = "00:00";
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

        yield return StartCoroutine(FadeManager.FadeOut());
        yield return StartCoroutine(FadeManager.FadeIn());
        CurrentTime = TotalTime; // 타이머 초기화

    }
    public void SetMessage(string msg)
    {
        if (TimerText != null)
        {
            TimerText.text = msg;
        }
    }
}
