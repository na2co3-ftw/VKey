using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKey
{
    public class MusicalKeyboard
    {
        public delegate void TransposeChangedEventHandler(int octave, int transpose);

        public event TransposeChangedEventHandler TransposeChanged;
        public int Channel { get; set; } = 1;
        public int Velocity { get; set; } = 100;
        public int Octave { get; set; } = 4;
        public int Transpose { get; set; } = 0;

        private Midi.MidiOut midiOut;

        public MusicalKeyboard()
        {
        }

        public void Reset()
        {
            Octave = 4;
            Transpose = 0;
            TransposeChanged(Octave, Transpose);
        }

        public void SetMidiOut(Midi.MidiOut midiOut)
        {
            this.midiOut = midiOut;
        }

        public void NoteOn(int note)
        {
            var transposed = GetTransposed(note);
            if (!transposed.HasValue)
            {
                return;
            }

            midiOut?.NoteOn(Channel, transposed.Value, Velocity);
        }

        public void NoteOff(int note)
        {
            var transposed = GetTransposed(note);
            if (!transposed.HasValue)
            {
                return;
            }

            midiOut.NoteOff(Channel, transposed.Value);
        }

        public void OctaveUp()
        {
            Octave++;
            if (Octave > 8)
            {
                Octave = 8;
            }

            TransposeChanged(Octave, Transpose);
        }

        public void OctaveDown()
        {
            Octave--;
            if (Octave < -1)
            {
                Octave = -1;
            }

            TransposeChanged(Octave, Transpose);
        }

        public void TransposeUp()
        {
            Transpose++;
            if (Transpose >= 12)
            {
                Octave++;
                Transpose -= 12;
            }

            TransposeChanged(Octave, Transpose);
        }

        public void TransposeDown()
        {
            Transpose--;
            if (Transpose <= -12)
            {
                Octave--;
                Transpose += 12;
            }

            TransposeChanged(Octave, Transpose);
        }

        private int? GetTransposed(int note)
        {
            var transposed = note + Octave * 12 + Transpose;
            if (transposed < 0 || 128 <= transposed)
            {
                return null;
            }

            return transposed;
        }
    }
}
