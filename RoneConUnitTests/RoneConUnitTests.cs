using Microsoft.VisualStudio.TestTools.UnitTesting;
using RconLib;
using System;

namespace RoneConUnitTests
{
    [TestClass]
    public class RoneConUnitTests
    {
        [TestMethod]
        public void TestWriteVals_Success()
        {
            byte[] buff = new byte[19];
            int index = 0;
            index = BytewiseIO.WriteUInt(index, buff, 15, Endianness.LittleEndian);
            Assert.AreEqual(index, 4);

            uint val = BytewiseIO.ReadUInt(index - (sizeof(uint)), buff, Endianness.LittleEndian);
            Assert.AreEqual(val, (uint)15);

            index = BytewiseIO.WriteUInt(index, buff, 0xA1B2C3D4, Endianness.BigEndian);
            Assert.AreEqual(index, 8);

            val = BytewiseIO.ReadUInt(index - (sizeof(uint)), buff, Endianness.BigEndian);
            Assert.AreEqual(val, (uint)0xA1B2C3D4);

            index = BytewiseIO.WriteULong(index, buff, 0xA1B2C3D4E5F6A1B2, Endianness.LittleEndian);
            Assert.AreEqual(index, 16);

            ulong lval = BytewiseIO.ReadULong(index - sizeof(ulong), buff, Endianness.LittleEndian);
            Assert.AreEqual(lval, (ulong)0xA1B2C3D4E5F6A1B2);

            index = BytewiseIO.WriteByte(index, buff, 0xAF);
            Assert.AreEqual(index, 17);

            byte bval = BytewiseIO.ReadByte(index - sizeof(byte), buff);
            Assert.AreEqual(bval, 0xAF);

            index = BytewiseIO.WriteUShort(index, buff, 0xFFAC, Endianness.BigEndian);
            Assert.AreEqual(index, 19);

            ushort sval = BytewiseIO.ReadUShort(index - sizeof(ushort), buff, Endianness.BigEndian);
            Assert.AreEqual(sval, 0xFFAC);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWriteVals_BufferTooSmall()
        {
            byte[] buff = new byte[3];
            int index = 0;
            _ = BytewiseIO.WriteUInt(index, buff, 15, Endianness.LittleEndian);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWriteVals_BadIndex()
        {
            byte[] buff = new byte[4];
            int index = -1;
            _ = BytewiseIO.WriteUShort(index, buff, 15, Endianness.LittleEndian);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWriteVals_NullBuffer()
        {
            byte[] buff = null;
            int index = 0;
            _ = BytewiseIO.WriteULong(index, buff, 15, Endianness.LittleEndian);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestReadVals_NullBuffer()
        {
            byte[] buff = null;
            int index = 0;
            _ = BytewiseIO.ReadByte(index, buff);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestReadVals_BufferTooSmall()
        {
            byte[] buff = new byte[3];
            int index = 0;
            _ = BytewiseIO.ReadULong(index, buff, Endianness.BigEndian);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestReadVals_BadIndex()
        {
            byte[] buff = new byte[4];
            int index = -1;
            _ = BytewiseIO.ReadULong(index, buff, Endianness.LittleEndian);
        }
    }
}
