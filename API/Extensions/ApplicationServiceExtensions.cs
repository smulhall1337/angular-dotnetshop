using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Extensions
{
    // we can define the builder services here to keep our program.cs class neat and tidy
    // (well...you know, except for the SUPER HELPFUL comments)
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                // if the modelstate is in an errored state (from a validation error)...
                opt.InvalidModelStateResponseFactory = actionContext =>
                {
                    // go inside the model state and extract any errors (if any)
                    // and return those error messages in an array
                    // this makes it easier for the client app to iterate over them
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    ApiValidationError errorResponse = new ApiValidationError()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });
            return services;
        }
    }
}
