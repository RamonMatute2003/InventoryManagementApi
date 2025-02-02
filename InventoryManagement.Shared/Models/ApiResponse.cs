namespace InventoryManagement.Shared.Models;

public class ApiResponse<T>
{
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Body { get; set; }
    public object? Error { get; set; }

    public ApiResponse() { }
}