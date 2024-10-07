using System.Net;

namespace DynamicObjectCreation.Application.Common.Dtos
{
	public class DefaultResponse<T> where T : class
	{
		public T Result { get; set; }
		public string Message { get; set; }
		public HttpStatusCode StatusCode { get; set; }

		public static DefaultResponse<T> Successful(T data) => new DefaultResponse<T>() { Result = data, Message = "Successful", StatusCode = HttpStatusCode.OK };
		public static DefaultResponse<T> Successful(T data, string message) => new DefaultResponse<T>() { Result = data, Message = message, StatusCode = HttpStatusCode.OK };
		public static DefaultResponse<T> Successful(T data, HttpStatusCode statusCode) => new DefaultResponse<T>() { Result = data, Message = "Successful", StatusCode = HttpStatusCode.OK };
		public static DefaultResponse<T> Successful(T data, string message, HttpStatusCode statusCode) => new DefaultResponse<T>() { Result = data, Message = message, StatusCode = statusCode };
		public static DefaultResponse<T> Failure(string message, HttpStatusCode statusCode) => new DefaultResponse<T>() { Result = null, Message = message, StatusCode = statusCode };
	}

	public class DefaultResponse
	{
		public string Message { get; set; }
		public HttpStatusCode StatusCode { get; set; }
		public static DefaultResponse Successful() => new DefaultResponse() { Message = "Successful", StatusCode = HttpStatusCode.OK };
		public static DefaultResponse Successful(string message) => new DefaultResponse() { Message = message };
		public static DefaultResponse Successful(string message, HttpStatusCode statusCode) => new DefaultResponse() { Message = message, StatusCode = statusCode };
		public static DefaultResponse Failure(string message) => new DefaultResponse() { Message = message };
		public static DefaultResponse Failure(string message, HttpStatusCode statusCode) => new DefaultResponse() { Message = message, StatusCode = statusCode };
	}
}
