using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using ProjectVS.Shop.NPCAffinityModel;
using ProjectVS.Utils.UIManager;

using UnityEngine;


namespace ProjectVS.NPC.NPCBehaviour
{
    public class NPCBehaviour : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private LayerMask _playerLayer;

        [SerializeField] private GameObject HeartEmoji;
        [SerializeField] private GameObject CryEmoji;

        [Header("호감도 증가 설정")]
        [SerializeField] private int _affinityIncrease = 15;

        [Header("다이아몬드 강탈 확률 설정")]
        [SerializeField, Range(0, 100)] private int _robChance = 10;

        [Header("NPC 사라짐 모션 설정")]
        [SerializeField, Range(0f, 10f)] private float _vanishDuration = 2f;
        [SerializeField, Range(0f, 10f)] private float _waitBeforeVanishTime = 2f;
        [SerializeField, Range(0f, 10f)] float _moveDistance = 3f;

        private void Awake()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        private void OnEnable()
        {
            Color color = _spriteRenderer.color;
            color.a = 1f;
            _spriteRenderer.color = color;

            HeartEmoji.SetActive(false);
            CryEmoji.SetActive(false);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & _playerLayer) != 0)
            {
                UIManager.Instance.Show("NPC Interaction Select Panel");
                Vanish();
            }
        }

        public void Save()
        {
            HeartEmoji.SetActive(true);
            NPCAffinityModel.Instance.IncreaseAffinity(_affinityIncrease);
        }

        public void Rob()
        {
            CryEmoji.SetActive(true);
            RandomlyGetDiamond();
        }

        private void RandomlyGetDiamond()
        {
            int randomValue = Random.Range(0, 100);

            if (randomValue < _robChance)
            {
                // TODO: 다이아 얻는 로직
            }
        }

        private void Vanish()
        {
            StartCoroutine(IE_WaitBeforeVanish());

            Vector2 randomDir = Random.insideUnitCircle.normalized;
            Vector3 targetPos = transform.position + (Vector3)(randomDir * _moveDistance);

            _spriteRenderer.DOFade(0f, _vanishDuration);
            transform.DOMove(targetPos, _vanishDuration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }

        private IEnumerator IE_WaitBeforeVanish()
        {
            yield return new WaitForSeconds(_waitBeforeVanishTime);
        }
    }
}
