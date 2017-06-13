using Skypush.Classes;
using System;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using System.Net.Http;
using System.IO;

[assembly: AssemblyVersion("1.2")]

namespace Skypush
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var applicationContext = new CustomApplicationContext();
            Application.Run(applicationContext);
        }
    }

    public class CustomApplicationContext : ApplicationContext
    {
        private HttpClient Client = new HttpClient();
        private System.ComponentModel.IContainer components;
        private Grabber grabberForm;
        private Settings settingsForm;
        public NotifyIcon notifyIcon;
        public KeyboardHook areaHook = new KeyboardHook();
        public KeyboardHook windowHook = new KeyboardHook();
        public KeyboardHook everythingHook = new KeyboardHook();

        public CustomApplicationContext()
        {
            areaHook.KeyPressed += new EventHandler<KeyPressedEventArgs>(AreaHook_KeyPressed);
            windowHook.KeyPressed += new EventHandler<KeyPressedEventArgs>(WindowHook_KeyPressed);
            everythingHook.KeyPressed += new EventHandler<KeyPressedEventArgs>(EverythingHook_KeyPressed);
            var allKeysSet = true;
            if (!areaHook.RegisterHotKey((CustomModifierKeys)Properties.Settings.Default.AreaModifiers, (Keys)Properties.Settings.Default.AreaKeys))
            {
                MessageBox.Show("Hotkey for Capture area already in use!");
                allKeysSet = false;
            }
            if (!windowHook.RegisterHotKey((CustomModifierKeys)Properties.Settings.Default.WindowModifiers, (Keys)Properties.Settings.Default.WindowKeys))
            {
                MessageBox.Show("Hotkey for Capture window already in use!");
                allKeysSet = false;
            }
            if (!everythingHook.RegisterHotKey((CustomModifierKeys)Properties.Settings.Default.EverythingModifiers, (Keys)Properties.Settings.Default.EverythingKeys))
            {
                MessageBox.Show("Hotkey for Capture everything already in use!");
                allKeysSet = false;
            }
            if (!allKeysSet)
            {
                settingsForm = new Settings(this);
                settingsForm.FormClosing += DisposeSettings;
                settingsForm.Show();
            }
            InitializeContext();
            EstablishConnection();
        }

        private void InitializeContext()
        {
            components = new System.ComponentModel.Container();
            notifyIcon = new NotifyIcon(components)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = Properties.Resources.icon,
                Text = "Skypush",
                Visible = true
            };
            notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            notifyIcon.MouseUp += NotifyIcon_MouseUp;
        }

        private async void EstablishConnection()
        {
            this.notifyIcon.Text = "Skypush - Connecting...";
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await Client.GetAsync("https://skyweb.nu/api/connect.php");
                try
                {
                    responseMessage.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException ex)
                {
                    this.notifyIcon.ShowBalloonTip(10000, "Connecting failed", "Error connecting to server: " + ex.Message, ToolTipIcon.Error);
                }
            }
            catch (HttpRequestException ex)
            {
                this.notifyIcon.ShowBalloonTip(10000, "Connecting failed", "Error connecting to server: " + ex.InnerException.Message, ToolTipIcon.Error);
            }
            this.notifyIcon.Text = "Skypush";
        }

        void AreaHook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            grabberForm = new Grabber(this);
            grabberForm.FormClosing += DisposeGrabber;
            grabberForm.Show();
        }

        public void DisposeGrabber(object sender, FormClosingEventArgs e)
        {
            grabberForm = null;
        }

        void WindowHook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            var rect = Helpers.GetCurrentWindowRect();
            var printscreen = Helpers.GetWindowScreenshot(rect);
            SaveToClipboard(printscreen);
        }

        void EverythingHook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            var printscreen = Helpers.GetAllMonitorsScreenshot();
            SaveToClipboard(printscreen);
        }

        public void SaveToClipboard(Bitmap image)
        {
            ImageConverter converter = new ImageConverter();
            byte[] _imgBytes = (byte[])converter.ConvertTo(image, typeof(byte[]));
            var content = new MultipartFormDataContent
            {
                { new StreamContent(new MemoryStream(_imgBytes)), "data", "upload.png" }
            };
            UploadImage(content);
        }

        public async void UploadImage(MultipartFormDataContent Content)
        {
            this.notifyIcon.Text = "Skypush - Uploading...";
            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await Client.PostAsync("https://skyweb.nu/api/upload.php", Content);
                try
                {
                    responseMessage.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException ex)
                {
                    var resultContent = responseMessage.Content.ReadAsStringAsync().Result;
                    this.notifyIcon.ShowBalloonTip(10000, "Upload failed", "Upload failed: " + ex.Message, ToolTipIcon.Error);
                }
            }
            catch (HttpRequestException ex)
            {
                this.notifyIcon.ShowBalloonTip(10000, "Upload failed", "Upload failed: " + ex.InnerException.Message, ToolTipIcon.Error);
            }

            if (responseMessage != null && responseMessage.IsSuccessStatusCode)
            {
                var resultContent = responseMessage.Content.ReadAsStringAsync().Result.Trim();
                Clipboard.SetText(resultContent);
                this.notifyIcon.ShowBalloonTip(5000, "Success", resultContent, ToolTipIcon.Info);
            }
            this.notifyIcon.Text = "Skypush";
        }

        private void NotifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(notifyIcon, null);
            }
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            notifyIcon.ContextMenuStrip.Items.Clear();

            if (grabberForm != null)
            {
                notifyIcon.ContextMenuStrip.Items.Add("Uploading...");
                notifyIcon.ContextMenuStrip.Items[0].Enabled = false;
            }

            var settingsContextItem = new ToolStripMenuItem("&Settings");
            settingsContextItem.Click += SettingsItem_Click;
            notifyIcon.ContextMenuStrip.Items.Add(settingsContextItem);

            var exitContextItem = new ToolStripMenuItem("&Exit");
            exitContextItem.Click += ExitItem_Click;
            notifyIcon.ContextMenuStrip.Items.Add(exitContextItem);
        }

        private void SettingsItem_Click(object sender, EventArgs e)
        {
            settingsForm = new Settings(this);
            settingsForm.FormClosing += DisposeSettings;
            settingsForm.Show();
        }

        public void DisposeSettings(object sender, FormClosingEventArgs e)
        {
            settingsForm = null;
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            notifyIcon.Dispose();
            ExitThread();
        }
    }
}
