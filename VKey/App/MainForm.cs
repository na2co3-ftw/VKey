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
        ComputerKeyboard keyboard;
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
            keyboard = new ComputerKeyboard(midiOut, 1);
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            this.midiOut.Reset();
        }

        private void GlobalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.global = this.GlobalCheckBox.Checked;
            this.GlobalCheckBox.Text = this.global ? "Global" : "Local";

            if (this.global)
            {
                keyboardHook = new KeyboardHook(this.keyboard);
            }
            else
            {
                keyboardHook.Dispose();
                keyboardHook = null;
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            keyboard.KeyDown(e.KeyCode);
            e.Handled = true;
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            keyboard.KeyUp(e.KeyCode);
            e.Handled = true;
        }
    }
}
