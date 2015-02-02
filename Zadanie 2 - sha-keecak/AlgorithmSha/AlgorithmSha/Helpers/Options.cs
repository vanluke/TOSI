using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmSha.Helpers
{
    public class Options
    {
        public static Options c224 = new Options(1152, 448, 28);
        public static Options c256 = new Options(1088, 512, 32);
        public static Options c384 = new Options(832, 768, 48);
        public static Options c512 = new Options(576, 1024, 64);
        private Options(int blockSize, int capacities, int diversifier)
        {
            this.BlockSize = blockSize;
            this.Capacities = capacities;
            this.Diversifier = diversifier;
        }
        public int BlockSize;
        public int Capacities;
        public int Diversifier;
    }
}
