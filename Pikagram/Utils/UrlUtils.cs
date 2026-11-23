namespace Pikagram.Utils;

public static class UrlUtils
{
    public static string WithRandomQueryParam(string url)
    {
        return $"{url}?q={DateTime.UtcNow.Ticks}";
    }
}