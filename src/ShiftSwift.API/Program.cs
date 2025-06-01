using ShiftSwift.API.Extention;
using ShiftSwift.API.MiddleWare;
using ShiftSwift.Application.Extentions;
using ShiftSwift.Infrastructure.Extention;
using ShiftSwift.Presistence.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddPresistence(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApi(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())

app.UseSwagger();
app.UseSwaggerUI();


// Serve static files from wwwroot (e.g., http://shiftswift.tryasp.net/images/file.jpg)
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Cache static files for 30 days in production
        if (!app.Environment.IsDevelopment())
        {
            ctx.Context.Response.Headers.Append(
                "Cache-Control",
                "public,max-age=2592000" // 30 days
            );
        }
    }
});


app.UseCors(c => c
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlerMiddleWare>();

app.MapControllers();

var webRootPath = Path.Combine(builder.Environment.WebRootPath ?? "wwwroot");
Directory.CreateDirectory(Path.Combine(webRootPath, "images"));

app.Run();
