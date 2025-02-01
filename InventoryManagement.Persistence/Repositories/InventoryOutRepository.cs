using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Persistence.Contexts;
using InventoryManagement.Persistence.Models;
using InventoryManagement.Shared.Models;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Persistence.Repositories;

public class InventoryOutRepository : IInventoryOutRepository
{
    private readonly InventoryDbContext _context;

    public InventoryOutRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InventoryOutDto>> GetAllAsync()
    {
        return await _context.InventoryOutHeaders
            .Select(ioh => new InventoryOutDto
            {
                IdOutHeader = ioh.IdOutHeader,
                OutDate = ioh.OutDate,
                TotalCost = ioh.TotalCost,
                IdUser = ioh.IdUser,
                IdBranch = ioh.IdBranch,
                IdStatus = ioh.IdStatus
            }).ToListAsync();
    }

    public async Task<InventoryOutDto?> GetByIdAsync(int id)
    {
        var inventoryOut = await _context.InventoryOutHeaders
            .Where(ioh => ioh.IdOutHeader == id)
            .Select(ioh => new InventoryOutDto
            {
                IdOutHeader = ioh.IdOutHeader,
                OutDate = ioh.OutDate,
                TotalCost = ioh.TotalCost,
                IdUser = ioh.IdUser,
                IdBranch = ioh.IdBranch,
                IdStatus = ioh.IdStatus
            }).FirstOrDefaultAsync();

        return inventoryOut;
    }

    public async Task<InventoryOutDto> CreateAsync(CreateInventoryOutDto inventoryOutDto, int userId)
    {
        if(inventoryOutDto.IdBranch == 1)
        {
            throw new InvalidOperationException("⛔ la Bodega Central solo puede realizar envíos de productos");
        }

        decimal totalPending = await _context.InventoryOutHeaders
            .Where(io => io.IdBranch == inventoryOutDto.IdBranch && io.IdStatus == 1)
            .SumAsync(io => io.TotalCost);

        if(totalPending + inventoryOutDto.TotalCost > 5000)
        {
            throw new InvalidOperationException("❌ La sucursal ya tiene más de L 5000 en envíos pendientes.");
        }

        var newInventoryOut = new InventoryOutHeader
        {
            OutDate = DateTime.UtcNow,
            IdUser = userId,
            IdBranch = inventoryOutDto.IdBranch,
            IdStatus = 1
        };

        _context.InventoryOutHeaders.Add(newInventoryOut);
        await _context.SaveChangesAsync();

        decimal totalCost = 0;

        foreach(var detail in inventoryOutDto.Details)
        {
            var availableLots = await _context.InventoryLots
                .Where(l => l.IdProduct == detail.IdProduct)
                .OrderBy(l => l.ExpirationDate)
                .ToListAsync();

            int remainingQuantity = detail.Quantity;

            foreach(var lot in availableLots)
            {
                if(remainingQuantity <= 0)
                    break;

                int quantityToTake = Math.Min(lot.BatchQuantity, remainingQuantity);

                var newDetail = new InventoryOutDetail
                {
                    IdOutHeader = newInventoryOut.IdOutHeader,
                    IdProduct = detail.IdProduct,
                    IdBatch = lot.IdBatch,
                    Quantity = quantityToTake,
                    Cost = lot.Cost * quantityToTake
                };

                _context.InventoryOutDetails.Add(newDetail);

                totalCost += lot.Cost * quantityToTake;
                remainingQuantity -= quantityToTake;

                lot.BatchQuantity -= quantityToTake;
                if(lot.BatchQuantity == 0)
                    _context.InventoryLots.Remove(lot);
            }

            if(remainingQuantity > 0)
            {
                throw new InvalidOperationException($"❌ No hay suficiente stock para el producto {detail.IdProduct}.");
            }
        }

        newInventoryOut.TotalCost = totalCost;
        await _context.SaveChangesAsync();

        return new InventoryOutDto
        {
            IdOutHeader = newInventoryOut.IdOutHeader,
            OutDate = newInventoryOut.OutDate,
            TotalCost = newInventoryOut.TotalCost,
            IdUser = newInventoryOut.IdUser,
            IdBranch = newInventoryOut.IdBranch,
            IdStatus = newInventoryOut.IdStatus
        };
    }

    public async Task<IEnumerable<FilteredInventoryOutDto>> GetFilteredInventoryOutsAsync(DateTime? startDate, DateTime? endDate, int? branchId, string? status)
    {
        var query = _context.InventoryOutHeaders
            .Include(io => io.IdUserNavigation)
            .Include(io => io.IdBranchNavigation)
            .Include(io => io.IdStatusNavigation)
            .AsQueryable();

        if(startDate.HasValue)
            query = query.Where(io => io.OutDate >= startDate.Value);
        if(endDate.HasValue)
            query = query.Where(io => io.OutDate <= endDate.Value);
        if(branchId.HasValue)
            query = query.Where(io => io.IdBranch == branchId.Value);
        if(!string.IsNullOrEmpty(status))
            query = query.Where(io => io.IdStatusNavigation.StatusName == status);

        return await query.Select(io => new FilteredInventoryOutDto
        {
            IdOutHeader = io.IdOutHeader,
            OutDate = io.OutDate,
            TotalCost = io.TotalCost,
            IdUser = io.IdUser,
            UserName = io.IdUserNavigation.UserName,
            IdBranch = io.IdBranch,
            BranchName = io.IdBranchNavigation.BranchName,
            Status = io.IdStatusNavigation.StatusName,
            TotalUnits = io.InventoryOutDetails.Sum(d => d.Quantity),
            ReceivedBy = io.ReceivedByNavigation != null ? io.ReceivedByNavigation.UserName : null,
            ReceivedDate = io.ReceivedDate
        }).ToListAsync();
    }

    public async Task<bool> ReceiveInventoryOutAsync(int id, int receivedByUserId)
    {
        var inventoryOut = await _context.InventoryOutHeaders
            .FirstOrDefaultAsync(io => io.IdOutHeader == id);

        if(inventoryOut == null || inventoryOut.IdStatus == 2)
        {
            return false;
        }

        inventoryOut.IdStatus = 2;
        inventoryOut.ReceivedBy = receivedByUserId;
        inventoryOut.ReceivedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ProductDetailsDto?> GetProductDetailsAsync(int productId)
    {
        var product = await _context.Products
            .Where(p => p.IdProduct == productId)
            .Select(p => new ProductDetailsDto
            {
                IdProduct = p.IdProduct,
                ProductName = p.Name,
                Lots = p.InventoryLots
                    .Where(l => l.BatchQuantity > 0)
                    .OrderBy(l => l.ExpirationDate)
                    .Select(l => new ProductLotDto
                    {
                        BatchId = l.IdBatch,
                        BatchQuantity = l.BatchQuantity,
                        Cost = l.Cost,
                        ExpirationDate = l.ExpirationDate
                    }).ToList()
            })
            .FirstOrDefaultAsync();

        return product;
    }
}