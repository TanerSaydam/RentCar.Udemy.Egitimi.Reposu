using GenericRepository;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Branchs;
using RentCarServer.Infrastructure.Context;

namespace RentCarServer.Infrastructure.Repositories;
internal sealed class BranchRepository : Repository<Branch, ApplicationDbContext>, IBranchRepository
{
    public BranchRepository(ApplicationDbContext context) : base(context)
    {
    }
}