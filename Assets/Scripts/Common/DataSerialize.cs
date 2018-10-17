using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.Runtime.Serialization;

namespace XWorld
{
    public class DataSerialize
    {
        public static readonly DataSerialize Instance = new DataSerialize();
        public void SerialSByte(ref sbyte data, byte[] buffer, ref int offset, bool bWrite)
        {
            if (bWrite)
            {
                buffer[offset] = Convert.ToByte(data);
            }
            else
            {
                data = Convert.ToSByte(buffer[offset]);
            }
            offset += sizeof(sbyte);
        }
        public void SerialByte(ref byte data, byte[] buffer, ref int offset, bool bWrite)
        {
            if (bWrite)
            {
                buffer[offset] = data;
            }
            else
            {
                data = buffer[offset];
            }
            offset += sizeof(byte);
        }
        public void SerialInt16(ref short data, byte[] buffer, ref int offset, bool bWrite)
        {
            if (bWrite)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer, offset, sizeof(short));
            }
            else
            {
                data = BitConverter.ToInt16(buffer, offset);
            }
            offset += sizeof(short);
        }
        public void SerialUInt16(ref ushort data, byte[] buffer, ref int offset, bool bWrite)
        {
            if (bWrite)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer, offset, sizeof(ushort));
            }
            else
            {
                data = BitConverter.ToUInt16(buffer, offset);
            }
            offset += sizeof(ushort);
        }
        public void SerialInt32(ref int data, byte[] buffer, ref int offset, bool bWrite)
        {
            if (bWrite)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer, offset, sizeof(int));

            }
            else
            {
                data = BitConverter.ToInt32(buffer, offset);
            }
            offset += sizeof(int);
        }
        public void SerialUInt32(ref uint data, byte[] buffer, ref int offset, bool bWrite)
        {
            if (bWrite)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer, offset, sizeof(uint));

            }
            else
            {
                data = BitConverter.ToUInt32(buffer, offset);
            }
            offset += sizeof(uint);
        }
        public void Serialf32(ref float data, byte[] buffer, ref int offset, bool bWrite)
        {
            if (bWrite)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer, offset, sizeof(float));
            }
            else
            {
                data = BitConverter.ToSingle(buffer, offset);
            }
            offset += sizeof(float);
        }
        public void Serialf64(ref double data, byte[] buffer, ref int offset, bool bWrite)
        {
            if (bWrite)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer, offset, sizeof(double));
            }
            else
            {
                data = BitConverter.ToDouble(buffer, offset);
            }
            offset += sizeof(double);
        }
        public void SerialString(ref byte[] dataBuffer,int dataSize, byte[] buffer, ref int offset, bool bWrite)
        {
            if (dataSize <= 0)
                return;

            if (bWrite)
            {
                Buffer.BlockCopy(dataBuffer, 0, buffer, offset, dataSize);
            }
            else
            {
                Buffer.BlockCopy(buffer, offset, dataBuffer, 0, dataSize);
            }

            offset += dataSize;
        }
        public void SerialFloatAsInt16(ref float data, float scale, byte[] buffer, ref int offset, bool bWrite)
        {
            if (bWrite)
            {
                short temp = (short)(data * scale);
                Buffer.BlockCopy(BitConverter.GetBytes(temp), 0, buffer, offset, sizeof(short));
            }
            else
            {
                short temp = BitConverter.ToInt16(buffer, offset);
                data = ((float)temp) / scale;
            }
            offset += sizeof(short);
        }
    }
}
