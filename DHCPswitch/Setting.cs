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
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings["IP"].Value = textBox_IP.Text;
            configuration.AppSettings.Settings["SubnetMask"].Value = textBox_Mask.Text;
            configuration.Save();

            MessageBox.Show("Setting Success");
            this.Close();
        }
    }
}
