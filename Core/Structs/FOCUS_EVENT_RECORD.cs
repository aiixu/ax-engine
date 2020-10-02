﻿using System.Runtime.InteropServices;

namespace Ax.Engine.Core
{
    public static partial class Native
    {
        /// <summary>
        ///  Describes a focus event in a console <see cref="INPUT_RECORD"/> structure. <b>These events are used internally and should be ignored.</b>
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct FOCUS_EVENT_RECORD
        {
            /// <summary>
            ///  Reserved.
            /// </summary>
            public uint bSetFocus;
        }
    }
}