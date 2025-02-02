using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Shared.Models;

namespace InventoryManagement.Domain.Services;
public class BranchService(IBranchRepository branchRepository) : IBranchService
{
    public async Task<List<BranchDto>> GetAllBranchesAsync()
    {
        return await branchRepository.GetAllBranchesAsync();
    }
}