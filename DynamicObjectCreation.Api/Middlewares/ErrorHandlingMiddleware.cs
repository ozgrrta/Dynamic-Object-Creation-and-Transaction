using DynamicObjectCreation.Application.Common.Exceptions;
using FluentValidation;
using System.Data.Common;
using System.Text.Json;

namespace DynamicObjectCreation.Api.Middlewares
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ErrorHandlingMiddleware> _logger;

		public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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

		private Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";

			switch (exception)
			{
				case InvalidOperationException invalidOperationException:
					context.Response.StatusCode = StatusCodes.Status400BadRequest;
					_logger.LogError("Invalid operation: {message}", invalidOperationException.Message);
					return context.Response.WriteAsync(new ErrorDetails
					{
						StatusCode = context.Response.StatusCode,
						Message = invalidOperationException.Message ?? "Invalid request format or missing fields."
					}.ToString());

				case DbException dbException:
					context.Response.StatusCode = StatusCodes.Status500InternalServerError;
					_logger.LogError("Database error: {message}", dbException.Message);
					return context.Response.WriteAsync(new ErrorDetails
					{
						StatusCode = context.Response.StatusCode,
						Message = "Database connection error."
					}.ToString());

				case ValidationException validationException:
					context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
					_logger.LogError("Validation error: {message}", validationException.Message);
					return context.Response.WriteAsync(new ErrorDetails
					{
						StatusCode = context.Response.StatusCode,
						Message = validationException.Message
					}.ToString());

				case JsonException jsonException:
					context.Response.StatusCode = StatusCodes.Status400BadRequest;
					_logger.LogError("Invalid operation: {message}", jsonException.Message);
					return context.Response.WriteAsync(new ErrorDetails
					{
						StatusCode = context.Response.StatusCode,
						Message = jsonException.Message
					}.ToString());

				default:
					context.Response.StatusCode = StatusCodes.Status400BadRequest;
					_logger.LogError("An unexpected error occurred: {message}", exception.Message);
					return context.Response.WriteAsync(new ErrorDetails
					{
						StatusCode = context.Response.StatusCode,
						Message = "An unexpected error occurred."
					}.ToString());
			}
		}
	}
}