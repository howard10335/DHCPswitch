using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Configuration;

namespace DHCPswitch
{
    class Program
    {
        static void Main(string[] args)
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
    }
}
