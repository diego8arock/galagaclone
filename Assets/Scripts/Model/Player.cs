using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Model {

    public class Player {

        public List<Bullet> Bullets { set; get; }

        public Player()
        {
            Bullets = new List<Bullet>();
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
