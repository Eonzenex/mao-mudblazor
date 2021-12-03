using System.Collections.Generic;
using System.Linq;
using mao_mudblazor_server.Shared.Structures;
using NAudio.Wave;

namespace mao_mudblazor_server.Shared.Controllers
{
    public class DeviceController
    {
        public static readonly HashSet<int> OutputDevices = new ();
        public static readonly HashSet<int> InputDevices = new ();
        
        
        public static ICollection<OutputDevice> GetOutputDevices()
        {
            Utils.ScanOutputDevices();
            var outputDevices = OutputDevices.Select(
                deviceId => new OutputDevice(deviceId, WaveOutEvent.GetCapabilities(deviceId).ProductName)
            ).ToList();

            return outputDevices;
        }

        public static OutputDevice GetOutputDevice(int deviceId)
        {
            if (!OutputDevices.Contains(deviceId))
            {
                var msg = $"OutputDevices does not contain device ID '{deviceId}'";
                Utils.Log(msg);
                throw new KeyNotFoundException(msg);
            }

            return new OutputDevice(deviceId, WaveOutEvent.GetCapabilities(deviceId).ProductName);
        }
        
        public static ICollection<InputDevice> GetInputDevices()
        {
            Utils.ScanInputDevices();
            var inputDevices = InputDevices.Select(
                deviceId => new InputDevice(deviceId, WaveInEvent.GetCapabilities(deviceId).ProductName)
            ).ToList();

            return inputDevices;
        }
        
        public static InputDevice GetInputDevice(int deviceId)
        {
            if (!InputDevices.Contains(deviceId))
            {
                var msg = $"InputDevices does not contain device ID '{deviceId}'";
                Utils.Log(msg);
                throw new KeyNotFoundException(msg);
            }

            return new InputDevice(deviceId, WaveInEvent.GetCapabilities(deviceId).ProductName);
        }
    }
}