using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        // 원형으로 케스트를 쏘고 모든 결과을 반환
        // 캐스팅 시작 위치, 원의 반지름, 캐스팅 방향, 캐스팅 길이, 대상 레이어
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);

        nearestTarget = GetNearest();
    }

    // 가장 가까운 것을 찾는 함수
    public Transform GetNearest()
    {
        // 현재 결과물은 비어있음
        Transform result = null;

        float distance = 100;

        foreach (RaycastHit2D target in targets)
        {
            // 플레이어의 위치
            Vector3 playerPos = transform.position;

            // 타겟의 위치
            Vector3 targetPos = target.transform.position;

            // 벡터 두 개 거리 계산
            float currentDis = Vector3.Distance(playerPos, targetPos);

            if (currentDis < distance)
            {
                distance = currentDis;
                result = target.transform;
            }
        }

        return result;
    }
}
