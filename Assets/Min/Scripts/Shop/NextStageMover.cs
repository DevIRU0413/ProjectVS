using System.Collections;
using System.Collections.Generic;
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
                //SceneManager.LoadScene();
                Debug.Log("[NextStageMover] 씬 이동");
            }
        }
    }
}
