using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
    }
}
