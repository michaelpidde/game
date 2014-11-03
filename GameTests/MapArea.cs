using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameTests.DataModel {
    class MapArea {
        public int tileSize { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public MapData mapData { get; set; }
        public bool[][] collisionArray { get; set; }
        public int[][] doorArray { get; set; }
        public int[][] itemArray { get; set; }
        private int mapId = 2;
        private int doorLinkId = -1;
        private Vector2 offset;
        private bool reloadingMap = false;
        private bool doCallback = false;

        public bool ReloadingMap() {
            return this.reloadingMap;
        }

        public bool Callback() {
            return this.doCallback;
        }

        public void VoidCallback() {
            this.doCallback = false;
        }

        public void SetOffset(Vector2 offset) {
            this.offset = offset;
        }

        public Vector2 GetDoorPosition() {
            Vector2 position = this.mapData.start;
            int doorId = this.mapData.HasDoorLink(this.doorLinkId);
            if(doorId > -1) {
                position = this.mapData.doors[doorId].position;
            }
            return position;
        }

        public MapArea(int tileSize, Vector2 offset) {
            this.tileSize = tileSize;
            this.offset = offset;
            ReloadMap();
        }

        private MapData LoadMap() {
            //string base64;
            string unencoded;
            using(StreamReader reader = new StreamReader("Content/Maps/" + this.mapId.ToString() + ".map")) {
                //base64 = reader.ReadToEnd();
                unencoded = reader.ReadToEnd();
            }
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new Vector2Convertor());

            //unencoded = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
            return JsonConvert.DeserializeObject<MapData>(unencoded, settings);
        }

        public void ReloadMap() {
            this.mapData = LoadMap();
            this.height = mapData.map.Length - 1;
            this.width = mapData.map[0].Length - 1;
            this.collisionArray = GetCollisionArray();
            this.doorArray = GetDoorArray();
            this.itemArray = GetItemArray();
            this.reloadingMap = false;
            this.doCallback = true;
        }

        public bool DoorAction(Vector2 currentPosition, Enums.SpriteDirection spriteDirection) {
            currentPosition = Fn.UnscaleVector(currentPosition);
            Door door = this.mapData.doors[
                doorArray[(int)currentPosition.Y][(int)currentPosition.X]    
            ];
            if((int)spriteDirection == door.direction) {
                this.mapId = door.loadMapId;
                this.doorLinkId = door.linkId;
                this.reloadingMap = true;
                ReloadMap();
            }
            return true;
        }

        public bool IsDoor(Vector2 currentPosition) {
            currentPosition = Fn.UnscaleVector(currentPosition);
            if(doorArray[(int)currentPosition.Y][(int)currentPosition.X] > -1) {
                return true;
            } else {
                return false;
            }
        }

        public bool IsCollision(Vector2 currentPosition) {
            Vector2 nextPosition = Fn.GetNextPosition(currentPosition);
            
            if(nextPosition.Y >= this.collisionArray.Length || nextPosition.Y < 0 ||
                nextPosition.X >= this.collisionArray[0].Length || nextPosition.X < 0) {
                // Out of bounds - don't allow to move out of map area.
                return true;
            } else {
                return this.collisionArray[(int)nextPosition.Y][(int)nextPosition.X];
            }
        }

        private bool[][] GetCollisionArray() {
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

        private int[][] GetDoorArray() {
            Door[] doorMap = this.mapData.doors;
            int[][] doorArray = GetEmptyMapArray();
            for(int door = 0; door < doorMap.Length; door++) {
                doorArray[(int)doorMap[door].position.Y][(int)doorMap[door].position.X] = door;
            }
            return doorArray;
        }

        private int[][] GetItemArray() {
            Item[] itemMap = this.mapData.items;
            int[][] itemArray = GetEmptyMapArray();
            for(int item = 0; item < itemMap.Length; item++) {
                itemArray[(int)itemMap[item].position.Y][(int)itemMap[item].position.X] = item;
            }
            return itemArray;
        }

        public int[][] GetEmptyMapArray() {
            int[][] map = this.mapData.map;
            int[][] emptyArray = new int[map.Length][];
            for(int y = 0; y < map.Length; y++) {
                emptyArray[y] = new int[map[0].Length];
                for(int x = 0; x < map[0].Length; x++) {
                    emptyArray[y][x] = -1;
                }
            }
            return emptyArray;
        }

        public void DrawBackground(SpriteBatch spriteBatch, Texture2D textureSheet) {
            Draw(this.mapData.map, spriteBatch, textureSheet);
        }

        public void DrawSecond(SpriteBatch spriteBatch, Texture2D textureSheet) {
            Draw(this.mapData.map2, spriteBatch, textureSheet);
        }

        public void DrawCollision(SpriteBatch spriteBatch, Texture2D textureSheet) {
            Draw(this.mapData.collision, spriteBatch, textureSheet);
        }

        public void DrawItems(SpriteBatch spriteBatch, Texture2D textureSheet) {
            DrawItems(this.itemArray, spriteBatch, textureSheet);
        }

        private void DrawItems(int[][] itemArray, SpriteBatch spriteBatch, Texture2D textureSheet) {
            int sheetWidth = textureSheet.Width / this.tileSize;
            int itemId = -1;
            int tileY = 0;
            int tileX = 0;
            double tilePositionPrecision = 0;

            spriteBatch.Begin();
            for(int y = 0; y <= this.height; y++) {
                for(int x = 0; x <= this.width; x++) {
                    itemId = itemArray[y][x];
                    if(itemId > -1) {
                        Item item = this.mapData.items[itemId];

                        tilePositionPrecision = (double)item.itemId / (double)sheetWidth;
                        tileY = (int)tilePositionPrecision;
                        tileX = (int)((tilePositionPrecision - (int)tilePositionPrecision) * sheetWidth);

                        spriteBatch.Draw(
                            textureSheet,
                            new Rectangle(
                                ((int)item.position.X * this.tileSize) + (int)item.offset.X + (int)this.offset.X, 
                                ((int)item.position.Y * this.tileSize) + (int)item.offset.Y + (int)this.offset.Y, 
                                this.tileSize, this.tileSize),
                            new Rectangle(tileX * this.tileSize, tileY * this.tileSize, this.tileSize, this.tileSize),
                            Color.White
                        );
                    }
                }
            }
            spriteBatch.End();
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
                            new Rectangle(
                                (x * this.tileSize) + (int)this.offset.X, 
                                (y * this.tileSize) + (int)this.offset.Y, 
                                this.tileSize, this.tileSize
                            ),
                            new Rectangle(tileX * this.tileSize, tileY * this.tileSize, this.tileSize, this.tileSize),
                            Color.White
                        );
                    }
                }
            }
            spriteBatch.End();
        }

        public string WriteDebug() {
            string debug = "";
            /* debug += "\nDoorArray: [";
            for(int y = 0; y < doorArray.Length; y++) {
                debug += "\n[";
                for(int x = 0; x < doorArray[0].Length; x++) {
                    debug += doorArray[y][x].ToString() + ",";
                }
                debug += "]";
            }
            debug += "\n]";
             */
            debug += "\nDoorId: " + this.doorLinkId.ToString();
            return debug;
        }
    }
}
