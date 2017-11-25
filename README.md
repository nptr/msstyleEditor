# msstyleEditor [![Latest Release](https://img.shields.io/github/release/nptr/msstyleEditor.svg)](https://github.com/nptr/msstyleEditor/releases/latest) [![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://opensource.org/licenses/MIT)

The msstyleEditor is a simple editor for Visual Styles (.msstyles Files). It allows 
you to change visual styles without using a hex editor or a PE resource tool.
It shows all components, can modify the majority of properties and extract and replace
images, but it currently cannot create or delete entries or create styles from scratch.

## Features:
+ Loading of existing visual styles
+ Grouping of the respective components
+ Editing most properties (Colors, Sizes, Enums, ...)
+ Exporting images *
+ Replacing images *
+ Saving the changes
+ Searching for properties


*Only PNGs are supported!

## Compatibility:
Basic compatibility with Visual Styles for Windows 7, 8, 8.1 and 10, meaning that open, modify and save works.
Take note that a few part names are not labeled since their purpose is unknown and
documented nowhere. Also some parts might be labeled wrong in Win10 because of changes in the part enumeration (eg. in DWMWindow), and my lack of data for it.



Successfully tested with Visual Styles 
If you encounter any problems, please feel free to report them.

## Installation:
The application is a single, portable executeable. It requires no installation.

In order to run the application, the [Microsoft Visual C++ Redistributable 2013](https://www.microsoft.com/en-US/download/details.aspx?id=40784)
is required, but it may have been installed along with your OS already.

## Usage:

A basic understanding of the logical structure of visual styles is helpful when using this program. Fortunately its quite simple as can be seen in the section below.

#### Logical Structure of a Visual Style
+ Class 1 (e.g. Button, Window)
    + Part 1 (e.g. Pushbutton, Left Frame)
        + State 1 (e.g. Pressed, Disabled)
        +  State 2
            + Property 1 (e.g. BackgroundColor, Margins)
            + Property 2
+ Class 2
    + Part 1
    + Part 2
+ Class 3

#### User Interface Description

![Ui of the msstyleEditor](https://user-images.githubusercontent.com/5485569/33234570-7ff10f9a-d229-11e7-997a-cbce2f3f1770.png)

In the treeview on the left, the classes, parts and images are listed. On selection
of an image, it is shown in the middle area. With a right-click on the image view a way to change
the background is provided. Export and replace of the selected image can be done via the menubar.

On selection of a part, its properties are shown on the right side, grouped by 
their states. This is also the place where the properties can be edited.

After the changes are done, the style can be saved via the menubar as well.
It is recommended to save often, and to a new file, since there is no undo/redo functionality.
Also remember to backup your original style and not to work in the "Themes" directory directly.
