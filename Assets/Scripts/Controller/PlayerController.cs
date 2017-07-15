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
        public float ShipRadius = 0.6f;

        private float _screenRation;
        private float _widthOrtho;
        private Player _player;

        // Use this for initialization
        void Start()
        {
            _player = new Player();
            _screenRation = (float)Screen.width / Screen.height;
            _widthOrtho = Camera.main.orthographicSize * _screenRation;
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
                if (_player.Bullets.Count < 2)
                {
                    GameObject bulletItem = (GameObject)Instantiate(Bullet, transform.position, Quaternion.identity);
                    _player.AddBullet(bulletItem, VelocityBullet);
                }
            }

            for (int i = 0; i < _player.Bullets.Count; i++)
            {
                Bullet bulletFire = _player.Bullets[i];
                if (bulletFire != null)
                {
                    bulletFire.gameObject.transform.Translate(new Vector3(0, 1) * Time.deltaTime * bulletFire.Velocity);

                    Vector3 bulletScreenPosition = Camera.main.WorldToScreenPoint(bulletFire.gameObject.transform.position);
                    if(bulletScreenPosition.y >= Screen.height || bulletScreenPosition.y < 0)
                    {
                        DestroyObject(bulletFire.gameObject);
                        _player.Bullets.Remove(bulletFire);
                    }
                }
            }
        }

        void Fly()
        {
            Vector3 posicion = transform.position + new Vector3(Input.GetAxis("Horizontal"), 0, 0) * Velocity * Time.deltaTime;
               
            if(posicion.x + ShipRadius > _widthOrtho)
                posicion.x = _widthOrtho - ShipRadius;

            if (posicion.x - ShipRadius < -_widthOrtho)
                posicion.x = -_widthOrtho + ShipRadius;

            transform.position = posicion;       
        }
    }
}
