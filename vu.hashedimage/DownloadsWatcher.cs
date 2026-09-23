namespace vu.hashedimage;

public class DownloadsWatcher
{
    private FileSystemWatcher _watcher;
    
    
    public DownloadsWatcher()
    {
        _watcher = new FileSystemWatcher(Utils.GetDownloadFolder());
    }
}