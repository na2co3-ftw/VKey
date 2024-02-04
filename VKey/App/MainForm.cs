using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using VKey.Hook;
using VKey.Hotkey;
using VKey.Midi;

namespace VKey
{
    public partial class MainForm : Form
    {
        MidiOut midiOut;
        MusicalKeyboard musicalKeyboard;
        Transposer transposer;
        ComputerKeyboard computerKeyboard;
        KeyboardHook keyboardHook;
        HotkeyRegistration globalHotkey;

        bool global = false;

        public MainForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitDeviceCombobox();

            musicalKeyboard = new MusicalKeyboard();
            transposer = new Transposer();
            computerKeyboard = new ComputerKeyboard(musicalKeyboard, transposer);

            DeviceChanged();

            transposer.TransposeChanged += MusicalKeyboard_TransposeChanged;
            transposer.Reset();

            globalHotkey = new HotkeyRegistration(Handle, Keys.K, controlKey: true, altKey: true);
        }

        private void InitDeviceCombobox()
        {
            var deviceTable = new DataTable();
            deviceTable.Columns.Add("id", typeof(int));
            deviceTable.Columns.Add("name", typeof(string));

            IEnumerable<MidiDevice> devices = MidiOut.GetDevices();

            foreach (var device in devices)
            {
                var row = deviceTable.NewRow();
                row["id"] = device.Id;
                row["name"] = device.Name;
                deviceTable.Rows.Add(row);
            }
            deviceTable.AcceptChanges();

            DeviceComboBox.DisplayMember = "name";
            DeviceComboBox.ValueMember = "id";
            DeviceComboBox.DataSource = deviceTable;

            DeviceComboBox.SelectedIndex = 0;
            LoadDeviceSetting(devices);
        }

        private void LoadDeviceSetting(IEnumerable<MidiDevice> devices)
        {
            var settingDeviceName = Properties.Settings.Default.MidiDeviceName;
            if (string.IsNullOrEmpty(settingDeviceName))
            {
                return;
            }

            var settingDevice = devices.FirstOrDefault(x => x.Name == settingDeviceName);
            if (settingDevice == null)
            {
                return;
            }

            DeviceComboBox.SelectedValue = settingDevice.Id;
        }

        private void DeviceChanged()
        {
            var deviceId = (int)DeviceComboBox.SelectedValue;

            midiOut?.Dispose();
            midiOut = new MidiOut(deviceId);

            musicalKeyboard?.SetMidiOut(midiOut);

            StoreDeviceSetting();
        }

        private void StoreDeviceSetting()
        {
            Properties.Settings.Default.MidiDeviceName = (string)((DataRowView)DeviceComboBox.SelectedItem)["name"];
            Properties.Settings.Default.Save();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            midiOut.Reset();
            transposer.Reset();
        }

        private void GlobalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            global = GlobalCheckBox.Checked;

            GlobalCheckBox.Text = global ? "Global" : "Local";
            this.TopMost = global;
            this.Text = global ? "VKey [Global]" : "VKey";

            if (global)
            {
                keyboardHook = new KeyboardHook(computerKeyboard);
            }
            else
            {
                keyboardHook.Dispose();
                keyboardHook = null;
            }
        }

        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (globalHotkey != null && globalHotkey.IsPressed(message))
            {
                GlobalCheckBox.Checked = !GlobalCheckBox.Checked;
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            computerKeyboard.KeyDown(e.KeyCode);
            e.Handled = true;
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            computerKeyboard.KeyUp(e.KeyCode);
            e.Handled = true;
        }

        private void MusicalKeyboard_TransposeChanged(int octave, int transpose)
        {
            TransposeLabel.Text = $"Octave: {octave}, Transpose: {transpose}";
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            return false;
        }

        private void VelocityTrackBar_Scroll(object sender, EventArgs e)
        {
            musicalKeyboard.Velocity = VelocityTrackBar.Value;
            VelocityLabel.Text = $"Velocity: {VelocityTrackBar.Value}";
        }

        private void DeviceComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DeviceChanged();
        }
    }
}
