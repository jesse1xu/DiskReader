using DiskReadLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiskReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ArrayList logicalList = Mapping.getAllLogicalDisksArray("C:\\Users\\XuJesseW\\Documents\\Programming");
            ArrayList physicalList = Mapping.getAllPhysicalDrivesArray();

            TreeViewItem physicalDrives = new TreeViewItem()
            {
                Header = "Physical Drives",
                IsExpanded = true
            };
            TreeViewItem logicalDisks = new TreeViewItem()
            {
                Header = "Logical Disks",
                IsExpanded = true
            };

            if (logicalList.Count > 0)
            {
                foreach (string s in logicalList)
                {
                    logicalDisks.Items.Add(new TreeViewItem() { Header = s });
                }
            }
            else
            {
                logicalDisks.Items.Add(new TreeViewItem() { Header = "No Logical Disks Found" });
            }

            if (physicalList.Count > 0)
            {
                foreach (string s in physicalList)
                {
                    physicalDrives.Items.Add(new TreeViewItem() { Header = "Physical Drive " + s });
                }
            }
            else
            {
                physicalDrives.Items.Add(new TreeViewItem() { Header = "No Physical Drives Found" });
            }

            //MenuItem physicalDrives = new MenuItem() { Title = "Physical Drives" };
            //MenuItem logicalDisks = new MenuItem() { Title = "Logical Disks" };

            //if(logicalList.Count>0)
            //{
            //    foreach (string s in logicalList)
            //    {           
            //        logicalDisks.Items.Add(new MenuItem() { Title = s });

            //    }
            //}
            //else
            //{
            //    logicalDisks.Items.Add(new MenuItem() { Title = "No Logical Disks Found" });
            //}

            //if (physicalList.Count > 0)
            //{
            //    foreach (string s in physicalList)
            //    {
            //        physicalDrives.Items.Add(new MenuItem() { Title = s });
            //    }
            //}
            //else
            //{
            //    physicalDrives.Items.Add(new MenuItem() { Title = "No Physical Drives Found" });
            //}


            trvMenu.Items.Add(physicalDrives);
            trvMenu.Items.Add(logicalDisks);

        }

        void treeViewItem_MouseLeftButtonUp(Object sender, RoutedEventArgs e)
        {
            TreeViewItem tvi = e.OriginalSource as TreeViewItem;
            string identifier= ((string)tvi.Header).Replace("Physical Drive ","");
            String text = Mapping.getLabelIdentifierOnly(identifier);
            labelBox.Text = text;
        }
    }

    public class MenuItem
    {
        public MenuItem()
        {
            this.Items = new ObservableCollection<MenuItem>();
        }

        public string Title { get; set; }

        public ObservableCollection<MenuItem> Items { get; set; }
    }
}
