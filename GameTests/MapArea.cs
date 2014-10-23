using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTests {
    class MapArea {
        public int tileSize { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public MapData mapData { get; set; }
        public bool[][] collisionArray { get; set; }

        public MapArea(int tileSize, int id) {
            this.tileSize = tileSize;
            this.mapData = LoadMap(id);
            this.height = mapData.map.Length - 1;
            this.width = mapData.map[0].Length - 1;
            this.collisionArray = GetCollisionArray();
        }

        private MapData LoadMap(int id) {
            //string base64;
            string unencoded;
            using(StreamReader reader = new StreamReader("Content/Maps/" + id.ToString() + ".map")) {
                //base64 = reader.ReadToEnd();
                unencoded = reader.ReadToEnd();
            }
            //string unencoded = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
            return JsonConvert.DeserializeObject<MapData>(unencoded);
        }

        public bool[][] GetCollisionArray() {
            int[][] collisionMap = this.mapData.collision;
            bool[][] collisionArray = new bool[collisionMap.Length][];
            for (int y = 0; y < collisionMap.Length; y++) {
                collisionArray[y] = new bool[collisionMap[0].Length];
                for (int x = 0; x < collisionMap[0].Length; x++) {
                    if (collisionMap[y][x] == -1) {
                        collisionArray[y][x] = false;
                    } else {
                        collisionArray[y][x] = true;
                    }
                }
            }
            return collisionArray;
        }

        public void DrawBackground(SpriteBatch spriteBatch, Texture2D textureSheet) {
            Draw(this.mapData.map, spriteBatch, textureSheet);
        }

        public void DrawCollision(SpriteBatch spriteBatch, Texture2D textureSheet) {
            Draw(this.mapData.collision, spriteBatch, textureSheet);
        }

        private void Draw(int[][] map, SpriteBatch spriteBatch, Texture2D textureSheet) {
            int sheetWidth = textureSheet.Width / this.tileSize;
            int tilePosition = 0;
            int tileY = 0;
            int tileX = 0;
            double tilePositionPrecision = 0;

            spriteBatch.Begin();
            for(int y = 0; y <= this.height; y++) {
                for(int x = 0; x <= this.width; x++) {
                    tilePosition = map[y][x];
                    if(tilePosition > -1) {
                        tilePositionPrecision = (double)tilePosition / (double)sheetWidth;
                        tileY = (int)tilePositionPrecision;
                        tileX = (int)((tilePositionPrecision - (int)tilePositionPrecision) * sheetWidth);

                        spriteBatch.Draw(
                            textureSheet,
                            new Rectangle(x * this.tileSize, y * this.tileSize, this.tileSize, this.tileSize),
                            new Rectangle(tileX * this.tileSize, tileY * this.tileSize, this.tileSize, this.tileSize),
                            Color.White
                        );
                    }
                }
            }
            spriteBatch.End();
        }
    }
}
