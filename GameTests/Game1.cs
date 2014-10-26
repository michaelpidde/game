#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using GameTests.DataModel;
#endregion

namespace GameTests {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game {
        //
        // Constants that used to be here are now in Fn.cs. You can call me a dirty little bastard for
        // using procedural-type static globals in an OO program, but the scope of this program
        // allows for it. There are also some useful static functions in there. I'll determine 
        // later if the way that I'm using that class is really a hellish implementation.
        //
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D textureSheet;
        private SpriteFont font;
        private Player player;
        private MapArea mapArea;
        bool showDebug = true;

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
            graphics.PreferredBackBufferHeight = Fn.TILESIZE * Fn.Y_TILES;
            graphics.PreferredBackBufferWidth = Fn.TILESIZE * Fn.X_TILES;
            graphics.ApplyChanges();

            // Load initial map.
            mapArea = new MapArea(Fn.TILESIZE, 2);

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
            player = new Player(texture, 1, 4, mapArea.mapData.direction, Fn.ScaleVector(mapArea.mapData.start));
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

            player.Update(gameTime, mapArea.IsCollision);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            // Draw first map layer.
            mapArea.DrawBackground(spriteBatch, textureSheet);

            // Draw second map layer.
            mapArea.DrawSecond(spriteBatch, textureSheet);

            // Draw collision layer.
            mapArea.DrawCollision(spriteBatch, textureSheet);

            // Draw player on screen.
            player.Draw(spriteBatch);

            if(showDebug) {
                // Draw some debug output.
                spriteBatch.Begin();
                string stats = "Map: " + mapArea.mapData.title;
                stats += "\nHeight: " + mapArea.height;
                stats += "\nWidth: " + mapArea.width;
                stats += player.WriteDebug();
                spriteBatch.DrawString(font, stats, new Vector2(10, 10), Color.White);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
