using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameTests.DataModel;

namespace GameTests {
    class AnimatedSprite {
        public Texture2D texture { get; set; }
        public int rows { get; set; }
        public int columns { get; set; }
        public int moveTime { get; set; }
        public float currentMoveTime { get; set; }
        public Vector2 lastPosition { get; set; }
        public Vector2 currentPosition { get; set; }
        public Vector2 endPosition { get; set; }
        public Vector2 offset { get; set; }
        private int currentFrame;
        private int totalFrames;
        private int timeSinceLastFrame;
        public int animateSpeed { get; set; } 
        public bool moving = false;
        public Enums.SpriteDirection spriteDirection { get; set; }

        public AnimatedSprite(Texture2D texture, int rows, int columns, int spriteDirection, Vector2 position, 
            int animateSpeed, int moveTime, Vector2 offset) {

            this.texture = texture;
            this.rows = rows;
            this.columns = columns;
            this.spriteDirection = Enums.GetDirectionKey(spriteDirection);
            this.lastPosition = position;
            this.currentPosition = position;
            this.animateSpeed = animateSpeed;
            this.moveTime = moveTime;
            this.offset = offset;

            this.currentFrame = 0;
            this.totalFrames = rows * columns;
            this.timeSinceLastFrame = 0;
            this.currentMoveTime = 0;
        }

        public void Update(GameTime time, Enums.SpriteDirection spriteDirection) {
            this.spriteDirection = spriteDirection;
            timeSinceLastFrame += time.ElapsedGameTime.Milliseconds;
            if(timeSinceLastFrame > animateSpeed) {
                timeSinceLastFrame = 0;
                currentFrame++;
                if(currentFrame == totalFrames) {
                    currentFrame = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location) {
            int width = texture.Width / columns;
            int height = Fn.TILESIZE * rows;
            int row = (int)spriteDirection;
            int column = currentFrame % columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Vector2 offsetVector = Fn.OffsetVector(location, offset);
            Rectangle destinationRectangle = new Rectangle(
                (int)offsetVector.X, 
                (int)offsetVector.Y, 
                width, height
            );

            spriteBatch.Begin();
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }

        public void SetIdleState() {
            // Just reset frame to zero since that's the "stand still" pose.
            currentFrame = 0;
        }
    }
}
