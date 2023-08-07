using Entities.ErrorModel;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Repositories.Contracts;
using System.Net;
using System.Runtime.CompilerServices;

namespace WebAPI.Extensions
{
	public static class ExceptionMiddlewareExtensions
	{
		public static void ConfigureExceptionHandler(this WebApplication app, ILoggerService logger)
		{
			app.UseExceptionHandler(appError =>
			{
				appError.Run(async context =>
				{
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					context.Response.ContentType = "application/json"; 

					var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
					if(contextFeature is not null)
					{
						logger.LogError($"Something went wrong: {contextFeature.Error}");
						await context.Response.WriteAsync(new ErrorDetails()
						{
							StatusCode = context.Response.StatusCode,
							Message = "Internal Server Error"
						}.ToString());
					}
				});
			});
		}
	}
}
