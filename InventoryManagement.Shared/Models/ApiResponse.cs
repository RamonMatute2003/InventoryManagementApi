namespace InventoryManagement.Shared.Models;

public class ApiResponse<T>
{
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Body { get; set; }
    public object? Error { get; set; }

    public ApiResponse() { }

    public ApiResponse(int code, string message, T? body = default, object? error = null)
    {
        Code = code;
        Message = message;
        Body = body;
        Error = error;
    }
}