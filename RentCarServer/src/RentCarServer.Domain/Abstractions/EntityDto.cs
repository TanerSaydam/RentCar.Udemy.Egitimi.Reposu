namespace RentCarServer.Domain.Abstractions;
public abstract class EntityDto
{
    public IdentityId Id { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public IdentityId CreatedBy { get; set; } = default!;
    public string CreatedFullName { get; set; } = default!;
    public DateTimeOffset? UpdatedAt { get; set; }
    public IdentityId? UpdatedBy { get; set; }
    public string? UpdatedFullName { get; set; }
}
