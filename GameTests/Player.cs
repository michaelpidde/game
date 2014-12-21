using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Common.DataModel;

namespace GameTests {
    class Player {
        private AnimatedSprite sprite { get; set; }
        private MapArea mapArea { get; set; }
        private int delayedMilliseconds = 0;

        public Player(Texture2D texture, int rows, int columns, int spriteDirection, Vector2 position, ref MapArea mapArea) {
            int animateSpeed = 150;
            int moveTime = 300;
            this.sprite = new AnimatedSprite(texture, rows, columns, spriteDirection, position, animateSpeed, moveTime);
            this.mapArea = mapArea;
        }

        public void ResetSprite(Vector2 position, Enums.SpriteDirection direction) {
            delayedMilliseconds = 0;
            this.sprite.currentPosition = position;
            this.sprite.lastPosition = position;
            this.sprite.endPosition = position;
            this.sprite.spriteDirection = direction;
        }

        /*
         * This gets called from the Update function.
         */
        private void PlayerMove() {

            if(delayedMilliseconds > Fn.MOVE_DELAY && mapArea.IsDoor(sprite.currentPosition)) {
                mapArea.DoorAction(sprite.currentPosition, sprite.spriteDirection);
            }

            if(delayedMilliseconds > Fn.MOVE_DELAY && !mapArea.IsCollision(sprite.currentPosition)) {
                sprite.moving = true;
                sprite.endPosition = Fn.ScaleVector(Fn.GetNextPosition(sprite.currentPosition));
            }
        }

        public void Update(GameTime gameTime) {

            if(sprite.moving) {
                // Update movement time.
                sprite.currentMoveTime += gameTime.ElapsedGameTime.Milliseconds;

                if(sprite.currentMoveTime <= sprite.moveTime) {
                    // Do linear interpolation.
                    float lerpAmount = (float)(sprite.currentMoveTime / sprite.moveTime);
                    sprite.currentPosition = Vector2.Lerp(sprite.lastPosition, sprite.endPosition, lerpAmount);
                } else {
                    sprite.moving = false;
                    sprite.currentMoveTime = 0;
                    sprite.currentPosition = sprite.endPosition;
                    sprite.lastPosition = sprite.currentPosition;
                }

                sprite.Update(gameTime, sprite.spriteDirection);

            } else {

                // This is a delay that allows the player to turn in a direction
                // without automatically moving in that direction right away.
                if(Fn.ArrowKeyDown()) {
                    delayedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                } else {
                    delayedMilliseconds = 0;
                }

                if(Keyboard.GetState().IsKeyDown(Keys.Down)) {
                    sprite.Update(gameTime, Enums.SpriteDirection.South);
                    PlayerMove();
                } else if(Keyboard.GetState().IsKeyDown(Keys.Right)) {
                    sprite.Update(gameTime, Enums.SpriteDirection.East);
                    PlayerMove();
                } else if(Keyboard.GetState().IsKeyDown(Keys.Left)) {
                    sprite.Update(gameTime, Enums.SpriteDirection.West);
                    PlayerMove();
                } else if(Keyboard.GetState().IsKeyDown(Keys.Up)) {
                    sprite.Update(gameTime, Enums.SpriteDirection.North);
                    PlayerMove();
                } else {
                    // Set sprite to idle state
                    sprite.SetIdleState();
                    delayedMilliseconds = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            sprite.Draw(spriteBatch, sprite.currentPosition);
        }

        public string WriteDebug() {
            string stats = "\nDelay mils: " + delayedMilliseconds.ToString();
            stats += "\nPosition: " + sprite.currentPosition.ToString();
            stats += "\nTile: " + Fn.UnscaleVector(sprite.currentPosition).ToString();
            return stats;
        }
    }
}
