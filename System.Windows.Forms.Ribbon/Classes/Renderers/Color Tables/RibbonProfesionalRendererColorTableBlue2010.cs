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

namespace System.Windows.Forms
{
    public class RibbonProfesionalRendererColorTableBlue2010
        : RibbonProfesionalRendererColorTable
    {
        public RibbonProfesionalRendererColorTableBlue2010()
        {
            // Rebuild the solution for the first time
            // for this ColorTable to take effect.
            // Guide for applying new theme: http://officeribbon.codeplex.com/wikipage?title=How%20to%20Make%20a%20New%20Theme%2c%20Skin%20for%20this%20Ribbon%20Programmatically

            ThemeName = "Blue_2010";
            ThemeAuthor = "Steve Towner";
            ThemeAuthorWebsite = "";
            ThemeAuthorEmail = "";
            ThemeDateCreated = "11 Dec 14";

            OrbDropDownDarkBorder = FromHex("#9BAFCA");
            OrbDropDownLightBorder = FromHex("#FFFFFF");
            OrbDropDownBack = FromHex("#BFD3EB");
            OrbDropDownNorthA = FromHex("#D7E5F7");
            OrbDropDownNorthB = FromHex("#D4E1F3");
            OrbDropDownNorthC = FromHex("#C6D8EE");
            OrbDropDownNorthD = FromHex("#B7CAE6");
            OrbDropDownSouthC = FromHex("#B0C9EA");
            OrbDropDownSouthD = FromHex("#CFE0F5");
            OrbDropDownContentbg = FromHex("#E9EAEE");
            OrbDropDownContentbglight = FromHex("#FAFAFA");
            OrbDropDownSeparatorlight = FromHex("#F5F5F5");
            OrbDropDownSeparatordark = FromHex("#C5C5C5");
            Caption1 = FromHex("#E3EBF6");
            Caption2 = FromHex("#DAE9FD");
            Caption3 = FromHex("#D5E5FA");
            Caption4 = FromHex("#D9E7F9");
            Caption5 = FromHex("#CADEF7");
            Caption6 = FromHex("#E4EFFD");
            Caption7 = FromHex("#B0CFF7");
            QuickAccessBorderDark = FromHex("#B6CAE2");
            QuickAccessBorderLight = FromHex("#F2F6FB");
            QuickAccessUpper = FromHex("#E0EBF9");
            QuickAccessLower = FromHex("#C9D9EE");
            OrbOptionBorder = FromHex("#7793B9");
            OrbOptionBackground = FromHex("#E8F1FC");
            OrbOptionShine = FromHex("#D2E1F4");
            Arrow = FromHex("#797C80");
            ArrowLight = Color.FromArgb(200, FromHex("#FFFFFF"));
            ArrowDisabled = FromHex("#B7B7B7");
            Text = FromHex("#15428B");
            RibbonBackground = FromHex("#BED0E8");
            TabBorder = FromHex("#9FB2C7");
            TabSelectedBorder = FromHex("#B1B5BA");
            TabNorth = FromHex("#F5FAFF");
            TabSouth = FromHex("#F0F7FD");
            TabGlow = FromHex("#D1FBFF");
            TabText = FromHex("#15428B");
            TabActiveText = FromHex("#15428B");
            TabContentNorth = FromHex("#EFF6FD");
            TabContentSouth = FromHex("#D8E4F2");
            TabSelectedGlow = FromHex("#E1D2A5");
            PanelDarkBorder = FromHex("#93A7C3");
            PanelLightBorder = Color.FromArgb(127, FromHex("#FFFFFF"));
            PanelTextBackground = FromHex("#C2D9F0");
            PanelTextBackgroundSelected = FromHex("#C2D9F0");
            PanelText = FromHex("#15428B");
            PanelBackgroundSelected = Color.FromArgb(110, FromHex("#FFFFFF"));
            PanelOverflowBackground = FromHex("#B9D1F0");
            PanelOverflowBackgroundPressed = FromHex("#7699C8");
            PanelOverflowBackgroundSelectedNorth = Color.FromArgb(100, FromHex("#FFFFFF"));
            PanelOverflowBackgroundSelectedSouth = Color.FromArgb(102, FromHex("#B8D7FD"));
            ButtonBgOut = FromHex("#C1D5F1");
            ButtonBgCenter = FromHex("#CFE0F7");
            ButtonBorderOut = FromHex("#B9D0ED");
            ButtonBorderIn = FromHex("#E3EDFB");
            ButtonGlossyNorth = FromHex("#DEEBFE");
            ButtonGlossySouth = FromHex("#CBDEF6");
            ButtonDisabledBgOut = FromHex("#E0E4E8");
            ButtonDisabledBgCenter = FromHex("#E8EBEF");
            ButtonDisabledBorderOut = FromHex("#C5D1DE");
            ButtonDisabledBorderIn = FromHex("#F1F3F5");
            ButtonDisabledGlossyNorth = FromHex("#F0F3F6");
            ButtonDisabledGlossySouth = FromHex("#EAEDF1");
            ButtonSelectedBgOut = FromHex("#FFD646");
            ButtonSelectedBgCenter = FromHex("#FFEAAC");
            ButtonSelectedBorderOut = FromHex("#C2A978");
            ButtonSelectedBorderIn = FromHex("#FFF2C7");
            ButtonSelectedGlossyNorth = FromHex("#FFFDDB");
            ButtonSelectedGlossySouth = FromHex("#FFE793");
            ButtonPressedBgOut = FromHex("#F88F2C");
            ButtonPressedBgCenter = FromHex("#FDF1B0");
            ButtonPressedBorderOut = FromHex("#8E8165");
            ButtonPressedBorderIn = FromHex("#F9C65A");
            ButtonPressedGlossyNorth = FromHex("#FDD5A8");
            ButtonPressedGlossySouth = FromHex("#FBB062");
            ButtonCheckedBgOut = FromHex("#FFD86B");
            ButtonCheckedBgCenter = FromHex("#FFF480");
            ButtonCheckedBorderOut = FromHex("#C2963D");
            ButtonCheckedBorderIn = Color.FromArgb(0, FromHex("#F9C65A"));
            ButtonCheckedGlossyNorth = FromHex("#FFE575");
            ButtonCheckedGlossySouth = Color.FromArgb(0, FromHex("#FFDA6D"));
            ButtonCheckedSelectedBgOut = FromHex("#F9AA45");
            ButtonCheckedSelectedBgCenter = FromHex("#FDEA9D");
            ButtonCheckedSelectedBorderOut = FromHex("#8E8165");
            ButtonCheckedSelectedBorderIn = FromHex("#F9C65A");
            ButtonCheckedSelectedGlossyNorth = FromHex("#F8DBB7");
            ButtonCheckedSelectedGlossySouth = FromHex("#FED18E");
            ItemGroupOuterBorder = FromHex("#9EBAE1");
            ItemGroupInnerBorder = Color.FromArgb(51, FromHex("#FFFFFF"));
            ItemGroupSeparatorLight = Color.FromArgb(64, FromHex("#FFFFFF"));
            ItemGroupSeparatorDark = Color.FromArgb(38, FromHex("#9EBAE1"));
            ItemGroupBgNorth = FromHex("#CADCF0");
            ItemGroupBgSouth = FromHex("#D0E1F7");
            ItemGroupBgGlossy = FromHex("#BCD0E9");
            ButtonListBorder = FromHex("#B9D0ED");
            ButtonListBg = FromHex("#D4E6F8");
            ButtonListBgSelected = FromHex("#ECF3FB");
            DropDownBg = FromHex("#FFFFFF");
            DropDownImageBg = FromHex("#FFFFFF");
            DropDownImageSeparator = FromHex("#E2E4E7");
            DropDownBorder = FromHex("#A7ABB0");
            DropDownGripNorth = FromHex("#FFFFFF");
            DropDownGripSouth = FromHex("#DFE9EF");
            DropDownGripBorder = FromHex("#DDE7EE");
            DropDownGripDark = FromHex("#5574A7");
            DropDownGripLight = FromHex("#FFFFFF");
            DropDownCheckedButtonGlyphBg = FromHex("#FCF1C2");
            DropDownCheckedButtonGlyphBorder = FromHex("#F29536");
            SeparatorLight = Color.FromArgb(140, FromHex("#FFFFFF"));
            SeparatorDark = FromHex("#91A6C2");
            QATSeparatorLight = Color.FromArgb(95, FromHex("#FFFFFF"));
            QATSeparatorDark = FromHex("#999B9E");
            SeparatorBg = FromHex("#DAE6EE");
            SeparatorLine = FromHex("#C5C5C5");
            TextBoxUnselectedBg = FromHex("#EAF2FB");
            TextBoxBorder = FromHex("#ABC1DE");
            ToolTipContentNorth = FromHex("#FAFCFE");
            ToolTipContentSouth = FromHex("#CEDCF1");
            ToolTipDarkBorder = FromHex("#A9A9A9");
            ToolTipLightBorder = Color.FromArgb(102, FromHex("#FFFFFF"));
            ToolStripItemTextPressed = FromHex("#444444");
            ToolStripItemTextSelected = FromHex("#444444");
            ToolStripItemText = FromHex("#444444");
            clrVerBG_Shadow = FromHex("#FFFFFF");
            ButtonChecked_2013 = FromHex("#FFFFFF");
            ButtonPressed_2013 = FromHex("#92C0E0");
            ButtonSelected_2013 = FromHex("#CDE6F7");
            OrbButton_2013 = FromHex("#0072C6");
            OrbButtonSelected_2013 = FromHex("#2A8AD4");
            OrbButtonPressed_2013 = FromHex("#2A8AD4");
            TabText_2013 = FromHex("#0072C6");
            TabTextSelected_2013 = FromHex("#444444");
            PanelBorder_2013 = FromHex("#15428B");
            RibbonBackground_2013 = FromHex("#FFFFFF");
            TabCompleteBackground_2013 = FromHex("#FFFFFF");
            TabNormalBackground_2013 = FromHex("#FFFFFF");
            TabActiveBackbround_2013 = FromHex("#FFFFFF");
            TabBorder_2013 = FromHex("#D4D4D4");
            TabCompleteBorder_2013 = FromHex("#D4D4D4");
            TabActiveBorder_2013 = FromHex("#D4D4D4");
            OrbButtonText_2013 = FromHex("#FFFFFF");
            PanelText_2013 = FromHex("#666666");
            RibbonItemText_2013 = FromHex("#444444");
            ToolTipText_2013 = FromHex("#262626");
            ToolStripItemTextPressed_2013 = FromHex("#444444");
            ToolStripItemTextSelected_2013 = FromHex("#444444");
            ToolStripItemText_2013 = FromHex("#444444");
        }
    }
}
