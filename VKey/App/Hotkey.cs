﻿using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VKey.Hotkey
{
    internal static class NativeMethods
    {
        internal const uint MOD_ALT = 0x0001;
        internal const uint MOD_CONTROL = 0x0002;
        internal const uint MOD_SHIFT = 0x0004;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool RegisterHotKey(
            IntPtr hWnd,
            int id,
            uint fsModifiers,
            uint vk
        );

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UnregisterHotKey(
            IntPtr hWnd,
            int id
        );
    }

    public class HotkeyRegistration : IDisposable
    {
        private const int WM_HOTKEY = 0x0312;

        static int lastId = 0;

        private readonly IntPtr handle;
        public readonly int id;

        public HotkeyRegistration(
            IntPtr handle,
            Keys key,
            bool altKey = false,
            bool controlKey = false,
            bool shiftKey = false)
        {
            id = ++lastId;
            this.handle = handle;

            uint modifiers = 0;
            if (altKey)
            {
                modifiers |= NativeMethods.MOD_ALT;
            }
            if (controlKey)
            {
                modifiers |= NativeMethods.MOD_CONTROL;
            }
            if (shiftKey)
            {
                modifiers |= NativeMethods.MOD_SHIFT;
            }

            NativeMethods.RegisterHotKey(handle, id, modifiers, (uint)key);
        }

        public bool IsPressed(Message message)
        {
            if (message.Msg != WM_HOTKEY)
            {
                return false;
            }

            if ((int)message.WParam != id)
            {
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            NativeMethods.UnregisterHotKey(handle, id);
        }
    }
}
