using System;
using System.Collections.Generic;
using System.Text;

namespace RconLib
{
    public enum Endianness
    {
        BigEndian,
        LittleEndian
    }

    public static class BytewiseIO
    {
        /// <summary>
        /// Reads a value from a buffer
        /// </summary>
        /// <param name="index">the index from which to read from</param>
        /// <param name="buffer">the buffer to read from</param>
        /// <param name="length">the amount of bytes to read</param>
        /// <param name="endianness">the endianness of the buffer</param>
        /// <returns>The </returns>
        private static ulong ReadBuffer(int index, byte[]buffer, int length, Endianness endianness)
        {
            ulong retVal = 0;

            if (index < 0)
            {
                throw new ArgumentException("ReadBuffer:: index cannot be below 0");
            }

            if (buffer == null)
            {
                throw new ArgumentException("ReadBuffer:: buffer cannot be null");
            }

            if (index + length > buffer.Length)
            {
                throw new ArgumentException("ReadBuffer:: Requested read length is greater than the length of the buffer");
            }

            if(length > sizeof(ulong))
            {
                throw new ArgumentException($"ReadBuffer:: Can only read up to length of ulong ({sizeof(ulong)}) bytes.");
            }

            if (endianness == Endianness.LittleEndian)
            {
                for (int i = index + length - 1; i >= index; i--)
                {
                    retVal |= ((ulong)buffer[i] << ((i - index) * 8));
                }
            }
            else
            {
                for (int i = index; i < index + length; i++)
                {
                    retVal |= ((ulong)buffer[i] << ((length * 8 - 8) - (i - index) * 8));
                }
            }

            return retVal;
        }

        /// <summary>
        /// Writes a specified value to a buffer. Handles up to ulong values
        /// </summary>
        /// <param name="index">the index in the buffer to write to</param>
        /// <param name="buffer">the buffer to write the value to</param>
        /// <param name="val">the value to write to the buffer</param>
        /// <param name="length">the size of the value (use sizeof(type) to get this value)</param>
        /// <param name="endianness">Endianness in which to write the value</param>
        /// <returns></returns>
        private static int WriteBuffer(int index, byte[] buffer, ulong val, ushort length, Endianness endianness)
        {
            //don't modify parameters directly
            ulong writeVal = val;
            int retIndex = index;

            if (index < 0)
            {
                throw new ArgumentException("ReadBuffer:: index cannot be below 0");
            }

            if (buffer == null)
            {
                throw new ArgumentException("ReadBuffer:: buffer cannot be null");
            }

            if (index + length > buffer.Length)
            {
                throw new ArgumentException("ReadBuffer:: Requested read length is greater than the length of the buffer");
            }

            byte curVal;
            try
            {
                if (endianness == Endianness.LittleEndian)
                {
                    for (int i = index; i < index + length; i++)
                    {
                        curVal = (byte)writeVal;
                        buffer[i] = curVal;
                        writeVal >>= 8;
                    }

                    retIndex += length;
                }
                else
                {
                    for (int i = index + length - 1; i >= index; i--)
                    {
                        curVal = (byte)writeVal;
                        buffer[i] = curVal;
                        writeVal >>= 8;
                    }

                    retIndex += length;
                }
            }
            catch (Exception ex)
            {
                // write failed, don't update index
                Console.WriteLine($"Error while trying to write to buffer {ex.Message}");
                retIndex = index;
            }

            return retIndex;
        }

        public static int WriteByte(int index, byte[] buffer, byte val)
        {
            //endianness does not matter for a single byte. Use little endian on WriteBuffer just to reuse code
            int retIndex = WriteBuffer(index, buffer, val, sizeof(byte), Endianness.LittleEndian);
            return retIndex;
        }

        public static int WriteUShort(int index, byte[] buffer, ushort val, Endianness endianness)
        {
            int retIndex = WriteBuffer(index, buffer, val, sizeof(ushort), endianness);
            return retIndex;
        }

        public static int WriteUInt(int index, byte[] buffer, uint val, Endianness endianness)
        {
            int retIndex = WriteBuffer(index, buffer, val, sizeof(uint), endianness);
            return retIndex;
        }

        public static int WriteULong(int index, byte[] buffer, ulong val, Endianness endianness)
        {
            int retIndex = WriteBuffer(index, buffer, val, sizeof(ulong), endianness);
            return retIndex;
        }

        public static byte ReadByte(int index, byte[] buffer)
        {
            //endianness does not matter for a single byte. Use little endian on WriteBuffer just to reuse code
            return (byte)ReadBuffer(index, buffer, sizeof(byte), Endianness.LittleEndian);
        }

        public static ushort ReadUShort(int index, byte[] buffer, Endianness endianness)
        {
            return (ushort)ReadBuffer(index, buffer, sizeof(ushort), endianness);
        }

        public static uint ReadUInt(int index, byte[] buffer, Endianness endianness)
        {
            return (uint)ReadBuffer(index, buffer, sizeof(uint), endianness);
        }

        public static ulong ReadULong(int index, byte[] buffer, Endianness endianness)
        {
            return ReadBuffer(index, buffer, sizeof(ulong), endianness);
        }
    }
}
