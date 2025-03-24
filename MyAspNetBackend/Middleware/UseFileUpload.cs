using MyAspNetBackend.Middleware;

public static class FileUploadMiddlewareExtensions
{
    public static IApplicationBuilder UseFileUpload(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<FileUploadMiddleware>();
    }
}