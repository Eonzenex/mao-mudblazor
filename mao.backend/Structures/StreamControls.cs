#nullable enable
namespace mao.backend.Structures
{
    public class StreamControls
    {
        public bool Kill { get; set; } = false;
        public bool Paused { get; set; } = true;
        public bool Loop { get; set; } = false;
        public float Volume { get; set; } = 1;
        public string FilePath { get; set; } = "";
        
        public float Progress { get; set; } = 0;
        public float Length { get; set; } = 0;
        
        public bool DoRestart { get; set; } = false;
        public bool DoStop { get; set; } = false;
        public bool DoChangeProgress { get; set; } = false;
    }
}