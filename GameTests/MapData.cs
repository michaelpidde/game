using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTests {
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
        public int[] start { get; set; }
        public int[][] collision { get; set; }
    }
}
