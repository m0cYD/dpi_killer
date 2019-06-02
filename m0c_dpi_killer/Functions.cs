using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace m0c_dpi_killer
{
    public static class Functions
    {
        public static void run_netsh_command(string command)
        {
            var pProcess = new Process();
            pProcess.StartInfo.FileName = "netsh.exe";
            pProcess.StartInfo.Arguments = command;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            pProcess.StartInfo.CreateNoWindow = true; //not diplay a windows
            pProcess.Start();
            pProcess.WaitForExit(4000);
        }
        public static NetworkInterface get_interneet_nic()
        {
            UdpClient u = new UdpClient("8.8.8.8", 1);
            IPAddress localAddr = ((IPEndPoint)u.Client.LocalEndPoint).Address;

            NetworkInterface target_nic = null;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                IPInterfaceProperties ipProps = nic.GetIPProperties();

                // check if localAddr is in ipProps.UnicastAddresses
                for (int j = 0; j < ipProps.UnicastAddresses.Count; j++)
                {
                    if (localAddr.Equals(ipProps.UnicastAddresses[j].Address))
                    {
                        target_nic = nic;
                        break;
                    }
                }
            }
            return target_nic;
        }
        public static void remove_dns(NetworkInterface target_nic)
        {
            run_netsh_command("dnsclient delete dnsserver \"" + target_nic.Name + "\" all");
        }
        public static void set_dns(NetworkInterface target_nic, string dns_server)
        {
            string[] servers = dns_server.Split(',');

            int this_num;
            for (int i = 0; i < servers.Length; i++)
            {
                this_num = i + 1;
                run_netsh_command("dnsclient add dnsserver \"" + target_nic.Name + "\" " + servers[i] + " " + this_num.ToString());
            }
        }
        public static Process run_process_in_bg(string path)
        {
            var pProcess = new Process();
            pProcess.StartInfo.FileName = path;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            pProcess.StartInfo.CreateNoWindow = true; //not diplay a windows
            pProcess.Start();
            return pProcess;
        }
        public static void task_kill(string exe_name)
        {
            var pProcess = new Process();
            pProcess.StartInfo.FileName = "cmd.exe";
            pProcess.StartInfo.Arguments = "/c taskkill /F /IM " + exe_name + " /T";
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            pProcess.StartInfo.CreateNoWindow = true; //not diplay a windows
            pProcess.Start();
            pProcess.WaitForExit(4000);
        }

    }
}
