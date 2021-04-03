using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VKey
{
    public partial class MainForm : Form
    {
        Midi.MidiOut midiOut;
        MusicalKeyboard musicalKeyboard;
        Transposer transposer;
        ComputerKeyboard computerKeyboard;
        Hook.KeyboardHook keyboardHook;
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
        }

        private void InitDeviceCombobox()
        {
            var deviceTable = new DataTable();
            deviceTable.Columns.Add("id", typeof(int));
            deviceTable.Columns.Add("name", typeof(string));

            IEnumerable<Tuple<int, string>> devices = Midi.MidiOut.GetDevices();

            foreach (var device in devices)
            {
                var row = deviceTable.NewRow();
                row["id"] = device.Item1;
                row["name"] = device.Item2;
                deviceTable.Rows.Add(row);
            }
            deviceTable.AcceptChanges();

            DeviceComboBox.DisplayMember = "name";
            DeviceComboBox.ValueMember = "id";
            DeviceComboBox.DataSource = deviceTable;

            DeviceComboBox.SelectedIndex = 0;
            LoadDeviceSetting(devices);
        }

        private void LoadDeviceSetting(IEnumerable<Tuple<int, string>> devices)
        {
            var settingDeviceName = Properties.Settings.Default.MidiDeviceName;
            if (settingDeviceName == null || !settingDeviceName.Any())
            {
                return;
            }

            var settingDevice = devices.FirstOrDefault(x => x.Item2 == settingDeviceName);
            if (settingDevice == null)
            {
                return;
            }

            DeviceComboBox.SelectedValue = settingDevice.Item1;
        }

        private void DeviceChanged()
        {
            var deviceId = (int)DeviceComboBox.SelectedValue;

            if (midiOut != null)
            {
                midiOut.Dispose();
            }
            midiOut = new Midi.MidiOut(deviceId);

            if (musicalKeyboard != null)
            {
                musicalKeyboard.SetMidiOut(midiOut);
            }

            StoreDeviceSetting();
        }

        private void StoreDeviceSetting()
        {
            Properties.Settings.Default.MidiDeviceName = (string)((DataRowView)DeviceComboBox.SelectedItem)["name"];
            Properties.Settings.Default.Save();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            this.midiOut.Reset();
            this.transposer.Reset();
        }

        private void GlobalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.global = this.GlobalCheckBox.Checked;
            this.GlobalCheckBox.Text = this.global ? "Global" : "Local";
            this.TopMost = this.global;
            this.Text = this.global ? "VKey [Global]" : "VKey";

            if (this.global)
            {
                keyboardHook = new Hook.KeyboardHook(this.computerKeyboard);
            }
            else
            {
                keyboardHook.Dispose();
                keyboardHook = null;
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
