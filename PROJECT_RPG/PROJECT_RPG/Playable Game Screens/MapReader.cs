using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PROJECT_RPG
{
    class MapReader
    {
        public static MapTile[,] readTileMap(String inputFile, PlayableMainGameScreen owner)
        {
            StreamReader reader = new StreamReader(inputFile);
            int width = Int32.Parse(reader.ReadLine());
            int height = Int32.Parse(reader.ReadLine());
            MapTile[,] mapArr = new MapTile[height,width];
            char[,] tileArr = new char[height,width];
            String temp = reader.ReadLine();
            String[] tempArr;
            int i = 0;
            int j = 0;
            while (temp != null)
            {
                tempArr = temp.Split(' ');
                for (j = 0; j < width; j++)
                {
                    char[] tempChar = tempArr[j].ToCharArray();
                    tileArr[i,j] = tempChar[0];
                }
                i++;
                temp = reader.ReadLine();
            }

            mapTiles(tileArr, mapArr, width, height, owner);

            return mapArr;
        }

        private static void mapTiles(char[,] strArr, MapTile[,] mapArr, int width, int height, PlayableMainGameScreen owner)
        {
            char tempChar;
            MapTile tempTile;
            String fileName;
            MapTileCollisionType collision;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    tempChar = strArr[i, j];
                    switch (tempChar)
                    {
                        case '0':
                            fileName = "tiles/null_space";
                            collision = MapTileCollisionType.FullCollision;
                            break;
                        case '1':
                            fileName = "tiles/floor_space";
                            collision = MapTileCollisionType.NoCollision;
                            break;
                        case '2':
                            fileName = "tiles/rug_space";
                            collision = MapTileCollisionType.NoCollision;
                            break;
                        case '3':
                            fileName = "tiles/top_space";
                            collision = MapTileCollisionType.HalfCollisionTop;
                            break;
                        case '4':
                            fileName = "tiles/bot_space";
                            collision = MapTileCollisionType.HalfCollisionBot;
                            break;
                        case '5':
                            fileName = "tiles/left_space";
                            collision = MapTileCollisionType.HalfCollisionLeft;
                            break;
                        case '6':
                            fileName = "tiles/right_space";
                            collision = MapTileCollisionType.HalfCollisionRight;
                            break;
                        case '7':
                            fileName = "tiles/topleft_space";
                            collision = MapTileCollisionType.HalfCollisionCornerTopLeft;
                            break;
                        case '8':
                            fileName = "tiles/topright_space";
                            collision = MapTileCollisionType.HalfCollisionCornerTopRight;
                            break;
                        case '9':
                            fileName = "tiles/botright_space";
                            collision = MapTileCollisionType.HalfCollisionCornerBotRight;
                            break;
                        case 'A':
                            fileName = "tiles/botleft_space";
                            collision = MapTileCollisionType.HalfCollisionCornerBotLeft;
                            break;
                        default:
                            fileName = "";
                            collision = MapTileCollisionType.FullCollision;
                            break;
                    }
                    tempTile = new MapTile(fileName, new Microsoft.Xna.Framework.Vector2(j * 20, i * 20), collision, owner);
                    mapArr[i, j] = tempTile;
                }
            }
        }
    }
}
