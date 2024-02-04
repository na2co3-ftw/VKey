using System.Collections.Generic;
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
            {Keys.Oem5, 44}, // ¥
        };

        private struct KeyState
        {
            public bool pressed;
            public int? note;
        }

        private readonly Dictionary<Keys, KeyState> keyStates = new Dictionary<Keys, KeyState>();

        private readonly MusicalKeyboard musicalKeyboard;
        private readonly Transposer transposer;

        public ComputerKeyboard(MusicalKeyboard musicalKeyboard, Transposer transposer)
        {
            this.musicalKeyboard = musicalKeyboard;
            this.transposer = transposer;

            Reset();
        }

        public void Reset()
        {
            foreach (var key in KeyMap.Keys)
            {
                keyStates[key] = new KeyState { pressed = false, note = null };
            }
        }


        public void KeyDown(Keys keyCode)
        {
            if (!KeyMap.ContainsKey(keyCode))
            {
                ProcessKeyPress(keyCode);
                return;
            }

            if (keyStates[keyCode].pressed == false)
            {
                var note = transposer.GetTransposedNote(KeyMap[keyCode]);
                if (note.HasValue)
                {
                    musicalKeyboard.NoteOn(note.Value);
                }
                keyStates[keyCode] = new KeyState { pressed = true, note = note };
            }
        }

        public void KeyUp(Keys keyCode)
        {
            if (!KeyMap.ContainsKey(keyCode))
            {
                return;
            }

            if (keyStates[keyCode].pressed == true)
            {
                var note = keyStates[keyCode].note;
                if (note.HasValue)
                {
                    musicalKeyboard.NoteOff(note.Value);
                }
            }
            keyStates[keyCode] = new KeyState { pressed = false, note = null };
        }

        private void ProcessKeyPress(Keys keyCode)
        {
            switch(keyCode)
            {
                case Keys.Up:
                    transposer.OctaveUp();
                    break;
                case Keys.Down:
                    transposer.OctaveDown();
                    break;
                case Keys.Right:
                    transposer.TransposeUp();
                    break;
                case Keys.Left:
                    transposer.TransposeDown();
                    break;
            }
        }
    }
}
