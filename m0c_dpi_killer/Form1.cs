using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace m0c_dpi_killer
{
    public partial class Form1 : Form
    {
        public static string dnscrypt_path, windivert_dll_path, goodbyedpi_path, windivert_sys_path,
            current_dns;
        public static Process dnscrypt_process, goodbyedpi_process;
        public static NetworkInterface internet_nic;
        public static bool cleaned=false;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public Form1()
        {
            InitializeComponent();
        }

        static void progress(int value, Form1 myForm)
        {
            myForm.progressBar1.Value = value;
            myForm.Refresh();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            clean(this);
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            progress(10, this);
            timer1.Enabled = false;
            bool is64bit = !string.IsNullOrEmpty(
                Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"));

            internet_nic = Functions.get_interneet_nic();
            if (internet_nic == null)
            {
                lblStatus.Text = "Cannot find any internet interface.";
                cleaned = true;
                return;
            }
            progress(20, this);

            current_dns = "";
            for (int i = 0; i < internet_nic.GetIPProperties().DnsAddresses.Count; i++)
            {
                current_dns += internet_nic.GetIPProperties().DnsAddresses[i].ToString() + ",";
            }
            current_dns = current_dns.Substring(0, current_dns.Length - 1);


            //Run dnscrypt
            dnscrypt_path = Path.Combine(Path.GetTempPath(), "dnscrypt_proxy.exe");
            try
            {
                File.WriteAllBytes(dnscrypt_path, m0c_dpi_killer.Resource1.dnscrypt_proxy);
            }
            catch(Exception eee)
            {

            }
            progress(30, this);
            dnscrypt_process = Functions.run_process_in_bg(dnscrypt_path);
            progress(40, this);

            windivert_dll_path = Path.Combine(Path.GetTempPath(), "WinDivert.dll");
            goodbyedpi_path = Path.Combine(Path.GetTempPath(), "goodbyedpi.exe");
            windivert_sys_path = "";
            if (is64bit == true)
            {
                try
                {
                    File.WriteAllBytes(goodbyedpi_path, m0c_dpi_killer.Resource1._64_goodbyedpi);
                }
                catch(Exception ee)
                {

                }
                progress(50, this);

                try
                {
                    File.WriteAllBytes(windivert_dll_path, m0c_dpi_killer.Resource1._64_WinDivert_dll);
                }
                catch(Exception ee)
                {

                }
                progress(60, this);
                windivert_sys_path = Path.Combine(Path.GetTempPath(), "WinDivert64.sys");
                try
                {
                    File.WriteAllBytes(windivert_sys_path, m0c_dpi_killer.Resource1._64_WinDivert64_sys);
                }
                catch (Exception ee)
                {

                }
            }
            else
            {
                try
                {
                    File.WriteAllBytes(goodbyedpi_path, m0c_dpi_killer.Resource1._32_goodbyedpi);
                }
                catch (Exception ee)
                {

                }
                progress(50, this);
                try
                {
                    File.WriteAllBytes(windivert_dll_path, m0c_dpi_killer.Resource1._32_WinDivert_dll);
                }
                catch (Exception ee)
                {

                }
                progress(60, this);
                windivert_sys_path = Path.Combine(Path.GetTempPath(), "WinDivert32.sys");
                try
                {
                    File.WriteAllBytes(windivert_sys_path, m0c_dpi_killer.Resource1._32_WinDivert32_sys);
                }
                catch (Exception ee)
                {

                }
            }
            progress(70, this);
            Functions.remove_dns(internet_nic);
            progress(75, this);
            Functions.set_dns(internet_nic, "127.0.0.1");
            progress(80, this);
            goodbyedpi_process = Functions.run_process_in_bg(goodbyedpi_path);
            progress(85, this);
            

            Thread.Sleep(1000);
            progress(90, this);
            lblStatus.Text = "Enjoy !";
            lblStatus.ForeColor = Color.ForestGreen;
            Thread.Sleep(1000);
            progress(100, this);
            //System.Diagnostics.Process.Start("https://www.youtube.com");

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState= FormWindowState.Minimized;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            Form1_MouseDown(sender, e);
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            Form1_MouseDown(sender, e);
        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            Form1_MouseDown(sender, e);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Form1_MouseDown(sender, e);
        }

        private void lblStatus_MouseDown(object sender, MouseEventArgs e)
        {
            Form1_MouseDown(sender, e);
        }

        private void progressBar1_MouseDown(object sender, MouseEventArgs e)
        {
            Form1_MouseDown(sender, e);
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            Form1_MouseDown(sender, e);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Process.Start("tg://resolve?domain=m0c_dpi_killer");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("tg://resolve?domain=m0c_dpi_killer");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            progressBar1.ForeColor = Color.FromArgb(56, 105, 129);
            progressBar1.BackColor = Color.FromArgb(41, 44, 51);
        }

        static void clean(Form1 myForm)
        {
            if (cleaned == true) return;
            cleaned = true;
            myForm.lblStatus.ForeColor = Color.SteelBlue;
            myForm.lblStatus.Text = "Please wait... Reverting...";
            myForm.Refresh();
            try
            {
                dnscrypt_process.Kill();
            }
            catch (Exception e)
            {

            }
            progress(80, myForm);

            try
            {
                goodbyedpi_process.Kill();
            }

            catch (Exception e)
            {

            }
            progress(60, myForm);

            Thread.Sleep(1000);
            progress(50, myForm);
            Thread.Sleep(1000);
            progress(40, myForm);
            try
            {
                File.Delete(dnscrypt_path);
            }
            catch (Exception eee)
            {
                Functions.task_kill("dnscrypt_proxy.exe");
                Thread.Sleep(500);
                File.Delete(dnscrypt_path);
            }

            progress(30, myForm);
            try
            {
                File.Delete(goodbyedpi_path);
            }
            catch (Exception eee)
            {
                Functions.task_kill("goodbyedpi.exe");
                Thread.Sleep(500);
                File.Delete(goodbyedpi_path);
            }
            progress(20, myForm);

            Functions.remove_dns(internet_nic);
            progress(10, myForm);
            if (current_dns.Substring(0, 9) != "127.0.0.1")
            {
                Functions.set_dns(internet_nic, current_dns);
            }

            progress(0, myForm);
        }
    }
}
