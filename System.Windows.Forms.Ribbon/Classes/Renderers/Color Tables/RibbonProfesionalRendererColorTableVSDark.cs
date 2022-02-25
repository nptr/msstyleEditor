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
    public class RibbonProfesionalRendererColorTableVSDark
         : RibbonProfesionalRendererColorTable
    {
        public RibbonProfesionalRendererColorTableVSDark()
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

            Arrow = FromHex("#999999");
            ArrowLight = FromHex("#999999");
            ArrowDisabled = FromHex("#B7B7B7");

            Text = FromHex("#F1F1F1");
            RibbonBackground = FromHex("#2D2D30");
            TabBorder = FromHex("#252526");
            TabSelectedBorder = FromHex("#B1B5BA");
            TabNorth = FromHex("#2D2D30");
            TabSouth = FromHex("#2D2D30");
            TabGlow = FromHex("#2D2D30");
            TabText = FromHex("#F1F1F1");
            TabActiveText = FromHex("#F1F1F1");
            TabContentNorth = FromHex("#2D2D30");
            TabContentSouth = FromHex("#2D2D30");
            TabSelectedGlow = FromHex("#2D2D30");
            PanelDarkBorder = FromHex("#3E3E40");
            PanelLightBorder = FromHex("#C0C0C0");
            PanelTextBackground = FromHex("#333337");
            PanelTextBackgroundSelected = FromHex("#333337");
            PanelText = FromHex("#C0C0C0");
            PanelBackgroundSelected = FromHex("#2D2D30");
            PanelOverflowBackground = FromHex("#2D2D30");
            PanelOverflowBackgroundPressed = FromHex("#2D2D30");
            PanelOverflowBackgroundSelectedNorth = FromHex("#2D2D30");
            PanelOverflowBackgroundSelectedSouth = FromHex("#2D2D30");
            ButtonBgOut = FromHex("#3F3F46");
            ButtonBgCenter = FromHex("#3F3F46");
            ButtonBorderOut = FromHex("#555555");
            ButtonBorderIn = FromHex("#555555");
            ButtonGlossyNorth = FromHex("#3F3F46");
            ButtonGlossySouth = FromHex("#3F3F46");
            ButtonDisabledBgOut = FromHex("#2D2D30");
            ButtonDisabledBgCenter = FromHex("#2D2D30");
            ButtonDisabledBorderOut = FromHex("#3F3F46");
            ButtonDisabledBorderIn = FromHex("#3F3F46");
            ButtonDisabledGlossyNorth = FromHex("#2D2D30");
            ButtonDisabledGlossySouth = FromHex("#2D2D30");
            ButtonSelectedBgOut = FromHex("#3F3F46");
            ButtonSelectedBgCenter = FromHex("#3F3F46");
            ButtonSelectedBorderOut = FromHex("#007ACC");
            ButtonSelectedBorderIn = FromHex("#007ACC");
            ButtonSelectedGlossyNorth = FromHex("#3F3F46");
            ButtonSelectedGlossySouth = FromHex("#3F3F46");
            ButtonPressedBgOut = FromHex("#007ACC");
            ButtonPressedBgCenter = FromHex("#007ACC");
            ButtonPressedBorderOut = FromHex("#007ACC");
            ButtonPressedBorderIn = FromHex("#007ACC");
            ButtonPressedGlossyNorth = FromHex("#007ACC");
            ButtonPressedGlossySouth = FromHex("#007ACC");
            ButtonCheckedBgOut = FromHex("#3F3F46");
            ButtonCheckedBgCenter = FromHex("#3F3F46");
            ButtonCheckedBorderOut = FromHex("#007ACC");
            ButtonCheckedBorderIn = FromHex("#007ACC");
            ButtonCheckedGlossyNorth = FromHex("#3F3F46");
            ButtonCheckedGlossySouth = FromHex("#3F3F46");
            ButtonCheckedSelectedBgOut = FromHex("#3F3F46");
            ButtonCheckedSelectedBgCenter = FromHex("#3F3F46");
            ButtonCheckedSelectedBorderOut = FromHex("#007ACC");
            ButtonCheckedSelectedBorderIn = FromHex("#007ACC");
            ButtonCheckedSelectedGlossyNorth = FromHex("#3F3F46");
            ButtonCheckedSelectedGlossySouth = FromHex("#3F3F46");
            ItemGroupOuterBorder = FromHex("#555555");
            ItemGroupInnerBorder = FromHex("#555555");
            ItemGroupSeparatorLight = Color.FromArgb(64, FromHex("#FFFFFF"));
            ItemGroupSeparatorDark = Color.FromArgb(38, FromHex("#9EBAE1"));
            ItemGroupBgNorth = FromHex("#2D2D30");
            ItemGroupBgSouth = FromHex("#2D2D30");
            ItemGroupBgGlossy = FromHex("#2D2D30");
            ButtonListBorder = FromHex("#434346");
            ButtonListBg = FromHex("#2D2D30");
            ButtonListBgSelected = FromHex("#3F3F46");
            DropDownBg = FromHex("#1B1B1C");
            DropDownImageBg = FromHex("#1B1B1C");
            DropDownImageSeparator = FromHex("#434346");
            DropDownBorder = FromHex("#3F3F46");
            DropDownGripNorth = FromHex("#3F3F46");
            DropDownGripSouth = FromHex("#3F3F46");
            DropDownGripBorder = FromHex("#3F3F46");
            DropDownGripDark = FromHex("#1B1B1C");
            DropDownGripLight = FromHex("#FFFFFF");
            DropDownCheckedButtonGlyphBg = FromHex("#FCF1C2");
            DropDownCheckedButtonGlyphBorder = FromHex("#F29536");
            SeparatorLight = Color.FromArgb(140, FromHex("#FFFFFF"));
            SeparatorDark = FromHex("#434346");
            QATSeparatorLight = Color.FromArgb(95, FromHex("#FFFFFF"));
            QATSeparatorDark = FromHex("#999B9E");
            SeparatorBg = FromHex("#DAE6EE");
            SeparatorLine = FromHex("#434346");

            TextBoxUnselectedBg = FromHex("#2D2D30");
            TextBoxBorder = FromHex("#555555");
            TextBoxSelectedBg = FromHex("#3F3F46");
            TextBoxSelectedBorder = FromHex("#007ACC");
            TextBoxDisabledBg = FromHex("#2D2D30");
            TextBoxDisabledBorder = FromHex("#434346");

            ToolTipContentNorth = FromHex("#F1F1F1");
            ToolTipContentSouth = FromHex("#F1F1F1");
            ToolTipDarkBorder = FromHex("#2D2D30");
            ToolTipLightBorder = FromHex("#2D2D30");

            ToolStripItemTextPressed = FromHex("#F1F1F1");
            ToolStripItemTextSelected = FromHex("#F1F1F1");
            ToolStripItemText = FromHex("#F1F1F1");
            ToolStripitemTextDisabled = FromHex("#656565");

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

            #endregion
        }


    }
}