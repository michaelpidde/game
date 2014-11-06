using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MapEditor {
    class MapCell {
        public int layer1 { get; set; }
        public int layer2 { get; set; }
        public int collision { get; set; }
        public int item { get; set; }
        public int door { get; set; }
    }
}
