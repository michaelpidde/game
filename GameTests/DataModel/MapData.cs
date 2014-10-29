using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameTests.DataModel {
    /// <summary>
    /// This is a class representation of the .map JSON data files.
    /// This is a member of the MapArea composite object and probably
    /// should not be instantiated on it's own in the game code 
    /// unless you can find a really good reason for it (doubtful).
    /// </summary>
    class MapData {
        public String title { get; set; }
        public String id { get; set; }
        public int[][] map { get; set; }
        public int[][] map2 { get; set; }
        public int[][] collision { get; set; }
        public Vector2 start { get; set; }
        public int direction { get; set; }
        public Door[] doors { get; set; }

        public bool HasDoorLink(int doorLinkId) {
            foreach(Door door in this.doors) {
                if(door.linkId == doorLinkId) {
                    return true;
                }
            }
            return false;
        }
    }
}
