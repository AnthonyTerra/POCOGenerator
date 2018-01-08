using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace POCOGenerator.Extensions
{
    public static class TreeViewExtensions
    {
        #region Hide CheckBox

        // constants used to hide a checkbox
        public const int TVIF_STATE = 0x8;
        public const int TVIS_STATEIMAGEMASK = 0xF000;
        public const int TV_FIRST = 0x1100;
        public const int TVM_SETITEM = TV_FIRST + 63;

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        // struct used to set node properties
        public struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public String lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;
        }

        public static void HideCheckBox(this TreeView treeView, TreeNode node)
        {
            TVITEM tvi = new TVITEM();
            tvi.hItem = node.Handle;
            tvi.mask = TVIF_STATE;
            tvi.stateMask = TVIS_STATEIMAGEMASK;
            tvi.state = 0;
            IntPtr lparam = Marshal.AllocHGlobal(Marshal.SizeOf(tvi));
            Marshal.StructureToPtr(tvi, lparam, false);
            SendMessage(treeView.Handle, TVM_SETITEM, IntPtr.Zero, lparam);
        }

        #endregion

        #region Add Sorted

        public static int CompareTo(this TreeNode node1, TreeNode node2)
        {
            if (node1 == null && node2 == null)
                return 0;

            if (node1 == null && node2 != null)
                return -1;

            if (node1 != null && node2 == null)
                return 1;

            return node1.ToString().CompareTo(node2.ToString());
        }

        public static void AddSorted(this TreeNodeCollection nodes, TreeNode node)
        {
            if (nodes.Count == 0)
            {
                nodes.Add(node);
                return;
            }

            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                TreeNode node2 = nodes[i];
                int result = node.CompareTo(node2);
                if (result >= 0)
                {
                    nodes.Insert(i + 1, node);
                    return;
                }
            }

            nodes.Insert(0, node);
        }

        #endregion
    }
}
