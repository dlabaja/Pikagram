using Pikagram.Enums;
using SixLabors.ImageSharp;

namespace Pikagram.Services;

public class MediaService
{
    private readonly IWebHostEnvironment _environment;
    private readonly string[] _imgFormats = ["jpeg", "jpg", "png", "webp", "avif", "gif", "bmp"];
    private readonly string[] _videoFormats = ["mp4", "webm", "mov", "mkv", "avi"];

    public MediaService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    
    public async Task<(string relpath, MediaType type)?> SaveMedia(IFormFile file, string mediaFolderName, string name)
    {
        var stream = file.OpenReadStream();
        var fileType = await FileTypeChecker.FileTypeValidator.GetFileTypeAsync(stream);
        stream.Position = 0;
        if (_imgFormats.Contains(fileType.Extension.ToLower()))
        {
            return await SaveImage(stream, mediaFolderName, name);
        }

        if (_videoFormats.Contains(fileType.Extension.ToLower()))
        {
            return await SaveVideo(stream, mediaFolderName, name, fileType.Extension.ToLower());
        }

        return null;
    }

    public void RemoveMedia(string relPath)
    {
        // čekal bych že tohle bude asynchronní
        File.Delete(Path.Combine(_environment.WebRootPath, relPath));
    }

    private async Task<(string relpath, MediaType type)?> SaveImage(Stream stream, string mediaFolderName, string name)
    {
        var relPath = Path.Combine("media", mediaFolderName, $"{name}.webp");
        var path = Path.Combine(_environment.WebRootPath, relPath);
        using var image = await Image.LoadAsync(stream);
        await image.SaveAsWebpAsync(path);
        return ("/" + relPath, MediaType.Image);
    }
    
    private async Task<(string relpath, MediaType type)?> SaveVideo(Stream stream, string mediaFolderName, string name, string extension)
    {
        // nechce se mi nic konvertovat, prostě to uložím (jinak bych byl závislej na ffmpeg, možná jindy)
        var relPath = Path.Combine("media", mediaFolderName, $"{name}.{extension}");
        var path = Path.Combine(_environment.WebRootPath, relPath);
        await using var fileStream = new FileStream(path, FileMode.Create);
        await stream.CopyToAsync(fileStream);
        return ("/" + relPath, MediaType.Video);
    }
}