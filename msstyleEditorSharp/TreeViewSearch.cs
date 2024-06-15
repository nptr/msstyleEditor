using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace msstyleEditor
{
    class TreeViewSearch
    {
        public static TreeNode FindNextNode(TreeView tree, TreeNode node, bool includeCurrentNode, Predicate<TreeNode> predicate)
        {
            TreeNode originalNode = node;
            while (node != null)
            {
                // skip the first node to not get stuck.
                if (node != originalNode || includeCurrentNode)
                {
                    if (predicate(node))
                        return node;
                }

                // find nodes: depth
                var nextNode = node.FirstNode;
                if (nextNode == null)
                {
                    // find nodes: breadth
                    nextNode = node.NextNode;
                    if (nextNode == null)
                    {
                        // back out until we find a sibling
                        nextNode = node.Parent;
                        while (nextNode != null && nextNode != tree.Nodes[0] && nextNode.NextNode == null)
                        {
                            nextNode = nextNode.Parent;
                        }

                        //// root has no sibling.
                        //if (nextNode == classView.Nodes[0])
                        //    return null; // done


                        if (nextNode != null)
                            nextNode = nextNode.NextNode;
                        //else: parent was not ok because we reached the root node -> no nodes left anymore -> search done
                    }
                }

                node = nextNode;
            }

            return null; // nothing found
        }
    }
}
