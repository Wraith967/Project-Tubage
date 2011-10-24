using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace PROJECT_RPG
{
    class GameEntity
    {
        private String _name;
        private Vector2 _position;
        private Texture2D _texture;
        private ArrayList _components;
        private ArrayList _drawables;
        private ArrayList _entities;

        public GameEntity(String name)
            : this(name, new Vector2(20,20), null)
        {
        }

        public GameEntity(String name, Vector2 position, Texture2D texture)
        {
            _name = name;
            _position = position;
            _texture = texture;
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; }
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

        public void addComponent(ViralComponentInterface component)
        {
            _components.Add(component);
            if (component is DrawableComponent)
                _drawables.Add(component);
        }

        public void addGameEntity(GameEntity entity)
        {
            _entities.Add(entity);
        }

        public void Update(GameTime gametime)
        {
            foreach (ViralComponentInterface a in _components)
            {
                a.Update(gametime);
            }
            foreach (GameEntity e in _entities)
            {
                e.Update(gametime);
            }
        }

        public void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            foreach (DrawableComponent a in _drawables)
            {
                a.Draw(gametime, spritebatch);
            }
            foreach (GameEntity e in _entities)
            {
                e.Draw(gametime, spritebatch);
            }
        }
    }
}
