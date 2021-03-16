using System;
using OpenFlowCore.Nodes;
using NAudio.CoreAudioApi;
using System.Diagnostics;

namespace WindowsVolumeControlNodes
{
    public class Change_System_Volume : NodeBase
    {
        NodeField<double> volume = new NodeField<double>("Volume", NodeFieldType.Input, 0.5);

        MMDeviceEnumerator _deviceEnum = new MMDeviceEnumerator();
        MMDevice _playbackDevice;

        public Change_System_Volume() : base()
        {
            _playbackDevice = _deviceEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
        }

        public override void Evaluate()
        {
            if (volume > 0 && volume < 1)
            {
                Debug.WriteLine(volume);
                _playbackDevice.AudioEndpointVolume.MasterVolumeLevelScalar = Convert.ToSingle(volume);
            }
        }
    }
}