using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameTests.DataModel;

namespace GameTests {
    class Player {
        private AnimatedSprite sprite { get; set; }
        private int delayedMilliseconds = 0;

        public Player(Texture2D texture, int rows, int columns, int spriteDirection, Vector2 position) {
            int animateSpeed = 200;
            int moveTime = 600;
            this.sprite = new AnimatedSprite(texture, rows, columns, spriteDirection, position, animateSpeed, moveTime);
        }

        public void Update(GameTime gameTime, Func<Vector2, bool> IsCollision) {
            if(Keyboard.GetState().IsKeyDown(Keys.Down)) {
                sprite.Update(gameTime, Enums.SpriteDirection.South);
                delayedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if(delayedMilliseconds > Fn.MOVE_DELAY && !IsCollision(sprite.currentPosition)) {
                    /*
                    if(sprite.currentMoveTime < sprite.moveTime) {
                        // Update movement time.
                        sprite.currentMoveTime += gameTime.ElapsedGameTime.Milliseconds;
                        
                        // Do linear interpolation.
                        float lerpAmount = (float)(sprite.currentMoveTime / sprite.moveTime);
                        position = Vector2.Lerp(sprite.lastPosition, new Vector2(position.X, position.Y + TILESIZE), lerpAmount);
                    } else {
                        delayedMilliseconds = 0;
                        sprite.currentMoveTime = 0;
                        sprite.lastPosition = position;
                    }
                     */
                    sprite.currentPosition = new Vector2(sprite.currentPosition.X, sprite.currentPosition.Y + Fn.TILESIZE);
                    delayedMilliseconds = 0;
                }
            } else if(Keyboard.GetState().IsKeyDown(Keys.Right)) {
                sprite.Update(gameTime, Enums.SpriteDirection.East);
                delayedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if(delayedMilliseconds > Fn.MOVE_DELAY && !IsCollision(sprite.currentPosition)) {
                    sprite.currentPosition = new Vector2(sprite.currentPosition.X + Fn.TILESIZE, sprite.currentPosition.Y);
                    delayedMilliseconds = 0;
                }
            } else if(Keyboard.GetState().IsKeyDown(Keys.Left)) {
                sprite.Update(gameTime, Enums.SpriteDirection.West);
                delayedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if(delayedMilliseconds > Fn.MOVE_DELAY && !IsCollision(sprite.currentPosition)) {
                    sprite.currentPosition = new Vector2(sprite.currentPosition.X - Fn.TILESIZE, sprite.currentPosition.Y);
                    delayedMilliseconds = 0;
                }
            } else if(Keyboard.GetState().IsKeyDown(Keys.Up)) {
                sprite.Update(gameTime, Enums.SpriteDirection.North);
                delayedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if(delayedMilliseconds > Fn.MOVE_DELAY && !IsCollision(sprite.currentPosition)) {
                    sprite.currentPosition = new Vector2(sprite.currentPosition.X, sprite.currentPosition.Y - Fn.TILESIZE);
                    delayedMilliseconds = 0;
                }
            } else {
                // Set sprite to idle state
                sprite.SetIdleState();
                delayedMilliseconds = 0;
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
