using ProjectVS.Manager;

using UnityEngine;

public class Reposition : MonoBehaviour
{
    public bool isActive = true;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isActive) return;
        // 벗어난 콜라이더가 "Area" 태그를 가지고 있지 않으면 아무 작업도 하지 않고 종료
        if (!collision.CompareTag("Area"))
            return;
        // 플레이어의 현재 위치를 가져옴
        Vector3 _playerPos = GameManager.Instance.playerMove.transform.position;
        // 현재 오브젝트의 위치
        Vector3 _myPos = transform.position;
        // 플레이어 위치와 오브젝트 위치의 x, y 거리 차이 계산
        float _diffx = Mathf.Abs(_playerPos.x - _myPos.x);
        float _diffy = Mathf.Abs(_playerPos.y - _myPos.y);
        // 플레이어가 입력한 방향 벡터
        Vector3 _playerDir = GameManager.Instance.playerMove.MoveInput;
        // x축 방향 : 왼쪽이면 -1, 아니면 +1
        float _dirx = _playerDir.x < 0 ? -1 : 1;
        // y축 방향 : 아래쪽이면 -1 , 아니면 +1
        float _diry = _playerDir.y < 0 ? -1 : 1;

        //현재 오브젝트의 태그에 따라 처리 분기
        switch (transform.tag)
        {
            case "Ground":
                // x축 차이가 더 크면 좌우로 이동
                if (_diffx > _diffy)
                {
                    // x축 방향으로 40만큼 이동
                    transform.Translate(Vector3.right * _dirx * 80);
                }
                // y축 차이가 더 크면 상하로 이동
                else if (_diffx < _diffy)
                {
                    // y축 방향으로 40만큼 이동
                    transform.Translate(Vector3.up * _diry * 80);
                }
                break;
            case "Enemy":
                break;
        }
    }
}
