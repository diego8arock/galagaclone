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
        private float startTime;
        private float journeyLength;
        public float Speed = 3.0f;
        private Vector3 startPosition;
        private Vector3 endPosition;
        private GameObject TileToMove;
        private Animator animator;
        public AudioClip Explode;
        public AudioClip Results;
        private float archSize = 0.8f;

        // Use this for initialization
        void Start()
        {
            _allien = new Allien();
            disparo = false;
            animator = GetComponent<Animator>();
            GetComponent<AudioSource>().playOnAwake = false;
        }

        // Update is called once per frame
        void Update()
        {
           if (GameController._alliens != null && GameController._alliens.Count > 0 && !disparo)
            {
                int alienIndex = Random.Range(1, 2);

                if (gameObject.Equals(GameController._alliens[alienIndex].gameObject))
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

                            //Vector3 bulletScreenPosition = Camera.main.WorldToScreenPoint(bulletFire.gameObject.transform.position);
                            if (bulletFire.gameObject.transform.position.y < -5f)
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

            if (_allien != null)
            {
                if (_allien.state == Model.AllienState.ENTERING)
                {
                    MoveAlientToGrid();
                }

                if (_allien.state == AllienState.STILL_IN_GRID)
                {
                    transform.position = TileToMove.transform.position;
                    startPosition = transform.position;
                }

                if (_allien.state == AllienState.DIVING_ONE)
                {
                    AllienAttack();
                }

                if (_allien.state == AllienState.RETURN_TO_GRID)
                {
                    ReturnAlientToGrid();
                }
            }
        }

        void MoveAlientToGrid()
        {
            float distCovered = (Time.time - startTime) * Speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startPosition, endPosition, fracJourney);
            if (Vector3.Distance(transform.position, endPosition) < 0.1f)
                _allien.state = AllienState.STILL_IN_GRID;
        }

        void AllienAttack()
        {
            float distCovered = (Time.time - _allien.TimeToAttack) * _allien.AttackSpeed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startPosition, _allien.PlayerPosition, fracJourney);
            if (Vector3.Distance(transform.position, _allien.PlayerPosition) < 0.1f)
            {
                startPosition = transform.position;
                endPosition = TileToMove.transform.position;
                startTime = Time.time;
                _allien.state = AllienState.RETURN_TO_GRID;
            }
        }

        void ReturnAlientToGrid()
        {      
            float distCovered = (Time.time - startTime) * _allien.AttackSpeed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startPosition, endPosition, fracJourney);
            if (Vector3.Distance(transform.position, endPosition) < 0.1f)
            {
                transform.position = TileToMove.transform.position;
                startPosition = transform.position;
                _allien.state = AllienState.STILL_IN_GRID;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "bullet")
            {
                DestroyObject(collision.gameObject);
            }
            Model.Allien allienRemove = null;
            if (animator.GetInteger("state") >= 0)
            {
                bool destroy = false;
                if (collision.gameObject.tag == "ship")
                {
                    destroy = true;
                } else
                {
                    if (GameController._alliens != null)
                    {
                        foreach (Model.Allien allien in GameController._alliens)
                        {
                            if (gameObject.Equals(allien.gameObject))
                            {
                                Debug.Log("allien.type " + allien.type);
                                if (allien.type.Equals(AllienType.BOSS_GREEN))
                                {
                                    allien.type = AllienType.BOSS;
                                    animator.SetInteger("state", 1);
                                }
                                else
                                {
                                    allienRemove = allien;
                                    destroy = true;
                                }
                                break;
                            }
                        }
                    }
                }
                GameController.setScore(this.gameObject);
                if(destroy)
                {
                    if (allienRemove != null)
                    {
                        GameController._alliens.Remove(allienRemove);
                    }
                    animator.SetInteger("state", -1);
                    GetComponent<AudioSource>().clip = Explode;
                    GetComponent<AudioSource>().Play();
                    StartCoroutine(Dead());
                }
            }
        }

        public void SetTileToAlien(object[] fparams)
        {
            object allien = fparams[0];
            object tile = fparams[1];
            TileToMove = (GameObject)tile;
            _allien = (Allien)allien;
            startPosition = transform.position;
            endPosition = TileToMove.transform.position;
            _allien.state = AllienState.ENTERING;
            startTime = Time.time;
            journeyLength = Vector3.Distance(startPosition, endPosition);
        }

        IEnumerator Dead()
        {
            //animation.Play();
            yield return new WaitForSeconds(1);
            if (GameController._alliens.Count == 0)
            {
                gameObject.GetComponent<Renderer>().enabled = false;
                Text GameOver = GameObject.Find("GameOverText").GetComponent<Text>();
                GameOver.enabled = true;
                GetComponent<AudioSource>().clip = Results;
                GetComponent<AudioSource>().loop = true;
                GetComponent<AudioSource>().Play();
            } else
            {
                DestroyObject(gameObject);
            }
        }

    }
}
