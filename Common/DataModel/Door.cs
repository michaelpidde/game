using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Common.DataModel {
    /// <summary>
    /// This is an interaction type for teleporting between maps.
    /// </summary>
    public class Door {
        public Vector2 position { get; set; }
        public int linkId { get; set; }
        public int loadMapId { get; set; }
        public int direction { get; set; }
    }
}
