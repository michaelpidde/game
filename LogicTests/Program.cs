using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicTests {
    class Program {
        static void Main(string[] args) {
            int[] positions = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            double tilePositionPrecision;
            int col = 0;
            int row = 0;
            foreach(int pos in positions) {
                Console.WriteLine("Tile position: " + pos.ToString());
                tilePositionPrecision = (double)pos / (double)4;
                Console.WriteLine("Precision: " + tilePositionPrecision.ToString("F"));
                row = (int)tilePositionPrecision;
                Console.WriteLine("Row: " + row.ToString());
                switch((tilePositionPrecision - (int)tilePositionPrecision).ToString("F")) {
                    case "0.00":
                        col = 0;
                        break;
                    case "0.25":
                        col = 1;
                        break;
                    case "0.50":
                        col = 2;
                        break;
                    case "0.75":
                        col = 3;
                        break;
                }
                Console.WriteLine("Column: " + col.ToString());
                Console.WriteLine("-------------------------------------");
            }
            Console.Read();
        }
    }
}
