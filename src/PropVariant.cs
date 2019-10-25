using System;
using System.Runtime.InteropServices;
using RTPolicyConfigClientLib;

namespace ToggleLoudnessEqualization {
    [StructLayout(LayoutKind.Explicit)]
    class PropVariant {
        [FieldOffset(0)] public tag_inner_PROPVARIANT InnerPV;
        [FieldOffset(0)] public VarEnum Type;
        [FieldOffset(8)] public uint U4;
    }
}
