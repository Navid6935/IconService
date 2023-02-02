using Application.Common;
using Application.Common.Statics;
using Application.DTOs;
using Infra.Data.DbContext;
using Infra.IOC;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//fulDublexConnectionS.startDublexServer();
builder.Services.AddControllers().ConfigureApiBehaviorOptions(opt =>
{
    opt.InvalidModelStateResponseFactory = context =>
    {
        var responseObj = new ResponseDTO()
        {
            StatusCode = 400,
            Message = context.ModelState.Keys.SelectMany(k =>
            {
                return context.ModelState[k]?.Errors.Select(e => e.ErrorMessage).ToList();
            }).ToList()
        };
        return new BadRequestObjectResult(responseObj);
    };
});;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerGeneratorOptions.DescribeAllParametersInCamelCase = true;
});

StaticData.Config(builder.Configuration);

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(StaticData.DatabaseConnectionString);
});
builder.Services.RegisterServices(builder.Configuration);
#if KESTREL
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(StaticData.ServicePort);
    });

    builder.Host.UseSystemd();
#endif

var app = builder.Build();

#if KESTREL

    app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
#endif

// Configure the HTTP request pipeline.
#if !EXCLUDEOPENAPI
    app.UseSwagger();
    app.UseSwaggerUI();
#endif

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();