# msstyleEditor [![Latest Release](https://img.shields.io/github/release/nptr/msstyleEditor.svg)](https://github.com/nptr/msstyleEditor/releases/latest) [![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://opensource.org/licenses/MIT)

The msstyleEditor is an editor for Windows Vista, 7, 8, 10 and 11 visual styles (.msstyles files). It allows you to change visual styles without using a hex editor or a PE resource tool. It lists all components, can add/remove/modify the majority of properties, as well as extract and replace images.


## Installation
The application is a single, portable executeable. It needs no installation but requires .NET Framework 4.5 on your PC. That comes preinstalled with Windows 8+, and likely exists on your Windows 7 already as well.

In order to apply the visual styles, your Windows must also be patched with one of the following tools, because Microsoft locks-out third party themes.
* SecureUxTheme: https://github.com/namazso/SecureUxTheme/releases
* UltraUXThemePatcher: https://mhoefs.eu/software_uxtheme.php


## Limitations
* can't create styles from scratch
* can't add new fonts
* no preview of items in image atlases
* supports only .png ' s

No abstraction over the internals is done. Things are named and structured as they were by the developers at Microsoft. It may be a bit puzzling at first to figure out what each class/part/property effects. The following chapter might help a bit though.

## Visual Style Structure

Understanding of the logical structure of visual styles is helpful when using this program. Fortunately its quite simple, so the figure below should give you the basic understanding required.
```bash
├─ Class 1 (e.g. Button, Window)
│   └─ Part 1 (e.g. Pushbutton, Left Frame)
│      ├─ State 1 (e.g. Pressed, Disabled)
│      └─ State 2
│          ├─ Property 1 (e.g. BackgroundColor, Margins)
│          └─ Property 2
├─ Class 2
│    ├─ Part 1
│    └─ Part 2
└─ Class 3
```

You will also encounter classes with :: in their names, such as `BrowserTab::Tab`. This means that the
`BrowserTab` class inherits (reuses) the parts, properties and states of `Tab`, and just defines a few that override existing ones or are an addition. With this concept, duplication of properties is minimized and further provides a single place for common properties.

## User Interface Description

![Ui of the msstyleEditor](https://user-images.githubusercontent.com/5485569/154944873-e6f03424-6c8f-4114-8f15-6f1af7343f1b.png)

In the treeview on the left, the classes, parts and images are listed. On selection of an image, it is shown in the middle area. Right-click the image area to change the background if images are barely visible. Export and replace of the currently visible image can be done via the menu bar.

On selection of a part, its properties are shown on the right side, grouped by their states. This is also the place where the properties can be added, removed and edited. The search function is invoked via CTRL+F; it allows to search for classes, parts and properties with specific values.

Saving the style is done via the menubar. It is recommended to save often, and to a new file, since there is no undo/redo functionality. Also remember to backup your original style and don't work in the "Themes" directory directly (probably no write permission anyways, but still).

## Contributers
* [Shomnipotence](https://github.com/Shomnipotence) - New application icon design