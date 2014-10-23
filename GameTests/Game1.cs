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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D textureSheet;
        private SpriteFont font;
        private AnimatedSprite sprite;
        private int spriteDirection;
        private MapArea mapArea;
        Vector2 center;

        private enum SpriteDirection {
            South = 0,
            East = 1,
            West = 2,
            North = 3
        }

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
            spriteDirection = (int)SpriteDirection.South;
            sprite = new AnimatedSprite(texture, 1, 4, TILESIZE, spriteDirection);
            
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
                spriteDirection = (int)SpriteDirection.South;
                sprite.Update(gameTime, spriteDirection);
            } else if(Keyboard.GetState().IsKeyDown(Keys.Right)) {
                spriteDirection = (int)SpriteDirection.East;
                sprite.Update(gameTime, spriteDirection);
            } else if(Keyboard.GetState().IsKeyDown(Keys.Left)) {
                spriteDirection = (int)SpriteDirection.West;
                sprite.Update(gameTime, spriteDirection);
            } else if(Keyboard.GetState().IsKeyDown(Keys.Up)) {
                spriteDirection = (int)SpriteDirection.North;
                sprite.Update(gameTime, spriteDirection);
            } else {
                // Set sprite to idle state
                sprite.setIdleState();
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

            spriteBatch.Begin();
            string stats = "Map: " + mapArea.mapData.title;
            stats += "\nHeight: " + mapArea.height;
            stats += "\nWidth: " + mapArea.width;
            spriteBatch.DrawString(font, stats, new Vector2(10, 10), Color.White);
            spriteBatch.End();

            sprite.Draw(spriteBatch, center);

            base.Draw(gameTime);
        }
    }
}
