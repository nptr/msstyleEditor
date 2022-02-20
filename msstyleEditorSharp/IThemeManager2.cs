using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace msstyleEditor
{
    public class ThemeManagerConstants
    {
        internal const string IID = "c1e8c83e-845d-4d95-81db-e283fdffc000"; // Win 10 only?
        internal const string CLSID = "9324da94-50ec-4a14-a770-e90ca03e7c8f";
    }

    [Guid("c1e8c83e-845d-4d95-81db-e283fdffc000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IThemeManager2
    {
        int Init(int themeFlags);
        int InitAsync(int hwnd, int unk);
        int Refresh();
        int RefreshAsync(int hwnd, int unk);
        int RefreshComplete();
        int GetThemeCount(IntPtr cnt);
        // TODO: complete interface definition
    }
}
