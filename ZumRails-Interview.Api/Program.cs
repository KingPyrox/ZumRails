using ZumRail_Interview.Applications;
using ZumRail_Interview.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("TestClient", config =>
{
    var url = "https://api.hatchways.io/assessment/blog/posts";
    config.BaseAddress = new Uri(url);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


builder.Services.AddScoped<IDataProcessing, DataProcessing>();
//builder.Services.Scan(scan =>
//    scan.FromCallingAssembly()
//        .AddClasses(classes => classes.InNamespaces(nameof(PostController)))
//        .AsMatchingInterface());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();

app.MapControllers();

app.Run();
