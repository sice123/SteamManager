using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Steam_Manager
{
    public partial class Form1 : Form
    {
        string path = @"E:\data.txt";
        struct user
        {
            
            public user(string user , string pass)
            {
                username = user;
                password = pass;
            }
            public string username { get; }
            public string password { get; }
            
        }
        List<user> userlist = new List<user>();
        public Form1()
        {//
            InitializeComponent();
        }

        private void LoginBButt_Click(object sender, EventArgs e)
        {
            foreach (Process p in Process.GetProcessesByName("steam"))
            {
                p.Kill();
            }
            ProcessStartInfo pstart = new ProcessStartInfo();

            pstart.FileName = (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam",
                "SteamExe", "null");
            pstart.Arguments = " -login " + userlist[comboBox1.SelectedIndex].username + " " + userlist[comboBox1.SelectedIndex].password;
            Process.Start(pstart);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UserControl1 u = new UserControl1();
            this.Hide();
            u.Show();
            CheckForIllegalCrossThreadCalls = false;

            
        }

        private void addUser_Click(object sender, EventArgs e)
        {
            List<string> lines = File.ReadAllLines(path).ToList();
            var User = new user(textBox1.Text , textBox2.Text);
            userlist.Add(User);
            lines.Add(User.username + "," + User.password);
            File.WriteAllLines(path, lines);

            comboBox1.Items.Clear();
            foreach (user v in userlist)
            {
                comboBox1.Items.Add(v.username);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists(path))
            {
                StreamWriter sw = File.CreateText(path);
                sw.Flush();
                sw.Dispose();
            }
            List<string> lines = File.ReadAllLines(path).ToList();
            foreach (var line in lines)
            {
                string[] entries = line.Split(';');
                user newuser = new user(entries[0], entries[1]);
                comboBox1.Items.Add(entries[0]);
                userlist.Add(newuser);
            }
        }
    }
}
