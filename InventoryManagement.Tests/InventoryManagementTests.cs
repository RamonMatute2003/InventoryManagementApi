using InventoryManagement.Api.Controllers;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace InventoryManagement.Tests;

public class InventoryManagementTests
{
    private readonly Mock<IBranchService> _mockBranchService;
    private readonly Mock<IInventoryService> _mockInventoryService;
    private readonly BranchesController _branchesController;
    private readonly InventoryController _inventoryController;

    public InventoryManagementTests()
    {
        _mockBranchService = new Mock<IBranchService>();
        _mockInventoryService = new Mock<IInventoryService>();

        _branchesController = new BranchesController(_mockBranchService.Object);
        _inventoryController = new InventoryController(_mockInventoryService.Object);
    }

    [Fact]
    public async Task GetBranches_ShouldReturnBranchList()
    {
        var branches = new List<BranchDto> { new() { Id = 1, Name = "Sucursal Centro" } };
        _mockBranchService.Setup(s => s.GetAllBranchesAsync()).ReturnsAsync(branches);

        var result = await _branchesController.GetBranches() as ObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task GetAvailableStock_ShouldReturnStock_WhenProductExists()
    {
        int productId = 1;
        _mockInventoryService
            .Setup(s => s.GetAvailableStockAsync(It.IsAny<int>()))
            .ReturnsAsync(10);

        var result = await _inventoryController.GetAvailableStock(productId) as ObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Value);
    }
}