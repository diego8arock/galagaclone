using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Model {

    public enum AllienState { CREATED, ENTERING, STILL_IN_GRID, RETURN_TO_GRID, DIVING_ONE, DIVING_TWO }
    public enum AllienType { BEE, BUTTERFLY, BOSS, BOSS_GREEN }

    public class Allien {

        public GameObject gameObject { set; get; }

        public float Velocity { set; get; }

        public AllienState state { set; get; }

        public AllienType type { set; get; }

        public List<Bullet> Bullets { set; get; }

        public string TileName { set; get; }

        public Vector3 PlayerPosition { set; get; }

        public float TimeToAttack { set; get; }

        public float AttackSpeed { set; get; }

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
                    case AllienState.STILL_IN_GRID:
                        return 50;
                    case AllienState.ENTERING:
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
                    case AllienState.STILL_IN_GRID:
                        return 80;
                    case AllienState.ENTERING:
                        return 160;
                    case AllienState.DIVING_ONE:
                        return 160;
                    case AllienState.DIVING_TWO:
                        return 160;
                }
            }
            else if (AllienType.BOSS == type || AllienType.BOSS_GREEN == type)
            {
                switch (state)
                {
                    case AllienState.STILL_IN_GRID:
                        return 150;
                    case AllienState.ENTERING:
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

        public void AddBullet(GameObject gameObject, float velocity)
        {
            Bullet bullet = new Bullet();
            bullet.gameObject = gameObject;
            bullet.Velocity = velocity;
            Bullets.Add(bullet);
        }
    }
}
