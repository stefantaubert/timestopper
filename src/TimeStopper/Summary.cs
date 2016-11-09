using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TimeStopper
{
    public partial class Summary : Form
    {
        private PersistentStopwatch stopWatch;

        public Summary(PersistentStopwatch stopWatch)
        {
            InitializeComponent();

            this.stopWatch = stopWatch;

            this.Render();
        }

        private void Render()
        {
            this.richTextBox1.Text = this.stopWatch.RecentRecords.GetSummary();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                stopWatch.ExportRecentRecords(this.saveFileDialog1.FileName);
            }
        }
    }
}
