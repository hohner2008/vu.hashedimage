namespace vu.hashedimage;

public class BizarreCaptcha
{
    public int Id { get; set; }
    
    public ulong Average { get; set; }
    
    public ulong Difference { get; set; }
    
    public ulong Perceptual { get; set; }
    
    public byte[] ImageData { get; set; }
    
    public string Caption { get; set; }
}