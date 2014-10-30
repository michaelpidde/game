﻿using System;
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
            int moveTime = 200;
            this.sprite = new AnimatedSprite(texture, rows, columns, spriteDirection, position, animateSpeed, moveTime);
        }

        public void ResetSprite(Vector2 position, Enums.SpriteDirection direction) {
            this.sprite.currentPosition = position;
            this.sprite.spriteDirection = direction;
            delayedMilliseconds = 0;
        }

        public void Update(GameTime gameTime, Func<Vector2, bool> IsCollision, Func<Vector2, bool> IsDoor, 
            Func<Vector2, Enums.SpriteDirection, bool> DoorAction) {

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
                    delayedMilliseconds = 0;
                }
            } else {
                if(Keyboard.GetState().IsKeyDown(Keys.Down)) {
                    sprite.Update(gameTime, Enums.SpriteDirection.South);
                    delayedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;

                    if(delayedMilliseconds > Fn.MOVE_DELAY && IsDoor(sprite.currentPosition)) {
                        DoorAction(sprite.currentPosition, sprite.spriteDirection);
                    }

                    if(delayedMilliseconds > Fn.MOVE_DELAY && !IsCollision(sprite.currentPosition)) {
                        sprite.moving = true;
                        sprite.endPosition = Fn.ScaleVector(Fn.GetNextPosition(sprite.currentPosition));
                    }
                } else if(Keyboard.GetState().IsKeyDown(Keys.Right)) {
                    sprite.Update(gameTime, Enums.SpriteDirection.East);
                    delayedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;

                    if(delayedMilliseconds > Fn.MOVE_DELAY && IsDoor(sprite.currentPosition)) {
                        DoorAction(sprite.currentPosition, sprite.spriteDirection);
                    }

                    if(delayedMilliseconds > Fn.MOVE_DELAY && !IsCollision(sprite.currentPosition)) {
                        sprite.moving = true;
                        sprite.endPosition = Fn.ScaleVector(Fn.GetNextPosition(sprite.currentPosition));
                    }
                } else if(Keyboard.GetState().IsKeyDown(Keys.Left)) {
                    sprite.Update(gameTime, Enums.SpriteDirection.West);
                    delayedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;

                    if(delayedMilliseconds > Fn.MOVE_DELAY && IsDoor(sprite.currentPosition)) {
                        DoorAction(sprite.currentPosition, sprite.spriteDirection);
                    }

                    if(delayedMilliseconds > Fn.MOVE_DELAY && !IsCollision(sprite.currentPosition)) {
                        sprite.moving = true;
                        sprite.endPosition = Fn.ScaleVector(Fn.GetNextPosition(sprite.currentPosition));
                    }
                } else if(Keyboard.GetState().IsKeyDown(Keys.Up)) {
                    sprite.Update(gameTime, Enums.SpriteDirection.North);
                    delayedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;

                    if(delayedMilliseconds > Fn.MOVE_DELAY && IsDoor(sprite.currentPosition)) {
                        DoorAction(sprite.currentPosition, sprite.spriteDirection);
                    }

                    if(delayedMilliseconds > Fn.MOVE_DELAY && !IsCollision(sprite.currentPosition)) {
                        sprite.moving = true;
                        sprite.endPosition = Fn.ScaleVector(Fn.GetNextPosition(sprite.currentPosition));
                    }
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
