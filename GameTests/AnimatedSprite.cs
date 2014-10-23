using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTests {
    class AnimatedSprite {
        public Texture2D texture { get; set; }
        public int rows { get; set; }
        public int columns { get; set; }
        public int tileSize { get; set; }
        private int currentFrame;
        private int totalFrames;
        private int timeSinceLastFrame;
        private int animateSpeed;
        public Enums.SpriteDirection spriteDirection { get; set; }

        public AnimatedSprite(Texture2D texture, int rows, int columns, int tileSize, Enums.SpriteDirection spriteDirection) {
            this.texture = texture;
            this.rows = rows;
            this.columns = columns;
            this.tileSize = tileSize;
            this.spriteDirection = spriteDirection;

            this.currentFrame = 0;
            this.totalFrames = rows * columns;
            this.timeSinceLastFrame = 0;
            this.animateSpeed = 2;
        }

        public void Update(GameTime time, Enums.SpriteDirection spriteDirection) {
            this.spriteDirection = spriteDirection;
            timeSinceLastFrame += time.ElapsedGameTime.Milliseconds;
            if(timeSinceLastFrame > animateSpeed * 100) {
                timeSinceLastFrame = 0;
                currentFrame++;
                if(currentFrame == totalFrames) {
                    currentFrame = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location) {
            int width = texture.Width / columns;
            int height = tileSize * rows;
            int row = (int)spriteDirection;
            int column = currentFrame % columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Begin();
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }

        public void setIdleState() {
            // Just reset frame to zero since that's the "stand still" pose.
            currentFrame = 0;
        }
    }
}
