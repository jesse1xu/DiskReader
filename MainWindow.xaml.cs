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
using System.Windows.Controls.Primitives;
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
        private string logDestinationString = "C:\\Users\\" + Environment.UserName + "\\Documents";
        private string logicalDiskDestinationString = "C:\\ProgramData\\Unisys\\MCP Firmware\\Vdisks";
        private string outputText = "";
        private string outputType = "";
        private bool readOnly = true;
        public MainWindow()
        {
            InitializeComponent();

            if (!Mapping.IsAdministrator())
            {
                MessageBox.Show("Non-administrator user detected. Please run DiskReader as an administrator in order to view physical drives");
            }

            reinitialize();
            labelBox.IsReadOnly = true;

            filetypeBox.SelectedIndex = 0;
        }

        void treeViewItem_MouseLeftButtonUp(Object sender, RoutedEventArgs e)
        {
            TreeViewItem tvi = e.OriginalSource as TreeViewItem;
            string header = (string)tvi.Header;
            string identifier = header.Replace("Physical Drive ", "");
            String text = Mapping.getLabelIdentifierOnlyNoLabels(identifier, logicalDiskDestinationString + "\\", readOnly);
            labelInfo.Text = text;
            outputText = text;
            outputType = header;
            if (text.Length > 0 && text != "")
            {
                lunName.Text = ((string)tvi.Header).ToUpper();
            }
            else
            {
                lunName.Text = "LUN";
            }

        }

        void reinitialize()
        {
            if (trvMenu.Items.Count > 0)
            {
                trvMenu.Items.Clear();
            }

            ArrayList logicalList = Mapping.getAllLogicalDisksArray(logicalDiskDestinationString);
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
                logicalDisks.Items.Add(new TreeViewItem() { Header = "None Found" });
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
                physicalDrives.Items.Add(new TreeViewItem() { Header = "None Found" });
            }

            labelBox.Text = "Vol 1 Identifier\n" +
                "Pack Serial Number\n" +
                "Family Name\n" +
                "System-Interchange Code\n" +
                "Owner's Id\n" +
                "Time Stamp\n" +
                "Relative Index within Set\n" +
                "Mask of Online Copies\n" +
                "Recovery Option\n" +
                "Auditing Marker\n" +
                "MDPF (Spare Disk) Marker\n" +
                "Vol 2 Identifier\n" +
                "Initialization Date\n" +
                "Initializing System\n" +
                "Directory Link\n" +
                "Integrity Flag";



            logDestination.Text = logDestinationString;
            logicalDiskDestination.Text = logicalDiskDestinationString;

            trvMenu.Items.Add(physicalDrives);
            trvMenu.Items.Add(logicalDisks);

            // For write-mode
            labelInfo.IsReadOnly = readOnly;
        }

        void logDestinationTextChanged(Object sender, TextChangedEventArgs args)
        {
            logDestinationString = logDestination.Text;
        }

        void logicalDiskDestinationTextChanged(Object sender, TextChangedEventArgs args)
        {
            logicalDiskDestinationString = logicalDiskDestination.Text;         
        }

        void logOutputClicked(Object sender, RoutedEventArgs e)
        {
            string messageText;
            if (outputType != "" && outputText != "")
            {
                messageText = Mapping.writeToTextFile(logDestinationString, outputType, outputText);
            }
            else
            {
                messageText = "Nothing to output.";
            }
            MessageBox.Show(messageText);
        }

        private void logBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();

            dlg.Description = "Choose a file location for outputted logs.";

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                logDestinationString = dlg.SelectedPath;
                logDestination.Text = dlg.SelectedPath;
            }
        }

        private void logicalDiskBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();

            dlg.Description = "Choose a file location for outputted logs.";

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                logicalDiskDestinationString = dlg.SelectedPath;
                logicalDiskDestination.Text = dlg.SelectedPath;
            }
        }

        private void readOnly_Checked(object sender, RoutedEventArgs e)
        {
            readOnly = true;
        }

        private void readOnly_Unchecked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("WARNING: enabling write-mode may compromise integrity of opened disks.");
            readOnly = false;
        }

        private void reinitializeButton_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 0;
            reinitialize();
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
