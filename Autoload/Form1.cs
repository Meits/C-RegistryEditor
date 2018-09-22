using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autoload
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            getApps();
            getAutorunApps();

        }

        private void getAutorunApps()
        {
            listBox2.Items.Clear();
            string RunKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
            
            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(RunKey))
            {
                foreach (string skName in rk.GetValueNames())
                {
                    listBox2.Items.Add(skName);
                }
            }
        }

        private void getApps()
        {
            string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths";

            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(uninstallKey))
            {
                foreach (string skName in rk.GetSubKeyNames())
                {
                    listBox1.Items.Add(skName);
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите элемент");
                return;
            }
            string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths";

            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(uninstallKey))
            {
                foreach (string skName in rk.GetSubKeyNames())
                {
                    if (skName == listBox1.SelectedItem.ToString())
                    {
                        string RunKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                        using (RegistryKey rkRun = Registry.CurrentUser.OpenSubKey(RunKey, true))
                        {
                            rkRun.SetValue(skName, rk.OpenSubKey(skName).GetValue(""));
                        }
                        break;
                    }
                }
            }
            getAutorunApps();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Выберите элемент");
                return;
            }
            string RunKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

            using (RegistryKey rkRun = Registry.CurrentUser.OpenSubKey(RunKey, true))
            {
                rkRun.DeleteValue(listBox2.SelectedItem.ToString());
            }
            getAutorunApps();
        }
    }
}
