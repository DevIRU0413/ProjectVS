using ProjectVS;

using UnityEngine;

namespace ProjectVS.JDW
{
    public class Reposition : MonoBehaviour
    {
        public bool IsActive = true;

        private PlayerMove _playerMove;
        private PlayerConfig _player;

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!IsActive) return;
            // 벗어난 콜라이더가 "Area" 태그를 가지고 있지 않으면 아무 작업도 하지 않고 종료
            if (!collision.CompareTag("Area"))
                return;
            if (_player == null)
            {
                GameObject go = GameObject.FindWithTag("Player");
                if (go != null)
                    _player = go.GetComponent<PlayerConfig>();
            }

            if (_player == null)
            {
                Debug.LogWarning("Reposition: Player가 null입니다.");
                return;
            }
            // 플레이어의 현재 위치를 가져옴
            Vector3 playerPos = _player.transform.position;
            // 현재 오브젝트의 위치
            Vector3 myPos = transform.position;
            // 플레이어 위치와 오브젝트 위치의 x, y 거리 차이 계산
            float diffx = Mathf.Abs(playerPos.x - myPos.x);
            float diffy = Mathf.Abs(playerPos.y - myPos.y);
            // 플레이어가 입력한 방향 벡터
            Vector3 playerDir = GetPlayerMoveInput();
            // x축 방향 : 왼쪽이면 -1, 아니면 +1
            float dirx = playerDir.x < 0 ? -1 : 1;
            // y축 방향 : 아래쪽이면 -1 , 아니면 +1
            float diry = playerDir.y < 0 ? -1 : 1;

            //현재 오브젝트의 태그에 따라 처리 분기
            switch (transform.tag)
            {
                case "Ground":
                    // x축 차이가 더 크면 좌우로 이동
                    if (diffx > diffy)
                    {
                        // x축 방향으로 40만큼 이동
                        transform.Translate(Vector3.right * dirx * 80);
                    }
                    // y축 차이가 더 크면 상하로 이동
                    else if (diffx < diffy)
                    {
                        // y축 방향으로 40만큼 이동
                        transform.Translate(Vector3.up * diry * 80);
                    }
                    break;
                case "Enemy":
                    break;
            }
        }

        private Vector3 GetPlayerMoveInput()
        {
            if (_playerMove == null)
            {
                GameObject go = GameObject.FindWithTag("Player");
                if (go != null)
                    _playerMove = go.GetComponent<PlayerMove>();
            }

            return (_playerMove == null) ? Vector3.zero : _playerMove.MoveInput;
        }
    }
}
