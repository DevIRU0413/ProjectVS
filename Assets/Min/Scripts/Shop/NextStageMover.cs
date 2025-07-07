using System.Collections;
using System.Collections.Generic;
using ProjectVS.Manager;

using UnityEngine;
using UnityEngine.SceneManagement;


namespace ProjectVS.Shop.NextStageMover
{
    public class NextStageMover : MonoBehaviour
    {
        [SerializeField] private LayerMask _playerLayer;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & _playerLayer) != 0)
            {
                Debug.Log("[NextStageMover] InGameScene으로 이동");
                SceneLoader.Instance.LoadSceneAsync(SceneID.InGameScene);
            }
        }
    }
}
