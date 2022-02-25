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

namespace System.Windows.Forms
{
    public class RibbonProfesionalRendererColorTableJellyBelly
        : RibbonProfesionalRendererColorTable
    {
        public RibbonProfesionalRendererColorTableJellyBelly()
        {
            // Rebuild the solution for the first time
            // for this ColorTable to take effect.
            // Guide for applying new theme: http://officeribbon.codeplex.com/wikipage?title=How%20to%20Make%20a%20New%20Theme%2c%20Skin%20for%20this%20Ribbon%20Programmatically

            ThemeName = "JellyBelly";
            ThemeAuthor = "Michael Spradlin";
            ThemeAuthorWebsite = "";
            ThemeAuthorEmail = "";
            ThemeDateCreated = "08/23/2013";

            OrbDropDownDarkBorder = FromHex("#282828");
            OrbDropDownLightBorder = FromHex("#484848");
            OrbDropDownBack = FromHex("#282828");
            OrbDropDownNorthA = FromHex("#282828");
            OrbDropDownNorthB = FromHex("#282828");
            OrbDropDownNorthC = FromHex("#282828");
            OrbDropDownNorthD = FromHex("#282828");
            OrbDropDownSouthC = FromHex("#484848");
            OrbDropDownSouthD = FromHex("#282828");
            OrbDropDownContentbg = FromHex("#8D8D8D");
            OrbDropDownContentbglight = FromHex("#B1B1B1");
            OrbDropDownSeparatorlight = FromHex("#F5F5F5");
            OrbDropDownSeparatordark = FromHex("#C5C5C5");
            Caption1 = FromHex("#484848");
            Caption2 = FromHex("#484848");
            Caption3 = FromHex("#484848");
            Caption4 = FromHex("#484848");
            Caption5 = FromHex("#484848");
            Caption6 = FromHex("#484848");
            Caption7 = FromHex("#484848");
            QuickAccessBorderDark = FromHex("#8D8D8D");
            QuickAccessBorderLight = FromHex("#B1B1B1");
            QuickAccessUpper = FromHex("#282828");
            QuickAccessLower = FromHex("#282828");
            OrbOptionBorder = FromHex("#8D8D8D");
            OrbOptionBackground = FromHex("#8D8D8D");
            OrbOptionShine = FromHex("#8D8D8D");
            Arrow = FromHex("#282828");
            ArrowLight = FromHex("#FFFFFF");
            ArrowDisabled = FromHex("#B7B7B7");
            Text = FromHex("#EBEBEB");
            RibbonBackground = FromHex("#282828");
            TabBorder = FromHex("#B1B1B1");
            TabSelectedBorder = FromHex("#B1B1B1");
            TabNorth = FromHex("#B1B1B1");
            TabSouth = FromHex("#8D8D8D");
            TabGlow = FromHex("#B1B1B1");
            TabText = FromHex("#EBEBEB");
            TabActiveText = FromHex("#EBEBEB");
            TabContentNorth = FromHex("#484848");
            TabContentSouth = FromHex("#484848");
            TabSelectedGlow = FromHex("#30B4E4");
            PanelDarkBorder = FromHex("#282828");
            PanelLightBorder = FromHex("#FFFFFF");
            PanelTextBackground = FromHex("#B7B7B7");
            PanelTextBackgroundSelected = FromHex("#B7B7B7");
            PanelText = FromHex("#282828");
            PanelBackgroundSelected = FromHex("#757575");
            PanelOverflowBackground = FromHex("#B1B1B1");
            PanelOverflowBackgroundPressed = FromHex("#B1B1B1");
            PanelOverflowBackgroundSelectedNorth = FromHex("#757575");
            PanelOverflowBackgroundSelectedSouth = FromHex("#B1B1B1");
            ButtonBgOut = FromHex("#B1B1B1");
            ButtonBgCenter = FromHex("#B1B1B1");
            ButtonBorderOut = FromHex("#282828");
            ButtonBorderIn = FromHex("#B1B1B1");
            ButtonGlossyNorth = FromHex("#B1B1B1");
            ButtonGlossySouth = FromHex("#B1B1B1");
            ButtonDisabledBgOut = FromHex("#E0E0E0");
            ButtonDisabledBgCenter = FromHex("#E0E0E0");
            ButtonDisabledBorderOut = FromHex("#F1F3F5");
            ButtonDisabledBorderIn = FromHex("#F1F3F5");
            ButtonDisabledGlossyNorth = FromHex("#E0E0E0");
            ButtonDisabledGlossySouth = FromHex("#E0E0E0");
            ButtonSelectedBgOut = FromHex("#30B4E4");
            ButtonSelectedBgCenter = FromHex("#30B4E4");
            ButtonSelectedBorderOut = FromHex("#0B4956");
            ButtonSelectedBorderIn = FromHex("#3A8DB5");
            ButtonSelectedGlossyNorth = FromHex("#30B4E4");
            ButtonSelectedGlossySouth = FromHex("#30B4E4");
            ButtonPressedBgOut = FromHex("#0B4956");
            ButtonPressedBgCenter = FromHex("#0B4956");
            ButtonPressedBorderOut = FromHex("#3A8DB5");
            ButtonPressedBorderIn = FromHex("#3A8DB5");
            ButtonPressedGlossyNorth = FromHex("#0B4956");
            ButtonPressedGlossySouth = FromHex("#0B4956");
            ButtonCheckedBgOut = FromHex("#0B4956");
            ButtonCheckedBgCenter = FromHex("#0B4956");
            ButtonCheckedBorderOut = FromHex("#3A8DB5");
            ButtonCheckedBorderIn = FromHex("#3A8DB5");
            ButtonCheckedGlossyNorth = FromHex("#0B4956");
            ButtonCheckedGlossySouth = FromHex("#0B4956");
            ButtonCheckedSelectedBgOut = FromHex("#0B4956");
            ButtonCheckedSelectedBgCenter = FromHex("#0B4956");
            ButtonCheckedSelectedBorderOut = FromHex("#3A8DB5");
            ButtonCheckedSelectedBorderIn = FromHex("#3A8DB5");
            ButtonCheckedSelectedGlossyNorth = FromHex("#0B4956");
            ButtonCheckedSelectedGlossySouth = FromHex("#0B4956");
            ItemGroupOuterBorder = FromHex("#282828");
            ItemGroupInnerBorder = FromHex("#FFFFFF");
            ItemGroupSeparatorLight = FromHex("#FFFFFF");
            ItemGroupSeparatorDark = FromHex("#9EBAE1");
            ItemGroupBgNorth = FromHex("#C5C5C5");
            ItemGroupBgSouth = FromHex("#C5C5C5");
            ItemGroupBgGlossy = FromHex("#C5C5C5");
            ButtonListBorder = FromHex("#F1F3F5");
            ButtonListBg = FromHex("#484848");
            ButtonListBgSelected = FromHex("#757575");
            DropDownBg = FromHex("#8D8D8D");
            DropDownImageBg = FromHex("#E9EEEE");
            DropDownImageSeparator = FromHex("#C5C5C5");
            DropDownBorder = FromHex("#868686");
            DropDownGripNorth = FromHex("#FFFFFF");
            DropDownGripSouth = FromHex("#C5C5C5");
            DropDownGripBorder = FromHex("#C5C5C5");
            DropDownGripDark = FromHex("#484848");
            DropDownGripLight = FromHex("#C5C5C5");
            DropDownCheckedButtonGlyphBg = FromHex("#FCF1C2");
            DropDownCheckedButtonGlyphBorder = FromHex("#F29536");

            SeparatorLight = FromHex("#FAFBFD");
            SeparatorDark = FromHex("#484848");
            QATSeparatorLight = FromHex("#FAFBFD");
            QATSeparatorDark = FromHex("#484848");
            SeparatorBg = FromHex("#C5C5C5");
            SeparatorLine = FromHex("#C5C5C5");

            TextBoxUnselectedBg = FromHex("#C5C5C5");
            TextBoxBorder = FromHex("#FFFFFF");
            ToolTipContentNorth = FromHex("#B7B7B7");
            ToolTipContentSouth = FromHex("#B7B7B7");
            ToolTipDarkBorder = FromHex("#484848");
            ToolTipLightBorder = FromHex("#484848");
            ToolStripItemTextPressed = FromHex("#262626");
            ToolStripItemTextSelected = FromHex("#262626");
            ToolStripItemText = FromHex("#FFFFFF");
            clrVerBG_Shadow = FromHex("#FFFFFF");
            ButtonChecked_2013 = FromHex("#92C0E0");
            ButtonPressed_2013 = FromHex("#2A8AD4");
            ButtonSelected_2013 = FromHex("#92C0E0");
            OrbButton_2013 = FromHex("#333333");
            OrbButtonSelected_2013 = FromHex("#2A8AD4");
            OrbButtonPressed_2013 = FromHex("#2A8AD4");
            TabText_2013 = FromHex("#0072C6");
            TabTextSelected_2013 = FromHex("#FFFFFF");
            PanelBorder_2013 = FromHex("#15428B");
            RibbonBackground_2013 = FromHex("#484848");
            TabCompleteBackground_2013 = FromHex("#F3F3F3");
            TabNormalBackground_2013 = FromHex("#484848");
            TabActiveBackbround_2013 = FromHex("#F3F3F3");
            TabBorder_2013 = FromHex("#ABABAB");
            TabCompleteBorder_2013 = FromHex("#ABABAB");
            TabActiveBorder_2013 = FromHex("#ABABAB");
            OrbButtonText_2013 = FromHex("#FFFFFF");
            PanelText_2013 = FromHex("#262626");
            RibbonItemText_2013 = FromHex("#262626");
            ToolTipText_2013 = FromHex("#262626");
            ToolStripItemTextPressed_2013 = FromHex("#262626");
            ToolStripItemTextSelected_2013 = FromHex("#262626");
            ToolStripItemText_2013 = FromHex("#FFFFFF");
        }
    }
}