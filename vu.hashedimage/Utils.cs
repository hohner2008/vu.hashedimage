using System.Runtime.InteropServices;
using System.Text;

namespace vu.hashedimage;

public static class Utils
{
    public static async Task<Memory<byte>> FileToMemoryAsync(string path)
    {
        FileStream stream = File.Open(path, FileMode.Open);
        Memory<byte> buffer = new byte[stream.Length]; 
        int count = await stream.ReadAsync(buffer);
        stream.Close();
        return buffer;
    }
    
    public static string GetDownloadFolder()
    {
        var user = Environment.UserName;
        StringBuilder builder = new StringBuilder();
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {   
            builder.Append("/home/");
            builder.Append(user);
            builder.Append("/Downloads");
            
        } else
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {   
            // Получаем путь к системной папке (например, "C:\Windows\System32")
            string systemDir = Environment.SystemDirectory;

            // Извлекаем корень диска (например, "C:\")
            string rootDrive = Path.GetPathRoot(systemDir);
            
            builder.Append(rootDrive);
            builder.Append("Users\\");
            builder.Append(user);
            builder.Append("\\Downloads");
        } else 
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            builder.Append("/Users/");
            builder.Append(user);
            builder.Append("/Downloads");
        } else
        
        {
            throw new PlatformNotSupportedException("This platform is not supported!");
        }
        return builder.ToString();
    }
}