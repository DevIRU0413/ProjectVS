using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using ShopItemTriggerClass = ProjectVS.Shop.ShopItemTrigger.ShopItemTrigger;

namespace ProjectVS.Shop.ItemPlacer
{

    // TODO: 랜덤 아이템 배치 기능 추가
    public class ItemPlacer : MonoBehaviour
    {
        [SerializeField] private List<ScriptableObject> _itemPrefabs; // 추후 클래스는 변경해야될 듯
        [SerializeField] private List<Sprite> _itemSprites; // 스프라이트가 SO에 없다면 사용
        [SerializeField] private List<Transform> _itemLocations;
        [SerializeField] private GameObject _colliderPrefab;

        private void Awake()
        {
            for (int i = 0; i < _itemLocations.Count; i++)
            {
                Transform location = _itemLocations[i];

                GameObject obj = Instantiate(_colliderPrefab, location.position, Quaternion.identity, transform);

                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sprite = _itemSprites[i];
                }
                else
                {
                    Debug.LogWarning($"[ItemPlacer] SpriteRenderer 컴포넌트가 _colliderPrefab에 없음");
                }

                ShopItemTriggerClass trigger = obj.GetComponent<ShopItemTriggerClass>();
                if (trigger != null)
                {
                    trigger.ItemSO = _itemPrefabs[i];
                }
                else
                {
                    Debug.LogWarning($"[ItemPlacer] ShopItemTrigger 컴포넌트가 _colliderPrefab에 없음");
                }
            }
        }
    }
}
