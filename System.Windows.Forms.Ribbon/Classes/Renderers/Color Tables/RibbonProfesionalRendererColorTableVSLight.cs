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
    public class RibbonProfesionalRendererColorTableVSLight
         : RibbonProfesionalRendererColorTable
    {
        public RibbonProfesionalRendererColorTableVSLight()
        {
            #region Fields

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

            Arrow = FromHex("#717171");
            ArrowLight = FromHex("#717171");
            ArrowDisabled = FromHex("#B7B7B7");

            Text = FromHex("#1E1E1E");
            RibbonBackground = FromHex("#EEEEF2");
            TabBorder = FromHex("#CCCEDB");
            TabSelectedBorder = FromHex("#B1B5BA");
            TabNorth = FromHex("#EEEEF2");
            TabSouth = FromHex("#EEEEF2");
            TabGlow = FromHex("#EEEEF2");
            TabText = FromHex("#1E1E1E");
            TabActiveText = FromHex("#1E1E1E");
            TabContentNorth = FromHex("#EEEEF2");
            TabContentSouth = FromHex("#EEEEF2");
            TabSelectedGlow = FromHex("#EEEEF2");
            PanelDarkBorder = FromHex("#3E3E40");
            PanelLightBorder = FromHex("#C0C0C0");
            PanelTextBackground = FromHex("#E1E6F1");
            PanelTextBackgroundSelected = FromHex("#C9DEF5");
            PanelText = FromHex("#1E1E1E");
            PanelBackgroundSelected = FromHex("#EEEEF2");
            PanelOverflowBackground = FromHex("#EEEEF2");
            PanelOverflowBackgroundPressed = FromHex("#EEEEF2");
            PanelOverflowBackgroundSelectedNorth = FromHex("#EEEEF2");
            PanelOverflowBackgroundSelectedSouth = FromHex("#EEEEF2");
            ButtonBgOut = FromHex("#ECECF0");
            ButtonBgCenter = FromHex("#ECECF0");
            ButtonBorderOut = FromHex("#CCCEDB");
            ButtonBorderIn = FromHex("#CCCEDB");
            ButtonGlossyNorth = FromHex("#ECECF0");
            ButtonGlossySouth = FromHex("#ECECF0");
            ButtonDisabledBgOut = FromHex("#F5F5F5");
            ButtonDisabledBgCenter = FromHex("#F5F5F5");
            ButtonDisabledBorderOut = FromHex("#CCCEDB");
            ButtonDisabledBorderIn = FromHex("#CCCEDB");
            ButtonDisabledGlossyNorth = FromHex("#F5F5F5");
            ButtonDisabledGlossySouth = FromHex("#F5F5F5");
            ButtonSelectedBgOut = FromHex("#C9DEF5");
            ButtonSelectedBgCenter = FromHex("#C9DEF5");
            ButtonSelectedBorderOut = FromHex("#CCCEDB");
            ButtonSelectedBorderIn = FromHex("#CCCEDB");
            ButtonSelectedGlossyNorth = FromHex("#C9DEF5");
            ButtonSelectedGlossySouth = FromHex("#C9DEF5");
            ButtonPressedBgOut = FromHex("#007ACC");
            ButtonPressedBgCenter = FromHex("#007ACC");
            ButtonPressedBorderOut = FromHex("#007ACC");
            ButtonPressedBorderIn = FromHex("#007ACC");
            ButtonPressedGlossyNorth = FromHex("#007ACC");
            ButtonPressedGlossySouth = FromHex("#007ACC");
            ButtonCheckedBgOut = FromHex("#F5F5F5");
            ButtonCheckedBgCenter = FromHex("#F5F5F5");
            ButtonCheckedBorderOut = FromHex("#CCCEDB");
            ButtonCheckedBorderIn = FromHex("#CCCEDB");
            ButtonCheckedGlossyNorth = FromHex("#F5F5F5");
            ButtonCheckedGlossySouth = FromHex("#F5F5F5");
            ButtonCheckedSelectedBgOut = FromHex("#F5F5F5");
            ButtonCheckedSelectedBgCenter = FromHex("#F5F5F5");
            ButtonCheckedSelectedBorderOut = FromHex("#007ACC");
            ButtonCheckedSelectedBorderIn = FromHex("#007ACC");
            ButtonCheckedSelectedGlossyNorth = FromHex("#F5F5F5");
            ButtonCheckedSelectedGlossySouth = FromHex("#F5F5F5");
            ItemGroupOuterBorder = FromHex("#CCCEDB");
            ItemGroupInnerBorder = FromHex("#CCCEDB");
            ItemGroupSeparatorLight = Color.FromArgb(64, FromHex("#FFFFFF"));
            ItemGroupSeparatorDark = Color.FromArgb(38, FromHex("#9EBAE1"));
            ItemGroupBgNorth = FromHex("#EEEEF2");
            ItemGroupBgSouth = FromHex("#EEEEF2");
            ItemGroupBgGlossy = FromHex("#EEEEF2");
            ButtonListBorder = FromHex("#CCCEDB");
            ButtonListBg = FromHex("#F6F6F6");
            ButtonListBgSelected = FromHex("#F5F5F5");
            DropDownBg = FromHex("#F6F6F6");
            DropDownImageBg = FromHex("#F6F6F6");
            DropDownImageSeparator = FromHex("#434346");
            DropDownBorder = FromHex("#CCCEDB");
            DropDownGripNorth = FromHex("#CCCEDB");
            DropDownGripSouth = FromHex("#CCCEDB");
            DropDownGripBorder = FromHex("#CCCEDB");
            DropDownGripDark = FromHex("#1B1B1C");
            DropDownGripLight = FromHex("#FFFFFF");
            DropDownCheckedButtonGlyphBg = FromHex("#FCF1C2");
            DropDownCheckedButtonGlyphBorder = FromHex("#F29536");
            SeparatorLight = FromHex("#F5F5F5");
            SeparatorDark = FromHex("#CCCEDB");
            QATSeparatorLight = Color.FromArgb(95, FromHex("#FFFFFF"));
            QATSeparatorDark = FromHex("#999B9E");
            SeparatorBg = FromHex("#DAE6EE");
            SeparatorLine = FromHex("#434346");

            TextBoxUnselectedBg = FromHex("#FFFFFF");
            TextBoxBorder = FromHex("#CCCEDB");
            TextBoxSelectedBg = FromHex("#FFFFFF");
            TextBoxSelectedBorder = FromHex("#007ACC");
            TextBoxDisabledBg = FromHex("#EEEEF2");
            TextBoxDisabledBorder = FromHex("#CCCEDB");

            ToolTipContentNorth = FromHex("#ECECF0");
            ToolTipContentSouth = FromHex("#ECECF0");
            ToolTipDarkBorder = FromHex("#CCCEDB");
            ToolTipLightBorder = FromHex("#CCCEDB");

            ToolStripItemTextPressed = FromHex("#FFFFFF");
            ToolStripItemTextSelected = FromHex("#1E1E1E");
            ToolStripItemText = FromHex("#1E1E1E");
            ToolStripitemTextDisabled = FromHex("#A2A4A5");

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
            ToolTipText_2013 = FromHex("#1E1E1E");
            ToolStripItemTextPressed_2013 = FromHex("#FFFFFF");
            ToolStripItemTextSelected_2013 = FromHex("#1E1E1E");
            ToolStripItemText_2013 = FromHex("#1E1E1E");

            #endregion
        }


    }
}