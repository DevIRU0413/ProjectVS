using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float totalTime = 900f; //15분
    private float _currentTime;

    public TextMeshProUGUI timerText; // TMP 사용

    private void Start()
    {
        _currentTime = totalTime;
    }
    private void Update()
    {
        if (_currentTime > 0f)
        {
            // 현재 남은 시간에서 실시간 감소
            _currentTime -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(_currentTime / 60f);
            int seconds = Mathf.FloorToInt(_currentTime % 60f);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
            timerText.text = "Time Over!";
    }
}
