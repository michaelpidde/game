#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace GameTests {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game {
        const int TILESIZE = 32;
        const int X_TILES = 11;
        const int Y_TILES = 11;
        const int MOVE_DELAY = 300;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D textureSheet;
        private SpriteFont font;
        private AnimatedSprite sprite;
        private MapArea mapArea;
        Vector2 center;
        Vector2 position;
        int delayedMilliseconds = 0;

        public Game1()
            : base() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // Set window size.
            graphics.PreferredBackBufferHeight = TILESIZE * Y_TILES;
            graphics.PreferredBackBufferWidth = TILESIZE * X_TILES;
            graphics.ApplyChanges();

            this.center = new Vector2((X_TILES / 2) * TILESIZE, (Y_TILES / 2) * TILESIZE);
            this.position = center;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textureSheet = this.Content.Load<Texture2D>("texturemap_large");
            font = this.Content.Load<SpriteFont>("font");
            Texture2D texture = this.Content.Load<Texture2D>("character_move");
            sprite = new AnimatedSprite(texture, 1, 4, TILESIZE, Enums.SpriteDirection.South);
            
            // Testing maps...
            mapArea = new MapArea(TILESIZE, 1);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        protected bool IsCollision() {
            Vector2 currentPosition = new Vector2(position.X / TILESIZE, position.Y / TILESIZE);
            Vector2 nextPosition = currentPosition;
            if(Keyboard.GetState().IsKeyDown(Keys.Down)) {
                nextPosition.Y++;
            } else if(Keyboard.GetState().IsKeyDown(Keys.Right)) {
                nextPosition.X++;
            } else if(Keyboard.GetState().IsKeyDown(Keys.Left)) {
                nextPosition.X--;
            } else if(Keyboard.GetState().IsKeyDown(Keys.Up)) {
                nextPosition.Y--;
            }
            // Add collision logic - check mapArea.collisionArray
            return mapArea.collisionArray[(int)nextPosition.Y][(int)nextPosition.X];
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                Exit();
            }

            if(Keyboard.GetState().IsKeyDown(Keys.Down)) {
                sprite.Update(gameTime, Enums.SpriteDirection.South);
                delayedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if(delayedMilliseconds > MOVE_DELAY && !IsCollision()) {
                    position = new Vector2(position.X, position.Y + TILESIZE);
                    delayedMilliseconds = 0;
                }
            } else if(Keyboard.GetState().IsKeyDown(Keys.Right)) {
                sprite.Update(gameTime, Enums.SpriteDirection.East);
                delayedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if (delayedMilliseconds > MOVE_DELAY && !IsCollision()) {
                    position = new Vector2(position.X + TILESIZE, position.Y);
                    delayedMilliseconds = 0;
                }
            } else if(Keyboard.GetState().IsKeyDown(Keys.Left)) {
                sprite.Update(gameTime, Enums.SpriteDirection.West);
                delayedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if(delayedMilliseconds > MOVE_DELAY && !IsCollision()) {
                    position = new Vector2(position.X - TILESIZE, position.Y);
                    delayedMilliseconds = 0;
                }
            } else if(Keyboard.GetState().IsKeyDown(Keys.Up)) {
                sprite.Update(gameTime, Enums.SpriteDirection.North);
                delayedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if(delayedMilliseconds > MOVE_DELAY && !IsCollision()) {
                    position = new Vector2(position.X, position.Y - TILESIZE);
                    delayedMilliseconds = 0;
                }
            } else {
                // Set sprite to idle state
                sprite.setIdleState();
                delayedMilliseconds = 0;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            // Draw first map layer.
            this.mapArea.DrawBackground(spriteBatch, textureSheet);

            // Draw collision layer.
            this.mapArea.DrawCollision(spriteBatch, textureSheet);

            // Draw player on screen.
            sprite.Draw(spriteBatch, position);

            // Draw some debug output.
            spriteBatch.Begin();
            string stats = "Map: " + mapArea.mapData.title;
            stats += "\nHeight: " + mapArea.height;
            stats += "\nWidth: " + mapArea.width;
            stats += "\nPosition: " + position.ToString();
            stats += "\nTile: " + (position.X / TILESIZE).ToString() + "," + (position.Y / TILESIZE).ToString();
            stats += "\nDelay mils: " + delayedMilliseconds.ToString();
            spriteBatch.DrawString(font, stats, new Vector2(10, 10), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
