# msstyleEditor [![Latest Release](https://img.shields.io/github/release/nptr/msstyleEditor.svg)](https://github.com/nptr/msstyleEditor/releases/latest) [![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://opensource.org/licenses/MIT)

The msstyleEditor is an editor for Visual Styles (.msstyles Files). 
It allows you to change visual styles without using a hex editor or a PE resource tool.
It lists all components, can add/remove/modify the majority of properties, as well as extract and replace
images.

## Features
+ Loading of existing visual styles
+ Listing all components
+ Adding/Removing most properties
+ Editing most properties (Colors, Sizes, Enums, ...)
+ Exporting images *
+ Replacing images *
+ Saving the changes
+ Searching for properties
+ Applying the currently loaded style

*Only PNGs are supported!

## Compatibility
Basic compatibility with Visual Styles for Windows 7, 8, 8.1 and 10, meaning that open, modify and save works. Take note that a few part names are not labeled since their purpose is unknown and documented nowhere. Also some parts might be labeled wrong in Win10 because of changes in the part enumeration (eg. in DWMWindow), and my lack of data for it.

If you encounter any problems, please feel free to report them.

## Installation
The application is a single, portable executeable. It requires no installation.
Just put it anywhere and run it.

Note: Versions prior to 1.3.0.0 required the [Microsoft Visual C++ Redistributable 2013](https://www.microsoft.com/en-US/download/details.aspx?id=40784) installed.

## Visual Style Structure

Understanding of the logical structure of visual styles is helpful when using this program. Fortunately its quite simple, so the figure below should give you the basic understanding required.
```bash
├─ Class 1 (e.g. Button, Window)
│   └─ Part 1 (e.g. Pushbutton, Left Frame)
│      ├─ State 1 (e.g. Pressed, Disabled)
│      └─ State 2
│          ├─ Property 1 (e.g. BackgroundColor, Margins)
│          └─ Property 2
└─ Class 2
│    ├─ Part 1
│    └─ Part 2
└─ Class 3
```

You will also encounter classes with :: in their names, such as `BrowserTab::Tab`. This means that the
`BrowserTab` class inherits (reuses) the parts, properties and states of `Tab`, and just defines a few that override existing ones or are an addition.
With this concept, duplication of properties is minimized and further provides a single place for common properties.

## User Interface Description

![Ui of the msstyleEditor](https://user-images.githubusercontent.com/5485569/39672137-b5960a2c-5124-11e8-9c96-18f5dc17b795.png)

In the treeview on the left, the classes, parts and images are listed. On selection
of an image, it is shown in the middle area. With a right-click on the image view a way to change
the background is provided. Export and replace of the selected image can be done via the menubar.

On selection of a part, its properties are shown on the right side, grouped by 
their states. This is also the place where the properties can be added, removed and edited.
The search function is invoked via CTRL+F.

Saving the style is done via the menubar.
It is recommended to save often, and to a new file, since there is no undo/redo functionality.
Also remember to backup your original style and not to work in the "Themes" directory directly.
