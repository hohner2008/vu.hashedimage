using System.Runtime.CompilerServices;

namespace vu.hashedimage;

public class DownloadsWatcher
{
    private FileSystemWatcher _watcher;
    private string fileName = string.Empty;

    public string FileName
    {
        get => fileName;
        set => fileName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public event EventHandler<FileDownloadedEventArgs>? FileDownloaded;
    
    public DownloadsWatcher()
    {
        _watcher = new FileSystemWatcher(Utils.GetDownloadFolder());
    }

    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        var path = e.FullPath;
        var currentFileName = Path.GetFileNameWithoutExtension(path);
        var condition = currentFileName.Contains(fileName);
        string tempDirectory = Path.GetTempPath();

        try
        {
            File.Move(path, tempDirectory + "captcha", true);
            FileDownloadedEventArgs ev = new FileDownloadedEventArgs
            {
                Path = tempDirectory + "captcha"
            };
            FileDownloaded?.Invoke(this, ev);
        }
        catch (Exception ex)
        {
            Console.Out.WriteLine(ex.Message);
        }

    }
}

public class FileDownloadedEventArgs : EventArgs
{
    public string Path { get; set; }
}