using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PROJECT_RPG
{
    // Any item that has a visual representation on screen
    class DrawableComponent : ViralComponentInterface
    {
        private Vector2 _position;
        private Texture2D _texture;

        public DrawableComponent()
            : this(new Vector2(20, 20), null)
        {
        }

        public DrawableComponent(Vector2 position, Texture2D texture)
        {
            _position = position;
            _texture = texture;
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        public override void Initialize()
        {
        }

        public override void Update(GameTime gametime)
        {
        }

        public virtual void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            spritebatch.Draw(_texture, _position, Color.White);
        }
    }
}
