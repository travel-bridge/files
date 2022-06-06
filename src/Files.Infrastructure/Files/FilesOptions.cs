using System.ComponentModel.DataAnnotations;

namespace Files.Infrastructure.Files;

public class FilesOptions
{
    public const string SectionKey = "Files";

    [Required]
    public string AccessKey { get; set; } = null!;

    [Required]
    public string SecretAccessKey { get; set; } = null!;

    [Required]
    public string ServiceUrl { get; set; } = null!;
}