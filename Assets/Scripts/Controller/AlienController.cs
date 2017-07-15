using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class AlienController : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
           
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "bullet")
            {
                DestroyObject(collision.gameObject);
                DestroyObject(gameObject);
            }
        }

    }
}
