using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    public class RsaTraits
    {
        public RsaTraits(int modulusLengthInBits)
        {
            int assumedLength = -1;
            double logbase = Math.Log(modulusLengthInBits, 2);
            if (logbase == (int)logbase)
            {
                assumedLength = modulusLengthInBits;
            }
            else
            {
                assumedLength = (int)(logbase + 1.0);
                assumedLength = (int)(Math.Pow(2, assumedLength));
                Debug.Assert(false);
            }

            SetTraits(assumedLength);
        }

        private void SetTraits(int assumedLength)
        {
            switch (assumedLength)
            {
                case 1024:
                    this.SizeMod = 0x80;
                    this.SizeExp = -1;
                    this.SizeD = 0x80;
                    this.SizeP = 0x40;
                    this.SizeQ = 0x40;
                    this.SizeDp = 0x40;
                    this.SizeDq = 0x40;
                    this.SizeInvQ = 0x40;
                    break;
                case 2048:
                    this.SizeMod = 0x100;
                    this.SizeExp = -1;
                    this.SizeD = 0x100;
                    this.SizeP = 0x80;
                    this.SizeQ = 0x80;
                    this.SizeDp = 0x80;
                    this.SizeDq = 0x80;
                    this.SizeInvQ = 0x80;
                    break;
                case 4096:
                    this.SizeMod = 0x200;
                    this.SizeExp = -1;
                    this.SizeD = 0x200;
                    this.SizeP = 0x100;
                    this.SizeQ = 0x100;
                    this.SizeDp = 0x100;
                    this.SizeDq = 0x100;
                    this.SizeInvQ = 0x100;
                    break;
                default:
                    Debug.Assert(false); // Unknown key size?
                    break;
            }
        }

        public int SizeMod { get { return _sizeMod; } set { _sizeMod = value; }}
        public int SizeExp { get { return _sizeExp; } set { _sizeExp = value; } }
        public int SizeD { get { return _sizeD; } set { _sizeD = value; } }
        public int SizeP { get { return _sizeP; } set { _sizeP = value; } }
        public int SizeQ { get { return _sizeQ; } set { _sizeQ = value; } }
        public int SizeDp { get { return _sizeDp; } set { _sizeDp = value; } }
        public int SizeDq { get { return _sizeDq; } set { _sizeDq = value; } }
        public int SizeInvQ { get { return _sizeInvQ; } set { _sizeInvQ = value; } }

        private int _sizeMod = -1;
        private int _sizeExp = -1;
        private int _sizeD = -1;
        private int _sizeP = -1;
        private int _sizeQ = -1;
        private int _sizeDp = -1;
        private int _sizeDq = -1;
        private int _sizeInvQ = -1;
    }
}
