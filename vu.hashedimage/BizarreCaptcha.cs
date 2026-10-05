namespace vu.hashedimage;

public class BizarreCaptcha
{
    public int Id { get; set; }
    
    public byte[] Hash { get; set; }
    
    public string StringHash { get; set; }
    
    public string Caption { get; set; }
    
    public byte[] Image { get; set; }
    
 
}