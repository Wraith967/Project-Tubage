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
            String fileIndex;
            MapTileCollisionType collision;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    tempChar = strArr[i, j];
                    switch (tempChar)
                    {
                        case '1':
                            fileIndex = "Tile";
                            collision = MapTileCollisionType.NoCollision;
                            break;
                        case '2':
                            fileIndex = "Wood";
                            collision = MapTileCollisionType.NoCollision;
                            break;
                        case '3':
                            fileIndex = "70s";
                            collision = MapTileCollisionType.NoCollision;
                            break;
                        case '4':
                            fileIndex = "Asphalt";
                            collision = MapTileCollisionType.NoCollision;
                            break;
                        case '5':
                            fileIndex = "Cement";
                            collision = MapTileCollisionType.NoCollision;
                            break;
                        case '6':
                            fileIndex = "Cobble";
                            collision = MapTileCollisionType.NoCollision;
                            break;
                        case '7':
                            fileIndex = "Grass";
                            collision = MapTileCollisionType.NoCollision;
                            break;
                        case '8':
                            fileIndex = "Wall";
                            collision = MapTileCollisionType.FullCollision;
                            break;
                        case '9':
                            fileIndex = "Window";
                            collision = MapTileCollisionType.FullCollision;
                            break;
                        case 'A':
                            fileIndex = "Cabinet";
                            collision = MapTileCollisionType.FullCollision;
                            break;
                        case 'B':
                            fileIndex = "TopCabinet";
                            collision = MapTileCollisionType.FullCollision;
                            break;
                        default:
                            fileIndex = "Empty";
                            collision = MapTileCollisionType.FullCollision;
                            break;
                    }
                    tempTile = new MapTile(fileIndex, new Microsoft.Xna.Framework.Vector2(j * 20, i * 20), collision, owner);
                    mapArr[i, j] = tempTile;
                }
            }
        }
    }
}
