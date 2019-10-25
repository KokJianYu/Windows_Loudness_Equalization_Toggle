using System;
using System.Runtime.InteropServices;

namespace ToggleLoudnessEqualization {
    class MiniCoreAudio {
        public static string GetDefaultAudioDeviceID(DataFlow dataFlow, Role role) {
            IMMDeviceEnumerator devenum = new MMDeviceEnumeratorComObject() as IMMDeviceEnumerator;
            IMMDevice endpoint;
            devenum.GetDefaultAudioEndpoint(dataFlow, role, out endpoint);
            string devID;
            Marshal.ThrowExceptionForHR(endpoint.GetId(out devID));
            Marshal.FinalReleaseComObject(endpoint);
            Marshal.FinalReleaseComObject(devenum);
            return devID;
        }

        [ComImport, Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")] class MMDeviceEnumeratorComObject { }

        [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IMMDeviceEnumerator {
            int EnumAudioEndpoints(DataFlow dataFlow, DeviceState stateMask,
                out IntPtr devices);
            int GetDefaultAudioEndpoint(DataFlow dataFlow, Role role, out IMMDevice endpoint);
            int GetDevice(string id, out IMMDevice deviceName);
            int RegisterEndpointNotificationCallback(IntPtr client);
            int UnregisterEndpointNotificationCallback(IntPtr client);
        }

        [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IMMDevice {
            int Activate(ref Guid id, uint clsCtx, IntPtr activationParams,
                [MarshalAs(UnmanagedType.IUnknown)] out object interfacePointer);
            int OpenPropertyStore(uint stgmAccess, out IntPtr properties);
            int GetId([MarshalAs(UnmanagedType.LPWStr)] out string id);
            int GetState(out DeviceState state);
        }

        public enum DataFlow { Render, Capture, All };
        public enum Role { Console, Multimedia, Communications }
        [Flags] enum DeviceState { Active = 1, Unplugged = 2, NotPresent = 4, All = 7 }
    }
}
