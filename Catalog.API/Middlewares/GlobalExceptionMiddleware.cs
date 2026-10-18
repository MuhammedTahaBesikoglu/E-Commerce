using FluentValidation;
using System.Text.Json;

namespace Catalog.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // İstek sorunsuzsa bir sonraki adıma (Controller'a) geçmesine izin ver
                await _next(context);
            }
            catch (Exception ex)
            {
                // Bir hata fırlatıldıysa burada yakala ve bizim metodumuza gönder
                _logger.LogError(ex, "Sistemde bir hata yakalandı.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Varsayılan olarak 500 (Sunucu Hatası) kabul edelim
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var response = new { message = "Sunucu tarafında beklenmeyen bir hata oluştu.", errors = new List<string>() };

            // Gelen hata bizim FluentValidation'dan geliyorsa tipi 400 (Bad Request) olarak değiştir
            if (exception is ValidationException validationException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response = new
                {
                    message = "Lütfen gönderdiğiniz bilgileri kontrol edin.",
                    errors = validationException.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            // Oluşturduğumuz şık JSON yanıtını dışarıya dön
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}