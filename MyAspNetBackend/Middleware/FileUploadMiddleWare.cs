namespace MyAspNetBackend.Middleware
{
    public class FileUploadMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");

        public FileUploadMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Kiểm tra yêu cầu POST và có kiểu form-data
            if (context.Request.Method == "POST" && context.Request.HasFormContentType)
            {
                var form = await context.Request.ReadFormAsync();
                var file = form.Files["file"];  // Truyền tham số "file" từ frontend

                if (file != null && file.Length > 0)
                {
                    // Kiểm tra định dạng file hợp lệ
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(file.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Invalid file type.");
                        return;
                    }

                    // Kiểm tra và tạo thư mục lưu file nếu chưa có
                    if (!Directory.Exists(_imagePath))
                    {
                        Directory.CreateDirectory(_imagePath); // Tạo thư mục nếu chưa tồn tại
                    }

                    // Đặt tên file mới bằng GUID để tránh trùng lặp
                    var fileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(_imagePath, fileName);

                    // Lưu file vào thư mục
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    // Lưu đường dẫn file vào context để có thể sử dụng trong controller
                    context.Items["FilePath"] = $"/images/products/{fileName}";
                }
            }
            // Chuyển tiếp yêu cầu tới middleware tiếp theo trong pipeline
            await _next(context);
        }
    }
}
