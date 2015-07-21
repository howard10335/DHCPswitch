using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Net;

namespace DHCPswitch
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.Hide();
        }

        public void SwitchDHCP(object sender, EventArgs e)
        {
            //read IP and Subnet Mask from App.config
            string ip = ConfigurationManager.AppSettings["IP"];
            string subnetMask = ConfigurationManager.AppSettings["SubnetMask"];

            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                //when the network adapter type is Ethernet and is using
                if (adapter.NetworkInterfaceType.ToString().Equals("Ethernet") && adapter.OperationalStatus == OperationalStatus.Up)
                {
                    ProcessStartInfo psi;
                    if (adapter.GetIPProperties().GetIPv4Properties().IsDhcpEnabled)
                    {
                        psi = new ProcessStartInfo("netsh", "interface ip set address \"" + adapter.Name + "\" static " + ip + " " + subnetMask);
                    }
                    else
                    {
                        psi = new ProcessStartInfo("netsh", "interface ip set address \"" + adapter.Name + "\" source=dhcp");
                    }
                    Process p = new Process();
                    p.StartInfo = psi;
                    //set no console window when execute
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();
                    //break foreach, only swith the first network adapter
                    break;
                }
            }
        }

        private void notifyIcon_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                //when the network adapter type is Ethernet and is using
                if (adapter.NetworkInterfaceType.ToString().Equals("Ethernet") && adapter.OperationalStatus == OperationalStatus.Up)
                {
                    //show DHCP or current static ip on notifyIcon.Text
                    if (adapter.GetIPProperties().GetIPv4Properties().IsDhcpEnabled)
                    {
                        notifyIcon.Text = "Current Status\nDHCP";
                    }
                    else
                    {
                        IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
                        IPAddress[] ipAddr = ipEntry.AddressList;
                        notifyIcon.Text = "Current Status\n";
                        notifyIcon.Text += ipAddr[ipAddr.Length - 1].ToString();
                    }
                }
                //break foreach, only show the first network adapter
                break;
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Setting().Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
