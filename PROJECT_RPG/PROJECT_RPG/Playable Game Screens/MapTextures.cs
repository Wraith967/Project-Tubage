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
            tiles.Add("70s", "tiles/70s_tile");
            tiles.Add("Asphalt", "tiles/asphalt");
            tiles.Add("Cement", "tiles/cement");
            tiles.Add("Cobble", "tiles/cobble");
            tiles.Add("Grass", "tiles/grass");
            tiles.Add("Wall", "tiles/wall");
            tiles.Add("Bed", "entities/bed");
            tiles.Add("Cabinet", "entities/cabinet");
            tiles.Add("CoffeePot", "entities/coffeepot");
            tiles.Add("EnergyDrink", "entities/energy_drink");
            tiles.Add("Router", "entities/router");
            tiles.Add("Table", "entities/table");
            tiles.Add("Toaster", "entities/toaster");
            tiles.Add("TopCabinet", "entities/top_cabinet");
            tiles.Add("TV", "entities/tv");
            tiles.Add("Window", "entities/window");
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
