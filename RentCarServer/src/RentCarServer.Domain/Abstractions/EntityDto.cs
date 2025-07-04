namespace RentCarServer.Domain.Abstractions;
public abstract class EntityDto
{
    public Guid Id { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid CreatedBy { get; set; } = default!;
    public string CreatedFullName { get; set; } = default!;
    public DateTimeOffset? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public string? UpdatedFullName { get; set; }
}
