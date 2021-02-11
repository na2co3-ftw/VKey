using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKey
{
    public class MusicalKeyboard
    {
        public int Channel { get; set; } = 1;
        public int Velocity { get; set; } = 100;

        private Midi.MidiOut midiOut;

        public MusicalKeyboard()
        {
        }

        public void SetMidiOut(Midi.MidiOut midiOut)
        {
            this.midiOut = midiOut;
        }

        public void NoteOn(int note)
        {
            midiOut?.NoteOn(Channel, note, Velocity);
        }

        public void NoteOff(int note)
        {
            midiOut.NoteOff(Channel, note);
        }
    }
}
