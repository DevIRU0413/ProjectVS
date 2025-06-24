using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPosition : MonoBehaviour
{
    public Transform Player;
    public float Radius = 1.5f;

    [SerializeField] private GameObject _swordPerfab;
    [SerializeField] private GameObject _axPerfab;
    [SerializeField] private GameObject _bulletPerfab;
    [SerializeField] private Transform _muzzlePos;
    [SerializeField] private float _bulletTime;
    [SerializeField] private float _meleeAttack = 0.2f;


    private Vector3 _direction;

    private Coroutine _currentRoutine;



    private void Start()
    {
        //StartCoroutine(Fire());
        //StartCoroutine(Ax());
        //StartCoroutine(Sword());
    }
    private void Update()
    {
        if (Player == null) return;
        // ���콺 ��ġ�� ����������� ��ȯ
        Vector3 _mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mouseWorldPos.z = 0f;
        // ������� ���
        Vector3 _direction = (_mouseWorldPos - Player.position).normalized;

        Vector3 _ovjPos = Player.position + _direction * Radius;

        transform.position = _ovjPos;

        transform.right = _direction;


        // �׽�Ʈ�� ���� ����Ī
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("��������");
            SwitchCoroutine(Fire());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("��������");
            SwitchCoroutine(Ax());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("�˰���");
            SwitchCoroutine(Sword());
        }


    }
    private void SwitchCoroutine(IEnumerator newRoutine)
    {
        // �ڷ�ƾ ����Ī �Լ�
        if(_currentRoutine != null)
        {
            StopCoroutine(_currentRoutine);
        }
        _currentRoutine = StartCoroutine(newRoutine);
    }
    private IEnumerator Fire()
    {
        // ���� �ݺ�
        while (true)
        {
            if (_bulletPerfab != null)
            {
                // ����ü ����
                GameObject _bullet = Instantiate(_bulletPerfab, _muzzlePos.position, Quaternion.identity);
                _bullet.transform.right = _direction;
                // �ð��� ������ ����
                Destroy(_bullet, _bulletTime);
            }
            // ����ü�� �߻� ����
            yield return new WaitForSeconds(GameManager.instance._player.MagicAttackSpeed);
        }
    }
    private IEnumerator Ax()
    {
        // ���� �ݺ�
        while (true)
        {
            if (_bulletPerfab != null)
            {
                // ����ü ����
                GameObject _ax = Instantiate(_axPerfab, _muzzlePos.position, Quaternion.identity);
                _ax.transform.right = _direction;
                // �ð��� ������ ����
                Destroy(_ax, _meleeAttack);
            }
            // ����ü�� �߻� ����
            yield return new WaitForSeconds(GameManager.instance._player.AxAttackSpeed);
        }
    }
    private IEnumerator Sword()
    {
        // ���� �ݺ�
        while (true)
        {
            if (_bulletPerfab != null)
            {
                // ����ü ����
                GameObject _sword = Instantiate(_swordPerfab, _muzzlePos.position, Quaternion.identity);
                _sword.transform.right = _direction;
                // �ð��� ������ ����
                Destroy(_sword, _meleeAttack);
            }
            // ����ü�� �߻� ����
            yield return new WaitForSeconds(GameManager.instance._player.SwordAttackSpeed);
        }
    }

}
