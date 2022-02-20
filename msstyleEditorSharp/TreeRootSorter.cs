using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace msstyleEditor
{
    class TreeRootSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            var a = x as TreeNode;
            var b = y as TreeNode;
            
            // leave child nodes as-is and only sort
            // the root nodes.
            if (a.Parent != null && b.Parent != null)
                return a.Index - b.Index;
            else return string.Compare(a.Text, b.Text, true);
        }
    }
}
