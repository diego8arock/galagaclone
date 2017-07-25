using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Model {

    public enum AllienState { IN_FORMATION, DIVING_ALONE, DIVING_ONE, DIVING_TWO }
    public enum AllienType { BEE, BUTTERFLY, BOSS }

    public class Allien {

        public GameObject gameObject { set; get; }

        public float Velocity { set; get; }

        public AllienState state { set; get; }

        public AllienType type { set; get; }
        public List<Bullet> Bullets { set; get; }

        public Allien()
        {
            Bullets = new List<Bullet>();
        }

        public int GetAllienValue()
        {
            if(AllienType.BEE == type)
            {
                switch (state)
                {
                    case AllienState.IN_FORMATION:
                        return 50;
                    case AllienState.DIVING_ALONE:
                        return 100;
                    case AllienState.DIVING_ONE:
                        return 100;
                    case AllienState.DIVING_TWO:
                        return 100;
                }
            }
            else if (AllienType.BUTTERFLY == type)
            {
                switch (state)
                {
                    case AllienState.IN_FORMATION:
                        return 80;
                    case AllienState.DIVING_ALONE:
                        return 160;
                    case AllienState.DIVING_ONE:
                        return 160;
                    case AllienState.DIVING_TWO:
                        return 160;
                }
            }
            else if (AllienType.BOSS == type)
            {
                switch (state)
                {
                    case AllienState.IN_FORMATION:
                        return 150;
                    case AllienState.DIVING_ALONE:
                        return 400;
                    case AllienState.DIVING_ONE:
                        return 800;
                    case AllienState.DIVING_TWO:
                        return 1600;
                }
            }

            //unhandled rarity?
            return 0;
        }

        public float GetAllienPositionX()
        {
            if (AllienType.BEE == type)
            {
                return -3.88f;
            }
            else if (AllienType.BUTTERFLY == type)
            {
                return -3.18f;
            }
            else if (AllienType.BOSS == type)
            {
                return -1.78f;
            }

            //unhandled rarity?
            return 0;
        }

        public void AddBullet(GameObject gameObject, float velocity)
        {
            Bullet bullet = new Bullet();
            bullet.gameObject = gameObject;
            bullet.Velocity = velocity;
            Bullets.Add(bullet);
        }
    }
}
