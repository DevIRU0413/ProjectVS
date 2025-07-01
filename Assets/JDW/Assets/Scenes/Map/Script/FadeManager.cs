using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public GameObject FadeImage;
    // 페이드 인 / 아웃 시간
    public float FadeDuration = 1f;
    // 화면을 검은 원으로 덮는 효과
    public IEnumerator FadeOut()
    {
        FadeImage.SetActive(true);
        Transform circle = FadeImage.transform; 
        circle.localScale = Vector3.zero;

        float time = 0f;

        while(time < FadeDuration)
        {
            time += Time.deltaTime;
            // 시간에 따라 스케일 값을 키움
            float scale = Mathf.Lerp(0f, 50f, time / FadeDuration);
            circle.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }
    }
    // 화면에 원이 작아지는 효과
    public IEnumerator FadeIn()
    {
        Transform circle = FadeImage.transform;
        circle.localScale = new Vector3(3f, 3f, 1f);

        float time = 0f;
        while (time < FadeDuration)
        {
            time += Time.deltaTime;
            // 시간에 따라 스케일 값을 줄임
            float scale = Mathf.Lerp(50f, 0f, time / FadeDuration);
            circle.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }
        FadeImage.SetActive(false);
    }
}
