namespace SeaEco.Reporter.Models;

public sealed class FileModel
{
    public byte[] Content { get; set; } = [];
    public string ContentType { get; set; } = "application/octet-stream";
    public string DownloadName { get; set; } = string.Empty;
}