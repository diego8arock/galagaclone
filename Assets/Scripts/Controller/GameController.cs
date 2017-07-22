using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine.UI;

namespace Controller
{
    public class GameController : MonoBehaviour
    {
        public GameObject PrefaBee;
        public GameObject PrefaButterfly;
        public GameObject PrefaBoss;
        public GameObject Ship;
        public GameObject Player;
        public float VelocityAllien;
        public static List<Allien> Alliens { set; get; }
        private bool Started = false;
        private Vector3 InitialPosition;
        private List<GameObject> Lives;
        private Text StartText;
        private Text StageText;
        private Text LiveText;
        private Text ReadyText;
        public float TimeLiveEnable = 0.02f;
        public static int ScoreValue;
        public static int HighScoreValue;
        private float y = 3.7f;

        // Use this for initialization
        void Start()
        {
            ScoreValue = 0;
            HighScoreValue = 30000;
            Alliens = new List<Allien>();
            Lives = new List<GameObject>();
            StartText = GameObject.Find("StartText").GetComponent<Text>();
            StageText = GameObject.Find("StageText").GetComponent<Text>();
            LiveText = GameObject.Find("LiveText").GetComponent<Text>();
            ReadyText = GameObject.Find("ReadyText").GetComponent<Text>();

            GameObject ship = (GameObject)Instantiate(Ship, new Vector3(4.66f, -0.53f, 0), Quaternion.identity);
            Lives.Add(ship);
            ship = (GameObject)Instantiate(Ship, new Vector3(5.25f, -0.53f, 0), Quaternion.identity);
            Lives.Add(ship);
            ship = (GameObject)Instantiate(Ship, new Vector3(5.901f, -0.53f, 0), Quaternion.identity);
            Lives.Add(ship);
            
            StageText.enabled = false;
            ReadyText.enabled = false;
            Player.SetActive(false);
            StartCoroutine(BeginStart());
        }

        IEnumerator BeginStart()
        {
            yield return new WaitForSeconds(1);
            StartText.enabled = false;
            yield return new WaitForSeconds(1);
            StageText.enabled = true;
            yield return new WaitForSeconds(2);
            StageText.enabled = false;
            ReadyText.enabled = true;
            Started = true;
            yield return new WaitForSeconds(2);
            ReadyText.enabled = false;
            StartGame();
        }

        void StartGame()
        {
            GetLive();

            AddAllien(4, PrefaBoss, AllienType.BOSS);
            AddAllien(8, PrefaButterfly, AllienType.BUTTERFLY);
            AddAllien(8, PrefaButterfly, AllienType.BUTTERFLY);
            AddAllien(10, PrefaBee, AllienType.BEE);
            AddAllien(10, PrefaBee, AllienType.BEE);
        }

        void GetLive()
        {
            Lives[Lives.Count - 1].SetActive(false);
            Lives.RemoveAt(Lives.Count - 1);
            Player.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            TimeLiveEnable -= Time.deltaTime;
            if (Started && TimeLiveEnable <= 0)
            {
                LiveText.enabled = !LiveText.enabled;
                TimeLiveEnable = 1;
            }
        }

        void ShowAlliens(List<Allien> AlliensTemp)
        {
            float x = 0;
            int index = 0;
            foreach (Allien Allien in AlliensTemp)
            {
                if (index == 0)
                {
                    x = Allien.GetAllienPositionX();
                }
                VelocityAllien += Time.deltaTime * VelocityAllien;
                Vector3 FinalPosition = new Vector3(x, this.y, 0);
                Allien.gameObject.transform.position = FinalPosition;
                index++;
                x += 0.7f;
            }
            this.y -= 0.7f;
        }

        public void AddAllien(int size, GameObject allienObject, AllienType type)
        {

            List<Allien> AlliensTemp = new List<Allien>();
            for (int i = 1; i <= size; i++)
            {
                InitialPosition = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(5.0f, 10.0f), 0);
                GameObject allienItem = (GameObject)Instantiate(allienObject, InitialPosition, Quaternion.identity);

                Allien allien = new Allien();
                allien.gameObject = allienItem;
                allien.Velocity = VelocityAllien;
                allien.state = AllienState.DIVING_ALONE;
                allien.type = type;
                Alliens.Add(allien);
                AlliensTemp.Add(allien);
            }
            ShowAlliens(AlliensTemp);
        }
    }
}
