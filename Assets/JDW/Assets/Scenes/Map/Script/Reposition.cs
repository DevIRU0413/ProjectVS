using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        // ��� �ݶ��̴��� "Area" �±׸� ������ ���� ������ �ƹ� �۾��� ���� �ʰ� ����
        if (!collision.CompareTag("Area"))
            return;
        // �÷��̾��� ���� ��ġ�� ������
        Vector3 _playerPos = GameManager.instance._playerMove.transform.position;
        // ���� ������Ʈ�� ��ġ
        Vector3 _myPos = transform.position;
        // �÷��̾� ��ġ�� ������Ʈ ��ġ�� x, y �Ÿ� ���� ���
        float _diffx = Mathf.Abs(_playerPos.x - _myPos.x);
        float _diffy = Mathf.Abs(_playerPos.y - _myPos.y);
        // �÷��̾ �Է��� ���� ����
        Vector3 _playerDir = GameManager.instance._playerMove.MoveInput;
        // x�� ���� : �����̸� -1, �ƴϸ� +1
        float _dirx = _playerDir.x < 0 ? -1 : 1;
        // y�� ���� : �Ʒ����̸� -1 , �ƴϸ� +1
        float _diry = _playerDir.y < 0 ? -1 : 1;

        //���� ������Ʈ�� �±׿� ���� ó�� �б�
        switch (transform.tag)
        {
            case "Ground":
                // x�� ���̰� �� ũ�� �¿�� �̵�
                if(_diffx > _diffy)
                {
                    // x�� �������� 40��ŭ �̵�
                    transform.Translate(Vector3.right * _dirx * 40);
                }
                // y�� ���̰� �� ũ�� ���Ϸ� �̵�
                else if(_diffx < _diffy)
                {
                    // y�� �������� 40��ŭ �̵�
                    transform.Translate(Vector3.up * _diry * 40);
                }
                break;
            case "Enemy":
                break;
        }
    }
}
