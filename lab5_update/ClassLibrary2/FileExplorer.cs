using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ClassLibrary2
{
    public class FileExplorer
    {
        public static void OpenFileExplorer(System.Windows.Forms.TreeView treeView1)
        {
            PopulateTreeView(treeView1);
        }

        public static TreeNode rootNode = new TreeNode();
        public static void PopulateTreeView(System.Windows.Forms.TreeView treeView1)
        {
            rootNode = new TreeNode();
            DirectoryInfo info = new DirectoryInfo("C:\\Users\\1\\Desktop");
            if (info.Exists)
            {
                rootNode = new TreeNode(info.Name);
                rootNode.Tag = info;
                GetDirectories(info.GetDirectories(), rootNode);
                treeView1.Nodes.Add(rootNode);
            }
        }

        public static void GetDirectories(DirectoryInfo[] subDirs, TreeNode nodeToAddTo)
        {
            TreeNode aNode;
            DirectoryInfo[] subSubDirs;
            foreach (DirectoryInfo subDir in subDirs)
            {
                aNode = new TreeNode(subDir.Name, 0, 0);
                aNode.Tag = subDir;
                aNode.ImageKey = "folder";
                subSubDirs = subDir.GetDirectories(); 
                nodeToAddTo.Nodes.Add(aNode);
                if (subSubDirs.Length != 0)
                {
                    GetDirectories(subSubDirs, aNode);
                        string[] allfiles = Directory.GetFiles(subDir.FullName);
                        foreach (string filename in allfiles)
                        {
                            nodeToAddTo.Nodes.Add(Path.GetFileName(filename));
                        }
                }
            }
        }
    }
}
