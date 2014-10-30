using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameTests {
    static class Fn {
        public const int TILESIZE = 32;
        public const int X_TILES = 22;
        public const int Y_TILES = 14;
        public const int MOVE_DELAY = 300;

        public static Vector2 ScaleVector(Vector2 vector) {
            return new Vector2(vector.X * TILESIZE, vector.Y * TILESIZE);
        }

        public static Vector2 UnscaleVector(Vector2 vector) {
            return new Vector2(vector.X / TILESIZE, vector.Y / TILESIZE);
        }

        public static Vector2 GetNextPosition(Vector2 currentPosition) {
            currentPosition = Fn.UnscaleVector(currentPosition);
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
            return nextPosition;
        }
    }
}
