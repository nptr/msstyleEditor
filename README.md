# msstyleEditor

The msstyleEditor is a simple editor for Visual Styles (.msstyles Files). It allows 
you to change visual styles without using a hex editor or a PE resource tool.
It can modify existing styles. Creating new ones from scratch is not possible yet.

## Features:
+ Load an existing visual style
+ Show the classes the style is made of
+ Edit most properties (Colors, Sizes, Enums, ...)
+ Export images *
+ Replace images *
+ Save the changes
+ Search function


*Only PNGs are supported!

## Compatibility:
+ Successfully tested with Visual Styles for Windows 7, 8, 8.1 and 10.

## Installation:
The application is a single, portable executeable. It requires no
installation.

In order to run the application, the [Microsoft Visual C++ Redistributable 2013](https://www.microsoft.com/en-US/download/details.aspx?id=40784)
is required, but it might already be installed on your system.

## Usage:
#### Logical Structure of a Visual Style
+ Class 1 (e.g. Button, Window)
    + Part 1 (e.g. Pushbutton, Left Frame)
    + Part 2
        + State 1 (e.g. Pressed, Disabled)
        +  State 2
            + Property 1 (e.g. BackgroundColor, Margins)
            + Property 2
+ Class 2
+ Class 3

#### UI Description

![Ui of the msstyleEditor](https://user-images.githubusercontent.com/5485569/29291552-3eb8c2ba-8144-11e7-8e12-ead476ed3e00.png)

In the treeview on the left, the classes, parts and images are listed. On selection
of an image, it is shown in the middle area. Export and replace of the selected 
image can be done via the menubar.

On selection of a part, its properties are shown on the right side, grouped by 
their states. This is also the place where the properties can be edited.
