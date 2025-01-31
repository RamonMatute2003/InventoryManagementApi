using InventoryManagement.Shared.Models;

namespace InventoryManagement.Domain.Interfaces;

public interface IBranchService
{
    Task<List<BranchDto>> GetAllBranchesAsync();
}