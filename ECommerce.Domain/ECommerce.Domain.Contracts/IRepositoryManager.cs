namespace ECommerce.Domain.Contracts;

public interface IRepositoryManager
{
    IClaimRepository Claim { get; }
    IPersonRepository Person { get; }
    IRoleClaimRepository RoleClaim { get; }
    IRoleRepository Role { get; }
    IStartupRepository Startup { get; }
}