using libmsstyle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace libmsstyleTests
{
    [TestClass]
    public class BinaryComparison
    {
        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                File.Delete("tmp.msstyles");
            }
            catch (Exception) { }
        }


        [DllImport("kernel32.dll", EntryPoint = "LoadLibraryExW", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, uint dwFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeLibrary(IntPtr hModule);


        public static byte[] GetResource(IntPtr moduleHandle, string type, string name)
        {
            IntPtr resHandle = Win32Api.FindResource(moduleHandle, name, type);
            if (resHandle == IntPtr.Zero)
                return null;

            IntPtr dataHandle = Win32Api.LoadResource(moduleHandle, resHandle);
            IntPtr data = Win32Api.LockResource(dataHandle);
            uint size = Win32Api.SizeofResource(moduleHandle, resHandle);

            byte[] managedArray = new byte[size];
            Marshal.Copy(data, managedArray, 0, (int)size);
            return managedArray;
        }


        // NOTE: We can not reasonably binary compare AMAP resources < Win 10. Read the note from
        // the original research (https://winclassic.net/thread/1825/research-dwm-animations-uxtheme-amap):
        //
        // "Following the header will be another 4 bytes of padding.It seems that Microsoft didn't
        // even care to zero this out prior to Windows 10, since the AMAP from Windows 8.1 and prior
        // show what seem to be random memory contents from their build machines in the gaps (that is,
        // snippets of what appears to be English text)."
        [DataTestMethod]
        [DataRow(@"..\..\..\styles\w10_2004_aero\aero.msstyles")]
        [DataRow(@"..\..\..\styles\w11_pre_aero.msstyles")]
        public void AMAP(string file)
        {
            using (var original = new VisualStyle())
            {
                original.Load(file);
                original.Save("tmp.msstyles");
            }

            IntPtr origModuleHandle = LoadLibraryEx(file, IntPtr.Zero, 0x00000040); // LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE
            byte[] originalAMAP = GetResource(origModuleHandle, "AMAP", "AMAP");
            FreeLibrary(origModuleHandle);

            IntPtr newModuleHandle = LoadLibraryEx("tmp.msstyles", IntPtr.Zero, 0x00000040); // LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE
            byte[] newAMAP = GetResource(newModuleHandle, "AMAP", "AMAP");
            FreeLibrary(newModuleHandle);

            CollectionAssert.AreEqual(originalAMAP, newAMAP);
        }
    }
}
