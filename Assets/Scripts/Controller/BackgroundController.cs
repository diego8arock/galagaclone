using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine.UI;
using System.Linq;

namespace Controller
{
    public class BackgroundController : MonoBehaviour
    {
        public float TimeLiveEnable = 0.3f;
        
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            TimeLiveEnable -= Time.deltaTime;
            int indicateEnable = Random.Range(1, 4);
            if (TimeLiveEnable <= 0)
            {
                if (indicateEnable == 1)
                {
                    gameObject.GetComponent<Renderer>().enabled = !gameObject.GetComponent<Renderer>().enabled;
                }
                TimeLiveEnable = 0.3f;
            }

            if(GameController.Started)
            {
                float velocity = 0;
                if (gameObject.tag.Equals("redStar"))
                {
                    velocity = 1.5f;
                }
                else if (gameObject.tag.Equals("blueStar"))
                {
                    velocity = 2f;
                }
                else if (gameObject.tag.Equals("yellowStar"))
                {
                    velocity = 3f;
                }
                else if (gameObject.tag.Equals("greenStar"))
                {
                    velocity = 1.5f;
                }
                transform.Translate(new Vector3(0, 1) * Time.deltaTime * -velocity);

                //Vector3 starScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
                if (transform.position.y < -5f)
                {
                    transform.position = new Vector3(transform.position.x, 5f, transform.position.z);
                }
            }
        }        
    }
}
