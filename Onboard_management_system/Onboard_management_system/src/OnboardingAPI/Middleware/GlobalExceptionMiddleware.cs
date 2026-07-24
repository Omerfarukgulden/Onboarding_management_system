using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using Onboard_management_system.OnboardingApplication.Common;
using Onboard_management_system.OnboardingApplication.Services.exceptions;

namespace Onboard_management_system.OnboardingAPI.Middleware;

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
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Beklenmeyen bir hata oluştu: {Message}", exception.Message);

        var (statusCode, message) = exception switch
        {
            NotFoundException => (HttpStatusCode.NotFound, exception.Message),
            KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
            UnauthorizedAccessException => (HttpStatusCode.Forbidden, exception.Message),
            ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
            DbUpdateException dbEx when dbEx.InnerException?.Message.Contains("FOREIGN KEY") == true
                => (HttpStatusCode.BadRequest, "İlişkili bir kayıt bulunamadı (geçersiz departman/pozisyon/kullanıcı id'si)."), // ← GÜNCELLENDİ (yeni satır)
            DbUpdateException => (HttpStatusCode.BadRequest, "Veritabanı işlemi sırasında bir hata oluştu."), // ← GÜNCELLENDİ (yeni satır)
            _ => (HttpStatusCode.InternalServerError, "Sunucu tarafında beklenmeyen bir hata oluştu.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new ApiErrorResponse
        {
            Message = message,
            StatusCode = (int)statusCode,
            // Detaylı stack trace sadece Development ortamında dönsün, prod'da güvenlik riski
            Details = context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment()
                ? exception.StackTrace
                : null
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}