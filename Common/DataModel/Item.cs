using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Common.DataModel {
    public class Item {
        public Vector2 position { get; set; }
        public Vector2 offset { get; set; }
        public int itemId { get; set; }
    }
}
