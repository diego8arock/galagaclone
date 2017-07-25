using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine.UI;

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

        Animator animator;

        // Use this for initialization
        void Start()
        {
            _player = new Player();
            _screenRation = (float)Screen.width / Screen.height;
            _widthOrtho = Camera.main.orthographicSize * _screenRation;
            animator = GetComponent<Animator>();
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
                if (bulletFire != null && bulletFire.gameObject != null)
                {
                    bulletFire.gameObject.transform.Translate(new Vector3(0, 1) * Time.deltaTime * bulletFire.Velocity);

                    Vector3 bulletScreenPosition = Camera.main.WorldToScreenPoint(bulletFire.gameObject.transform.position);
                    if (bulletScreenPosition.y >= Screen.height || bulletScreenPosition.y < 0)
                    {
                        DestroyObject(bulletFire.gameObject);
                        _player.Bullets.Remove(bulletFire);
                    }
                }

                if (bulletFire.gameObject == null)
                    _player.Bullets.Remove(bulletFire);
            }
        }

        void Fly()
        {
            Vector3 posicion = transform.position + new Vector3(Input.GetAxis("GalagaHInput"), 0, 0) * Velocity * Time.deltaTime;

            if (posicion.x + ShipRadius > (_widthOrtho- 2.4f))
            {
                posicion.x = (_widthOrtho - 2.4f) - ShipRadius;
            }

            if (posicion.x - ShipRadius < -_widthOrtho)
                posicion.x = -_widthOrtho + ShipRadius;

            transform.position = posicion;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (animator.GetInteger("state") == 0)
            {
                animator.SetInteger("state", 1);
                if (collision.gameObject.tag == "bulletAlien")
                {
                    DestroyObject(collision.gameObject);
                }
                if (collision.gameObject.tag == "alien")
                {
                    GameController.setScore(collision.gameObject);
                }
                StartCoroutine(TakeLive());
            }
        }

        IEnumerator TakeLive()
        {
            yield return new WaitForSeconds(1);
            if (GameController.Lives.Count == 0)
            {
                gameObject.SetActive(false);
                Text GameOver = GameObject.Find("GameOverText").GetComponent<Text>();
                GameOver.enabled = true;
            } else
            {
                gameObject.GetComponent<Renderer>().enabled = false;
                yield return new WaitForSeconds(5);
                animator.SetInteger("state", 0);
                Text ReadyText = GameObject.Find("ReadyText").GetComponent<Text>();
                ReadyText.enabled = true;
                yield return new WaitForSeconds(2);
                ReadyText.enabled = false;
                GameController.GetLive(gameObject);
                gameObject.GetComponent<Renderer>().enabled = true;
            }
        }
    }
}
