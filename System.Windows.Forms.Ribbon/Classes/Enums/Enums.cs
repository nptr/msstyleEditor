namespace System.Windows.Forms
{
    public enum RibbonTheme
    {
        /// <summary>
        /// The default theme is identical to the Blue theme.
        /// </summary>
        Normal,
        /// <summary>
        /// This theme is identical to the normal "default" theme.
        /// </summary>
        Blue,
        Blue_2010,
        Black,
        Green,
        Purple,
        JellyBelly,
        Halloween,
        VSLight,
        VSDark
    }

    public enum RibbonOrbStyle
    {
        Office_2007,
        Office_2010,
        Office_2010_Extended,
        Office_2013 //Michael Spradlin - 05/03/2013 Office 2013 Style Changes
    }

    /// <summary>
    /// Represents the directions that arrows can have
    /// </summary>
    public enum RibbonArrowDirection
    {
        /// <summary>
        /// The arrow points up
        /// </summary>
        Up,

        /// <summary>
        /// The arrow points down
        /// </summary>
        Down,

        /// <summary>
        /// The arrow points right
        /// </summary>
        Right,

        /// <summary>
        /// The arrow points left
        /// </summary>
        Left
    }

    public enum RibbonButtonStyle
    {
        /// <summary>
        /// Simple clickable button
        /// </summary>
        Normal,
        /// <summary>
        /// Button with a right side drop down
        /// </summary>
        DropDown,
        /// <summary>
        /// Button with an optional dropdown attachment on the right
        /// </summary>
        SplitDropDown,
        /// <summary>
        /// Mimics a standard drop down list item with no image
        /// </summary>
        DropDownListItem
    }

    /// <summary>
    /// Possible modes for the ribbon to be placed on the window
    /// </summary>
    public enum RibbonWindowMode
    {
        InsideWindow,

        NonClientAreaCustomDrawn,

        NonClientAreaGlass
    }

    /// <summary>
    /// Represents possible flow directions of items on the panels
    /// </summary>
    public enum RibbonPanelFlowDirection
    {
        /// <summary>
        /// Layout of items flows to the left, then down
        /// </summary>
        Left = 2,
        /// <summary>
        /// Layout of items flows to the Right, then down
        /// </summary>
        Right = 1,
        /// <summary>
        /// Layout of items flows to the bottom, then to the right
        /// </summary>
        Bottom = 0
    }

    /// <summary>
    /// Represents the size modes that a RibbonElement can be
    /// </summary>
    public enum RibbonElementSizeMode
    {
        /// <summary>
        /// The item is being shown on a dropdown
        /// </summary>
        DropDown = 5,

        /// <summary>
        /// Maximum size the element can reach
        /// </summary>
        Large = 4,

        /// <summary>
        /// A medium size for the element when not much space is available
        /// </summary>
        Medium = 3,

        /// <summary>
        /// The minimum size the element can be
        /// </summary>
        Compact = 2,

        /// <summary>
        /// The item doesn't fit as compact, so it must be shown as a single button
        /// </summary>
        Overflow = 1,

        /// <summary>
        /// No size mode specified
        /// </summary>
        None = 0
    }

    /// <summary>
    /// The width of the Separator bar when displayed on a drop down
    /// </summary>
    public enum RibbonSeparatorDropDownWidth
    {
        /// <summary>
        /// Full width to divide different controls
        /// </summary>
        Full = 1,

        /// <summary>
        /// Partial width to divide similar items
        /// </summary>
        Partial = 0
    }
}