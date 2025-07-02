using System.Collections;
using System.Collections.Generic;

using ProjectVS.JDW;

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target; // 카메라가 따라갈 대상
    public Vector3 Offset = new Vector3(0f, 2f, -10f);
    public float SmoothSpeed = 5f;

    public PlayerConfig Player;

    private Coroutine _shakeRoutine;
    private bool _isShaking = false;
    void Update()
    {
        if (Target == null)// 타겟이 없으면 씬에서 playerConfig를 찾아서 할당함
        {
            PlayerConfig player = FindObjectOfType<PlayerConfig>();
            if (player != null)
            {
                Target = player.transform;
            }
        }
    }
    private void LateUpdate()
    {
        if (Target == null || _isShaking) return; // 타겟이 없거나 흔들림 중일 때 따라가기 무시

        Vector3 desiredPosition = Target.position + Offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed * Time.deltaTime);
    }
    public void ShakeCamera(float duration,float magnitude) // 외부에서 카메라를 흔들 때 호출 할 함수
    {
        if(_shakeRoutine != null)
        {
            StopCoroutine(_shakeRoutine);
        }
            _shakeRoutine = StartCoroutine(Shake(duration, magnitude));
    }
    private IEnumerator Shake(float duration, float magnitude)// 카메라 흔들림(지진효과)
    {
        _isShaking = true;

        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while(elapsed < duration)
        {
            Debug.Log("지진효과");
            float offsetX = Random.Range(-1f, 1f) * magnitude; // x축 흔들림
            float offsetY = Random.Range(-1f, 1f) * magnitude; // y축 흔들림

            transform.position = originalPos + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPos;
        _isShaking = false;
    }
}
