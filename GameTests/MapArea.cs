using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace GameTests {
    class MapArea {
        public int tileSize { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public MapData mapData { get; set; }

        public MapArea(int tileSize, int id) {
            this.tileSize = tileSize;
            this.mapData = loadMap(id);
            this.height = mapData.map.Length - 1;
            this.width = mapData.map[0].Length - 1;
        }

        private MapData loadMap(int id) {
            //string base64;
            string unencoded;
            using(StreamReader reader = new StreamReader("Content/Maps/" + id.ToString() + ".map")) {
                //base64 = reader.ReadToEnd();
                unencoded = reader.ReadToEnd();
            }
            //string unencoded = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
            return JsonConvert.DeserializeObject<MapData>(unencoded);
        }
    }
}
