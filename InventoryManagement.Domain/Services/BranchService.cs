using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Shared.Models;

namespace InventoryManagement.Domain.Services;
public class BranchService : IBranchService
{
    private readonly IBranchRepository _branchRepository;

    public BranchService(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<List<BranchDto>> GetAllBranchesAsync()
    {
        return await _branchRepository.GetAllBranchesAsync();
    }
}