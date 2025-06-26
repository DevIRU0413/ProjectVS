using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class AttackPosition : MonoBehaviour
{
    public Transform Player;
    public float Radius = 1.5f;
    public Vector3 Direction => _direction;
    [SerializeField] private GameObject _swordPerfab;
    [SerializeField] private GameObject _axPerfab;
    [SerializeField] private GameObject _bulletPerfab;
    [SerializeField] private GameObject Store;
    [SerializeField] private Transform _muzzlePos;
    [SerializeField] private float _bulletTime;
    [SerializeField] private float _meleeAttack = 0.2f;

    private bool storeWasActive = false;


    private Vector3 _direction;

    private Coroutine _currentRoutine;
    
    private void Update()
    {
        if (Player == null) return;
        CursorCoordinates();
        Vector3 _ovjPos = Player.position + _direction * Radius;
        transform.position = _ovjPos;
        transform.right = _direction;

        if (Store.activeSelf && _currentRoutine != null)
        {
            StopCoroutine(_currentRoutine);
            _currentRoutine = null;
            Debug.Log("Store 활성화 → 코루틴 중단");
        }
        if (!Store.activeSelf && storeWasActive && _currentRoutine == null)
        {
            Debug.Log("Store 비활성화 → 공격 재시작");

            // 클래스에 따라 재실행 (GameManager에 저장된 클래스 인덱스 사용)
            int index = GameManager.instance.CurrentClassIndex;
            switch (index)
            {
                case 0: SwitchCoroutine(Axe()); break;
                case 1: SwitchCoroutine(Sword()); break;
                case 2: SwitchCoroutine(Fire()); break;
            }
        }

        storeWasActive = Store.activeSelf;

    }
    public void CursorCoordinates()
    {
        if (Mouse.current == null || Camera.main == null) return;
        // 마우스 위치를 월드기준으로 전환
        // Vector3 _mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 _mouseWorldPos = Camera.main.ScreenToWorldPoint(
        new Vector3(mouseScreenPos.x, mouseScreenPos.y, Mathf.Abs(Camera.main.transform.position.z))
    );
        _mouseWorldPos.z = 0f;
        // 방향백터 계산
        _direction = (_mouseWorldPos - Player.position).normalized;
    }
    public void SwitchCoroutine(IEnumerator newRoutine)
    {
        // 코루틴 스위칭 함수
        if(_currentRoutine != null)
        {
            StopCoroutine(_currentRoutine);
        }
        _currentRoutine = StartCoroutine(newRoutine);
    }
    
    public IEnumerator Axe()
    {
        // 무한 반복
        while (true)
        {
            if (_bulletPerfab != null)
            {
                // 투사체 생성
                GameObject _ax = Instantiate(_axPerfab, _muzzlePos.position, Quaternion.identity);
                _ax.transform.right = _direction;
                // 시간이 지나면 삭제
                Destroy(_ax, _meleeAttack);
            }
            // 투사체의 발사 간격
            yield return new WaitForSeconds(GameManager.instance.player.stats.AttackSpeed);
        }
    }
    public IEnumerator Sword()
    {
        // 무한 반복
        while (true)
        {
            if (_bulletPerfab != null)
            {
                // 투사체 생성
                GameObject _sword = Instantiate(_swordPerfab, _muzzlePos.position, Quaternion.identity);
                _sword.transform.right = _direction;
                // 시간이 지나면 삭제
                Destroy(_sword, _meleeAttack);
            }
            // 투사체의 발사 간격
            yield return new WaitForSeconds(GameManager.instance.player.stats.AttackSpeed);
        }
    }
public IEnumerator Fire()
    {
        // 무한 반복
        while (true)
        {
            if (_bulletPerfab != null)
            {
                // 투사체 생성
                GameObject _bullet = Instantiate(_bulletPerfab, _muzzlePos.position, Quaternion.identity);
                _bullet.transform.right = _direction;
                // 시간이 지나면 삭제
                Destroy(_bullet, _bulletTime);
            }
            // 투사체의 발사 간격
            yield return new WaitForSeconds(GameManager.instance.player.stats.AttackSpeed);
        }
    }
}
