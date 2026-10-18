var builder = WebApplication.CreateBuilder(args);

// ===== Add services =====
builder.Services.AddControllers();

// ===== CORS =====
// يسمح لأي frontend بالوصول للـ API (يمكن تضييقه لاحقاً على الدومين الخاص بك)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ===== Swagger =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ===== Configure middleware =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// تمكين CORS
app.UseCors("AllowAll");

app.UseAuthorization();

// Map Controllers
app.MapControllers();

app.Run();
