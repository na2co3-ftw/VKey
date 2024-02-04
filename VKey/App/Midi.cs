using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace VKey.Midi
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct MIDIOUTCAPS
    {
        public ushort wMid;
        public ushort wPid;
        public uint vDriverVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szPname;
        public ushort wTechnology;
        public ushort wVoices;
        public ushort wNotes;
        public ushort wChannelMask;
        public uint dwSupport;
    }

    internal enum CallbackFlag : uint
    {
        CALLBACK_NULL = 0x00000000
    }

    internal static class NativeMethods
    {
        [DllImport("winmm.dll")]
        internal static extern uint midiOutGetNumDevs();

        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        internal static extern uint midiOutGetDevCaps(
            IntPtr uDeviceID,
            out MIDIOUTCAPS pmoc,
            uint cbmoc
        );

        [DllImport("winmm.dll")]
        internal static extern uint midiOutOpen(
            out MidiOutHandle phmo,
            uint uDeviceID,
            IntPtr dwCallback,
            IntPtr dwInstance,
            CallbackFlag fdwOpen
        );

        [DllImport("winmm.dll")]
        internal static extern uint midiOutClose(
            IntPtr hmo
        );

        [DllImport("winmm.dll")]
        internal static extern uint midiOutShortMsg(
            MidiOutHandle hmo,
            uint dwMsg
        );

        [DllImport("winmm.dll")]
        internal static extern uint midiOutReset(
            MidiOutHandle hmo
        );
    }

    internal class MidiOutHandle : SafeHandle
    {
        public MidiOutHandle() : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            NativeMethods.midiOutClose(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public class MidiOut : IDisposable
    {
        public static int GetDeviceNum()
        {
            return (int)NativeMethods.midiOutGetNumDevs();
        }

        public static string GetDeviceName(int deviceId)
        {
            NativeMethods.midiOutGetDevCaps((IntPtr)deviceId, out var midiOutCaps, (uint)Marshal.SizeOf<MIDIOUTCAPS>());
            return midiOutCaps.szPname;
        }

        public static IEnumerable<MidiDevice> GetDevices()
        {
            var devices = new List<MidiDevice>
            {
                new MidiDevice { Id = -1, Name = "Auto" }
            };

            devices.AddRange(
                Enumerable.Range(0, GetDeviceNum())
                .Select(id => new MidiDevice { Id = id, Name = GetDeviceName(id) }));
            return devices;
        }

        private readonly MidiOutHandle handle;

        public MidiOut(int deviceId)
        {
            NativeMethods.midiOutOpen(out handle, (uint)deviceId, IntPtr.Zero, IntPtr.Zero, CallbackFlag.CALLBACK_NULL);
        }

        public void Dispose()
        {
            if (handle != null && !handle.IsInvalid)
            {
                handle.Dispose();
            }
        }

        public void SendMessage(uint message)
        {
            if (handle == null || handle.IsInvalid)
            {
                return;
            }

            NativeMethods.midiOutShortMsg(handle, message);
        }

        public void SendMessage(MidiMessage message)
        {
            SendMessage(message.ToUint());
        }

        public void Reset()
        {
            if (handle == null || handle.IsInvalid)
            {
                return;
            }

            NativeMethods.midiOutReset(handle);
        }

        public void NoteOn(int channel, int note, int velocity)
        {
            SendMessage(new MidiMessage
            {
                Type = MidiMessageType.NoteOn,
                Channel = channel,
                Value1 = note,
                Value2 = velocity
            });
        }

        public void NoteOff(int channel, int note, int velocity = 64)
        {
            SendMessage(new MidiMessage
            {
                Type = MidiMessageType.NoteOff,
                Channel = channel,
                Value1 = note,
                Value2 = velocity
            });
        }
    }

    public class MidiDevice
    {
        public int Id;
        public string Name;
    }

    public enum MidiMessageType
    {
        NoteOff = 8,
        NoteOn = 9,
        PolyphnicAfterToutch = 10,
        ControlChange = 11,
        ProgramChange = 12,
        ChannelPressure = 13,
        PitchBend = 14
    }

    public struct MidiMessage
    {
        public MidiMessageType Type;
        public int Channel;
        public int Value1;
        public int Value2;

        public uint ToUint()
        {
            if (Channel <= 0 || 16 < Channel)
            {
                throw new ApplicationException();
            }
            if (Value1 < 0 || 128 <= Value1)
            {
                throw new ApplicationException();
            }
            if (Value2 < 0 || 128 <= Value2)
            {
                throw new ApplicationException();
            }

            return ((uint)Value2 << 16) | ((uint)Value1 << 8) | ((uint)Type << 4) | ((uint)Channel - 1);
        }
    }
}
