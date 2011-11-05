using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PROJECT_RPG
{
    public class MapTextures
    {
        // Dictionaries use same key with different values
        Dictionary<String,Texture2D> textures;
        private ContentManager content;
        Dictionary<String, String> tiles;

        public MapTextures(ContentManager c)
        { 
            content = c;
            populateTileDictionary();
            populateTextureList();
        }

        private void populateTileDictionary()
        {
            tiles = new Dictionary<string, string>();
            tiles.Add("Empty", "tiles/null_space");
            tiles.Add("Tile", "tiles/tile");
            tiles.Add("Wood", "tiles/wood");
            tiles.Add("Top", "tiles/top_space");
            tiles.Add("Bottom", "tiles/bot_space");
            tiles.Add("Left", "tiles/left_space");
            tiles.Add("Right", "tiles/right_space");

        }
        
        private Texture2D loadTexture(String name)
        {
            return content.Load<Texture2D>(name);
        }

        private void populateTextureList()
        {
            textures = new Dictionary<string, Texture2D>();
            foreach (var pair in tiles)
                textures.Add(pair.Key, loadTexture(pair.Value));
        }

        public Texture2D getTexture(String key)
        { return textures[key]; }
    }
}
