using InventoryManagement.Shared.Models;

namespace InventoryManagement.Domain.Interfaces;

public interface IBranchRepository
{
    Task<List<BranchDto>> GetAllBranchesAsync();
}