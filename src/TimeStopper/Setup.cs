using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TimeStopper
{
    public partial class Setup : Form
    {
        private SetupSettings settings;
        public Setup(SetupSettings settings)
        {
            InitializeComponent();

            this.settings = settings;
            this.Render();
        }

        public static void ShowSetup()
        {
            using (var form = new Setup(SetupSettings.CurrentSettings))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    SetupSettings.CurrentSettings.Save();
                }
            }
        }

        private void ApplySettings()
        {
            settings.RecordsFileName = this.recordsNameBox.Text;
            settings.RunningColorArgb = this.runningPanel.BackColor.ToArgb();
            settings.StopColorArgb = this.stopPanel.BackColor.ToArgb();
            settings.HourlyWage = Convert.ToDouble(this.wageUpDown.Value);
        }

        private void Render()
        {
            this.SuspendLayout();
            this.recordsNameBox.Text = settings.RecordsFileName;
            this.runningPanel.BackColor = settings.RunningColor;
            this.stopPanel.BackColor = settings.StopColor;
            this.wageUpDown.Value = Convert.ToDecimal(settings.HourlyWage);
            this.ResumeLayout();
        }

        private void runningPanel_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = this.runningPanel.BackColor;

            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.runningPanel.BackColor = this.colorDialog1.Color;
            }
        }

        private void stopPanel_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = this.stopPanel.BackColor;

            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.stopPanel.BackColor = this.colorDialog1.Color;
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.recordsNameBox.Text))
            {
                MessageBox.Show("Please enter an filename for the records.");
                this.DialogResult = DialogResult.None;
            }
            else
            {
                this.ApplySettings();
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
