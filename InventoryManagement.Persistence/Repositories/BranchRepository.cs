using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Persistence.Contexts;
using InventoryManagement.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Persistence.Repositories;

public class BranchRepository(InventoryDbContext context) : IBranchRepository
{
    public async Task<List<BranchDto>> GetAllBranchesAsync()
    {
        return await context.Branches
            .Select(b => new BranchDto
            {
                Id = b.IdBranch,
                Name = b.BranchName,
                Location = b.BranchLocation!
            })
            .ToListAsync();
    }
}