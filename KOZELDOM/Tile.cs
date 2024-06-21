using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOZELDOM
{
    public class Tile
    {
        public int Value1 { get; set; }
        public int Value2 { get; set; }
        public string filepath { get; set; }

        public bool isRotate { get; set; }

        public Tile(int value1, int value2, string filepath)
        {
            Value1 = value1;
            Value2 = value2;
            this.filepath = filepath;
        }

        public Tile()
        {

        }

        public int Sum()
        {
            return Value1 + Value2;
        }

        public static Tile GetMin(Tile tile1, Tile tile2)
        {
            return tile1.Sum() > tile2.Sum() ? tile2 : tile1;
        }
    }

}
