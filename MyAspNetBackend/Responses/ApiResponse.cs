namespace MyAspNetBackend.Responses;

public class ApiResponse<T>
{
    public string Status { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }  

    private ApiResponse(string status, T? data = default, string? message = null)  
    {
        Status = status;
        Data = data;
        Message = message ?? "Operation was successful.";  
    }

    public static ApiResponse<T> Success(T data, string? message = "Operation was successful.")
    {
        return new ApiResponse<T>("success", data, message);
    }

    public static ApiResponse<T> Fail(string message, T? data = default)
    {
        return new ApiResponse<T>("fail", data, message);
    }

    public static ApiResponse<T> Error(string message, T? data = default)
    {
        return new ApiResponse<T>("error", data, message);
    }
}
