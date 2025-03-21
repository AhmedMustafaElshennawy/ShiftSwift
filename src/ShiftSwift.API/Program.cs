using GymManagement.Application.Extention;
using ShiftSwift.API.Extention;
using ShiftSwift.Infrastructure.Extention;
using ShiftSwift.Presistence.Extentions;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicaion(builder.Configuration);
builder.Services.AddPresistence(builder.Configuration);
builder.Services.AddApi(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())

app.UseSwagger();
app.UseSwaggerUI();


app.UseStaticFiles();
app.UseCors(c => c.SetIsOriginAllowed(_ => true) 
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials()); // Enables credentials (cookies, tokens)

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
