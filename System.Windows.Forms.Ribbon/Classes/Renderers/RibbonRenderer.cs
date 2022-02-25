// *********************************
// Message from Original Author:
//
// 2008 Jose Menendez Poo
// Please give me credit if you use this code. It's all I ask.
// Contact me for more info: menendezpoo@gmail.com
// *********************************
//
// Original project from http://ribbon.codeplex.com/
// Continue to support and maintain by http://officeribbon.codeplex.com/


using System.Drawing;
using System.Drawing.Imaging;

namespace System.Windows.Forms
{
    /// <summary>
    /// Provides render functionallity for the Ribbon control
    /// </summary>
    /// <remarks></remarks>
    public class RibbonRenderer
    {
        #region Static

        private static ColorMatrix _disabledImageColorMatrix;

        /// <summary>
        /// Gets the disabled image color matrix
        /// </summary>
        private static ColorMatrix DisabledImageColorMatrix
        {
            get
            {
                if (_disabledImageColorMatrix == null)
                {
                    float[][] numArray = new float[5][];
                    numArray[0] = new[] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f };
                    numArray[1] = new[] { 0.2577f, 0.2577f, 0.2577f, 0f, 0f };
                    numArray[2] = new[] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f };
                    float[] numArray3 = new float[5];
                    numArray3[3] = 1f;
                    numArray[3] = numArray3;
                    numArray[4] = new[] { 0.38f, 0.38f, 0.38f, 0f, 1f };
                    float[][] numArray2 = new float[5][];
                    float[] numArray4 = new float[5];
                    numArray4[0] = 1f;
                    numArray2[0] = numArray4;
                    float[] numArray5 = new float[5];
                    numArray5[1] = 1f;
                    numArray2[1] = numArray5;
                    float[] numArray6 = new float[5];
                    numArray6[2] = 1f;
                    numArray2[2] = numArray6;
                    float[] numArray7 = new float[5];
                    numArray7[3] = 0.7f;
                    numArray2[3] = numArray7;
                    numArray2[4] = new float[5];
                    _disabledImageColorMatrix = MultiplyColorMatrix(numArray2, numArray);
                }
                return _disabledImageColorMatrix;
            }
        }

        /// <summary>
        /// Multiplies the color matrix (Extracted from .NET Framework
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        internal static ColorMatrix MultiplyColorMatrix(float[][] matrix1, float[][] matrix2)
        {
            int num = 5;
            float[][] newColorMatrix = new float[num][];
            for (int i = 0; i < num; i++)
            {
                newColorMatrix[i] = new float[num];
            }
            float[] numArray2 = new float[num];
            for (int j = 0; j < num; j++)
            {
                for (int k = 0; k < num; k++)
                {
                    numArray2[k] = matrix1[k][j];
                }
                for (int m = 0; m < num; m++)
                {
                    float[] numArray3 = matrix2[m];
                    float num6 = 0f;
                    for (int n = 0; n < num; n++)
                    {
                        num6 += numArray3[n] * numArray2[n];
                    }
                    newColorMatrix[m][j] = num6;
                }
            }
            return new ColorMatrix(newColorMatrix);
        }

        /// <summary>
        /// Creates the disabled image for the specified Image
        /// </summary>
        /// <param name="normalImage"></param>
        /// <returns></returns>
        public static Image CreateDisabledImage(Image normalImage)
        {
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.ClearColorKey();
            imageAttr.SetColorMatrix(DisabledImageColorMatrix);
            Size size = normalImage.Size;
            Bitmap image = new Bitmap(size.Width, size.Height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.DrawImage(normalImage, new Rectangle(0, 0, size.Width, size.Height), 0, 0, size.Width, size.Height, GraphicsUnit.Pixel, imageAttr);
            graphics.Dispose();
            return image;
        }

        #endregion

        /// <summary>
        /// Renders the Ribbon Orb's DropDown background
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnRenderOrbDropDownBackground(RibbonOrbDropDownEventArgs e)
        {

        }

        /// <summary>
        /// Renders the Ribbon's caption bar
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnRenderRibbonCaptionBar(RibbonRenderEventArgs e)
        {
        }

        /// <summary>
        /// Renders the orb of the ribbon
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnRenderRibbonOrb(RibbonRenderEventArgs e)
        {
        }

        /// <summary>
        /// Renders the background of the QuickAccess toolbar
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnRenderRibbonQuickAccessToolbarBackground(RibbonRenderEventArgs e)
        {
        }

        /// <summary>
        /// Renders the Ribbon's background
        /// </summary>
        public virtual void OnRenderRibbonBackground(RibbonRenderEventArgs e)
        {

        }

        /// <summary>
        /// Renders a RibbonTab
        /// </summary>
        /// <param name="e">Event data and paint tools</param>
        public virtual void OnRenderRibbonTab(RibbonTabRenderEventArgs e)
        {

        }

        /// <summary>
        /// Renders a RibbonContext
        /// </summary>
        /// <param name="e">Event data and paint tools</param>
        public virtual void OnRenderRibbonContext(RibbonContextRenderEventArgs e)
        {

        }

        /// <summary>
        /// Renders a RibbonItem
        /// </summary>
        public virtual void OnRenderRibbonItem(RibbonItemRenderEventArgs e)
        {

        }

        /// <summary>
        /// Renders the background of the content of the specified tab
        /// </summary>
        /// <param name="e">Event data and paint tools</param>
        public virtual void OnRenderRibbonTabContentBackground(RibbonTabRenderEventArgs e)
        {

        }

        /// <summary>
        /// Renders the background of the specified ribbon panel
        /// </summary>
        /// <param name="e">Event data and paint tools</param>
        public virtual void OnRenderRibbonPanelBackground(RibbonPanelRenderEventArgs e)
        {

        }

        /// <summary>
        /// Renders the text of the tab specified on the event
        /// </summary>
        /// <param name="e">Event data and paint tools</param>
        public virtual void OnRenderRibbonTabText(RibbonTabRenderEventArgs e)
        {

        }

        /// <summary>
        /// Renders the text of the context specified on the event
        /// </summary>
        /// <param name="e">Event data and paint tools</param>
        public virtual void OnRenderRibbonContextText(RibbonContextRenderEventArgs e)
        {

        }

        /// <summary>
        /// Renders the text of the item specified on the event
        /// </summary>
        /// <param name="e">Event data and paint tools</param>
        public virtual void OnRenderRibbonItemText(RibbonTextEventArgs e)
        {

        }

        /// <summary>
        /// Renders the border of the item, after the text and image of the item
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnRenderRibbonItemBorder(RibbonItemRenderEventArgs e)
        {

        }

        /// <summary>
        /// Renders the image of the item specified on the event
        /// </summary>
        public virtual void OnRenderRibbonItemImage(RibbonItemBoundsEventArgs e)
        {

        }

        /// <summary>
        /// Renders the text of the specified panel
        /// </summary>
        /// <param name="e">Event data and paint tools</param>
        public virtual void OnRenderRibbonPanelText(RibbonPanelRenderEventArgs e)
        {

        }

        /// <summary>
        /// Renders the background of a dropdown
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnRenderDropDownBackground(RibbonCanvasEventArgs e)
        {
        }

        /// <summary>
        /// Renders the image separator of a dropdown
        /// </summary>
        /// <param name="item"></param>
        /// <param name="e"></param>
        public virtual void OnRenderDropDownDropDownImageSeparator(RibbonItem item, RibbonCanvasEventArgs e)
        {
        }

        /// <summary>
        /// Renders the background of a panel background
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnRenderPanelPopupBackground(RibbonCanvasEventArgs e)
        {

        }

        /// <summary>
        /// Call to draw the scroll buttons on the tab
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnRenderTabScrollButtons(RibbonTabRenderEventArgs e)
        {
        }

        /// <summary>
        /// Call to draw the scrollbar on the Control
        /// </summary>
        /// <param name="g"></param>
        /// <param name="item"></param>
        /// <param name="ribbon"></param>
        public virtual void OnRenderScrollbar(Graphics g, Control item, Ribbon ribbon)
        {
        }

        /// <summary>
        /// Call to draw the Tooltip
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnRenderToolTipBackground(RibbonToolTipRenderEventArgs e)
        {
        }

        /// <summary>
        /// Renders the text of the Tooltip specified on the event
        /// </summary>
        /// <param name="e">Event data and paint tools</param>
        public virtual void OnRenderToolTipText(RibbonToolTipRenderEventArgs e)
        {
        }

        public virtual void OnRenderToolTipImage(RibbonToolTipRenderEventArgs e)
        {
        }
    }
}
