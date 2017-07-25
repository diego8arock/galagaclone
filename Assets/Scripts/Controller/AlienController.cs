using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Controller
{
    public class AlienController : MonoBehaviour
    {
        public GameObject Bullet;
        private Allien _allien;
        public float VelocityBullet;
        private bool disparo;

        // Use this for initialization
        void Start()
        {
            _allien = new Allien();
            disparo = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!disparo)
            {
                int alienIndex = Random.Range(1, 2);

                if (gameObject.Equals(GameController.Alliens[alienIndex].gameObject))
                {
                    if (_allien.Bullets.Count < 1)
                    {
                        GameObject bulletItem = (GameObject)Instantiate(Bullet, transform.position, Quaternion.identity);
                        _allien.AddBullet(bulletItem, VelocityBullet);
                    }

                    for (int i = 0; i < _allien.Bullets.Count; i++)
                    {
                        Bullet bulletFire = _allien.Bullets[i];
                        if (bulletFire != null && bulletFire.gameObject != null)
                        {
                            bulletFire.gameObject.transform.Translate(new Vector3(0, 1) * Time.deltaTime * -bulletFire.Velocity);

                            Vector3 bulletScreenPosition = Camera.main.WorldToScreenPoint(bulletFire.gameObject.transform.position);
                            if (bulletScreenPosition.y >= Screen.height || bulletScreenPosition.y < 0)
                            {
                                disparo = true;
                                DestroyObject(bulletFire.gameObject);
                                    _allien.Bullets.Remove(bulletFire);
                                }
                            }

                        if (bulletFire.gameObject == null)
                            _allien.Bullets.Remove(bulletFire);
                    }
                }
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "bullet")
            {
                DestroyObject(collision.gameObject);
                DestroyObject(gameObject);
                GameController.setScore(this.gameObject);
            }
        }

    }
}
