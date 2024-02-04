using VKey.Midi;

namespace VKey
{
    public class MusicalKeyboard
    {
        public int Channel { get; set; } = 1;
        public int Velocity { get; set; } = 100;

        private MidiOut midiOut;

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
