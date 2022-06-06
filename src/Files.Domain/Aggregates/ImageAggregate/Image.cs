using System.Net.Mime;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Files.Domain.Aggregates.ImageAggregate;

public class Image : EntityBase<ImageId>, IAggregateRoot
{
    private const string BucketName = "images";
    private const string OriginalSizeName = "original";
    private const string LargeSizeName = "large";
    private const string MediumSizeName = "medium";
    private const string SmallSizeName = "small";
    private const string ExtraSmallSizeName = "extra-small";
    private const string MiniSizeName = "mini";
    private const string DefaultExtension = ".jpeg";
    private const int LargeSize = 2000;
    private const int MediumSize = 1000;
    private const int SmallSize = 500;
    private const int ExtraSmallSize = 250;
    private const int MiniSize = 100;

    private static readonly ImageValidator Validator = new();
    
    protected Image(
        string bucketName,
        string key,
        string contentType,
        byte[] content)
    {
        Id = new ImageId(bucketName, key);
        ContentType = contentType;
        Content = content;
    }
    
    public string ContentType { get; private set; }

    public byte[] Content { get; private set; }

    public static Image CreateOriginal(
        string name,
        string contentType,
        byte[] content)
    {
        var groupId = Guid.NewGuid().ToString().ToLower();
        var extension = Path.GetExtension(name);
        var key = CreateKey(groupId, OriginalSizeName, extension);
        var image = new Image(BucketName, key, contentType, content);
        Validator.ValidateEntityAndThrow(image);
        return image;
    }
    
    public static ImageId CreateOriginalId(string groupId)
        => new(BucketName, CreateKey(groupId, OriginalSizeName, DefaultExtension));
    
    public static ImageId CreateId(string groupId, string sizeName)
        => new(BucketName, CreateKey(groupId, sizeName, DefaultExtension));
    
    public string GetGroupId() => Id.Key.Split("/").First();

    public Image CreateLarge()
    {
        var key = CreateKey(LargeSizeName);
        var image = new Image(BucketName, key, ContentType, Content.ToArray());
        image.Process(LargeSize, LargeSize);
        return image;
    }

    public Image CreateMedium()
    {
        var key = CreateKey(MediumSizeName);
        var image = new Image(BucketName, key, ContentType, Content.ToArray());
        image.Process(MediumSize, MediumSize);
        return image;
    }
    
    public Image CreateSmall()
    {
        var key = CreateKey(SmallSizeName);
        var image = new Image(BucketName, key, ContentType, Content.ToArray());
        image.Process(SmallSize, SmallSize);
        return image;
    }
    
    public Image CreateExtraSmall()
    {
        var key = CreateKey(ExtraSmallSizeName);
        var image = new Image(BucketName, key, ContentType, Content.ToArray());
        image.Process(ExtraSmallSize, ExtraSmallSize);
        return image;
    }
    
    public Image CreateMini()
    {
        var key = CreateKey(MiniSizeName);
        var image = new Image(BucketName, key, ContentType, Content.ToArray());
        image.Process(MiniSize, MiniSize);
        return image;
    }

    private void Process(int width, int height)
    {
        using var image = SixLabors.ImageSharp.Image.Load(Content);
        image.Metadata.ExifProfile = null;
        image.Metadata.XmpProfile = null;

        if (width > image.Width)
            width = image.Width;

        if (height > image.Height)
            height = image.Height;
        
        image.Mutate(x => x.Resize(width, height));
        
        using var memoryStream = new MemoryStream();
        image.SaveAsJpeg(memoryStream);

        ContentType = MediaTypeNames.Image.Jpeg;
        Content = memoryStream.ToArray();
    }

    private string CreateKey(string sizeName)
        => CreateKey(GetGroupId(), sizeName, Path.GetExtension(Id.Key));

    private static string CreateKey(string groupId, string sizeName, string extension)
        => $"{groupId}/{sizeName}{extension}";
}