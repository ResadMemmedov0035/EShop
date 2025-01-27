namespace EShop.Domain.Entities.Common;

public interface IAuditableEntity
{
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}