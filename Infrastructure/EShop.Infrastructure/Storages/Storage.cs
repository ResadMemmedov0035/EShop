namespace EShop.Infrastructure.Storages
{
    public abstract class Storage
    {
        protected static string RenameFile(string fileName)
        {
            // Example: 20230417234539-c21459efd42a78gf.png
            return $"{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..16]}{Path.GetExtension(fileName).ToLower()}";
        }
    }
}
