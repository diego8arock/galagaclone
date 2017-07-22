using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Controller
{
    public class AlienController : MonoBehaviour
    {
        private Text Score;
        private Text HighScore;
        // Use this for initialization
        void Start()
        {
            Score = GameObject.Find("Score").GetComponent<Text>();
            HighScore = GameObject.Find("HighScore").GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "bullet" || collision.gameObject.tag == "ship")
            {
                DestroyObject(collision.gameObject);
                DestroyObject(gameObject);
                foreach(Model.Allien allien in GameController.Alliens)
                {
                    if(this.gameObject.Equals(allien.gameObject))
                    {
                        GameController.ScoreValue += allien.GetAllienValue();
                        break;
                    }
                }
                Score.text = GameController.ScoreValue.ToString();
                if (GameController.ScoreValue > GameController.HighScoreValue)
                {
                    GameController.HighScoreValue = GameController.ScoreValue;
                    HighScore.text = GameController.ScoreValue.ToString();
                }
            }
        }

    }
}
