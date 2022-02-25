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

using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms.RibbonHelpers;
using System.Xml;

namespace System.Windows.Forms
{
    public class RibbonProfesionalRendererColorTable
    {
        #region Theme Information

        public string ThemeName { get; set; }

        public string ThemeAuthor { get; set; }

        public string ThemeAuthorEmail { get; set; }

        public string ThemeAuthorWebsite { get; set; }

        public string ThemeDateCreated { get; set; }

        #endregion

        #region Pendent for black

        public Color FormBorder = FromHexStr("#3B5A82");

        public Color OrbDropDownDarkBorder = Color.FromArgb(0x9b, 0xaf, 0xca);
        public Color OrbDropDownLightBorder = Color.FromArgb(0xff, 0xff, 0xff);
        public Color OrbDropDownBack = Color.FromArgb(0xbf, 0xd3, 0xeb);
        public Color OrbDropDownNorthA = Color.FromArgb(0xd7, 0xe5, 0xf7);
        public Color OrbDropDownNorthB = Color.FromArgb(0xd4, 0xe1, 0xf3);
        public Color OrbDropDownNorthC = Color.FromArgb(0xc6, 0xd8, 0xee);
        public Color OrbDropDownNorthD = Color.FromArgb(0xb7, 0xca, 0xe6);
        public Color OrbDropDownSouthC = Color.FromArgb(0xb0, 0xc9, 0xea);
        public Color OrbDropDownSouthD = Color.FromArgb(0xcf, 0xe0, 0xf5);
        public Color OrbDropDownContentbg = Color.FromArgb(0xE9, 0xEA, 0xEE);
        public Color OrbDropDownContentbglight = Color.FromArgb(0xFA, 0xFA, 0xFA);
        public Color OrbDropDownSeparatorlight = Color.FromArgb(0xF5, 0xF5, 0xF5);
        public Color OrbDropDownSeparatordark = Color.FromArgb(0xC5, 0xC5, 0xC5);

        /// <summary>
        /// Caption bar is made of 4 rectangles height of each is indicated below
        /// </summary>
        public Color Caption1 = FromHexStr("#E3EBF6"); //4
        public Color Caption2 = FromHexStr("#DAE9FD");
        public Color Caption3 = FromHexStr("#D5E5FA"); //4
        public Color Caption4 = FromHexStr("#D9E7F9");
        public Color Caption5 = FromHexStr("#CADEF7"); //23
        public Color Caption6 = FromHexStr("#E4EFFD");
        public Color Caption7 = FromHexStr("#B0CFF7"); //1

        public Color QuickAccessBorderDark = FromHexStr("#B6CAE2");
        public Color QuickAccessBorderLight = FromHexStr("#F2F6FB");
        public Color QuickAccessUpper = FromHexStr("#E0EBF9");
        public Color QuickAccessLower = FromHexStr("#C9D9EE");

        public Color OrbOptionBorder = FromHexStr("#7793B9");
        public Color OrbOptionBackground = FromHexStr("#E8F1FC");
        public Color OrbOptionShine = FromHexStr("#D2E1F4");

        #endregion

        #region Fields

        public Color Arrow = FromHexStr("#678CBD");
        public Color ArrowLight = Color.FromArgb(200, Color.White);
        public Color ArrowDisabled = FromHexStr("#B7B7B7");
        public Color Text = FromHexStr("#15428B");

        /// <summary>
        /// Orb colors in normal state
        /// </summary>
        public Color OrbBackgroundDark = FromHexStr("#7C8CA4");
        public Color OrbBackgroundMedium = FromHexStr("#99ABC6");
        public Color OrbBackgroundLight = Color.White;
        public Color OrbLight = Color.White;

        /// <summary>
        /// Orb colors in selected state
        /// </summary>
        public Color OrbSelectedBackgroundDark = FromHexStr("#DFAA1A");
        public Color OrbSelectedBackgroundMedium = FromHexStr("#F9D12E");
        public Color OrbSelectedBackgroundLight = FromHexStr("#FFEF36");
        public Color OrbSelectedLight = FromHexStr("#FFF52B");

        /// <summary>
        /// Orb colors in pressed state
        /// </summary>
        public Color OrbPressedBackgroundDark = FromHexStr("#CE8410");
        public Color OrbPressedBackgroundMedium = FromHexStr("#CE8410");
        public Color OrbPressedBackgroundLight = FromHexStr("#F57603");
        public Color OrbPressedLight = FromHexStr("#F08500");
        public Color OrbBorderAero = FromHexStr("#99A1AD");

        /// <summary>
        /// 2010 style Orb colors
        /// </summary>
        public Color OrbButtonText = Color.White;
        public Color OrbButtonBackground = Color.FromArgb(60, 120, 187);
        public Color OrbButtonDark = Color.FromArgb(25, 65, 135);
        public Color OrbButtonMedium = Color.FromArgb(56, 135, 191);
        public Color OrbButtonLight = Color.FromArgb(64, 154, 207);
        public Color OrbButtonPressedCenter = Color.FromArgb(25, 64, 136);
        public Color OrbButtonPressedNorth = Color.FromArgb(71, 132, 194);
        public Color OrbButtonPressedSouth = Color.FromArgb(56, 135, 191);
        public Color OrbButtonGlossyNorth = Color.FromArgb(71, 132, 194);
        public Color OrbButtonGlossySouth = Color.FromArgb(46, 104, 178);
        public Color OrbButtonBorderDark = Color.FromArgb(68, 135, 213);
        public Color OrbButtonBorderLight = Color.FromArgb(160, 204, 243);

        //public Color RibbonBackground = FromHexStr("#BFDBFF");
        public Color RibbonBackground = FromHexStr("#BED0E8");

        public Color TabBorder = FromHexStr("#9FB2C7");
        public Color TabSelectedBorder = FromHexStr("#B1B5BA");
        public Color TabNorth = FromHexStr("#EBF3FE");
        public Color TabSouth = FromHexStr("#E1EAF6");
        public Color TabGlow = FromHexStr("#D1FBFF");
        public Color TabText = FromHexStr("#15428B");
        public Color TabActiveText = FromHexStr("#15428B");
        public Color TabContentNorth = FromHexStr("#C8D9ED");
        public Color TabContentSouth = FromHexStr("#E7F2FF");
        public Color TabSelectedGlow = FromHexStr("#E1D2A5");
        public Color PanelDarkBorder = Color.FromArgb(51, FromHexStr("#15428B"));
        public Color PanelLightBorder = Color.FromArgb(102, Color.White);
        public Color PanelTextBackground = FromHexStr("#C2D9F0");
        public Color PanelTextBackgroundSelected = FromHexStr("#C2D9F0");
        public Color PanelText = FromHexStr("#15428B");
        public Color PanelBackgroundSelected = Color.FromArgb(102, FromHexStr("#E8FFFD"));
        public Color PanelOverflowBackground = FromHexStr("#B9D1F0");
        public Color PanelOverflowBackgroundPressed = FromHexStr("#7699C8");
        public Color PanelOverflowBackgroundSelectedNorth = Color.FromArgb(100, Color.White);
        public Color PanelOverflowBackgroundSelectedSouth = Color.FromArgb(102, FromHexStr("#B8D7FD"));

        public Color ButtonBgOut = FromHexStr("#C1D5F1");
        public Color ButtonBgCenter = FromHexStr("#CFE0F7");
        public Color ButtonBorderOut = FromHexStr("#B9D0ED");
        public Color ButtonBorderIn = FromHexStr("#E3EDFB");
        public Color ButtonGlossyNorth = FromHexStr("#DEEBFE");
        public Color ButtonGlossySouth = FromHexStr("#CBDEF6");

        public Color ButtonDisabledBgOut = FromHexStr("#E0E4E8");
        public Color ButtonDisabledBgCenter = FromHexStr("#E8EBEF");
        public Color ButtonDisabledBorderOut = FromHexStr("#C5D1DE");
        public Color ButtonDisabledBorderIn = FromHexStr("#F1F3F5");
        public Color ButtonDisabledGlossyNorth = FromHexStr("#F0F3F6");
        public Color ButtonDisabledGlossySouth = FromHexStr("#EAEDF1");

        public Color ButtonSelectedBgOut = FromHexStr("#FFD646");
        public Color ButtonSelectedBgCenter = FromHexStr("#FFEAAC");
        public Color ButtonSelectedBorderOut = FromHexStr("#C2A978");
        public Color ButtonSelectedBorderIn = FromHexStr("#FFF2C7");
        public Color ButtonSelectedGlossyNorth = FromHexStr("#FFFDDB");
        public Color ButtonSelectedGlossySouth = FromHexStr("#FFE793");

        public Color ButtonPressedBgOut = FromHexStr("#F88F2C");
        public Color ButtonPressedBgCenter = FromHexStr("#FDF1B0");
        public Color ButtonPressedBorderOut = FromHexStr("#8E8165");
        public Color ButtonPressedBorderIn = FromHexStr("#F9C65A");
        public Color ButtonPressedGlossyNorth = FromHexStr("#FDD5A8");
        public Color ButtonPressedGlossySouth = FromHexStr("#FBB062");

        public Color ButtonCheckedBgOut = FromHexStr("#F9AA45");
        public Color ButtonCheckedBgCenter = FromHexStr("#FDEA9D");
        public Color ButtonCheckedBorderOut = FromHexStr("#8E8165");
        public Color ButtonCheckedBorderIn = FromHexStr("#F9C65A");
        public Color ButtonCheckedGlossyNorth = FromHexStr("#F8DBB7");
        public Color ButtonCheckedGlossySouth = FromHexStr("#FED18E");

        public Color ButtonCheckedSelectedBgOut = FromHexStr("#F9AA45");
        public Color ButtonCheckedSelectedBgCenter = FromHexStr("#FDEA9D");
        public Color ButtonCheckedSelectedBorderOut = FromHexStr("#8E8165");
        public Color ButtonCheckedSelectedBorderIn = FromHexStr("#F9C65A");
        public Color ButtonCheckedSelectedGlossyNorth = FromHexStr("#F8DBB7");
        public Color ButtonCheckedSelectedGlossySouth = FromHexStr("#FED18E");

        public Color ItemGroupOuterBorder = FromHexStr("#9EBAE1");
        public Color ItemGroupInnerBorder = Color.FromArgb(51, Color.White);
        public Color ItemGroupSeparatorLight = Color.FromArgb(64, Color.White);
        public Color ItemGroupSeparatorDark = Color.FromArgb(38, FromHexStr("#9EBAE1"));
        public Color ItemGroupBgNorth = FromHexStr("#CADCF0");
        public Color ItemGroupBgSouth = FromHexStr("#D0E1F7");
        public Color ItemGroupBgGlossy = FromHexStr("#BCD0E9");

        public Color ButtonListBorder = FromHexStr("#B9D0ED");
        public Color ButtonListBg = FromHexStr("#D4E6F8");
        public Color ButtonListBgSelected = FromHexStr("#ECF3FB");

        public Color DropDownBg = FromHexStr("#FAFAFA");
        public Color DropDownImageBg = FromHexStr("#E9EEEE");
        public Color DropDownImageSeparator = FromHexStr("#C5C5C5");
        public Color DropDownBorder = FromHexStr("#868686");
        public Color DropDownGripNorth = FromHexStr("#FFFFFF");
        public Color DropDownGripSouth = FromHexStr("#DFE9EF");
        public Color DropDownGripBorder = FromHexStr("#DDE7EE");
        public Color DropDownGripDark = FromHexStr("#5574A7");
        public Color DropDownGripLight = FromHexStr("#FFFFFF");
        public Color DropDownCheckedButtonGlyphBg = FromHexStr("#FCF1C2");
        public Color DropDownCheckedButtonGlyphBorder = FromHexStr("#F29536");

        public Color SeparatorLight = FromHexStr("#FAFBFD");
        public Color SeparatorDark = FromHexStr("#96B4DA");
        public Color QATSeparatorLight = FromHexStr("#FAFBFD");
        public Color QATSeparatorDark = FromHexStr("#96B4DA");
        public Color SeparatorBg = FromHexStr("#DAE6EE");
        public Color SeparatorLine = FromHexStr("#C5C5C5");

        public Color TextBoxUnselectedBg = FromHexStr("#EAF2FB");
        public Color TextBoxBorder = FromHexStr("#ABC1DE");
        public Color TextBoxSelectedBg = SystemColors.Window;
        public Color TextBoxSelectedBorder = FromHexStr("#ABC1DE");
        public Color TextBoxDisabledBg = SystemColors.Control;
        public Color TextBoxDisabledBorder = FromHexStr("#ABC1DE");

        public Color ToolTipContentNorth = Color.FromArgb(250, 252, 254);// SystemColors.MenuBar;// FromHex("#C8D9ED");
        public Color ToolTipContentSouth = Color.FromArgb(206, 220, 241);// SystemColors.MenuBar;// FromHex("#E7F2FF");
        public Color ToolTipDarkBorder = Color.DarkGray;// Color.FromArgb(51, FromHex("#15428B"));
        public Color ToolTipLightBorder = Color.FromArgb(102, Color.White);
        public Color ToolTipText = WinApi.IsVista ? SystemColors.InactiveCaptionText : FromHexStr("#15428B");  // in XP SystemColors.InactiveCaptionText is hardly readable

        public Color ToolStripItemTextPressed = FromHexStr("#444444");
        public Color ToolStripItemTextSelected = FromHexStr("#444444");
        public Color ToolStripItemText = FromHexStr("#444444");
        public Color ToolStripitemTextDisabled = Color.DarkGray;

        public Color clrVerBG_Shadow = Color.FromArgb(255, 181, 190, 206);

        /// <summary>
        /// 2013 Colors
        /// Office 2013 White Theme
        /// </summary>
        public Color ButtonChecked_2013 = FromHexStr("#CDE6F7");
        public Color ButtonPressed_2013 = FromHexStr("#92C0E0");
        public Color ButtonSelected_2013 = FromHexStr("#CDE6F7");
        public Color OrbButton_2013 = FromHexStr("#0072C6");
        public Color OrbButtonSelected_2013 = FromHexStr("#2A8AD4");
        public Color OrbButtonPressed_2013 = FromHexStr("#2A8AD4");

        public Color TabText_2013 = FromHexStr("#0072C6");
        public Color TabTextSelected_2013 = FromHexStr("#444444");
        public Color PanelBorder_2013 = FromHexStr("#15428B");

        public Color RibbonBackground_2013 = FromHexStr("#FFFFFF");
        public Color TabCompleteBackground_2013 = FromHexStr("#FFFFFF");
        public Color TabNormalBackground_2013 = FromHexStr("#FFFFFF");
        public Color TabActiveBackbround_2013 = FromHexStr("#FFFFFF");

        public Color TabBorder_2013 = FromHexStr("#D4D4D4");
        public Color TabCompleteBorder_2013 = FromHexStr("#D4D4D4");

        public Color TabActiveBorder_2013 = FromHexStr("#D4D4D4");

        public Color OrbButtonText_2013 = FromHexStr("#FFFFFF");
        public Color PanelText_2013 = FromHexStr("#666666");
        public Color RibbonItemText_2013 = FromHexStr("#444444");
        public Color ToolTipText_2013 = FromHexStr("#262626");

        public Color ToolStripItemTextPressed_2013 = FromHexStr("#444444");
        public Color ToolStripItemTextSelected_2013 = FromHexStr("#444444");
        public Color ToolStripItemText_2013 = FromHexStr("#444444");

        #endregion

        #region Methods

        //internal static Color FromHex(string hex)
        private static Color FromHexStr(string hex)
        {
            if (hex.StartsWith("#"))
                hex = hex.Substring(1);

            switch (hex.Length)
            {
                case 6:
                    return Color.FromArgb(
                        int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber),
                        int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber),
                        int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber));

                case 8:
                    return Color.FromArgb(
                        int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber),
                        int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber),
                        int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber),
                        int.Parse(hex.Substring(6, 2), NumberStyles.HexNumber));

                default:
                    throw new ArgumentException("Color not valid");
            }
        }

        public Color FromHex(string hex)
        {
            return FromHexStr(hex);
        }

        internal static Color ToGray(Color c)
        {
            int m = (c.R + c.G + c.B) / 3;
            return Color.FromArgb(m, m, m);
        }

        #endregion

        #region Colors and Theme

        public void SetColor(RibbonColorPart ribbonColorPart, int red, int green, int blue)
        {
            SetColor(ribbonColorPart, Color.FromArgb(red, green, blue));
        }

        public void SetColor(RibbonColorPart ribbonColorPart, string hexColor)
        {
            SetColor(ribbonColorPart, FromHex(hexColor));
        }

        public void SetColor(RibbonColorPart ribbonColorPart, Color color)
        {
            switch (ribbonColorPart)
            {
                case RibbonColorPart.OrbDropDownDarkBorder:
                    OrbDropDownDarkBorder = color; break;
                case RibbonColorPart.OrbDropDownLightBorder:
                    OrbDropDownLightBorder = color; break;
                case RibbonColorPart.OrbDropDownBack:
                    OrbDropDownBack = color; break;
                case RibbonColorPart.OrbDropDownNorthA:
                    OrbDropDownNorthA = color; break;

                case RibbonColorPart.OrbDropDownNorthB:
                    OrbDropDownNorthB = color; break;
                case RibbonColorPart.OrbDropDownNorthC:
                    OrbDropDownNorthC = color; break;
                case RibbonColorPart.OrbDropDownNorthD:
                    OrbDropDownNorthD = color; break;
                case RibbonColorPart.OrbDropDownSouthC:
                    OrbDropDownSouthC = color; break;
                case RibbonColorPart.OrbDropDownSouthD:
                    OrbDropDownSouthD = color; break;
                case RibbonColorPart.OrbDropDownContentbg:
                    OrbDropDownContentbg = color; break;
                case RibbonColorPart.OrbDropDownContentbglight:
                    OrbDropDownContentbglight = color; break;
                case RibbonColorPart.OrbDropDownSeparatorlight:
                    OrbDropDownSeparatorlight = color; break;
                case RibbonColorPart.OrbDropDownSeparatordark:
                    OrbDropDownSeparatordark = color; break;

                case RibbonColorPart.Caption1:
                    Caption1 = color; break;
                case RibbonColorPart.Caption2:
                    Caption2 = color; break;
                case RibbonColorPart.Caption3:
                    Caption3 = color; break;
                case RibbonColorPart.Caption4:
                    Caption4 = color; break;
                case RibbonColorPart.Caption5:
                    Caption5 = color; break;
                case RibbonColorPart.Caption6:
                    Caption6 = color; break;
                case RibbonColorPart.Caption7:
                    Caption7 = color; break;

                case RibbonColorPart.QuickAccessBorderDark:
                    QuickAccessBorderDark = color; break;
                case RibbonColorPart.QuickAccessBorderLight:
                    QuickAccessBorderLight = color; break;
                case RibbonColorPart.QuickAccessUpper:
                    QuickAccessUpper = color; break;
                case RibbonColorPart.QuickAccessLower:
                    QuickAccessLower = color; break;

                case RibbonColorPart.OrbOptionBorder:
                    OrbOptionBorder = color; break;
                case RibbonColorPart.OrbOptionBackground:
                    OrbOptionBackground = color; break;
                case RibbonColorPart.OrbOptionShine:
                    OrbOptionShine = color; break;

                case RibbonColorPart.Arrow:
                    Arrow = color; break;
                case RibbonColorPart.ArrowLight:
                    ArrowLight = color; break;
                case RibbonColorPart.ArrowDisabled:
                    ArrowDisabled = color; break;
                case RibbonColorPart.Text:
                    Text = color; break;

                //case RibbonColorPart.RibbonBackground:
                //RibbonBackground = color; break;
                case RibbonColorPart.RibbonBackground:
                    RibbonBackground = color; break;
                case RibbonColorPart.TabBorder:
                    TabBorder = color; break;
                case RibbonColorPart.TabNorth:
                    TabNorth = color; break;
                case RibbonColorPart.TabSouth:
                    TabSouth = color; break;
                case RibbonColorPart.TabGlow:
                    TabGlow = color; break;
                case RibbonColorPart.TabText:
                    TabText = color; break;
                case RibbonColorPart.TabActiveText:
                    TabActiveText = color; break;
                case RibbonColorPart.TabContentNorth:
                    TabContentNorth = color; break;
                case RibbonColorPart.TabContentSouth:
                    TabContentSouth = color; break;
                case RibbonColorPart.TabSelectedGlow:
                    TabSelectedGlow = color; break;
                case RibbonColorPart.PanelDarkBorder:
                    PanelDarkBorder = color; break;
                case RibbonColorPart.PanelLightBorder:
                    PanelLightBorder = color; break;
                case RibbonColorPart.PanelTextBackground:
                    PanelTextBackground = color; break;
                case RibbonColorPart.PanelTextBackgroundSelected:
                    PanelTextBackgroundSelected = color; break;
                case RibbonColorPart.PanelText:
                    PanelText = color; break;
                case RibbonColorPart.PanelBackgroundSelected:
                    PanelBackgroundSelected = color; break;
                case RibbonColorPart.PanelOverflowBackground:
                    PanelOverflowBackground = color; break;
                case RibbonColorPart.PanelOverflowBackgroundPressed:
                    PanelOverflowBackgroundPressed = color; break;
                case RibbonColorPart.PanelOverflowBackgroundSelectedNorth:
                    PanelOverflowBackgroundSelectedNorth = color; break;
                case RibbonColorPart.PanelOverflowBackgroundSelectedSouth:
                    PanelOverflowBackgroundSelectedSouth = color; break;

                case RibbonColorPart.ButtonBgOut:
                    ButtonBgOut = color; break;
                case RibbonColorPart.ButtonBgCenter:
                    ButtonBgCenter = color; break;
                case RibbonColorPart.ButtonBorderOut:
                    ButtonBorderOut = color; break;
                case RibbonColorPart.ButtonBorderIn:
                    ButtonBorderIn = color; break;
                case RibbonColorPart.ButtonGlossyNorth:
                    ButtonGlossyNorth = color; break;
                case RibbonColorPart.ButtonGlossySouth:
                    ButtonGlossySouth = color; break;

                case RibbonColorPart.ButtonDisabledBgOut:
                    ButtonDisabledBgOut = color; break;
                case RibbonColorPart.ButtonDisabledBgCenter:
                    ButtonDisabledBgCenter = color; break;
                case RibbonColorPart.ButtonDisabledBorderOut:
                    ButtonDisabledBorderOut = color; break;
                case RibbonColorPart.ButtonDisabledBorderIn:
                    ButtonDisabledBorderIn = color; break;
                case RibbonColorPart.ButtonDisabledGlossyNorth:
                    ButtonDisabledGlossyNorth = color; break;
                case RibbonColorPart.ButtonDisabledGlossySouth:
                    ButtonDisabledGlossySouth = color; break;

                case RibbonColorPart.ButtonSelectedBgOut:
                    ButtonSelectedBgOut = color; break;
                case RibbonColorPart.ButtonSelectedBgCenter:
                    ButtonSelectedBgCenter = color; break;
                case RibbonColorPart.ButtonSelectedBorderOut:
                    ButtonSelectedBorderOut = color; break;
                case RibbonColorPart.ButtonSelectedBorderIn:
                    ButtonSelectedBorderIn = color; break;
                case RibbonColorPart.ButtonSelectedGlossyNorth:
                    ButtonSelectedGlossyNorth = color; break;
                case RibbonColorPart.ButtonSelectedGlossySouth:
                    ButtonSelectedGlossySouth = color; break;

                case RibbonColorPart.ButtonPressedBgOut:
                    ButtonPressedBgOut = color; break;
                case RibbonColorPart.ButtonPressedBgCenter:
                    ButtonPressedBgCenter = color; break;
                case RibbonColorPart.ButtonPressedBorderOut:
                    ButtonPressedBorderOut = color; break;
                case RibbonColorPart.ButtonPressedBorderIn:
                    ButtonPressedBorderIn = color; break;
                case RibbonColorPart.ButtonPressedGlossyNorth:
                    ButtonPressedGlossyNorth = color; break;
                case RibbonColorPart.ButtonPressedGlossySouth:
                    ButtonPressedGlossySouth = color; break;

                case RibbonColorPart.ButtonCheckedBgOut:
                    ButtonCheckedBgOut = color; break;
                case RibbonColorPart.ButtonCheckedBgCenter:
                    ButtonCheckedBgCenter = color; break;
                case RibbonColorPart.ButtonCheckedBorderOut:
                    ButtonCheckedBorderOut = color; break;
                case RibbonColorPart.ButtonCheckedBorderIn:
                    ButtonCheckedBorderIn = color; break;
                case RibbonColorPart.ButtonCheckedGlossyNorth:
                    ButtonCheckedGlossyNorth = color; break;
                case RibbonColorPart.ButtonCheckedGlossySouth:
                    ButtonCheckedGlossySouth = color; break;

                case RibbonColorPart.ButtonCheckedSelectedBgOut:
                    ButtonCheckedSelectedBgOut = color; break;
                case RibbonColorPart.ButtonCheckedSelectedBgCenter:
                    ButtonCheckedSelectedBgCenter = color; break;
                case RibbonColorPart.ButtonCheckedSelectedBorderOut:
                    ButtonCheckedSelectedBorderOut = color; break;
                case RibbonColorPart.ButtonCheckedSelectedBorderIn:
                    ButtonCheckedSelectedBorderIn = color; break;
                case RibbonColorPart.ButtonCheckedSelectedGlossyNorth:
                    ButtonCheckedSelectedGlossyNorth = color; break;
                case RibbonColorPart.ButtonCheckedSelectedGlossySouth:
                    ButtonCheckedSelectedGlossySouth = color; break;

                case RibbonColorPart.ItemGroupOuterBorder:
                    ItemGroupOuterBorder = color; break;
                case RibbonColorPart.ItemGroupInnerBorder:
                    ItemGroupInnerBorder = color; break;
                case RibbonColorPart.ItemGroupSeparatorLight:
                    ItemGroupSeparatorLight = color; break;
                case RibbonColorPart.ItemGroupSeparatorDark:
                    ItemGroupSeparatorDark = color; break;
                case RibbonColorPart.ItemGroupBgNorth:
                    ItemGroupBgNorth = color; break;
                case RibbonColorPart.ItemGroupBgSouth:
                    ItemGroupBgSouth = color; break;
                case RibbonColorPart.ItemGroupBgGlossy:
                    ItemGroupBgGlossy = color; break;

                case RibbonColorPart.ButtonListBorder:
                    ButtonListBorder = color; break;
                case RibbonColorPart.ButtonListBg:
                    ButtonListBg = color; break;
                case RibbonColorPart.ButtonListBgSelected:
                    ButtonListBgSelected = color; break;

                case RibbonColorPart.DropDownBg:
                    DropDownBg = color; break;
                case RibbonColorPart.DropDownImageBg:
                    DropDownImageBg = color; break;
                case RibbonColorPart.DropDownImageSeparator:
                    DropDownImageSeparator = color; break;
                case RibbonColorPart.DropDownBorder:
                    DropDownBorder = color; break;
                case RibbonColorPart.DropDownGripNorth:
                    DropDownGripNorth = color; break;
                case RibbonColorPart.DropDownGripSouth:
                    DropDownGripSouth = color; break;
                case RibbonColorPart.DropDownGripBorder:
                    DropDownGripBorder = color; break;
                case RibbonColorPart.DropDownGripDark:
                    DropDownGripDark = color; break;
                case RibbonColorPart.DropDownGripLight:
                    DropDownGripLight = color; break;
                case RibbonColorPart.DropDownCheckedButtonGlyphBg:
                    DropDownCheckedButtonGlyphBg = color; break;
                case RibbonColorPart.DropDownCheckedButtonGlyphBorder:
                    DropDownCheckedButtonGlyphBorder = color; break;

                case RibbonColorPart.SeparatorLight:
                    SeparatorLight = color; break;
                case RibbonColorPart.SeparatorDark:
                    SeparatorDark = color; break;
                case RibbonColorPart.QATSeparatorLight:
                    QATSeparatorLight = color; break;
                case RibbonColorPart.QATSeparatorDark:
                    QATSeparatorDark = color; break;
                case RibbonColorPart.SeparatorBg:
                    SeparatorBg = color; break;
                case RibbonColorPart.SeparatorLine:
                    SeparatorLine = color; break;

                case RibbonColorPart.TextBoxUnselectedBg:
                    TextBoxUnselectedBg = color; break;
                case RibbonColorPart.TextBoxBorder:
                    TextBoxBorder = color; break;

                case RibbonColorPart.ToolTipContentNorth:
                    ToolTipContentNorth = color; break;
                case RibbonColorPart.ToolTipContentSouth:
                    ToolTipContentSouth = color; break;
                case RibbonColorPart.ToolTipDarkBorder:
                    ToolTipDarkBorder = color; break;
                case RibbonColorPart.ToolTipLightBorder:
                    ToolTipLightBorder = color; break;

                case RibbonColorPart.ToolStripItemTextPressed:
                    ToolStripItemTextPressed = color; break;
                case RibbonColorPart.ToolStripItemTextSelected:
                    ToolStripItemTextSelected = color; break;
                case RibbonColorPart.ToolStripItemText:
                    ToolStripItemText = color; break;

                case RibbonColorPart.ButtonChecked_2013:
                    ButtonChecked_2013 = color; break;
                case RibbonColorPart.ButtonPressed_2013:
                    ButtonPressed_2013 = color; break;
                case RibbonColorPart.ButtonSelected_2013:
                    ButtonSelected_2013 = color; break;
                case RibbonColorPart.OrbButton_2013:
                    OrbButton_2013 = color; break;
                case RibbonColorPart.OrbButtonSelected_2013:
                    OrbButtonSelected_2013 = color; break;
                case RibbonColorPart.OrbButtonPressed_2013:
                    OrbButtonPressed_2013 = color; break;
                case RibbonColorPart.TabText_2013:
                    TabText_2013 = color; break;
                case RibbonColorPart.TabTextSelected_2013:
                    TabTextSelected_2013 = color; break;
                case RibbonColorPart.PanelBorder_2013:
                    PanelBorder_2013 = color; break;
                case RibbonColorPart.RibbonBackground_2013:
                    RibbonBackground_2013 = color; break;
                case RibbonColorPart.TabCompleteBackground_2013:
                    TabCompleteBackground_2013 = color; break;
                case RibbonColorPart.TabNormalBackground_2013:
                    TabNormalBackground_2013 = color; break;
                case RibbonColorPart.TabActiveBackbround_2013:
                    TabActiveBackbround_2013 = color; break;
                case RibbonColorPart.TabBorder_2013:
                    TabBorder_2013 = color; break;
                case RibbonColorPart.TabCompleteBorder_2013:
                    TabCompleteBorder_2013 = color; break;
                case RibbonColorPart.TabActiveBorder_2013:
                    TabActiveBorder_2013 = color; break;
                case RibbonColorPart.OrbButtonText_2013:
                    OrbButtonText_2013 = color; break;
                case RibbonColorPart.PanelText_2013:
                    PanelText_2013 = color; break;
                case RibbonColorPart.RibbonItemText_2013:
                    RibbonItemText_2013 = color; break;
                case RibbonColorPart.ToolTipText_2013:
                    ToolTipText_2013 = color; break;

                case RibbonColorPart.ToolStripItemTextPressed_2013:
                    ToolStripItemTextPressed_2013 = color; break;
                case RibbonColorPart.ToolStripItemTextSelected_2013:
                    ToolStripItemTextSelected_2013 = color; break;
                case RibbonColorPart.ToolStripItemText_2013:
                    ToolStripItemText_2013 = color; break;
                default:
                    break;
            }
        }

        public string GetColorHexStr(RibbonColorPart ribbonColorPart)
        {
            Color c = GetColor(ribbonColorPart);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("#");
            sb.Append(BitConverter.ToString(new[] { c.R }));
            sb.Append(BitConverter.ToString(new[] { c.G }));
            sb.Append(BitConverter.ToString(new[] { c.B }));
            return sb.ToString();
        }

        public string GetFullColorHexStr(RibbonColorPart ribbonColorPart)
        {
            Color c = GetColor(ribbonColorPart);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("#");
            sb.Append(BitConverter.ToString(new[] { c.A }));
            sb.Append(BitConverter.ToString(new[] { c.R }));
            sb.Append(BitConverter.ToString(new[] { c.G }));
            sb.Append(BitConverter.ToString(new[] { c.B }));
            return sb.ToString();
        }

        public Color GetColor(RibbonColorPart ribbonColorPart)
        {
            switch (ribbonColorPart)
            {
                case RibbonColorPart.OrbDropDownDarkBorder:
                    return OrbDropDownDarkBorder;
                case RibbonColorPart.OrbDropDownLightBorder:
                    return OrbDropDownLightBorder;
                case RibbonColorPart.OrbDropDownBack:
                    return OrbDropDownBack;
                case RibbonColorPart.OrbDropDownNorthA:
                    return OrbDropDownNorthA;
                case RibbonColorPart.OrbDropDownNorthB:
                    return OrbDropDownNorthB;
                case RibbonColorPart.OrbDropDownNorthC:
                    return OrbDropDownNorthC;
                case RibbonColorPart.OrbDropDownNorthD:
                    return OrbDropDownNorthD;
                case RibbonColorPart.OrbDropDownSouthC:
                    return OrbDropDownSouthC;
                case RibbonColorPart.OrbDropDownSouthD:
                    return OrbDropDownSouthD;
                case RibbonColorPart.OrbDropDownContentbg:
                    return OrbDropDownContentbg;
                case RibbonColorPart.OrbDropDownContentbglight:
                    return OrbDropDownContentbglight;
                case RibbonColorPart.OrbDropDownSeparatorlight:
                    return OrbDropDownSeparatorlight;
                case RibbonColorPart.OrbDropDownSeparatordark:
                    return OrbDropDownSeparatordark;

                case RibbonColorPart.Caption1:
                    return Caption1;
                case RibbonColorPart.Caption2:
                    return Caption2;
                case RibbonColorPart.Caption3:
                    return Caption3;
                case RibbonColorPart.Caption4:
                    return Caption4;
                case RibbonColorPart.Caption5:
                    return Caption5;
                case RibbonColorPart.Caption6:
                    return Caption6;
                case RibbonColorPart.Caption7:
                    return Caption7;

                case RibbonColorPart.QuickAccessBorderDark:
                    return QuickAccessBorderDark;
                case RibbonColorPart.QuickAccessBorderLight:
                    return QuickAccessBorderLight;
                case RibbonColorPart.QuickAccessUpper:
                    return QuickAccessUpper;
                case RibbonColorPart.QuickAccessLower:
                    return QuickAccessLower;

                case RibbonColorPart.OrbOptionBorder:
                    return OrbOptionBorder;
                case RibbonColorPart.OrbOptionBackground:
                    return OrbOptionBackground;
                case RibbonColorPart.OrbOptionShine:
                    return OrbOptionShine;

                case RibbonColorPart.Arrow:
                    return Arrow;
                case RibbonColorPart.ArrowLight:
                    return ArrowLight;
                case RibbonColorPart.ArrowDisabled:
                    return ArrowDisabled;
                case RibbonColorPart.Text:
                    return Text;

                case RibbonColorPart.RibbonBackground:
                    return RibbonBackground;
                case RibbonColorPart.TabBorder:
                    return TabBorder;
                case RibbonColorPart.TabSelectedBorder:
                    return TabSelectedBorder;
                case RibbonColorPart.TabNorth:
                    return TabNorth;
                case RibbonColorPart.TabSouth:
                    return TabSouth;
                case RibbonColorPart.TabGlow:
                    return TabGlow;
                case RibbonColorPart.TabText:
                    return TabText;
                case RibbonColorPart.TabActiveText:
                    return TabActiveText;
                case RibbonColorPart.TabContentNorth:
                    return TabContentNorth;
                case RibbonColorPart.TabContentSouth:
                    return TabContentSouth;
                case RibbonColorPart.TabSelectedGlow:
                    return TabSelectedGlow;
                case RibbonColorPart.PanelDarkBorder:
                    return PanelDarkBorder;
                case RibbonColorPart.PanelLightBorder:
                    return PanelLightBorder;
                case RibbonColorPart.PanelTextBackground:
                    return PanelTextBackground;
                case RibbonColorPart.PanelTextBackgroundSelected:
                    return PanelTextBackgroundSelected;
                case RibbonColorPart.PanelText:
                    return PanelText;
                case RibbonColorPart.PanelBackgroundSelected:
                    return PanelBackgroundSelected;
                case RibbonColorPart.PanelOverflowBackground:
                    return PanelOverflowBackground;
                case RibbonColorPart.PanelOverflowBackgroundPressed:
                    return PanelOverflowBackgroundPressed;
                case RibbonColorPart.PanelOverflowBackgroundSelectedNorth:
                    return PanelOverflowBackgroundSelectedNorth;
                case RibbonColorPart.PanelOverflowBackgroundSelectedSouth:
                    return PanelOverflowBackgroundSelectedSouth;

                case RibbonColorPart.ButtonBgOut:
                    return ButtonBgOut;
                case RibbonColorPart.ButtonBgCenter:
                    return ButtonBgCenter;
                case RibbonColorPart.ButtonBorderOut:
                    return ButtonBorderOut;
                case RibbonColorPart.ButtonBorderIn:
                    return ButtonBorderIn;
                case RibbonColorPart.ButtonGlossyNorth:
                    return ButtonGlossyNorth;
                case RibbonColorPart.ButtonGlossySouth:
                    return ButtonGlossySouth;

                case RibbonColorPart.ButtonDisabledBgOut:
                    return ButtonDisabledBgOut;
                case RibbonColorPart.ButtonDisabledBgCenter:
                    return ButtonDisabledBgCenter;
                case RibbonColorPart.ButtonDisabledBorderOut:
                    return ButtonDisabledBorderOut;
                case RibbonColorPart.ButtonDisabledBorderIn:
                    return ButtonDisabledBorderIn;
                case RibbonColorPart.ButtonDisabledGlossyNorth:
                    return ButtonDisabledGlossyNorth;
                case RibbonColorPart.ButtonDisabledGlossySouth:
                    return ButtonDisabledGlossySouth;

                case RibbonColorPart.ButtonSelectedBgOut:
                    return ButtonSelectedBgOut;
                case RibbonColorPart.ButtonSelectedBgCenter:
                    return ButtonSelectedBgCenter;
                case RibbonColorPart.ButtonSelectedBorderOut:
                    return ButtonSelectedBorderOut;
                case RibbonColorPart.ButtonSelectedBorderIn:
                    return ButtonSelectedBorderIn;
                case RibbonColorPart.ButtonSelectedGlossyNorth:
                    return ButtonSelectedGlossyNorth;
                case RibbonColorPart.ButtonSelectedGlossySouth:
                    return ButtonSelectedGlossySouth;

                case RibbonColorPart.ButtonPressedBgOut:
                    return ButtonPressedBgOut;
                case RibbonColorPart.ButtonPressedBgCenter:
                    return ButtonPressedBgCenter;
                case RibbonColorPart.ButtonPressedBorderOut:
                    return ButtonPressedBorderOut;
                case RibbonColorPart.ButtonPressedBorderIn:
                    return ButtonPressedBorderIn;
                case RibbonColorPart.ButtonPressedGlossyNorth:
                    return ButtonPressedGlossyNorth;
                case RibbonColorPart.ButtonPressedGlossySouth:
                    return ButtonPressedGlossySouth;

                case RibbonColorPart.ButtonCheckedBgOut:
                    return ButtonCheckedBgOut;
                case RibbonColorPart.ButtonCheckedBgCenter:
                    return ButtonCheckedBgCenter;
                case RibbonColorPart.ButtonCheckedBorderOut:
                    return ButtonCheckedBorderOut;
                case RibbonColorPart.ButtonCheckedBorderIn:
                    return ButtonCheckedBorderIn;
                case RibbonColorPart.ButtonCheckedGlossyNorth:
                    return ButtonCheckedGlossyNorth;
                case RibbonColorPart.ButtonCheckedGlossySouth:
                    return ButtonCheckedGlossySouth;

                case RibbonColorPart.ButtonCheckedSelectedBgOut:
                    return ButtonCheckedSelectedBgOut;
                case RibbonColorPart.ButtonCheckedSelectedBgCenter:
                    return ButtonCheckedSelectedBgCenter;
                case RibbonColorPart.ButtonCheckedSelectedBorderOut:
                    return ButtonCheckedSelectedBorderOut;
                case RibbonColorPart.ButtonCheckedSelectedBorderIn:
                    return ButtonCheckedSelectedBorderIn;
                case RibbonColorPart.ButtonCheckedSelectedGlossyNorth:
                    return ButtonCheckedSelectedGlossyNorth;
                case RibbonColorPart.ButtonCheckedSelectedGlossySouth:
                    return ButtonCheckedSelectedGlossySouth;

                case RibbonColorPart.ItemGroupOuterBorder:
                    return ItemGroupOuterBorder;
                case RibbonColorPart.ItemGroupInnerBorder:
                    return ItemGroupInnerBorder;
                case RibbonColorPart.ItemGroupSeparatorLight:
                    return ItemGroupSeparatorLight;
                case RibbonColorPart.ItemGroupSeparatorDark:
                    return ItemGroupSeparatorDark;
                case RibbonColorPart.ItemGroupBgNorth:
                    return ItemGroupBgNorth;
                case RibbonColorPart.ItemGroupBgSouth:
                    return ItemGroupBgSouth;
                case RibbonColorPart.ItemGroupBgGlossy:
                    return ItemGroupBgGlossy;

                case RibbonColorPart.ButtonListBorder:
                    return ButtonListBorder;
                case RibbonColorPart.ButtonListBg:
                    return ButtonListBg;
                case RibbonColorPart.ButtonListBgSelected:
                    return ButtonListBgSelected;

                case RibbonColorPart.DropDownBg:
                    return DropDownBg;
                case RibbonColorPart.DropDownImageBg:
                    return DropDownImageBg;
                case RibbonColorPart.DropDownImageSeparator:
                    return DropDownImageSeparator;
                case RibbonColorPart.DropDownBorder:
                    return DropDownBorder;
                case RibbonColorPart.DropDownGripNorth:
                    return DropDownGripNorth;
                case RibbonColorPart.DropDownGripSouth:
                    return DropDownGripSouth;
                case RibbonColorPart.DropDownGripBorder:
                    return DropDownGripBorder;
                case RibbonColorPart.DropDownGripDark:
                    return DropDownGripDark;
                case RibbonColorPart.DropDownGripLight:
                    return DropDownGripLight;
                case RibbonColorPart.DropDownCheckedButtonGlyphBg:
                    return DropDownCheckedButtonGlyphBg;
                case RibbonColorPart.DropDownCheckedButtonGlyphBorder:
                    return DropDownCheckedButtonGlyphBorder;

                case RibbonColorPart.SeparatorLight:
                    return SeparatorLight;
                case RibbonColorPart.SeparatorDark:
                    return SeparatorDark;
                case RibbonColorPart.QATSeparatorLight:
                    return QATSeparatorLight;
                case RibbonColorPart.QATSeparatorDark:
                    return QATSeparatorDark;
                case RibbonColorPart.SeparatorBg:
                    return SeparatorBg;
                case RibbonColorPart.SeparatorLine:
                    return SeparatorLine;

                case RibbonColorPart.TextBoxUnselectedBg:
                    return TextBoxUnselectedBg;
                case RibbonColorPart.TextBoxBorder:
                    return TextBoxBorder;

                case RibbonColorPart.ToolTipContentNorth:
                    return ToolTipContentNorth;
                case RibbonColorPart.ToolTipContentSouth:
                    return ToolTipContentSouth;
                case RibbonColorPart.ToolTipDarkBorder:
                    return ToolTipDarkBorder;
                case RibbonColorPart.ToolTipLightBorder:
                    return ToolTipLightBorder;

                case RibbonColorPart.ToolStripItemTextPressed:
                    return ToolStripItemTextPressed;
                case RibbonColorPart.ToolStripItemTextSelected:
                    return ToolStripItemTextSelected;
                case RibbonColorPart.ToolStripItemText:
                    return ToolStripItemText;

                case RibbonColorPart.ButtonPressed_2013:
                    return ButtonPressed_2013;
                case RibbonColorPart.ButtonSelected_2013:
                    return ButtonSelected_2013;
                case RibbonColorPart.OrbButton_2013:
                    return OrbButton_2013;
                case RibbonColorPart.OrbButtonSelected_2013:
                    return OrbButtonSelected_2013;
                case RibbonColorPart.OrbButtonPressed_2013:
                    return OrbButtonPressed_2013;
                case RibbonColorPart.TabText_2013:
                    return TabText_2013;
                case RibbonColorPart.TabTextSelected_2013:
                    return TabTextSelected_2013;
                case RibbonColorPart.PanelBorder_2013:
                    return PanelBorder_2013;
                case RibbonColorPart.RibbonBackground_2013:
                    return RibbonBackground_2013;
                case RibbonColorPart.TabCompleteBackground_2013:
                    return TabCompleteBackground_2013;
                case RibbonColorPart.TabNormalBackground_2013:
                    return TabNormalBackground_2013;
                case RibbonColorPart.TabActiveBackbround_2013:
                    return TabActiveBackbround_2013;
                case RibbonColorPart.TabBorder_2013:
                    return TabBorder_2013;
                case RibbonColorPart.TabCompleteBorder_2013:
                    return TabCompleteBorder_2013;
                case RibbonColorPart.TabActiveBorder_2013:
                    return TabActiveBorder_2013;
                case RibbonColorPart.OrbButtonText_2013:
                    return OrbButtonText_2013;
                case RibbonColorPart.PanelText_2013:
                    return PanelText_2013;
                case RibbonColorPart.RibbonItemText_2013:
                    return RibbonItemText_2013;
                case RibbonColorPart.ToolTipText_2013:
                    return ToolTipText_2013;

                case RibbonColorPart.ToolStripItemTextPressed_2013:
                    return ToolStripItemTextPressed_2013;
                case RibbonColorPart.ToolStripItemTextSelected_2013:
                    return ToolStripItemTextSelected_2013;
                case RibbonColorPart.ToolStripItemText_2013:
                    return ToolStripItemText_2013;

                default:
                    return Color.White;
            }
        }

        #endregion

        #region Theme File Read / Write

        public string WriteThemeIniFile()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[Properties]");
            sb.AppendLine("ThemeName = " + ThemeName);
            sb.AppendLine("Author = " + ThemeAuthor);
            sb.AppendLine("AuthorEmail = " + ThemeAuthorEmail);
            sb.AppendLine("AuthorWebsite = " + ThemeAuthorWebsite);
            sb.AppendLine("DateCreated = " + ThemeDateCreated);
            sb.AppendLine();
            sb.AppendLine("[ColorTable]");

            int count = Enum.GetNames(typeof(RibbonColorPart)).Length;
            for (int i = 0; i < count; i++)
            {
                sb.AppendLine(((RibbonColorPart)i) + " = " + GetFullColorHexStr((RibbonColorPart)i));
            }

            return sb.ToString();
        }

        public void ReadThemeIniFile(string iniFileContent)
        {
            string[] sa = null;
            if (iniFileContent.Contains("\r\n"))
            {
                sa = iniFileContent.Split(new[] { "\r\n" }, StringSplitOptions.None);
            }
            else if (iniFileContent.Contains("\n"))
            {
                sa = iniFileContent.Split(new[] { "\n" }, StringSplitOptions.None);
            }
            else
            {
                throw new ArgumentException("Unrecognized end line delimeter.");
            }

            Dictionary<string, RibbonColorPart> dic1 = new Dictionary<string, RibbonColorPart>();
            foreach (RibbonColorPart e in Enum.GetValues(typeof(RibbonColorPart)))
            {
                dic1[e.ToString().ToLower()] = e;
            }

            foreach (string s in sa)
            {
                string a = s.Trim();
                if (a.Length == 0)
                {
                }
                else
                {
                    string[] sb = a.Split('=');
                    if (sb.Length != 2)
                        continue;
                    string b1 = sb[0].Trim().ToLower();
                    string b2 = sb[1].Trim();

                    if (b1 == "author")
                        ThemeAuthor = b2;
                    else if (b1 == "authorwebsite")
                        ThemeAuthorWebsite = b2;
                    else if (b1 == "authoremail")
                        ThemeAuthorEmail = b2;
                    else if (b1 == "datecreated")
                        ThemeDateCreated = b2;
                    else if (b1 == "themename")
                        ThemeName = b2;
                    else
                    {
                        if (dic1.ContainsKey(b1))
                        {
                            SetColor(dic1[b1], b2);
                        }
                    }
                }
            }
        }

        public string WriteThemeXmlFile()
        {
            string a = "";
            StringWriter str;
            {
                using (XmlTextWriter xml = new XmlTextWriter(str = new StringWriter()))
                {
                    xml.WriteStartDocument();
                    xml.WriteWhitespace("\r\n");
                    xml.WriteStartElement("RibbonColorTheme");
                    xml.WriteWhitespace("\r\n\t");
                    xml.WriteStartElement("Properties");
                    xml.WriteWhitespace("\r\n\t\t");
                    xml.WriteElementString("ThemeName", ThemeName);
                    xml.WriteWhitespace("\r\n\t\t");
                    xml.WriteElementString("Author", ThemeAuthor);
                    xml.WriteWhitespace("\r\n\t\t");
                    xml.WriteElementString("AuthorEmail", ThemeAuthorEmail);
                    xml.WriteWhitespace("\r\n\t\t");
                    xml.WriteElementString("AuthorWebsite", ThemeAuthorWebsite);
                    xml.WriteWhitespace("\r\n\t\t");
                    xml.WriteElementString("DateCreated", ThemeDateCreated);
                    xml.WriteWhitespace("\r\n\t");
                    xml.WriteEndElement();
                    xml.WriteWhitespace("\r\n\t");
                    xml.WriteStartElement("ColorTable");
                    int count = Enum.GetNames(typeof(RibbonColorPart)).Length;
                    for (int i = 0; i < count; i++)
                    {
                        xml.WriteWhitespace("\r\n\t\t");
                        xml.WriteElementString(((RibbonColorPart)i).ToString(), GetFullColorHexStr((RibbonColorPart)i));
                    }
                    xml.WriteWhitespace("\r\n\t");
                    xml.WriteEndElement(); xml.WriteWhitespace("\r\n");
                    xml.WriteEndElement(); xml.WriteWhitespace("\r\n");
                    xml.WriteEndDocument();
                    a = str.ToString();
                }
            }
            return a;
        }

        public void ReadThemeXmlFile(string xmlFileContent)
        {
            Dictionary<string, RibbonColorPart> dic1 = new Dictionary<string, RibbonColorPart>();
            foreach (RibbonColorPart e in Enum.GetValues(typeof(RibbonColorPart)))
            {
                dic1[e.ToString().ToLower()] = e;
            }

            StringReader stringReader;
            using (XmlTextReader reader = new XmlTextReader(stringReader = new StringReader(xmlFileContent)))
            {
                while (reader.Read())
                {
                    switch (reader.Name)
                    {
                        case "ThemeName":
                            ThemeName = reader.ReadString();
                            break;
                        case "Author":
                            ThemeAuthor = reader.ReadString();
                            break;
                        case "AuthorEmail":
                            ThemeAuthorEmail = reader.ReadString();
                            break;
                        case "AuthorWebsite":
                            ThemeAuthorWebsite = reader.ReadString();
                            break;
                        case "DateCreated":
                            ThemeDateCreated = reader.ReadString();
                            break;
                        default:
                            {
                                if (dic1.ContainsKey(reader.Name.ToLower()))
                                {
                                    SetColor(dic1[reader.Name.ToLower()], reader.ReadString());
                                }
                                break;
                            }

                    }
                }
            }
        }
        #endregion
    }
}