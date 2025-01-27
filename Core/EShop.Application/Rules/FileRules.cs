using EShop.Domain.Exceptions;

namespace EShop.Application.Rules;

internal class FileRules
{
    public static void FileExtensionMustBe(string fileName, params string[] extensions)
    {
        string ext = Path.GetExtension(fileName).ToLower();
        if (extensions.All(e => e != ext))
            throw new BusinessException($"File extension must be: {string.Join(", ", extensions)}. File name: {fileName}.");
    }

    /// <summary>
    /// .png, .jpg, .jpeg, .bmp
    /// </summary>
    public static void FileExtensionMustBeImage(string fileName)
        => FileExtensionMustBe(fileName, ".png", ".jpg", ".jpeg", ".bmp");
}
