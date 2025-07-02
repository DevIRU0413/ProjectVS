using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace ProjectVS.Player.DummyMovingObject
{
    public class DummyMovingObject : MonoBehaviour
    {
        private float _timer = 0f;

        private void Update()
        {
            if (_timer >= 0f && _timer < 5f)
            {
                transform.Translate(Vector3.right * Time.deltaTime * 1f);
                _timer += Time.deltaTime;
            }

            if (_timer >= 5f)
            {
                transform.Translate(Vector3.left * Time.deltaTime * 1f);
                _timer += Time.deltaTime;

                if (_timer >= 10f)
                {
                    _timer = 0f;
                }
            }
        }
    }
}
