﻿using libmsstyle;
using msstyleEditor.PropView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace msstyleEditor.Dialogs
{
    public partial class ClassViewWindow : ToolWindow
    {
        private VisualStyle m_style = null;

        public event TreeViewEventHandler OnSelectionChanged;

        public ClassViewWindow()
        {
            InitializeComponent();
            classView.TreeViewNodeSorter = new TreeRootSorter();
        }

        public void SetVisualStyle(VisualStyle style)
        {
            m_style = style;

            classView.BeginUpdate();
            if (m_style == null)
            {
                classView.Nodes.Clear();
            }
            else
            {
                foreach (var cls in m_style.Classes)
                {
                    var clsNode = new TreeNode(cls.Value.ClassName);
                    clsNode.Tag = cls.Value;
                    if (cls.Value.ClassName == "timingfunction")
                    {
                        foreach (TimingFunction timingFunction in m_style.TimingFunctions)
                        {
                            string name = VisualStyleAnimations.FindTimingFuncName(timingFunction.Header.partID);
                            if(name == null)
                            {
                                name = "Unknown part:" + timingFunction.Header.partID;
                            }

                            var partNode = new TreeNode(name);
                            partNode.Tag = timingFunction;
                            clsNode.Nodes.Add(partNode);
                        }
                    }
                    else if (cls.Value.ClassName == "animations")
                    {
                        // Contains <PartID, partid node>
                        Dictionary<int, TreeNode> nodes = new Dictionary<int, TreeNode>();
                        foreach (Animation animation in m_style.Animations)
                        {
                            var anim = VisualStyleAnimations.FindAnimStates(animation.Header.partID);

                            TreeNode partNode;
                            bool exists = nodes.TryGetValue(animation.Header.partID, out partNode);
                            if(!exists)
                            {
                                // Check if the animations name is known
                                if (anim != null)
                                {
                                    partNode = new TreeNode(anim.AnimationName);
                                }
                                else
                                {
                                    partNode = new TreeNode("Unknown part " + animation.Header.partID);
                                }
                                partNode.Tag = new AnimationTypeDescriptor(animation);
                            }

                            // Find the animation states name
                            string stateName;
                            if(anim != null)
                            {
                                if(!anim.AnimationStateDict.TryGetValue(animation.Header.stateID, out stateName))
                                {
                                    stateName = "Unknown state: " + animation.Header.stateID;
                                }
                            }
                            else
                            {
                                stateName = "Unknown state: " + animation.Header.stateID;
                            }

                            //add the new state if there is another state
                            if (exists)
                            {
                                ((AnimationTypeDescriptor)partNode.Tag).AddState(animation);
                            }

                            // Add the part node if it wasn't added
                            if (!exists)
                            {
                                nodes.Add(animation.Header.partID, partNode);
                                clsNode.Nodes.Add(partNode);
                            }

                        }
                    }
                    else
                    {
                        foreach (var part in cls.Value.Parts)
                        {
                            var partNode = new TreeNode(part.Value.PartName);
                            partNode.Tag = part.Value;

                            clsNode.Nodes.Add(partNode);
                        }
                    }

                    classView.Nodes.Add(clsNode);
                }
                classView.Sort();
            }
            classView.EndUpdate();
        }

        bool m_endReached = false;
        public void FindNextNode(SearchDialog.SearchMode mode, IDENTIFIER type, string searchString, object searchObject)
        {
            var startItem = classView.SelectedNode;
            if (startItem == null || m_endReached)
            {
                m_endReached = false;
                if (classView.Nodes.Count > 0)
                {
                    startItem = classView.Nodes[0];
                }
                else return;
            }

            var node = TreeViewSearch.FindNextNode(classView, startItem, (_node) =>
            {
                switch (mode)
                {
                    case SearchDialog.SearchMode.Name:
                        return _node.Text.ToUpper().Contains(searchString.ToUpper());
                    case SearchDialog.SearchMode.Property:
                        {
                            StylePart part = _node.Tag as StylePart;
                            if (part != null)
                            {
                                return part.States.Any((kvp) =>
                                {
                                    return kvp.Value.Properties.Any((p) =>
                                    {
                                        return p.Header.typeID == (int)type &&
                                               p.GetValue().Equals(searchObject);
                                    });
                                });
                            }
                        }
                        return false;
                    default: return false;
                }
            });

            if (node != null)
            {
                classView.SelectedNode = node;
            }
            else
            {
                MessageBox.Show($"No further match for \"{searchString}\" !\nSearch will begin from top again.", ""
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Information);
                m_endReached = true;
            }
        }

        public void ExpandAll()
        {
            classView.ExpandAll();
        }

        public void CollapseAll()
        {
            classView.CollapseAll();
        }

        private void OnTreeItemSelected(object sender, TreeViewEventArgs e)
        {
            if (OnSelectionChanged != null)
            {
                OnSelectionChanged(sender, e);
            }
        }
    }
}
