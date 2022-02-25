**********************************
5.0.1.0 (31 May 2019)
**********************************
64 Bit Windows issues
Windows 10 issues (Border)
Systemmenu for RibbonForm
Projectfiles for .NET2 and .NET4
signed Assembly
SetupProjects (Installing to GAC, Global Assembly Cache) with Wix Toolset VisualStudio Extension

**********************************
3.5.7.0 (10 May 2013)
**********************************
Added: items of the DropDownButton in the QuickAccessToolBar can be set by VS Designer "QuickAcessToolbar.DropDownButtonItems" (toAtWork)
Added: MDI RibbonForm demo (toAtWork)

Fixed bug: compiles again in .net 2.0 (toAtWork)
Fixed bug: if a RibbonItem with DropDownItems is within a RibbonItemGroup the DropDown is immediately closed after pop up (toAtWork)
Fixed bug: layout of RibbonForm (Aero only) is wrong if Form contains any controls with DockStyle.Fill or DockStyle.Top (toAtWork)
Fixed bug: reduce CPU load when mouse move in caption bar of RibbonForm (no Aero) (toAtWork)
 * use border less form, so there is no constant repainting of caption bar necessary
 * that removes also the occasional flicker of the standard windows control (minimize, maximize and close buttons) box and caption bar
Fixed bug: text by DrawTextOnGlass is persistent now - use it to draw form title (toAtWork)
Fixed bug: display scaling does not work for CheckBox, RadioButton and caption bar buttons if DWM is disabled (toAtWork)
Fixed bug: (hack) get rid of the black caption bar when form is maximized on multi-monitor setups with DWM enabled (toAtWork)
Fixed bug: after restore the window increased in size (width and height) (toAtWork)
Fixed bug: wrong size calculation of RibbonCheckBox can cause texts to be cut off (toAtWork)
Fixed bug: tool tips in dropdown menus were not visible but after clicking the item this tool tip is shown for all items (toAtWork)
Fixed bug: get rid of the gray form border when using RibbonForm as MDIParent (toAtWork)


**********************************
3.5.6.1 (24 Feb 2013)
**********************************
Added: Set SizeMode of RibbonQuickAccessToolbar items to "Compact", to correctly display "SplitDropDown" and "DropDown" (toAtWork)
Added: Removed DisplayName Attribute from "Flash" (RibbonItem + RibbonButton) properties because in Property Grid 2 properties with identical name are shown (toAtWork)

Fixed bug: Designer throws exception if an item is added via CollectionEditor and shown before the Form is closed and re-opened. CollectionEditor uses IList interface for add and removal of items - not the RibbonItemCollection specific Add. Therefore the Owner is null (toAtWork)
Fixed bug: Designer throws Exception (toAtWork)
 * Dispose of Ribbon iterates through tabs and disposes the first tab. RibbonDesigner registered tab disposal and removed it from the Ribbon Tabs collection. Dispose of Ribbon wants to iterate next tab => CollectionModifiedException.
 * Dispose of images in DesignMode caused InvalidParameterException.


**********************************
3.5.6 (19 Feb 2013)
**********************************
Added: Rework and unify ToolTip behavior in RibbonItem and RibbonTab (toAtWork)
Added: Set TypeConverter to enable setting of the Tag property by Designer (toAtWork)
Added: Automatic set the width of OrbDropDownItems (toAtWork)
Added: Minimum and Maximum size for RibbonButton (kcarbis)

Fixed bug: Cleanup memory usage. Dispose resources and unregister events. (toAtWork)
Fixed bug: Modal dialogs in XP don't process any user interaction (can't even be closed) (toAtWork)
Fixed bug: SplitDropDown in QuickAccessToolbar (toAtWork)
Fixed bug: Multi-Monitor bugs (RibbonForm) (toAtWork)
Fixed bug: Maximized WindowState in XP causes control to be drawn partially only if RibbonForm contains controls with DockStyle.Fill (toAtWork)
Fixed bug: SpinButton click causes the underlying text to be changed but not the displayed text (kcarbis)
Fixed bug: Completely rework global hooks, caused freeze on XP (toAtWork)
Fixed bug: Close popups when application is changed by ALT+TAB (toAtWork)
Fixed bug: Ribbon minimized caused the layout to be completely wrong (toAtWork)
Fixed bug: clear the selected item (after click) (toAtWork)


**********************************
3.5.5 (13 Jan 2013)
**********************************
Added Read/Write Theme File Feature. Ribbon can load a theme file to change its color. (adriancs)
> Related Files: RibbonColorPart.cs, RibbonProfesionalRendererColorTable.cs

Fixed bug: TextBox and CheckBox not displaying Cursor.Hand properly when mouse cursor enter bound of "CheckArea". (kcarbis)
> Related Files: Ribbon.cs
Fixed bug: If there is only 1 RibbonTab, TextBox will automatically lost focus after a character key in. (kcarbis)
> Related Files: Ribbon.cs

**********************************
3.5.4
**********************************
Added SelectedItem and SelectedValue feature to the RibbonButton.  This is identical to the RibbonDropDown logic added earlier so you can track DropDownItems as they are clicked.
Added CheckGroup ability to the RibbonButton.  Now Buttons will Toggle State when in a panel or a Popup window.
Added bottom border when Minimized. This closer resembles the Office Ribbon.

Fixed bug in RibbonDropDown when a dropdown in nested in another dropdown.  The dropdown list was incorrectly positioned on the screen.
Fixed Tooltip border so its darker and also the positioning was a pixel off so I fixed that.


**********************************
version 3.5.3
**********************************
Added new feature for Office 2010 style Orb
Added Tooltips
Added Minimize feature


Added CheckGroup for unchecking items automatically to simulate a option group of buttons
Fixed cursor issue when mousing off of an active text box that is in edit mode.


**********************************
verison 3.5.2
**********************************
Fixed focus problem with textboxes that are in edit mode and you click on a different control.  The edit box was still visible even after the focus was gone.
Fixed bug with SelectedItem property so the selected item is set before raising the DropDownItemClicked event.
Fixed bug in RibbonButtonList when in a dropdown window.  When you tried to scroll by clicking the scroll buttons it would close the dropdown window.
Fixed bug in ribbon orb so it clears the highlight when you mouse off the orb.
Fixed bug in RibbonButtonList scrolling logic. If the ButtonList was not the first item in a DropDown the scrolling was incorrectly calculated and you couldn't scroll down all the way.

Added new property to combobox called SelectedValue. This returns the value field of the selected item. Most importantly the setter will set the select item in the dropdown list that contains the value supplied.  This along with the selected item makes this control function almost identical to the standard dropdown control.
Added new property PanelCaptionHeight to allow you to manually set the height of the panels caption area.
Added new events ButtonItemClicked and DropDownItemClicked to the RibbonButtonList control so you can track when an item in the list is clicked.
Added scrolling ability to the RibbonDropDown.  I ran into a case where I had a lot of items in the list and the popup window would extend beyond the bottom of the screen.  I couldn't use a RibbonButtonList because I needed separators and such which the RibbonButtonList doesn't support.  I figured it was worth the time since this control is used by other controls.

**********************************
verison 3.5.2
**********************************

Bugs...
Fixed numerous bugs with the visibility property for items and panels. The panel had painting issues when resized and some of the items were hidden.
Fixed painting issue in quickaccesstoolbar when set to RightToLeft
Fixed Maximize/Restore button so it work correctly when in Form Mode.  This is still not working but a workaround has been found.
Fixed cursor issue in checkbox and combobox controls.
Fixed QuickAccessToolbar Adorner so it is not visible if the QuickAccessToolbar is hidden.
Fixed orb drowdown menu so it displays correctly when set to RightToLeft
Fixed Orb Menu so its dipsplayed correctly when in RTL mode.  Still need to fix Adorners and sub menus
Fixed RibbonPanel so it doesn't change the visibility of child items. This caused problems if one of the items was invisible and then the panel is shown.  Now done in the SetBounds
Fixed Ribbon Orb double click so you can capture the double-click on the orb.  Thanks to Steve Towner for helping resolve this.
Fixed bug in ComboBox so now the dropdown button clears the highlighting when the dropdown is closed.
Fixed ItemClick event so now the combobox will update the text when an item is selected in the dropdown.  This event was not getting fired due to the Dropdown window being dismissed in the MouseDown event and not in the Click event.
Fixed Painting issue with RibbonComboBox so the arrow clears the highlight when the dropdown window is dismissed.
Fixed RibbonPanel centering logic. The panel would incorrectly calculate the dimentions and therefore items wouldn't appear centered vertically.  Thanks to Rod for finding this.

New Stuff...
Added CaptionBarVisible property so you can hide the entire caption area including the Orb and QuickAccessToolbar to simulate a simple tab control.
Added new RibbonLabel item so you can place a simple text object anywhere.
Added TextAlignment property to the label portion of controls so you can set the text alignment(left,center,right)
Added new Host Item.  Now you can place any control in the ribbon.  Care must be taken on its size thought if it goes directly in a panel.
Added DrawBackground property to RibbonSeparator so you can hide the lines and use it as a flow control.
Added new DropDownItemClicked event to the ComboBox so you can tell when one of the items is clicked in the dropdown list.  This eliminates the need to catch the click event on each item in the dropdown individually.
Added SelectedItem property to RibbonComboBox so you can access the last selected item in the dropdown.

**********************************
version 3.5.1
**********************************

1. Added new RadioButton Item.  This is achieved with the RibbonCheckBox control.  I added a new Style property to specify which control to draw. I also added a new event called CheckBoxCheckChanging.  This allows you to cancel the change event if you need to.
2. Cleaned up the Visible property so now items stay visible when in design mode but will be invisible at run-time.  This makes designing easier.  Also found a few bugs here and there that caused painting issues.
3. Added new RightToLeft ability.  This is a native property of the control object but was not implemented in the ribbon control.
4. I removed the mousewheel support for the tabs.  Not only was this annoying but it had problems when trying to scroll on a modal form that was on top of the tabs.
5. Added Validating and Validated events to the TextBox and ComboBox controls so you can handle data validation.




**********************************
Version 3.5.0
**********************************

1. Added Visible property to Tabs, Panels, and Items.
2. Fixed Visible property on quick access toolbar.  I removed the QuickAccessVisible property.  Now you can use the QuickAccessToolbar.Visible property instead.
3. Created a new Checkbox Item.  Includes Orientation property so you can put the checkbox on the left or right side
4. Created a new Up/Down (spin) control. UpButtonPressed and DownButtonPressed events are fired so you can respond accordingly.
5. Added new property called LabelWidth to all controls with a label so you can right justify the labels and align controls vertically.
6. Added all the Ribbon Items to the Panel's collection designer.
7. Fixed various designer bugs in some of the controls.  You still need to close the form and reopen it to see your changes.  This only happens when adding a new control in the designer.
8. Added new Button Style called DropDownListItem so Button Items added to the DropDown can mimic a standard dropdown without an image and be left aligned.
9. Added new property called DrawIconsBar to Dropdown Item so you can hide the gray bar on the left and simulate an indented list item.
10. Added AllowTextEdit property to the textbox and combobox controls so you can prevent users from editing the textbox at run time.
11. Ribbon now has adjustable height.  It used to be fixed size at 136px. Unfortunately, different fonts would cause the button text to get cropped off so I need to be able to tweak the height so buttons with 2 rows of text would be visible.  Just change the height in the Size property.  No dragging allowed.
