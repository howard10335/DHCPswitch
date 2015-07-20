using System;
using System.Configuration;
using System.Windows.Forms;

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
            ConfigurationManager.AppSettings.Set("IP", textBox_IP.Text);
            ConfigurationManager.AppSettings.Set("SubnetMask", textBox_Mask.Text);
            MessageBox.Show("Setting Success");
            this.Close();
        }
    }
}
