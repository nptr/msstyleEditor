using libmsstyle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace libmsstyleTests
{
    [TestClass]
    public class StylePropertyTests
    {
        public StylePropertyTests()
        {
        }

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        Type GetTypeFromTypeId(IDENTIFIER typeId)
        {
            switch (typeId)
            {
                case IDENTIFIER.ENUM: return typeof(Int32);
                case IDENTIFIER.STRING: return typeof(string);
                case IDENTIFIER.INT: return typeof(Int32);
                case IDENTIFIER.BOOLTYPE: return typeof(bool);
                case IDENTIFIER.COLOR: return typeof(Color);
                case IDENTIFIER.MARGINS: return typeof(libmsstyle.Margins);
                case IDENTIFIER.FILENAME: return typeof(Int32);
                case IDENTIFIER.SIZE: return typeof(Int32);
                case IDENTIFIER.POSITION: return typeof(Size);
                case IDENTIFIER.RECTTYPE: return typeof(Margins);
                case IDENTIFIER.FONT: return typeof(Int32);
                case IDENTIFIER.INTLIST: return typeof(List<Int32>);
                case IDENTIFIER.HBITMAP: return null;
                case IDENTIFIER.DISKSTREAM: return typeof(Int32);
                case IDENTIFIER.STREAM: return typeof(Int32);
                case IDENTIFIER.BITMAPREF: return typeof(Int32);
                case IDENTIFIER.FLOAT: return typeof(float);
                case IDENTIFIER.FLOATLIST: return typeof(List<float>);
                case IDENTIFIER.COLORLIST: return typeof(List<Color>);
                case IDENTIFIER.HIGHCONTRASTCOLORTYPE: return typeof(Int32);
                case IDENTIFIER.BITMAPIMAGETYPE: return null;
                case IDENTIFIER.FILENAME_LITE: return typeof(Int32);
                default: return null;
            }
        }


        [TestMethod]
        public void VerifyPropValuesAreCorrectType()
        {
            foreach (var propInfo in VisualStyleProperties.PROPERTY_INFO_MAP)
            {
                // skip types themselves
                if (propInfo.Key < 400)
                    continue;
                // skip undiscovered props
                if (propInfo.Value.TypeId <= 0)
                    continue;

                var prop = new StyleProperty(
                    (IDENTIFIER)propInfo.Key,
                    (IDENTIFIER)propInfo.Value.TypeId);

                Type t = GetTypeFromTypeId((IDENTIFIER)propInfo.Value.TypeId);
                Assert.AreEqual(t, prop.GetValue()?.GetType(), 
                    $"Name Id: {propInfo.Key}, Type Id: {propInfo.Value.TypeId}");
            }
        }

        [TestMethod]
        public void VerifyPropValuesAreCorrectType_EdgeCases()
        {
            // Some props also uses their type ID as name ID.
            // Those must still work, but I didn't want to include this
            // check in "VerifyPropValuesAreCorrectType()".
            var prop = new StyleProperty(
                IDENTIFIER.COLOR, 
                IDENTIFIER.COLOR);
            Assert.AreEqual(typeof(Color), prop.GetValue().GetType());

            prop = new StyleProperty(
                IDENTIFIER.FONT,
                IDENTIFIER.FONT);
            Assert.AreEqual(typeof(Int32), prop.GetValue().GetType());
        }
    }
}
