using Skypush.Classes;
using System;
using System.Windows.Forms;
using ProgramSettings = Skypush.Properties.Settings;

namespace Skypush
{
    public partial class Settings : Form
    {
        public CustomApplicationContext main;
        
        private bool saveable;

        public Settings(CustomApplicationContext Main)
        {
            InitializeComponent();
            this.Text = string.Format("Skypush {0} - Settings", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            GetSettings(ProgramSettings.Default.AreaModifiers, ProgramSettings.Default.AreaKeys, textBoxAreaKeys);
            GetSettings(ProgramSettings.Default.WindowModifiers, ProgramSettings.Default.WindowKeys, textBoxWindowKeys);
            GetSettings(ProgramSettings.Default.EverythingModifiers, ProgramSettings.Default.EverythingKeys, textBoxEverythingKeys);
            main = Main;
        }

        private void textBoxAreaKeys_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateHotkey(textBoxAreaKeys, e);
        }

        private void textBoxWindowKeys_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateHotkey(textBoxWindowKeys, e);
        }

        private void textBoxEverythingKeys_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateHotkey(textBoxEverythingKeys, e);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (!saveable)
            {
                MessageBox.Show("Please use a combination of control, shift and alt, and any other key. Current settings are not usable.");
            }
            else
            {
                var allKeysSet = true;
                KeyCombination currentBoxKeyCombination = (KeyCombination)textBoxAreaKeys.Tag;
                if (main.areaHook.RegisterHotKey(currentBoxKeyCombination.Modifiers, currentBoxKeyCombination.Keys))
                {
                    main.areaHook.UnregisterHotKey(main.areaHook._currentId - 1);
                    ProgramSettings.Default.AreaModifiers = (int)currentBoxKeyCombination.Modifiers;
                    ProgramSettings.Default.AreaKeys = (int)currentBoxKeyCombination.Keys;
                }
                else
                {
                    MessageBox.Show("Hotkey for Capture area already in use!");
                    allKeysSet = false;
                }
                currentBoxKeyCombination = (KeyCombination)textBoxWindowKeys.Tag;
                if (main.windowHook.RegisterHotKey(currentBoxKeyCombination.Modifiers, currentBoxKeyCombination.Keys))
                {
                    main.windowHook.UnregisterHotKey(main.windowHook._currentId - 1);
                    ProgramSettings.Default.WindowModifiers = (int)currentBoxKeyCombination.Modifiers;
                    ProgramSettings.Default.WindowKeys = (int)currentBoxKeyCombination.Keys;
                }
                else
                {
                    MessageBox.Show("Hotkey for Capture window already in use!");
                    allKeysSet = false;
                }
                currentBoxKeyCombination = (KeyCombination)textBoxEverythingKeys.Tag;
                if (main.everythingHook.RegisterHotKey(currentBoxKeyCombination.Modifiers, currentBoxKeyCombination.Keys))
                {
                    main.everythingHook.UnregisterHotKey(main.everythingHook._currentId - 1);
                    ProgramSettings.Default.EverythingModifiers = (int)currentBoxKeyCombination.Modifiers;
                    ProgramSettings.Default.EverythingKeys = (int)currentBoxKeyCombination.Keys;
                }
                else
                {
                    MessageBox.Show("Hotkey for Capture everything already in use!");
                    allKeysSet = false;
                }

                if (allKeysSet)
                {
                    ProgramSettings.Default.Save();
                    this.Close();
                }
            }
        }

        private void GetSettings(int SettingsModifiers, int SettingsKeys, TextBox Box)
        {
            var currentModifiersText = "";
            var currentKeysText = "";
            var currentModifiers = (CustomModifierKeys)SettingsModifiers;
            var currentKeys = (Keys)SettingsKeys;

            if (currentKeys != 0)
            {
                if (currentKeys >= Keys.D0 && currentKeys <= Keys.D9)
                {
                    currentKeysText = currentKeys.ToString().Replace("D", "");
                }
                else
                {
                    currentKeysText = currentKeys.ToString();
                }
            }

            if (SettingsModifiers != 0)
            {
                currentModifiersText = currentModifiers.ToString().Replace(", ", " + ");
                if (currentKeysText != "")
                {
                    currentKeysText = " + " + currentKeysText;
                }
            }
            Box.Text = currentModifiersText + currentKeysText;
            Box.Tag = new KeyCombination() { Keys = currentKeys, Modifiers = currentModifiers };
        }

        private void UpdateHotkey(TextBox Box, KeyEventArgs e)
        {
            var newModifiers = CustomModifierKeys.None;
            if (e.Alt) newModifiers |= CustomModifierKeys.Alt;
            if (e.Control) newModifiers |= CustomModifierKeys.Control;
            if (e.Shift) newModifiers |= CustomModifierKeys.Shift;
            var newKeys = Keys.None;

            if (e.KeyCode == Keys.Back)
            {
                newModifiers = CustomModifierKeys.None;
            }

            string newModifierKeysText = "";
            string newKeyCodeText = "";
            if (e.KeyCode != Keys.Alt && e.KeyCode != Keys.Menu && e.KeyCode != Keys.ControlKey && e.KeyCode != Keys.ShiftKey && e.KeyCode != Keys.Back)
            {
                if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
                {
                    newKeyCodeText = e.KeyCode.ToString().Replace("D", "");
                }
                else
                {
                    newKeyCodeText = e.KeyCode.ToString();
                }
                newKeys = e.KeyCode;
            }
            if (newModifiers != 0)
            {
                newModifierKeysText = newModifiers.ToString().Replace(", ", " + ");
                if (newKeyCodeText != "")
                {
                    newKeyCodeText = " + " + newKeyCodeText;
                }
            }

            Box.Text = newModifierKeysText + newKeyCodeText;
            Box.Tag = new KeyCombination() { Keys = newKeys, Modifiers = newModifiers };
            ValidateSettings(Box);
        }

        private void ValidateSettings(TextBox Box)
        {
            KeyCombination newCombination = (KeyCombination)Box.Tag;
            saveable = true;
            Box.BackColor = System.Drawing.Color.Red;
            Box.ForeColor = System.Drawing.Color.Black;
            if (newCombination.Modifiers == CustomModifierKeys.None ^ newCombination.Keys == Keys.None) //Both none is good, both filled is good
            {
                saveable = false;
                Box.ForeColor = System.Drawing.Color.Red;
            }
            Box.BackColor = System.Drawing.Color.White;
        }
    }
}
