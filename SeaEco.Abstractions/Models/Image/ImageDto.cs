namespace SeaEco.Abstractions.Models.Image;

public class ImageDto
{
    public Guid Id { get; set; }
    public string Path { get; set; } = String.Empty;
    public bool Silt { get; set; }
    public DateTime UploadDate { get; set; }
}