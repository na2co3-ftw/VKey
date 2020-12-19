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
            {Keys.Z, 12},
            {Keys.S, 13},
            {Keys.X, 14},
            {Keys.D, 15},
            {Keys.C, 16},
            {Keys.V, 17},
            {Keys.G, 18},
            {Keys.B, 19},
            {Keys.H, 20},
            {Keys.N, 21},
            {Keys.J, 22},
            {Keys.M, 23},
            {Keys.Oemcomma, 24}, // ,
            {Keys.L, 25},
            {Keys.OemPeriod, 26}, // .
            {Keys.Oemplus, 27}, // ;
            {Keys.OemQuestion, 28}, // /
            {Keys.OemBackslash, 29}, // \
            {Keys.OemCloseBrackets, 30}, // ]

            {Keys.Q, 24},
            {Keys.D2, 25},
            {Keys.W, 26},
            {Keys.D3, 27},
            {Keys.E, 28},
            {Keys.R, 29},
            {Keys.D5, 30},
            {Keys.T, 31},
            {Keys.D6, 32},
            {Keys.Y, 33},
            {Keys.D7, 34},
            {Keys.U, 35},
            {Keys.I, 36},
            {Keys.D9, 37},
            {Keys.O, 38},
            {Keys.D0, 39},
            {Keys.P, 40},
            {Keys.Oemtilde, 41}, // @
            {Keys.Oem7, 42}, // ^
            {Keys.OemOpenBrackets, 43}, // [
        };

        private Dictionary<Keys, bool> keyStates = new Dictionary<Keys, bool>();

        private MusicalKeyboard musicalKeyboard;

        public ComputerKeyboard(MusicalKeyboard musicalKeyboard)
        {
            this.musicalKeyboard = musicalKeyboard;

            Reset();
        }

        public void Reset()
        {
            foreach (var key in KeyMap.Keys)
            {
                keyStates[key] = false;
            }
        }


        public void KeyDown(Keys keyCode)
        {
            if (!KeyMap.ContainsKey(keyCode))
            {
                ProcessKeyPress(keyCode);
                return;
            }

            if (keyStates[keyCode] == false)
            {
                musicalKeyboard.NoteOn(KeyMap[keyCode]);
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
                musicalKeyboard.NoteOff(KeyMap[keyCode]);
            }
            keyStates[keyCode] = false;
        }

        private void ProcessKeyPress(Keys keyCode)
        {
            switch(keyCode)
            {
                case Keys.Up:
                    musicalKeyboard.OctaveUp();
                    break;
                case Keys.Down:
                    musicalKeyboard.OctaveDown();
                    break;
                case Keys.Right:
                    musicalKeyboard.TransposeUp();
                    break;
                case Keys.Left:
                    musicalKeyboard.TransposeDown();
                    break;
            }
        }
    }
}
