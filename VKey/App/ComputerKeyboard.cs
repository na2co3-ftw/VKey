using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VKey
{
    public class ComputerKeyboard
    {
        public static readonly Dictionary<Keys, int> KeyMap = new Dictionary<Keys, int>
        {
            {Keys.Z, 60},
            {Keys.S, 61},
            {Keys.X, 62},
            {Keys.D, 63},
            {Keys.C, 64},
            {Keys.V, 65},
            {Keys.G, 66},
            {Keys.B, 67},
            {Keys.H, 68},
            {Keys.N, 69},
            {Keys.J, 70},
            {Keys.M, 71},
            {Keys.Oemcomma, 72}, // ,
            {Keys.L, 73},
            {Keys.OemPeriod, 74}, // .
            {Keys.Oemplus, 75}, // ;
            {Keys.OemQuestion, 76}, // /
            {Keys.OemBackslash, 77}, // \
            {Keys.OemCloseBrackets, 78}, // ]

            {Keys.Q, 72},
            {Keys.D2, 73},
            {Keys.W, 74},
            {Keys.D3, 75},
            {Keys.E, 76},
            {Keys.R, 77},
            {Keys.D5, 78},
            {Keys.T, 79},
            {Keys.D6, 80},
            {Keys.Y, 81},
            {Keys.D7, 82},
            {Keys.U, 83},
            {Keys.I, 84},
            {Keys.D9, 85},
            {Keys.O, 86},
            {Keys.D0, 87},
            {Keys.P, 88},
            {Keys.Oemtilde, 89}, // @
            {Keys.Oem7, 90}, // ^
            {Keys.OemOpenBrackets, 91}, // [
        };

        private Dictionary<Keys, bool> keyStates = new Dictionary<Keys, bool>();

        private Midi.MidiOut midiOut;
        private int channel;

        public ComputerKeyboard(Midi.MidiOut midiOut, int channel)
        {
            this.midiOut = midiOut;
            this.channel = channel;

            Reset();
        }

        public void KeyDown(Keys keyCode)
        {
            if (!KeyMap.ContainsKey(keyCode))
            {
                return;
            }

            if (keyStates[keyCode] == false)
            {
                midiOut.NoteOn(channel, KeyMap[keyCode], 100);
            }
            keyStates[keyCode] = true;
        }

        public void KeyUp(Keys keyCode)
        {
            if (!KeyMap.ContainsKey(keyCode))
            {
                return;
            }

            if (keyStates[keyCode] == true)
            {
                midiOut.NoteOff(channel, KeyMap[keyCode]);
            }
            keyStates[keyCode] = false;
        }

        public void Reset()
        {
            foreach (var key in KeyMap.Keys)
            {
                keyStates[key] = false;
            }
        }
    }
}
