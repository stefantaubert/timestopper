using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Arbeitszeiten
{
    public partial class Form1 : Form
    {
        Recorder r = new Recorder();
        public Form1()
        {
            InitializeComponent();
            r.Load();
            r.AddToday();
            notifyIcon1.Icon = GetIcon(Brushes.Red, new Size(1, 1));
            ShowInfos();
        }

        private Icon GetIcon(Brush color, Size s)
        {
            Bitmap b = new Bitmap(s.Width, s.Height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.FillRegion(color, new Region(new Rectangle(0, 0, s.Width, s.Height)));
            }
            return Icon.FromHandle(b.GetHicon());
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            r.Save();
            notifyIcon1.Dispose();
        }
        void ShowInfos()
        {
            label1.Text = "Arbeitszeit:\nheute: "
          + r.AktuellerTag.Dauer + " (" + r.AktuellerTag.Geldwert + "€)";
            //+"gesamt: "          + r.GetAlleTage().Dauer + " (" + r.GetAlleTage().Geldwert + "€)\n";
            richTextBox1.Text = r.AktuellerTag.Log.Trim();
            richTextBox1.Focus();
            richTextBox1.Select(richTextBox1.TextLength, 0);
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Normal;
                }
                else
                {
                    WindowState = FormWindowState.Minimized;
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (notifyIcon1.Tag.ToString() == "an")
                {
                    r.AktuellerTag.Aus();
                    notifyIcon1.Icon = GetIcon(Brushes.Red, new Size(1, 1));
                    notifyIcon1.Tag = "aus";
                }
                else
                {
                    r.AktuellerTag.An();
                    notifyIcon1.Icon = GetIcon(Brushes.Lime, new Size(1, 1));
                    notifyIcon1.Tag = "an";
                }
                ShowInfos();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((notifyIcon1.Tag.ToString() == "aus") && MessageBox.Show("Sicher?", "Hinweis", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                r.AktuellerTag.Log = string.Empty;
                r.AktuellerTag.Ts = new TimeSpan(0, 0, 0);
                ShowInfos();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer.exe", Application.StartupPath);
        }
    }
}
