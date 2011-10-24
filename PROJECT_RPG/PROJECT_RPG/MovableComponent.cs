using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PROJECT_RPG
{
    class MovableComponent : ViralComponentInterface
    {
        private float _speed;
        private int _direction;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public int Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public override void Initialize()
        {
        }

        public override void Update(GameTime gametime)
        {
            Speed += Direction;
        }

    }
}
