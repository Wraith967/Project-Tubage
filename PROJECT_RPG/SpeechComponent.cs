using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PROJECT_RPG
{
    // This class is used for entities that are involved in conversation
    class SpeechComponent : DrawableComponent
    {
        private float _updateSpeed;
        private float _updateCount;
        private String[] _convoLoop;
        private String _currentConvo;
        private int _convoCount;
        private bool isActive;

        public SpeechComponent(Vector2 position, bool active)
            : base(position, null)
        {
            isActive = active;
        }

        public float UpdateSpeed
        {
            get { return _updateSpeed; }
            set { _updateSpeed = value; }
        }

        public float UpdateCount
        {
            get { return _updateCount; }
            set { _updateCount = value; }
        }

        public int ConvoCount
        {
            get { return _convoCount; }
            set { _convoCount = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public override void Initialize()
        {
            _updateSpeed = 5000f;
            _updateCount = _updateSpeed;
            _convoCount = 0;
            _currentConvo = _convoLoop[_convoCount];
        }

        public override void Update(GameTime gametime)
        {
            if (isActive)
            {
                _updateCount -= gametime.ElapsedGameTime.Milliseconds;
                if (_updateCount <= 0)
                {
                    _updateCount = _updateSpeed;
                    _convoCount++;
                    _currentConvo = _convoLoop[_convoCount];
                }
            }
        }

        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            // TODO: Add spritebatch stuff
        }

    }
}
