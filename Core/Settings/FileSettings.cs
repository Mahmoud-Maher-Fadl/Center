namespace Core.Settings;

public static class FileSettings
{
    public const string ImagesPath = "assets/images";
    public const string AllowedExtension = ".jpg,.jpeg,.png";
    public const int MaxFileSizeInMb = 1;
    public const int MaxFileSizeInBytes = MaxFileSizeInMb * 1024 * 1024;
}