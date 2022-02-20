using libmsstyle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace libmsstyleTests
{
    [TestClass]
    public class PropertyStreamTests
    {
        [TestMethod]
        public void CanWriteEnum()
        {
            var prop = new StyleProperty(IDENTIFIER.BGTYPE, IDENTIFIER.ENUM);
            prop.Header.classID = 40;
            prop.Header.partID = 0;
            prop.Header.stateID = 0;
            prop.SetValue(2);

            var expected = new byte[] {
                0xA1, 0x0F, 0x00, 0x00, 0xC8, 0x00, 0x00, 0x00,
                0x28, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00,
                0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            using(var ms = new MemoryStream())
            using(var bw = new BinaryWriter(ms))
            {
                PropertyStream.WriteProperty(bw, prop);
                CollectionAssert.AreEqual(expected, ms.ToArray());
            }
        }

        [TestMethod]
        public void CanWriteString()
        {
            var prop = new StyleProperty(IDENTIFIER.CSSNAME, IDENTIFIER.STRING);
            prop.Header.classID = 5;
            prop.Header.partID = 0;
            prop.Header.stateID = 0;
            prop.SetValue("cpwebvw.css");

            var expected = new byte[] {
                0x79, 0x05, 0x00, 0x00, 0xC9, 0x00, 0x00, 0x00, 
                0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x18, 0x00, 0x00, 0x00, 
                0x63, 0x00, 0x70, 0x00, 0x77, 0x00, 0x65, 0x00, 
                0x62, 0x00, 0x76, 0x00, 0x77, 0x00, 0x2E, 0x00, 
                0x63, 0x00, 0x73, 0x00, 0x73, 0x00, 0x00, 0x00 };

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                PropertyStream.WriteProperty(bw, prop);
                CollectionAssert.AreEqual(expected, ms.ToArray());
            }
        }

        [TestMethod]
        public void CanWriteInt()
        {
            var prop = new StyleProperty(IDENTIFIER.MINCOLORDEPTH, IDENTIFIER.INT);
            prop.Header.classID = 5;
            prop.Header.partID = 0;
            prop.Header.stateID = 0;
            prop.SetValue(15);

            var expected = new byte[] {
                0x15, 0x05, 0x00, 0x00, 0xCA, 0x00, 0x00, 0x00, 
                0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 
                0x0F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                PropertyStream.WriteProperty(bw, prop);
                CollectionAssert.AreEqual(expected, ms.ToArray());
            }
        }

        [TestMethod]
        public void CanWriteBool()
        {
            var prop = new StyleProperty(IDENTIFIER.FLATMENUS, IDENTIFIER.BOOLTYPE);
            prop.Header.classID = 5;
            prop.Header.partID = 0;
            prop.Header.stateID = 0;
            prop.SetValue(true);

            var expected = new byte[] {
                0xE9, 0x03, 0x00, 0x00, 0xCB, 0x00, 0x00, 0x00, 
                0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                PropertyStream.WriteProperty(bw, prop);
                CollectionAssert.AreEqual(expected, ms.ToArray());
            }
        }

        [TestMethod]
        public void CanWriteColor()
        {
            var prop = new StyleProperty(IDENTIFIER.TEXTCOLOR, IDENTIFIER.COLOR);
            prop.Header.classID = 9;
            prop.Header.partID = 7;
            prop.Header.stateID = 2;
            prop.SetValue(Color.FromArgb(51, 153, 255));
            var expected = new byte[] {
                0xDB, 0x0E, 0x00, 0x00, 0xCC, 0x00, 0x00, 0x00, 
                0x09, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00,
                0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 
                0x33, 0x99, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00 };

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                PropertyStream.WriteProperty(bw, prop);
                CollectionAssert.AreEqual(expected, ms.ToArray());
            }
        }

        [TestMethod]
        public void CanWriteMargin()
        {
            var prop = new StyleProperty(IDENTIFIER.SIZINGMARGINS, IDENTIFIER.MARGINS);
            prop.Header.classID = 38;
            prop.Header.partID = 1;
            prop.Header.stateID = 0;
            prop.SetValue(new Margins(6, 6, 5, 5));
            var expected = new byte[] {
                0x11, 0x0E, 0x00, 0x00, 0xCD, 0x00, 0x00, 0x00, 
                0x26, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00,
                0x06, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 
                0x05, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00 };

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                PropertyStream.WriteProperty(bw, prop);
                CollectionAssert.AreEqual(expected, ms.ToArray());
            }
        }

        [TestMethod]
        public void CanWriteFileName()
        {
            var prop = new StyleProperty(IDENTIFIER.IMAGEFILE1, IDENTIFIER.FILENAME);
            prop.Header.classID = 38;
            prop.Header.partID = 2;
            prop.Header.stateID = 0;
            prop.SetValue(517);
            var expected = new byte[] {
                0xBA, 0x0B, 0x00, 0x00, 0xCE, 0x00, 0x00, 0x00, 
                0x26, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x05, 0x02, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00 };

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                PropertyStream.WriteProperty(bw, prop);
                CollectionAssert.AreEqual(expected, ms.ToArray());
            }
        }

        [TestMethod]
        public void CanWritePosition()
        {
            var prop = new StyleProperty(IDENTIFIER.MINSIZE, IDENTIFIER.POSITION);
            prop.Header.classID = 38;
            prop.Header.partID = 1;
            prop.Header.stateID = 0;
            prop.SetValue(new Size(10, 5));
            var expected = new byte[] {
                0x4B, 0x0D, 0x00, 0x00, 0xD0, 0x00, 0x00, 0x00, 
                0x26, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 
                0x0A, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00 };

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                PropertyStream.WriteProperty(bw, prop);
                CollectionAssert.AreEqual(expected, ms.ToArray());
            }
        }

        [TestMethod]
        public void CanWriteIntList()
        {
            var prop = new StyleProperty(IDENTIFIER.TRANSITIONDURATIONS, IDENTIFIER.INTLIST);
            prop.Header.classID = 42;
            prop.Header.partID = 6;
            prop.Header.stateID = 0;
            prop.SetValue(new List<Int32>() { 0x4, 0xAA });

            var expected = new byte[] {
                0x70, 0x17, 0x00, 0x00, 0xD3, 0x00, 0x00, 0x00, 
                0x2A, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 
                0x02, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 
                0xAA, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                PropertyStream.WriteProperty(bw, prop);
                CollectionAssert.AreEqual(expected, ms.ToArray());
            }
        }

        [TestMethod]
        public void CanReadEnum()
        {
            var data = new byte[] {
                0xA1, 0x0F, 0x00, 0x00, 0xC8, 0x00, 0x00, 0x00,
                0x28, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00,
                0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            int start = 0;
            var prop = PropertyStream.ReadNextProperty(data, ref start);
            Assert.IsNotNull(prop);
            Assert.AreEqual((int)IDENTIFIER.BGTYPE, prop.Header.nameID);
            Assert.AreEqual((int)IDENTIFIER.ENUM, prop.Header.typeID);
            Assert.AreEqual(40, prop.Header.classID);
            Assert.AreEqual(0, prop.Header.partID);
            Assert.AreEqual(0, prop.Header.stateID);
            Assert.AreEqual(2, prop.GetValue());
        }

        [TestMethod]
        public void CanReadString()
        {
            var data = new byte[] {
                0x79, 0x05, 0x00, 0x00, 0xC9, 0x00, 0x00, 0x00,
                0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x18, 0x00, 0x00, 0x00,
                0x63, 0x00, 0x70, 0x00, 0x77, 0x00, 0x65, 0x00,
                0x62, 0x00, 0x76, 0x00, 0x77, 0x00, 0x2E, 0x00,
                0x63, 0x00, 0x73, 0x00, 0x73, 0x00, 0x00, 0x00 };

            int start = 0;
            var prop = PropertyStream.ReadNextProperty(data, ref start);
            Assert.IsNotNull(prop);
            Assert.AreEqual((int)IDENTIFIER.CSSNAME, prop.Header.nameID);
            Assert.AreEqual((int)IDENTIFIER.STRING, prop.Header.typeID);
            Assert.AreEqual(5, prop.Header.classID);
            Assert.AreEqual(0, prop.Header.partID);
            Assert.AreEqual(0, prop.Header.stateID);
            Assert.AreEqual("cpwebvw.css", prop.GetValue());
        }

        [TestMethod]
        public void CanReadInt()
        {
            var data = new byte[] {
                0x15, 0x05, 0x00, 0x00, 0xCA, 0x00, 0x00, 0x00,
                0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00,
                0x0F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            int start = 0;
            var prop = PropertyStream.ReadNextProperty(data, ref start);
            Assert.IsNotNull(prop);
            Assert.AreEqual((int)IDENTIFIER.MINCOLORDEPTH, prop.Header.nameID);
            Assert.AreEqual((int)IDENTIFIER.INT, prop.Header.typeID);
            Assert.AreEqual(5, prop.Header.classID);
            Assert.AreEqual(0, prop.Header.partID);
            Assert.AreEqual(0, prop.Header.stateID);
            Assert.AreEqual(15, prop.GetValue());
        }

        [TestMethod]
        public void CanReadBool()
        {
            var data = new byte[] {
                0xE9, 0x03, 0x00, 0x00, 0xCB, 0x00, 0x00, 0x00,
                0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            int start = 0;
            var prop = PropertyStream.ReadNextProperty(data, ref start);
            Assert.IsNotNull(prop);
            Assert.AreEqual((int)IDENTIFIER.FLATMENUS, prop.Header.nameID);
            Assert.AreEqual((int)IDENTIFIER.BOOLTYPE, prop.Header.typeID);
            Assert.AreEqual(5, prop.Header.classID);
            Assert.AreEqual(0, prop.Header.partID);
            Assert.AreEqual(0, prop.Header.stateID);
            Assert.AreEqual(true, prop.GetValue());
        }

        [TestMethod]
        public void CanReadColor()
        {
            var data = new byte[] {
                0xDB, 0x0E, 0x00, 0x00, 0xCC, 0x00, 0x00, 0x00,
                0x09, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00,
                0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00,
                0x33, 0x99, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00 };

            int start = 0;
            var prop = PropertyStream.ReadNextProperty(data, ref start);
            Assert.IsNotNull(prop);
            Assert.AreEqual((int)IDENTIFIER.TEXTCOLOR, prop.Header.nameID);
            Assert.AreEqual((int)IDENTIFIER.COLOR, prop.Header.typeID);
            Assert.AreEqual(9, prop.Header.classID);
            Assert.AreEqual(7, prop.Header.partID);
            Assert.AreEqual(2, prop.Header.stateID);
            Assert.AreEqual(Color.FromArgb(51, 153, 255), prop.GetValue());
        }

        [TestMethod]
        public void CanReadMargin()
        {
            var data = new byte[] {
                0x11, 0x0E, 0x00, 0x00, 0xCD, 0x00, 0x00, 0x00,
                0x26, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00,
                0x06, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00,
                0x05, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00 };

            int start = 0;
            var prop = PropertyStream.ReadNextProperty(data, ref start);
            Assert.IsNotNull(prop);
            Assert.AreEqual((int)IDENTIFIER.SIZINGMARGINS, prop.Header.nameID);
            Assert.AreEqual((int)IDENTIFIER.MARGINS, prop.Header.typeID);
            Assert.AreEqual(38, prop.Header.classID);
            Assert.AreEqual(1, prop.Header.partID);
            Assert.AreEqual(0, prop.Header.stateID);
            Assert.AreEqual(new Margins(6, 6, 5, 5), prop.GetValue());
        }

        [TestMethod]
        public void CanReadFilename()
        {
            var data = new byte[] {
                0xBA, 0x0B, 0x00, 0x00, 0xCE, 0x00, 0x00, 0x00,
                0x26, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x05, 0x02, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00 };

            int start = 0;
            var prop = PropertyStream.ReadNextProperty(data, ref start);
            Assert.IsNotNull(prop);
            Assert.AreEqual((int)IDENTIFIER.IMAGEFILE1, prop.Header.nameID);
            Assert.AreEqual((int)IDENTIFIER.FILENAME, prop.Header.typeID);
            Assert.AreEqual(38, prop.Header.classID);
            Assert.AreEqual(2, prop.Header.partID);
            Assert.AreEqual(0, prop.Header.stateID);
            Assert.AreEqual(517, prop.GetValue());
        }

        [TestMethod]
        public void CanReadPosition()
        {
            var data = new byte[] {
                0x4B, 0x0D, 0x00, 0x00, 0xD0, 0x00, 0x00, 0x00,
                0x26, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00,
                0x0A, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00 };

            int start = 0;
            var prop = PropertyStream.ReadNextProperty(data, ref start);
            Assert.IsNotNull(prop);
            Assert.AreEqual((int)IDENTIFIER.MINSIZE, prop.Header.nameID);
            Assert.AreEqual((int)IDENTIFIER.POSITION, prop.Header.typeID);
            Assert.AreEqual(38, prop.Header.classID);
            Assert.AreEqual(1, prop.Header.partID);
            Assert.AreEqual(0, prop.Header.stateID);
            Assert.AreEqual(new Size(10, 5), prop.GetValue());
        }
    }
}
