using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // �����յ��� ������ ����
    public GameObject[] prefabs;

    // Ǯ ����� �ϴ� ����Ʈ
    public List<GameObject>[] poolLists;

    private void Awake()
    {
        // Ǯ�� ��� �迭 �ʱ�ȭ
        poolLists = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < poolLists.Length; i++)
        {
            // ��� ������Ʈ Ǯ ����Ʈ �ʱ�ȭ
            poolLists[i] = new List<GameObject>();
        }
    }

    // ���� ������Ʈ ��ȯ
    public GameObject ReturnObject(int i)
    {
        // ���� �����
        GameObject select = null;
    
        // �ٸ� ��ũ��Ʈ���� �� �Լ��� ����Ѵٸ�
        // ������ Ǯ�� ��� �ִ�(��Ȱ��ȭ��) ���� ������Ʈ ����
        foreach (GameObject item in poolLists[i])
        {
            // item(������Ʈ)�� ��Ȱ��ȭ(��� ����)���� Ȯ��
            // ���� ��Ȱ��ȭ ���¶��
            if (!item.activeSelf)
            {
                // �߰��ϸ� select ������ �Ҵ�
                select = item;

                // Ȱ��ȭ ���·� ����
                select.SetActive(true);
                break;
            }
        }
    
        // ���� ��� ����ϰ� �ִٸ�
        if (!select)
        {
            // ���Ӱ� �����ϰ� select ������ �Ҵ�
            select = Instantiate(prefabs[i], transform);

            // ������ ������Ʈ�� �ش� ������Ʈ Ǯ ����Ʈ�� Add �Լ��� �߰�(���)
            poolLists[i].Add(select);
        }
    
        return select;
    }
}
