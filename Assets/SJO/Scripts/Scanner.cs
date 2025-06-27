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
        // �������� �ɽ�Ʈ�� ��� ��� ����� ��ȯ
        // ĳ���� ���� ��ġ, ���� ������, ĳ���� ����, ĳ���� ����, ��� ���̾�
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);

        nearestTarget = GetNearest();
    }

    // ���� ����� ���� ã�� �Լ�
    public Transform GetNearest()
    {
        // ���� ������� �������
        Transform result = null;

        float distance = 100;

        foreach (RaycastHit2D target in targets)
        {
            // �÷��̾��� ��ġ
            Vector3 playerPos = transform.position;

            // Ÿ���� ��ġ
            Vector3 targetPos = target.transform.position;

            // ���� �� �� �Ÿ� ���
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
