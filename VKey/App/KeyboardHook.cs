using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VKey.Hook
{
    internal enum HookType : int
    {
        WH_KEYBOARD_LL = 13
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class KBDLLHOOKSTRUCT
    {
        public uint vkCode;
        public uint scanCode;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    internal static class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern HookHandle SetWindowsHookEx(
            HookType idHook,
            HookProc lpfn,
            IntPtr hmod,
            uint dwThreadId
        );

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UnhookWindowsHookEx(
            IntPtr hhk
        );

        [DllImport("user32.dll")]
        internal static extern IntPtr CallNextHookEx(
            HookHandle hhk,
            int nCode,
            IntPtr wParam,
            IntPtr lParam
        );

        internal delegate IntPtr HookProc(
            int nCode,
            IntPtr wParam,
            IntPtr lParam
        );
    }

    internal class HookHandle : SafeHandle
    {
        private HookHandle() : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => this.handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            var isSucceeded= NativeMethods.UnhookWindowsHookEx(this.handle);
            if (isSucceeded)
            {
                this.handle = IntPtr.Zero;
            }
            return isSucceeded;
        }
    }

    public class KeyboardHook : IDisposable
    {
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;

        private readonly HashSet<Keys> KeyFilter = new HashSet<Keys>
        {
            Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0, Keys.OemMinus, Keys.Oem7, Keys.Oem5,
            Keys.Q, Keys.W, Keys.E, Keys.R, Keys.T, Keys.Y, Keys.U, Keys.I, Keys.O, Keys.P, Keys.Oemtilde, Keys.OemOpenBrackets,
            Keys.A, Keys.S, Keys.D, Keys.F, Keys.G, Keys.H, Keys.J, Keys.K, Keys.L, Keys.Oemplus, Keys.Oem1, Keys.Oem6,
            Keys.Z, Keys.X, Keys.C, Keys.V, Keys.B, Keys.N, Keys.M, Keys.Oemcomma, Keys.OemPeriod, Keys.Oemplus, Keys.OemQuestion, Keys.OemBackslash,
            Keys.Up, Keys.Down, Keys.Left, Keys.Right
        };

        private HookHandle handle;
        private NativeMethods.HookProc proc;

        private ComputerKeyboard keyboard;

        public KeyboardHook(ComputerKeyboard keyboard)
        {
            this.keyboard = keyboard;
            this.proc = KeyboardProc;

            this.handle = NativeMethods.SetWindowsHookEx(HookType.WH_KEYBOARD_LL, this.proc, IntPtr.Zero, 0);
        }

        public void Dispose()
        {
            if (this.handle != null && !this.handle.IsInvalid)
            {
                this.handle.Dispose();
            }
        }

        private IntPtr KeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return NativeMethods.CallNextHookEx(this.handle, nCode, wParam, lParam);
            }

            if (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
            {
                var keyInfo = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);
                if (KeyFilter.Contains((Keys)keyInfo.vkCode))
                {
                    this.keyboard.KeyDown((Keys)keyInfo.vkCode);
                    return (IntPtr)1;
                }
            } else if (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP)
            {
                var keyInfo = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);
                if (KeyFilter.Contains((Keys)keyInfo.vkCode))
                {
                    this.keyboard.KeyUp((Keys)keyInfo.vkCode);
                    return (IntPtr)1;
                }
            }

            return NativeMethods.CallNextHookEx(this.handle, nCode, wParam, lParam);
        }
    }
}
