namespace mao.backend.Structures
{
    public class AudioGroup
    {
        public int GroupId { get; set; }
        public string Name { get; set; } = "New";
        public int DeviceId { get; set; } = -1;
        public bool DoChangeDevice { get; set; } = false;
        public Dictionary<int, StreamControls> Controls { get; set; } = new();

        
        public int AddAudio(string fileName)
        {
            var keysSorted = Controls.Keys.OrderBy(k => k).ToArray();
            var newId = keysSorted.Length == 0 ? 0 : keysSorted[^1] + 1;
            Controls[newId] = new StreamControls{FilePath = fileName};
            
            return newId;
        }
    }
}