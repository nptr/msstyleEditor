namespace System.Windows.Forms
{
    internal class RibbonWrappedDropDown
        : ToolStripDropDown
    {
        public RibbonWrappedDropDown()
        {
            DoubleBuffered = false;
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.Selectable, false);
            SetStyle(ControlStyles.ResizeRedraw, false);
            AutoSize = false;
        }
    }
}
