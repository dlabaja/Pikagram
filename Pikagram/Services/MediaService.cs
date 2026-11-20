using Pikagram.Enums;
using SixLabors.ImageSharp;

namespace Pikagram.Services;

public class MediaService
{
    private IWebHostEnvironment _environment;
    private readonly string[] _imgFormats = ["jpeg", "jpg", "png", "webp", "avif", "gif", "bmp"];
    private readonly string[] _videoFormats = ["mp4", "webm", "mov", "mkv", "avi"];

    public MediaService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    
    public async Task<(string path, MediaType type)?> SaveMedia(IFormFile file, string mediaFolderName, string name)
    {
        var stream = file.OpenReadStream();
        var fileType = await FileTypeChecker.FileTypeValidator.GetFileTypeAsync(stream);
        if (_imgFormats.Contains(fileType.Name.ToLower()))
        {
            return await SaveImage(stream, mediaFolderName, name);
        }

        if (_videoFormats.Contains(fileType.Name.ToLower()))
        {
            return await SaveVideo(stream, name, mediaFolderName, fileType.Name.ToLower());
        }

        return null;
    }

    private async Task<(string path, MediaType type)?> SaveImage(Stream stream, string mediaFolderName, string name)
    {
        var path = Path.Combine(_environment.WebRootPath, mediaFolderName, $"{name}.webp");
        using var image = await Image.LoadAsync(stream);
        await image.SaveAsWebpAsync(path);
        return (path, MediaType.Image);
    }
    
    private async Task<(string path, MediaType type)?> SaveVideo(Stream stream, string mediaFolderName, string name, string extension)
    {
        // nechce se mi nic konvertovat, prostě to uložím (jinak bych byl závislej na ffmpeg, možná jindy)
        var path = Path.Combine(_environment.WebRootPath, mediaFolderName, $"{name}.{extension}");
        await using var fileStream = new FileStream(path, FileMode.Create);
        await stream.CopyToAsync(fileStream);
        return (path, MediaType.Video);
    }
}