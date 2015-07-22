using System;
using System.Configuration;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DHCPswitch
{
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
            textBox_IP.Text = ConfigurationManager.AppSettings["IP"];
            textBox_Mask.Text = ConfigurationManager.AppSettings["SubnetMask"];
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (CheckIpFormat(textBox_IP.Text) && CheckIpFormat(textBox_Mask.Text))
            {
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configuration.AppSettings.Settings["IP"].Value = textBox_IP.Text;
                configuration.AppSettings.Settings["SubnetMask"].Value = textBox_Mask.Text;
                configuration.Save();

                ConfigurationManager.RefreshSection("appSettings");

                MessageBox.Show("Setting Success");
                this.Close();
            }
            else
            {
                MessageBox.Show("IP or SubMask format was wrong.");
            }
        }

        private bool CheckIpFormat(string ip)
        {
            Regex ipFormat = new Regex(@"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
            return ipFormat.IsMatch(ip);
        }
    }
}
