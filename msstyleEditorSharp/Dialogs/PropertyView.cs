using libmsstyle;
using msstyleEditor.PropView;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace msstyleEditor.Dialogs
{
    public partial class PropertyViewWindow : ToolWindow
    {
        private VisualStyle m_style;
        private StyleClass m_class;
        private StylePart m_part;

        private StyleState m_state;
        private StyleProperty m_prop;

        private AnimationTypeDescriptor m_SelectedAnimation;
        private Animation m_SelectedAnimationPart;

        private PropertyViewMode m_viewMode;

        public delegate void PropertyChangedHandler(object prop);
        public event PropertyChangedHandler OnPropertyAdded;
        public event PropertyChangedHandler OnPropertyRemoved;


        public PropertyViewWindow()
        {
            InitializeComponent();
        }

        public void SetAnimation(VisualStyle style, AnimationTypeDescriptor anim)
        {
            m_style = style;
            m_viewMode = PropertyViewMode.AnimationMode;
            newPropertyToolStripMenuItem.Enabled = false;
            deleteToolStripMenuItem.Enabled = false;

            propertyView.SelectedObject = anim;
        }

        public void SetTimingFunction(VisualStyle style, TimingFunction timing)
        {
            m_style = style;
            m_viewMode = PropertyViewMode.TimingFunction;
            newPropertyToolStripMenuItem.Enabled = false;
            deleteToolStripMenuItem.Enabled = false;

            propertyView.SelectedObject = timing;
        }

        public void SetStylePart(VisualStyle style, StyleClass cls, StylePart part)
        {
            m_viewMode = PropertyViewMode.ClassMode;
            newPropertyToolStripMenuItem.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;

            m_style = style;
            m_class = cls;
            m_part = part;

            propertyView.SelectedObject = (part != null)
                ? new TypeDescriptor(part, style)
                : null;
        }

        public void ShowPropertyAddDialog()
        {
            OnPropertyAdd(this, null);
        }

        public void RemoveSelectedProperty()
        {
            OnPropertyRemove(this, null);
        }


        private void OnPropertyAdd(object sender, EventArgs e)
        {
            if (m_style != null && m_class != null)
            {
                if (m_class.ClassName == "animations")
                {
                    var dlg2 = new NewAnimationDialog();
                    //typeid is the same for all animations
                    var anim = dlg2.ShowDialog(new PropertyHeader(20000, m_style.Animations[0].Header.typeID) {  classID = m_class.ClassId});
                    if (anim != null)
                    {
                        m_style.Animations.Add(anim);
                        if (OnPropertyAdded != null)
                        {
                            OnPropertyAdded(anim);
                        }
                        propertyView.Refresh();
                    }
                    return;
                }
            }
            if(m_style == null 
                || m_class == null 
                || m_part == null 
                || m_state == null)
            {
                MessageBox.Show("Select a state or property first!", "Add Property", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dlg = new PropertyDialog();
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.Text = "Add Property to " + m_state.StateName;
            var newProp = dlg.ShowDialog();
            if (newProp != null)
            {
                // Add the prop where requested. TODO: test this
                newProp.Header.classID = m_class.ClassId;
                newProp.Header.partID = m_part.PartId;
                newProp.Header.stateID = m_state.StateId;

                m_state.Properties.Add(newProp);
                if(OnPropertyAdded != null)
                {
                    OnPropertyAdded(newProp);
                }
                propertyView.Refresh();
            }
        }

        private void OnPropertyRemove(object sender, EventArgs e)
        {
            if (m_viewMode == PropertyViewMode.ClassMode)
            {
                if (m_state == null || m_prop == null)
                {
                    return;
                }

                m_state.Properties.Remove(m_prop);
                if (OnPropertyRemoved != null)
                {
                    OnPropertyRemoved(m_prop);
                }
                propertyView.Refresh();
            }
            else if (m_viewMode == PropertyViewMode.TimingFunction)
            {
                var func = (TimingFunction)propertyView.SelectedObject;
                if(func != null)
                {
                    m_style.TimingFunctions.Remove(func);
                    if (OnPropertyRemoved != null)
                    {
                        OnPropertyRemoved(m_prop);
                    }
                    propertyView.Refresh();
                }
            }
            else if (m_viewMode == PropertyViewMode.AnimationMode)
            {
                var anim = (AnimationTypeDescriptor)propertyView.SelectedObject;
                if(anim != null && m_SelectedAnimationPart == null)
                {
                    //remove all animations
                    foreach (var item in anim.Animations)
                    {
                        m_style.Animations.Remove(item);
                    }
                }
                if (m_SelectedAnimationPart != null)
                {
                    m_style.Animations.Remove(m_SelectedAnimationPart);
                }

                if (OnPropertyRemoved != null)
                {
                    if (m_prop != null)
                        OnPropertyRemoved(m_prop);
                    else if (m_SelectedAnimationPart != null)
                        OnPropertyRemoved(m_SelectedAnimationPart);
                }
                propertyView.Refresh();
            }
        }

        private void OnPropertySelected(object sender, SelectedGridItemChangedEventArgs e)
        {
            if (m_viewMode == PropertyViewMode.ClassMode)
            {
                const int CTX_ADD = 0;
                const int CTX_REM = 1;

                bool haveSelection = e.NewSelection != null;
                propViewContextMenu.Items[CTX_ADD].Enabled = haveSelection;
                propViewContextMenu.Items[CTX_REM].Enabled = haveSelection;
                if (!haveSelection)
                    return;

                var propDesc = e.NewSelection.PropertyDescriptor as StylePropertyDescriptor;
                if (propDesc != null)
                {
                    propViewContextMenu.Items[CTX_ADD].Text = "Add Property to [" + propDesc.Category + "]";
                    propViewContextMenu.Items[CTX_REM].Text = "Remove " + propDesc.Name;

                    m_state = propDesc.StyleState;
                    m_prop = propDesc.StyleProperty;
                    return;
                }

                var dummyDesc = e.NewSelection.PropertyDescriptor as PlaceHolderPropertyDescriptor;
                if (dummyDesc != null)
                {
                    propViewContextMenu.Items[CTX_ADD].Enabled = true;
                    propViewContextMenu.Items[CTX_ADD].Text = "Add Property to [" + dummyDesc.Category + "]";
                    propViewContextMenu.Items[CTX_REM].Enabled = false;
                    propViewContextMenu.Items[CTX_REM].Text = "Remove";
                    m_state = dummyDesc.StyleState;
                    return;
                }

                if (e.NewSelection.GridItemType == GridItemType.Category)
                {
                    OnPropertySelected(sender, new SelectedGridItemChangedEventArgs(null, e.NewSelection.GridItems[0])); // select child
                    return;
                }

                if (e.NewSelection.GridItemType == GridItemType.Property)
                {
                    OnPropertySelected(sender, new SelectedGridItemChangedEventArgs(null, e.NewSelection.Parent)); // select parent
                    return;
                }
            }
            else if (m_viewMode == PropertyViewMode.AnimationMode)
            {
                if (e.NewSelection == null)
                    return;

                var prop = e.NewSelection.PropertyDescriptor as AnimationPropertyDescriptior;
                if(prop != null)
                {
                    Debug.WriteLine("selected property descriptor");
                }

                if (e.NewSelection.GridItemType == GridItemType.Category)
                {
                    m_SelectedAnimationPart = (e.NewSelection.GridItems[0].PropertyDescriptor as AnimationPropertyDescriptior).m_animation;
                }

                if (e.NewSelection.GridItemType == GridItemType.Property)
                {
                    Debug.WriteLine("selected prop " + e.NewSelection.Parent.Label);
                }
            }
        }
    }

    public enum PropertyViewMode
    {
        ClassMode,
        TimingFunction,
        AnimationMode,
    }
}
