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
        private String[] tiles = {"tiles/null_space","tiles/tile","tiles/wood","tiles/top_space","tiles/bot_space",
                                     "tiles/left_space","tiles/right_space","tiles/topleft_space","tiles/topright_space",
                                     "tiles/botleft_space","tiles/botright_space"};
        private Texture2D[] textures;
        private ContentManager content;

        public MapTextures(ContentManager c)
        { 
            content = c;
            populateTextureList();
        }

        private Texture2D getTexture(String name)
        {
            return content.Load<Texture2D>(name);
        }

        private void populateTextureList()
        {
            textures = new Texture2D[tiles.Length];
            for (int i = 0; i < tiles.Length; i++)
                textures[i] = getTexture(tiles[i]);
        }

        public Texture2D getTexture(int index)
        { return textures[index]; }
    }
}
