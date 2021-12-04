using mao.backend.Controllers;
using NAudio.WinMM.MmeInterop;

namespace mao.backend
{
    public enum LogLevel
    {
        Info = 1,
        Success = 0,
        Warn = -1,
        Error = -2
    }
    
    public static class Utils
    {
        public static int ThreadSleep = 100;
        public static float FloatComparisonThreshold = 0.000001f;
        public static ICollection<string> AudioSourcePaths { get; set; } = new List<string>();
        
        public static ICollection<string> GetThisAudioSource(string filePath, params string[] extensions)
        {
            if (!Directory.Exists(filePath)) throw new DirectoryNotFoundException();
            if (extensions == null) throw new ArgumentNullException(nameof(extensions));

            var allFiles = new List<string>();
            foreach (var extension in extensions)
            {
                allFiles.AddRange(Directory.GetFiles(filePath, extension, SearchOption.AllDirectories));
            }

            return allFiles;
        }
        
        public static ICollection<string> GetAllAudioSources(params string[] extensions)
        {
            var allFiles = new List<string>();
            foreach (var audioSourcePath in AudioSourcePaths)
            {
                if (!Directory.Exists(audioSourcePath)) continue;
                allFiles.AddRange(GetThisAudioSource(audioSourcePath, extensions));
            }

            return allFiles;
        }

        public static void ScanOutputDevices()
        {
            DeviceController.OutputDevices.Clear();
            
            for (var i = -1; i < WaveInterop.waveOutGetNumDevs(); i++)
            {
                DeviceController.OutputDevices.Add(i);
            }
        }
        
        public static void ScanInputDevices()
        {
            DeviceController.InputDevices.Clear();
            
            for (var i = -1; i < WaveInterop.waveInGetNumDevs(); i++)
            {
                DeviceController.InputDevices.Add(i);
            }
        }

        public static bool SafeFloatEquality(float a, float b)
        {
            return Math.Abs(a - b) < FloatComparisonThreshold;
        }

        public static void Log(string msg, LogLevel logLevel = LogLevel.Info)
        {
            Console.ForegroundColor = logLevel switch
            {
                LogLevel.Success => ConsoleColor.Green,
                LogLevel.Warn => ConsoleColor.Yellow,
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Info => ConsoleColor.White,
                _ => ConsoleColor.White
            };
            Console.WriteLine($"{logLevel.ToString().ToUpper()}: {msg}");
        }
    }
}