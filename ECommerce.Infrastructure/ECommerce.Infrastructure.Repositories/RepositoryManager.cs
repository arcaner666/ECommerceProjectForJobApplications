using ECommerce.Domain.Contracts;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Infrastructure.Repositories;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IClaimRepository> _claimRepository;
    private readonly Lazy<IPersonRepository> _personRepository;
    private readonly Lazy<IRoleClaimRepository> _roleClaimRepository;
    private readonly Lazy<IRoleRepository> _roleRepository;
    private readonly Lazy<IStartupRepository> _startupRepository;

    public RepositoryManager(
        RepositoryContext repositoryContext,
        IConfiguration configuration)
    {
        _claimRepository = new Lazy<IClaimRepository>(() => new ClaimRepository(repositoryContext));
        _personRepository = new Lazy<IPersonRepository>(() => new PersonRepository(repositoryContext));
        _roleClaimRepository = new Lazy<IRoleClaimRepository>(() => new RoleClaimRepository(repositoryContext));
        _roleRepository = new Lazy<IRoleRepository>(() => new RoleRepository(repositoryContext));
        _startupRepository = new Lazy<IStartupRepository>(() => new StartupRepository(repositoryContext, configuration));
    }
    public IClaimRepository Claim => _claimRepository.Value;
    public IPersonRepository Person => _personRepository.Value;
    public IRoleClaimRepository RoleClaim => _roleClaimRepository.Value;
    public IRoleRepository Role => _roleRepository.Value;
    public IStartupRepository Startup => _startupRepository.Value;
}