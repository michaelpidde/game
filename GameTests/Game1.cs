#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Common.DataModel;
#endregion

namespace GameTests {
    

    public class Game1 : Game {
        //
        // Constants that used to be here are now in Fn.cs.
        //
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D textureSheet;
        private Texture2D itemSheet;
        private SpriteFont font;
        private Player player;
        private MapArea mapArea;
        bool showDebug = true;

        public Game1()
            : base() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        
        protected override void Initialize() {
            // Set window size.
            graphics.PreferredBackBufferHeight = Fn.TILESIZE * Fn.Y_TILES;
            graphics.PreferredBackBufferWidth = Fn.TILESIZE * Fn.X_TILES;
            graphics.ApplyChanges();

            // Load initial map.
            mapArea = new MapArea(Fn.TILESIZE);

            base.Initialize();
        }


        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textureSheet = this.Content.Load<Texture2D>("texturemap_large");
            itemSheet = this.Content.Load<Texture2D>("items");
            font = this.Content.Load<SpriteFont>("font");
            Texture2D texture = this.Content.Load<Texture2D>("character_move");
            player = new Player(texture, 1, 4, mapArea.mapData.direction, Fn.ScaleVector(mapArea.mapData.start), ref mapArea);
        }


        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime) {
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                Exit();
            }

            if(!mapArea.ReloadingMap()) {
                if(mapArea.Callback()) {
                    // This condition will happen after a new map is loaded.
                    // We'll need to reset the player position to the door position.
                    player.ResetSprite(Fn.ScaleVector(mapArea.GetDoorPosition()), Enums.GetDirectionKey(mapArea.mapData.direction));
                    mapArea.VoidCallback();
                }
                player.Update(gameTime);
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            if(!mapArea.ReloadingMap()) {
                // Draw first map layer.
                mapArea.DrawBackground(spriteBatch, textureSheet);

                // Draw second map layer.
                mapArea.DrawSecond(spriteBatch, textureSheet);

                // Draw collision layer.
                mapArea.DrawCollision(spriteBatch, textureSheet);

                // Draw items.
                mapArea.DrawItems(spriteBatch, itemSheet);

                // Draw player on screen.
                player.Draw(spriteBatch);

                if(showDebug) {
                    // Draw some debug output.
                    spriteBatch.Begin();
                    string stats = "Map: " + mapArea.mapData.title;
                    stats += "\nHeight: " + mapArea.height;
                    stats += "\nWidth: " + mapArea.width;
                    stats += player.WriteDebug();
                    stats += mapArea.WriteDebug();
                    spriteBatch.DrawString(font, stats, new Vector2(10, 10), Color.White);
                    spriteBatch.End();
                }
            }

            base.Draw(gameTime);
        }
    }
}
