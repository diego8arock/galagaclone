using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Model;

namespace Controller
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject Bullet;
        public float VelocityBullet;
        public float Velocity = 8f;
        public float LimitXPos = 2.49f;
        public float LimitXNeg = -6.1789f;

        private Player _player;

        // Use this for initialization
        void Start()
        {
            _player = new Player();
        }

        // Update is called once per frame
        void Update()
        {
            Fly();
            FireBullet();
        }

        void FireBullet()
        {
            if (Input.GetButtonDown("Jump"))
            {
                GameObject bulletItem = (GameObject)Instantiate(Bullet, transform.position, Quaternion.identity);
                _player.AddBullet(bulletItem, VelocityBullet);
            }

            for (int i = 0; i < _player.Bullets.Count; i++)
            {
                Bullet bulletFire = _player.Bullets[i];
                if (bulletFire != null)
                {
                    bulletFire.gameObject.transform.Translate(new Vector3(0, 1) * Time.deltaTime * bulletFire.Velocity);
                }
            }
        }

        void Fly()
        {
            Vector3 posicion = transform.position + new Vector3(Input.GetAxis("Horizontal"), 0, 0) * Velocity * Time.deltaTime;
            if (posicion.x >= LimitXNeg && posicion.x <= LimitXPos)
            {
                transform.position = posicion;
            }
        }
    }
}
