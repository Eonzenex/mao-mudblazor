namespace mao_mudblazor_server.Shared.Structures;

public class NewStreamControls
{
    // Fixed
    public string FilePath { get; set; }
    
    public bool Paused { get; set; } = true;
    public bool Loop { get; set; } = true;
    
    public float Progress { get; set; } = 0;
    public float Length { get; set; } = 0;
    public float Volume { get; set; } = 1;
    
    
    // Actions
    public bool Kill { get; set; } = false;
    public bool Restart { get; set; } = false;
    public bool Stop { get; set; } = false;
    public bool ChangeProgress { get; set; } = false;
}