﻿using System;

namespace ConsoleApplication4
{
    public sealed class Extractor
    {
        public static int DecodeIntegerSize(System.IO.BinaryReader rd)
        {
            byte byteValue;
            int count;

            byteValue = rd.ReadByte();
            if (byteValue != 0x02)        // indicates an ASN.1 integer value follows
                return 0;

            byteValue = rd.ReadByte();
            if (byteValue == 0x81)
            {
                count = rd.ReadByte();    // data size is the following byte
            }
            else if (byteValue == 0x82)
            {
                byte hi = rd.ReadByte();  // data size in next 2 bytes
                byte lo = rd.ReadByte();
                count = BitConverter.ToUInt16(new[] { lo, hi }, 0);
            }
            else
            {
                count = byteValue;        // we already have the data size
            }

            //remove high order zeros in data
            while (rd.ReadByte() == 0x00)
            {
                count -= 1;
            }
            rd.BaseStream.Seek(-1, System.IO.SeekOrigin.Current);

            return count;
        }
        public static byte[] AlignBytes(byte[] inputBytes, int alignSize)
        {
            int inputBytesSize = inputBytes.Length;

            if ((alignSize != -1) && (inputBytesSize < alignSize))
            {
                byte[] buf = new byte[alignSize];
                for (int i = 0; i < inputBytesSize; ++i)
                {
                    buf[i + (alignSize - inputBytesSize)] = inputBytes[i];
                }
                return buf;
            }
            else
            {
                return inputBytes;
            }
        }
    }
}
