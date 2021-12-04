namespace mao.backend.Structures
{
    public class AudioDevice
    {
        public AudioDevice(int id, string name)
        {
            Id = id;
            Name = name;
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
    }
    
    public class OutputDevice : AudioDevice
    {
        public OutputDevice(int id, string name) : base(id, name)
        {
        }
    }
    
    public class InputDevice : AudioDevice
    {
        public InputDevice(int id, string name) : base(id, name)
        {
        }
    }
}