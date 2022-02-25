using System.Drawing;

namespace System.Windows.Forms
{
    public delegate void RibbonElementPopupEventHandler(object sender, RibbonElementPopupEventArgs e);

    public class RibbonElementPopupEventArgs : PopupEventArgs
    {
        private readonly PopupEventArgs _args;

        public RibbonElementPopupEventArgs(IRibbonElement item)
            : base(item.Owner, item.Owner, false, new Size(-1, -1))
        {
            AssociatedRibbonElement = item;
        }

        public RibbonElementPopupEventArgs(IRibbonElement item, PopupEventArgs args)
            : base(args.AssociatedWindow, args.AssociatedControl, args.IsBalloon, args.ToolTipSize)
        {
            AssociatedRibbonElement = item;
            _args = args;
        }

        public IRibbonElement AssociatedRibbonElement { get; }


        public new bool Cancel
        {
            get => _args == null ? base.Cancel : _args.Cancel;
            set
            {
                if (_args != null)
                    _args.Cancel = value;
                base.Cancel = value;
            }
        }
    }

}
