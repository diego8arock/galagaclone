using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Model;

namespace Controller
{
    public class GameController : MonoBehaviour
    {
        public GameObject Allien1;
        public GameObject Allien2;
        public GameObject Allien3;
        public float VelocityAllien;
        public List<Allien> Alliens { set; get; }
        private bool Started = false;
        private Vector3 InitialPosition;
        public AnimationCurve Curva;
        public int AlliensXRow = 8;
        private List<GameObject> AllienObjects;

        // Use this for initialization
        void Start()
        {
            Alliens = new List<Allien>();
            AllienObjects = new List<GameObject>();
            AllienObjects.Add(Allien1);
            AllienObjects.Add(Allien3);
            AllienObjects.Add(Allien2);
            InitialPosition = new Vector3(-2.65f, 5.41f, 0);
        }

        // Update is called once per frame
        void Update()
        {
            bool presioneBoton = Input.GetKeyDown(KeyCode.KeypadEnter);
            if (presioneBoton && !Started)
            {
                Debug.Log("Enter");
                float x = -3.33f;
                float y = 3.7f;
                foreach (GameObject Allien in AllienObjects )
                {
                    for (int i = 1; i <= AlliensXRow; i++)
                    {
                        x += 0.7f;
                        Vector3 FinalPosition = new Vector3(x, y, 0);
                        GameObject allienItem = (GameObject)Instantiate(Allien, InitialPosition, Quaternion.identity);
                        Allien AllienItem = AddAllien(allienItem, VelocityAllien);

                        Vector3 nuevaPosicion = Vector3.LerpUnclamped(InitialPosition, FinalPosition, Curva.Evaluate(VelocityAllien));
                        AllienItem.gameObject.transform.position = nuevaPosicion;
                    }
                    x = -3.33f;
                    y -= 0.7f;
                }
                Started = true;
            }
            ShowAlliens();
        }

        void ShowAlliens()
        {
        }

        public Allien AddAllien(GameObject gameObject, float velocity)
        {
            Allien allien = new Allien();
            allien.gameObject = gameObject;
            allien.Velocity = velocity;
            Alliens.Add(allien);
            return allien;
        }
    }
}
