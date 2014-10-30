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
            int animateSpeed = 150;
            int moveTime = 300;
            this.sprite = new AnimatedSprite(texture, rows, columns, spriteDirection, position, animateSpeed, moveTime);
        }

        public void ResetSprite(Vector2 position, Enums.SpriteDirection direction) {
            delayedMilliseconds = 0;
            this.sprite.currentPosition = position;
            this.sprite.lastPosition = position;
            this.sprite.endPosition = position;
            this.sprite.spriteDirection = direction;
        }

        /*
         * This gets called from the Update function. See commentary to that function
         * regarding the delegates that are passed in.
         */
        private void PlayerMove(Func<Vector2, bool> IsCollision, Func<Vector2, bool> IsDoor,
            Func<Vector2, Enums.SpriteDirection, bool> DoorAction) {

            if(delayedMilliseconds > Fn.MOVE_DELAY && IsDoor(sprite.currentPosition)) {
                DoorAction(sprite.currentPosition, sprite.spriteDirection);
            }

            if(delayedMilliseconds > Fn.MOVE_DELAY && !IsCollision(sprite.currentPosition)) {
                sprite.moving = true;
                sprite.endPosition = Fn.ScaleVector(Fn.GetNextPosition(sprite.currentPosition));
            }
        }

        /*
         * This function is taking three functions from the MapArea as parameters. This is to avoid
         * directly passing the instance of MapArea into this player object. Instead, we want to 
         * use a few functions from the MapArea instance that exists elsewhere that can act on the
         * instance, thus not needing to know exactly where the instance comes from or store a 
         * reference to it in this object. I'm not sure whether that's a good or a bad thing
         * in the long run, but I'll figure out and modify as I go.
         */
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
                    PlayerMove(IsCollision, IsDoor, DoorAction);
                } else if(Keyboard.GetState().IsKeyDown(Keys.Right)) {
                    sprite.Update(gameTime, Enums.SpriteDirection.East);
                    PlayerMove(IsCollision, IsDoor, DoorAction);
                } else if(Keyboard.GetState().IsKeyDown(Keys.Left)) {
                    sprite.Update(gameTime, Enums.SpriteDirection.West);
                    PlayerMove(IsCollision, IsDoor, DoorAction);
                } else if(Keyboard.GetState().IsKeyDown(Keys.Up)) {
                    sprite.Update(gameTime, Enums.SpriteDirection.North);
                    PlayerMove(IsCollision, IsDoor, DoorAction);
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
