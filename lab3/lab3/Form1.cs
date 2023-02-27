using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace lab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox2.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;
            PopulateTreeView();
        }

        //окна 1 и 2 -----------------------------------------------------------------
        private void PopulateTreeView()
        {
            TreeNode rootNode;
            DirectoryInfo info = new DirectoryInfo("C:\\Users\\1\\test");
            if (info.Exists)
            {
                rootNode = new TreeNode(info.Name);
                rootNode.Tag = info;
                GetDirectories(info.GetDirectories(), rootNode);
                treeView1.Nodes.Add(rootNode);
            }
        }

        private void GetDirectories(DirectoryInfo[] subDirs, TreeNode nodeToAddTo)
        {
            TreeNode aNode;
            DirectoryInfo[] subSubDirs;
            foreach (DirectoryInfo subDir in subDirs)
            {
                aNode = new TreeNode(subDir.Name, 0, 0);
                aNode.Tag = subDir;
                aNode.ImageKey = "folder";
                subSubDirs = subDir.GetDirectories();
                if (subSubDirs.Length != 0)
                {
                    GetDirectories(subSubDirs, aNode);
                }
                nodeToAddTo.Nodes.Add(aNode);
            }
        }
        
        void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode newSelected = e.Node;
            listView1.Items.Clear();
            DirectoryInfo nodeDirInfo = (DirectoryInfo)newSelected.Tag;
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;

            foreach (DirectoryInfo dir in nodeDirInfo.GetDirectories())
            {
                item = new ListViewItem(dir.Name, 0);
                subItems = new ListViewItem.ListViewSubItem[]
                    {new ListViewItem.ListViewSubItem(item, "Directory"),
             new ListViewItem.ListViewSubItem(item,
                dir.LastAccessTime.ToShortDateString())};
                item.SubItems.AddRange(subItems);
                listView1.Items.Add(item);
            }
            foreach (FileInfo file in nodeDirInfo.GetFiles())
            {
                item = new ListViewItem(file.Name, 1);
                subItems = new ListViewItem.ListViewSubItem[]
                    { new ListViewItem.ListViewSubItem(item, "File"),
             new ListViewItem.ListViewSubItem(item,
                file.LastAccessTime.ToShortDateString())};

                item.SubItems.AddRange(subItems);
                listView1.Items.Add(item);
            }

            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        //----------------------------------------------------------------------------
        
        
        //кнопка добавить путь. добавляет путь к файлу или папке в зависимости от выбранного элемента в combobox1
        private void button2_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;
            var folderPath = string.Empty;
            if (comboBox2.SelectedIndex == 0)//file
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        filePath = openFileDialog.FileName;
                    }
                }
                listBox1.Items.Add(filePath);
                listBox1.SelectedItem = filePath;
            }
            else if (comboBox2.SelectedIndex == 1)//folder
            {
                DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    folderPath = folderBrowserDialog1.SelectedPath;
                }
                listBox1.Items.Add(folderPath);
                listBox1.SelectedItem = folderPath;
            }
        }
        //----------------------------------------------------------------------------

        //----------------------------------------------------------------------------
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1) //"rename",
            {
                textBox1.Text = "newname";
                MessageBox.Show("Введите в поле ввода новое имя файла или папки");
            } 
            else if (comboBox1.SelectedIndex == 2) //"copy",
            {
                textBox1.Text = Path.GetDirectoryName(listBox1.SelectedItem.ToString());
                MessageBox.Show("Введите в поле ввода путь к папке в которой нужно сохранить скопированный файл или папку");
            }
            else if (comboBox1.SelectedIndex == 3) //"move",
            {
                textBox1.Text = listBox1.SelectedItem.ToString();
                MessageBox.Show("Введите в поле ввода новый полный путь до файла или папки");
            }
            else if (comboBox1.SelectedIndex == 4) //create
            {
                textBox1.Text = "new_element.txt";
                MessageBox.Show("Введите в поле ввода имя файла с расширением. Файл создастся в выбранной папке или в папке выбранного файла");
            }
        }
        //----------------------------------------------------------------------------

        void CopyDir(string FromDir, string ToDir)
        {
            Directory.CreateDirectory(ToDir);
            foreach (string s1 in Directory.GetFiles(FromDir))
            {
                string s2 = ToDir + "\\" + Path.GetFileName(s1);
                File.Copy(s1, s2);
            }
            foreach (string s in Directory.GetDirectories(FromDir))
            {
                CopyDir(s, ToDir + "\\" + Path.GetFileName(s));
            }
        }



        //удаление файлов и папок
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int selind = listBox1.SelectedIndex;
                string Path = listBox1.Items[selind].ToString();
                if (comboBox2.SelectedIndex == 0)
                {
                    if (File.Exists($"{Path}"))
                    {
                        File.Delete($"{Path}");
                        MessageBox.Show("Файл удален");
                    }
                }
                else if (comboBox2.SelectedIndex == 1)
                {
                    if (Directory.Exists($"{Path}"))
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo($"{Path}");
                        foreach (FileInfo f in dirInfo.GetFiles())
                        {
                            f.Delete();
                        }
                        Directory.Delete($"{Path}");
                        MessageBox.Show("Папка удалена");
                    }
                }
                listView1.Items.Clear();
                treeView1.Nodes.Clear();
                PopulateTreeView();
            }
            catch
            {
                MessageBox.Show("Ошибка удаления");
            }

        }

        private void change_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1) //"rename"
            {
                try
                {
                    if (comboBox2.SelectedIndex == 0)
                    {
                        string directory = Path.GetDirectoryName(listBox1.SelectedItem.ToString());
                        directory += '\\';
                        directory += textBox1.Text;
                        directory += Path.GetExtension(listBox1.SelectedItem.ToString());
                        File.Move(listBox1.SelectedItem.ToString(), $"{directory}");
                        MessageBox.Show("Файл переименован");
                    }
                    else if (comboBox2.SelectedIndex == 1)
                    {
                        string directory = Path.GetDirectoryName(listBox1.SelectedItem.ToString());
                        directory += '\\';
                        directory += textBox1.Text;
                        directory += Path.GetExtension(listBox1.SelectedItem.ToString());
                        Directory.Move(listBox1.SelectedItem.ToString(), directory);
                        MessageBox.Show("Папка переименована");
                    }
                    comboBox1.SelectedIndex = 0;
                }
                catch
                {
                    MessageBox.Show("Ошибка переименования");
                }
            }
            else if (comboBox1.SelectedIndex == 2) //"copy",
            {
                try
                {
                    if (comboBox2.SelectedIndex == 0)
                    {
                        string name = textBox1.Text + '\\' + "copy_" +Path.GetFileName(listBox1.SelectedItem.ToString());
                        File.Copy(listBox1.SelectedItem.ToString(), name);
                        MessageBox.Show("Файл скопирован");
                    }
                    else if (comboBox2.SelectedIndex == 1)
                    {

                        string s = new DirectoryInfo(listBox1.SelectedItem.ToString()).Name;
                        string name = textBox1.Text + '\\' + "copy_" + s;
                        CopyDir(listBox1.SelectedItem.ToString(), name);
                        MessageBox.Show("Папка скопирована");
                    }
                    comboBox1.SelectedIndex = 0;
                }
                catch
                {
                    MessageBox.Show("Ошибка копирования");
                }
            }
            else if (comboBox1.SelectedIndex == 3) //"move",
            {
                try
                {
                    if (comboBox2.SelectedIndex == 0)
                    {
                        File.Move(listBox1.SelectedItem.ToString(), textBox1.Text);
                        MessageBox.Show("Файл перемещен");
                    }
                    else if (comboBox2.SelectedIndex == 1)
                    {
                        Directory.Move(listBox1.SelectedItem.ToString(), textBox1.Text);
                        MessageBox.Show("Папка перемещена");
                    }
                    comboBox1.SelectedIndex = 0;
                }
                catch
                {
                    MessageBox.Show("Ошибка перемещения");
                }
            }
            else if (comboBox1.SelectedIndex == 4) //create
            {
                try
                {
                    if (comboBox2.SelectedIndex == 0)
                    {
                        string directory = Path.GetDirectoryName(listBox1.SelectedItem.ToString());
                        directory += '\\';
                        directory += textBox1.Text;
                        File.Create(directory);
                        MessageBox.Show("Файл создан");
                    }
                    else if (comboBox2.SelectedIndex == 1)
                    {
                        string directory = listBox1.SelectedItem.ToString();
                        directory += '\\';
                        directory += textBox1.Text;
                        File.Create(directory);
                        MessageBox.Show("Файл создан");
                    }
                    comboBox1.SelectedIndex = 0;
                }
                catch
                {
                    MessageBox.Show("Ошибка создания");
                }
            }
            listView1.Items.Clear();
            treeView1.Nodes.Clear();
            PopulateTreeView();
        }
        //----------------------------------------------------------------------------
    }
}
