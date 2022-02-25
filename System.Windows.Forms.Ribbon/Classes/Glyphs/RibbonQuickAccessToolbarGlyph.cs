
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms
{
    public class RibbonQuickAccessToolbarGlyph
        : Glyph

    {
        private readonly BehaviorService _behaviorService;
        private readonly Ribbon _ribbon;
        private RibbonDesigner _componentDesigner;

        public RibbonQuickAccessToolbarGlyph(BehaviorService behaviorService, RibbonDesigner designer, Ribbon ribbon)
            : base(new RibbonQuickAccessGlyphBehavior(designer, ribbon))
        {
            _behaviorService = behaviorService;
            _componentDesigner = designer;
            _ribbon = ribbon;
        }

        public override Rectangle Bounds
        {
            get
            {
                Point edge = _behaviorService.ControlToAdornerWindow(_ribbon);
                if (!_ribbon.CaptionBarVisible || !_ribbon.QuickAccessToolbar.Visible)
                {
                    return Rectangle.Empty;
                }

                if (_ribbon.RightToLeft == RightToLeft.No)
                {
                    return new Rectangle(
                        edge.X + _ribbon.QuickAccessToolbar.Bounds.Right + _ribbon.QuickAccessToolbar.Bounds.Height / 2 + 4 + _ribbon.QuickAccessToolbar.DropDownButton.Bounds.Width,
                        edge.Y + _ribbon.QuickAccessToolbar.Bounds.Top, _ribbon.QuickAccessToolbar.Bounds.Height, _ribbon.QuickAccessToolbar.Bounds.Height);
                }

                return new Rectangle(
                    _ribbon.QuickAccessToolbar.Bounds.Left - _ribbon.QuickAccessToolbar.Bounds.Height / 2 - 4 - _ribbon.QuickAccessToolbar.DropDownButton.Bounds.Width,
                    edge.Y + _ribbon.QuickAccessToolbar.Bounds.Top, _ribbon.QuickAccessToolbar.Bounds.Height, _ribbon.QuickAccessToolbar.Bounds.Height);
            }
        }

        public override Cursor GetHitTest(Point p)
        {
            if (Bounds.Contains(p))
            {
                return Cursors.Hand;
            }

            return null;
        }


        public override void Paint(PaintEventArgs pe)
        {
            if (_ribbon.CaptionBarVisible && _ribbon.QuickAccessToolbar.Visible)
            {
                SmoothingMode smbuff = pe.Graphics.SmoothingMode;
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush b = new SolidBrush(Color.FromArgb(50, Color.Blue)))
                {
                    pe.Graphics.FillEllipse(b, Bounds);
                }
                StringFormat sf = StringFormatFactory.Center();
                pe.Graphics.DrawString("+", SystemFonts.DefaultFont, Brushes.White, Bounds, sf);
                pe.Graphics.SmoothingMode = smbuff;
            }
        }
    }

    public class RibbonQuickAccessGlyphBehavior
        : Behavior
    {
        private readonly Ribbon _ribbon;
        private readonly RibbonDesigner _designer;

        public RibbonQuickAccessGlyphBehavior(RibbonDesigner designer, Ribbon ribbon)
        {
            _designer = designer;
            _ribbon = ribbon;
        }



        public override bool OnMouseUp(Glyph g, MouseButtons button)
        {
            _designer.CreateItem(_ribbon, _ribbon.QuickAccessToolbar.Items, typeof(RibbonButton));
            return base.OnMouseUp(g, button);
        }
    }
}
