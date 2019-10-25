using System;
using System.Runtime.InteropServices;
using RTPolicyConfigClientLib;

namespace ToggleLoudnessEqualization
{
    class Program
    {
        static readonly Guid guidEnhancements = new Guid("E0A941A0-88A2-4df5-8D6B-DD20BB06E8FB");
        const int propLoudnessEqualization = 4;

        static int Main(string[] args) {
            bool exitMode = false;

            try {
                if (args.Length > 0) {
                    string arg1 = args[0].ToLowerInvariant();
                    string arg2 = args.Length >= 2 ? args[1].ToLowerInvariant() : null;

                    if (arg1 == "toggle") {
                        SetLoudnessEqualization(!GetLoudnessEqualization());
                    }
                    else if (arg1 == "1" || arg1 == "on") {
                        SetLoudnessEqualization(true);
                    }
                    else if (arg1 == "0" || arg1 == "off") {
                        SetLoudnessEqualization(false);
                    }
                    else if (arg1 == "exit") {
                        exitMode = true;
                    }

                    if (arg2 == "exit") {
                        exitMode = true;
                    }
                }

                bool val = GetLoudnessEqualization();

                if (exitMode) return val ? 0 : 1;
                else Console.WriteLine(val ? 1 : 0);
            }
            catch {
                return -1;
            }

            return 0;
        }

        static bool GetLoudnessEqualization() {
            PropVariant pv = GetProperty(GetDefaultDeviceID(), EPKey.eFXKey, guidEnhancements, propLoudnessEqualization);    
            return pv.U4 != 0;
        }

        static void SetLoudnessEqualization(bool on) {
            PropVariant pv = new PropVariant();
            pv.Type = VarEnum.VT_UI4;
            pv.U4 = on ? 1u : 0u;
            SetProperty(GetDefaultDeviceID(), EPKey.eFXKey, guidEnhancements, propLoudnessEqualization, pv);
        }

        static string GetDefaultDeviceID() {
            return MiniCoreAudio.GetDefaultAudioDeviceID(MiniCoreAudio.DataFlow.Render, MiniCoreAudio.Role.Multimedia);
        }

        static PropVariant GetProperty(string devID, EPKey ek, Guid formatId, uint propertyId) {
            var config = new PolicyConfigClient();
            var propvar = new PropVariant();
            var pk = new _tagpropertykey();
            pk.fmtid = formatId;
            pk.pid = propertyId;
            config.GetPropertyValue(devID, EPKey.eFXKey, ref pk, ref propvar.InnerPV);
            return propvar;
        }

        static void SetProperty(string devID, EPKey ek, Guid formatId, uint propertyId, PropVariant pv) {
            var config = new PolicyConfigClient();
            var pk = new _tagpropertykey();
            pk.fmtid = guidEnhancements;
            pk.pid = propLoudnessEqualization;
            config.SetPropertyValue(devID, EPKey.eFXKey, ref pk, ref pv.InnerPV);
        }
    }
}
