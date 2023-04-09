using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace libmsstyleTests
{
    [TestClass]
    public class StringTableTests
    {
        [TestMethod]
        public void CanFilterFontEntries()
        {
            var table = new Dictionary<int, string>()
            {
                { 105, "Microsoft Corporation" },
                { 106, "Copyright Microsoft Corporation, 2003.  All rights reserved." },
                { 112, "Aero Color Scheme" },
                { 113, "0, 0, 406, 440" },
                { 495, "Segoe UI, 9, Quality:Cleartype" },
                { 507, "Segoe UI, 9, Italic, Quality:ClearType" },
                { 1164, "Tahoma, 8" },
                { 1165, "Tahoma, 8, Bold" },
            };

            var fonts = libmsstyle.StringTable.FilterFonts(table);

            Assert.AreEqual(4, fonts.Count);
            Assert.IsTrue(fonts.ContainsKey(495));
            Assert.IsTrue(fonts.ContainsKey(507));
            Assert.IsTrue(fonts.ContainsKey(1164));
            Assert.IsTrue(fonts.ContainsKey(1165));
        }
    }
}
