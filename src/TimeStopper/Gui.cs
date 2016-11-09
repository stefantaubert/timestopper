using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TimeStopper.Properties;
using System.IO;
using Microsoft.Win32;

namespace TimeStopper
{
    public class Gui
    {
        private ToolStripMenuItem summaryButton;
        private ToolStripMenuItem resetButton;
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem exitButton;
        private ToolStripMenuItem setupButton;
        private NotifyIcon taskBarIcon;
        private PersistentStopwatch currentStopwatch;

        public static Gui GetForStopwatch(PersistentStopwatch stopwatch)
        {
            return new Gui(stopwatch);
        }

        private Gui(PersistentStopwatch stopwatch)
        {
            this.currentStopwatch = stopwatch;
            this.InitContextMenuStrip();
            this.InitContextMenuStripButtons();
            this.InitTaskbarIcon();
        }

        public void MakeVisibleToUser()
        {
            this.AddButtonsToContextMenuStrip();
            this.AddContextMenuStripToIcon();
            this.AddIconToTaskbar();
            this.RenderIconAndText();
        }

        private void BindPowerModeChangedEvent()
        {
            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
        }

        private void InitTaskbarIcon()
        {
            this.taskBarIcon = new NotifyIcon();

            this.taskBarIcon.MouseClick += new MouseEventHandler(this.HandleIconClick);
            this.taskBarIcon.MouseMove += new MouseEventHandler(this.HandleIconMouseMove);
        }

        private void AddContextMenuStripToIcon()
        {
            this.taskBarIcon.ContextMenuStrip = this.contextMenu;
        }

        private void InitContextMenuStrip()
        {
            this.contextMenu = new ContextMenuStrip();
            this.contextMenu.ShowCheckMargin = false;
            this.contextMenu.ShowImageMargin = false;
        }

        private void InitContextMenuStripButtons()
        {
            this.InitExitButton();
            this.InitResetButton();
            this.InitSummarytButton();
            this.InitSetupButton();
        }

        private void InitSetupButton()
        {
            this.setupButton = new ToolStripMenuItem("&Setup");
            this.setupButton.Click += new EventHandler(this.HandleSetupButtonClick);
        }

        private void InitSummarytButton()
        {
            this.summaryButton = new ToolStripMenuItem("&Summary");
            this.summaryButton.Click += new EventHandler(this.HandleSummaryButtonClick);
        }

        private void InitExitButton()
        {
            this.exitButton = new ToolStripMenuItem("&Exit");
            this.exitButton.Click += new EventHandler(this.HandleExitButtonClick);
        }

        private void InitResetButton()
        {
            this.resetButton = new ToolStripMenuItem("&Reset");
            this.resetButton.Click += new EventHandler(this.HandleResetButtonClick);
        }

        private void AddButtonsToContextMenuStrip()
        {
            this.contextMenu.Items.Add(this.resetButton);
            this.contextMenu.Items.Add(this.summaryButton);
            this.contextMenu.Items.Add(this.setupButton);
            this.contextMenu.Items.Add(this.exitButton);
        }

        private void AddIconToTaskbar()
        {
            this.taskBarIcon.Visible = true;
        }

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Suspend)
            {
                this.HandlePCSuspending();
            }
        }

        private void HandlePCSuspending()
        {
            this.StopRecordIfRunning();
            this.RenderIcon();
        }

        private void HandleIconMouseMove(object sender, MouseEventArgs e)
        {
            this.RenderIconText();
        }

        private void HandleResetButtonClick(object sender, EventArgs e)
        {
            this.currentStopwatch.Reset();
        }
        private void HandleSetupButtonClick(object sender, EventArgs e)
        {
            Setup.ShowSetup();
            this.RenderIconAndText();
        }

        private void HandleSummaryButtonClick(object sender, EventArgs e)
        {
            using (var dlg = new Summary(this.currentStopwatch))
            {
                dlg.ShowDialog();
            }
        }

        private void HandleExitButtonClick(object sender, EventArgs e)
        {
            this.StopRecordIfRunning();
            this.RemoveIconFromTaskbar();
            this.ExitApplication();
        }

        private void RemoveIconFromTaskbar()
        {
            this.taskBarIcon.Visible = false;
        }

        private void ExitApplication()
        {
            Application.Exit();
        }

        private void StopRecordIfRunning()
        {
            if (this.currentStopwatch.IsRunning)
            {
                this.currentStopwatch.StopRunningRecord();
            }
        }

        private void RenderIconAndText()
        {
            this.RenderIcon();
            this.RenderIconText();
        }

        private void RenderIcon()
        {
            this.taskBarIcon.Icon = this.currentStopwatch.IsRunning ? SetupSettings.CurrentSettings.RunningColor.GetIcon() : SetupSettings.CurrentSettings.StopColor.GetIcon();
        }

        private void RenderIconText()
        {
            this.taskBarIcon.Text = this.GetFormattedTimeSpans();
        }

        private string GetFormattedTimeSpans()
        {
            var todaysTimeSpanFormatted = this.GetFormattedTodaysTimeSpan();
            var totalTimeSpanFormatted = this.GetFormattedTotalTimeSpan();

            return string.Format("Today: {0}\n\nTotal: {1}", todaysTimeSpanFormatted, totalTimeSpanFormatted);
        }

        private string GetFormattedTotalTimeSpan()
        {
            return this.currentStopwatch.Elapsed.GetFormattedDuration();
        }

        private string GetFormattedTodaysTimeSpan()
        {
            var todaysTimeSpan = GetTodaysTimeSpan();

            return todaysTimeSpan.GetFormattedDuration();
        }

        private TimeSpan GetTodaysTimeSpan()
        {
            var todaysRecords = this.currentStopwatch.RecentRecords.GetTodaysRecords();
            var sumOfTodaysTicksAndRunningTicks = todaysRecords.ElapsedTicks + this.currentStopwatch.CurrentRecordElapsedTicks;

            return TimeSpan.FromTicks(sumOfTodaysTicksAndRunningTicks);
        }

        private void HandleIconClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.HandleIconLeftClick();
            }
        }

        private void HandleIconLeftClick()
        {
            this.StartOrStopRecord();
            this.RenderIconAndText();
        }

        private void StartOrStopRecord()
        {
            if (this.currentStopwatch.IsRunning)
            {
                this.currentStopwatch.StopRunningRecord();
            }
            else
            {
                this.currentStopwatch.StartNewRecord();
            }
        }
    }
}