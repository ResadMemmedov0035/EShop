using EShop.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Domain.Entities;

public class File : Entity<Guid>, IAuditableEntity
{
    public string Path { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    [NotMapped]
    public DateTime? LastModified { get; set; }
}
