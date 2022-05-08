var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(defaultScheme: "Bearer")
    .AddIdentityServerAuthentication("Bearer",configureOptions:option=>
    {
        option.ApiName = "Product.API";
        option.Authority = "https://localhost:7414";      
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
