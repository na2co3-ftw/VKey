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
        ComputerKeyboard computerKeyboard;
        KeyboardHook keyboardHook;
        bool global = false;

        public MainForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var count = Midi.MidiOut.GetDeviceNum();
            foreach (var id in Enumerable.Range(0, count))
            {
                Console.WriteLine(Midi.MidiOut.GetDeviceName(id));
            }

            midiOut = new Midi.MidiOut(1);
            musicalKeyboard = new MusicalKeyboard(midiOut);
            computerKeyboard = new ComputerKeyboard(musicalKeyboard);

            musicalKeyboard.TransposeChanged += MusicalKeyboard_TransposeChanged;
            musicalKeyboard.Reset();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            this.midiOut.Reset();
            this.musicalKeyboard.Reset();
        }

        private void GlobalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.global = this.GlobalCheckBox.Checked;
            this.GlobalCheckBox.Text = this.global ? "Global" : "Local";

            if (this.global)
            {
                keyboardHook = new KeyboardHook(this.computerKeyboard);
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
    }
}
