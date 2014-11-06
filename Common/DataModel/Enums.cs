using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DataModel {
    public static class Enums {
        public enum SpriteDirection {
            North = 3,
            South = 0,
            East = 1,
            West = 2
        };

        public static SpriteDirection GetDirectionKey(int direction) {
            switch(direction) {
                case 0:
                    return SpriteDirection.South;
                case 1:
                    return SpriteDirection.East;
                case 2:
                    return SpriteDirection.West;
                case 3:
                    return SpriteDirection.North;
                default:
                    return SpriteDirection.South;
            }
        }
    }
}
