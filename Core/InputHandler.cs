﻿using Ax.Engine.Utils;
using System;
using System.Collections.Generic;

using static Ax.Engine.Core.Native;

namespace Ax.Engine.Core
{
    public sealed class InputHandler
    {
        public const short KEY_SCREENSHOT = (short)KEY.F2;

        public IntPtr Handle { get => handle; }

        private IntPtr handle;
        private CONSOLE_MODE_INPUT inLast;

        private static readonly Dictionary<string, Axis> axises = new Dictionary<string, Axis>();

        private static List<KEY_EVENT_RECORD> LAST_KEY_EVENTS = new List<KEY_EVENT_RECORD>();

        public bool Enable()
        {
            if (!GetStdIn(out handle)) { return false; }
            if (!GetConsoleModeOut(Handle, out inLast)) { return false; }

            CONSOLE_MODE_INPUT mode = inLast | CONSOLE_MODE_INPUT.ENABLE_VIRTUAL_TERMINAL_INPUT;

            return SetConsoleMode(Handle, (uint)mode);
        }

        public bool Disable()
        {
            handle = IntPtr.Zero;
            return GetStdIn(out IntPtr hIn) && SetConsoleMode(hIn, (uint)inLast);
        }
        
        public uint Read(out INPUT_RECORD[] rec)
        {
            GetNumberOfConsoleInputEvents(Handle, out uint numberOfEvents);

            rec = new INPUT_RECORD[numberOfEvents];
            ReadConsoleInput(Handle, rec, numberOfEvents, out uint numberOfEventRead);

            return numberOfEventRead;
        }

        public uint Peek(out INPUT_RECORD[] rec)
        {
            GetNumberOfConsoleInputEvents(Handle, out uint numberOfEvents);

            rec = new INPUT_RECORD[numberOfEvents];
            PeekConsoleInput(Handle, rec, numberOfEvents, out uint numberOfEventRead);

            return numberOfEventRead;
        }

        private bool GetStdIn(out IntPtr handle)
        {
            handle = GetStdHandle((uint)HANDLE.STD_INPUT_HANDLE);
            return handle != INVALID_HANDLE;
        }

        private bool GetConsoleModeOut(IntPtr hConsoleHandle, out CONSOLE_MODE_INPUT mode)
        {
            if (!GetConsoleMode(hConsoleHandle, out uint lpMode))
            {
                mode = 0;
                return false;
            }

            mode = (CONSOLE_MODE_INPUT)lpMode;
            return true;
        }

        public static void RegisterAxis(Axis axis, bool overrideIfExists = false)
        {
            if(overrideIfExists || !axises.ContainsKey(axis.name))
            {
                axises[axis.name] = axis;
            }
        }

        public static Vector2 GetMousePosition()
        {
            return Vector2.Zero;
        }

        public static bool GetMouseButtonDown(MOUSE_BUTTON_STATE button)
        {
            return false;
        }

        public static bool GetMouseButton(MOUSE_BUTTON_STATE button)
        {
            return false;
        }

        public static bool GetMouseButtonUp(MOUSE_BUTTON_STATE button)
        {
            return false;
        }

        public static bool GetKeyDown(KEY key)
        {
            return false;
        }

        public static bool GetKey(KEY key)
        {
            return false;
        }

        public static bool GetKeyUp(KEY key)
        {
            return false;
        }

        public sealed class Axis
        {
            public string name;

            public KEY positiveKey;
            public KEY negativeKey;

            public KEY alternativePositiveKey;
            public KEY alternativeNegativeKey;
        }
    }
}
