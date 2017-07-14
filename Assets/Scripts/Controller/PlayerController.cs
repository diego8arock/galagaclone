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

        private Player _player;

        // Use this for initialization
        void Start()
        {
            _player = new Player();
        }

        // Update is called once per frame
        void Update()
        {
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
    }
}
