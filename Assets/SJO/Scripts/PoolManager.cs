using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리팹들을 보관할 변수
    public GameObject[] prefabs;

    // 풀 담당을 하는 리스트
    public List<GameObject>[] poolLists;

    private void Awake()
    {
        // 풀을 담는 배열 초기화
        poolLists = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < poolLists.Length; i++)
        {
            // 모든 오브젝트 풀 리스트 초기화
            poolLists[i] = new List<GameObject>();
        }
    }

    // 게임 오브젝트 반환
    public GameObject ReturnObject(int i)
    {
        // 현재 비었음
        GameObject select = null;
    
        // 다른 스크립트에서 이 함수를 사용한다면
        // 선택한 풀에 놀고 있는(비활성화된) 게임 오브젝트 접근
        foreach (GameObject item in poolLists[i])
        {
            // item(오브젝트)이 비활성화(대기 상태)인지 확인
            // 만약 비활성화 상태라면
            if (!item.activeSelf)
            {
                // 발견하면 select 변수에 할당
                select = item;

                // 활성화 상태로 변경
                select.SetActive(true);
                break;
            }
        }
    
        // 만약 모두 사용하고 있다면
        if (!select)
        {
            // 새롭게 생성하고 select 변수에 할당
            select = Instantiate(prefabs[i], transform);

            // 생성된 오브젝트를 해당 오브젝트 풀 리스트에 Add 함수로 추가(등록)
            poolLists[i].Add(select);
        }
    
        return select;
    }
}
