namespace EShop.Application.Storages;

// TODO: I think we are only need file path as return type. FileName is unnecessary hence FileResult too.
public record FileResult(string FileName, string Path);
