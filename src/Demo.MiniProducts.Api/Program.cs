using Serilog;
using Bootstrapper = Demo.MiniProducts.Api.Bootstrapper;
using Features = Demo.MiniProducts.Api.Features;

const string Route = "products";

var app = Bootstrapper.Setup(args);
app.UseSerilogRequestLogging();
app.UseSwagger();
app.UseSwaggerUI();

var productsApi = app.MapGroup($"/{Route}/").WithOpenApi();

Features.FindById.RouteService.Setup(productsApi);
Features.RegisterProduct.RouteService.Setup(productsApi);
Features.ChangeLocation.RouteService.Setup(productsApi);

app.Run();

namespace Demo.MiniProducts.Api
{
    public partial class Program{}
}