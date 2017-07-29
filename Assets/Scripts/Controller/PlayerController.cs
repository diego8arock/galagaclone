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
        public AudioClip Shot;
        public AudioClip Explode;
        public AudioClip StayDead;
        public AudioClip Results;

        // Use this for initialization
        void Start()
        {
            _player = new Player();
            _screenRation = (float)Screen.width / Screen.height;
            _widthOrtho = Camera.main.orthographicSize * _screenRation;
            animator = GetComponent<Animator>();
            GetComponent<AudioSource>().playOnAwake = false;
            GetComponent<AudioSource>().loop = false;
        }

        // Update is called once per frame
        void Update()
        {
            Fly();
            FireBullet();
        }

        void FireBullet()
        {
            if (animator.GetInteger("state") == 0)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    if (_player.Bullets.Count < 2)
                    {
                        GameObject bulletItem = (GameObject)Instantiate(Bullet, transform.position + new Vector3(0, 0.5f), Quaternion.identity);
                        _player.AddBullet(bulletItem, VelocityBullet);
                        GetComponent<AudioSource>().clip = Shot;
                        GetComponent<AudioSource>().loop = false;
                        GetComponent<AudioSource>().Play();
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
                GetComponent<AudioSource>().clip = Explode;
                GetComponent<AudioSource>().loop = false;
                GetComponent<AudioSource>().Play();
                StartCoroutine(TakeLive());
            }
        }

        IEnumerator TakeLive()
        {
            yield return new WaitForSeconds(1);
            if (GameController.Lives != null && GameController.Lives.Count == 0)
            {
                gameObject.GetComponent<Renderer>().enabled = false;
                Text GameOver = GameObject.Find("GameOverText").GetComponent<Text>();
                GameOver.enabled = true;
                GetComponent<AudioSource>().clip = Results;
                GetComponent<AudioSource>().loop = true;
                GetComponent<AudioSource>().Play();
            } else
            {
                gameObject.GetComponent<Renderer>().enabled = false;
                GetComponent<AudioSource>().clip = StayDead;
                GetComponent<AudioSource>().loop = true;
                GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(5);
                animator.SetInteger("state", 0);
                Text ReadyText = GameObject.Find("ReadyText").GetComponent<Text>();
                ReadyText.enabled = true;
                yield return new WaitForSeconds(2);
                ReadyText.enabled = false;
                GameController.GetLive(gameObject);
                gameObject.GetComponent<Renderer>().enabled = true;
                GetComponent<AudioSource>().Stop();
            }
        }
    }
}
