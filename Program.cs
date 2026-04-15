using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using core_first.API.Data;
using core_first.API.Services;
using core_first.API.Repositories;
using QuestPDF.Infrastructure;
using System.Text.Json.Serialization;


namespace core_first.API;

public class Program

{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        QuestPDF.Settings.License = LicenseType.Community;

        // تحديد ما إذا كنا في بيئة اختبار
        var isTesting = Environment.GetEnvironmentVariable("TESTING") == "true" 
                        || builder.Environment.IsEnvironment("Testing");

        // تسجيل DbContext حسب البيئة
        if (isTesting)
        {
            Console.WriteLine("Running in TESTING mode - using InMemory database");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("InMemoryDbForTesting"));
        }
        else
        {
            Console.WriteLine("Running in NORMAL mode - using PostgreSQL");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        }

        // تسجيل الـ Repositories
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
        builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
        builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        // JWT Authentication
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        var secret = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret missing");
        var key = Encoding.ASCII.GetBytes(secret);
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        builder.Services.AddAuthorization();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<PdfService>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your token (without 'Bearer ' prefix)"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });

        builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
        

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        // تهيئة قاعدة البيانات
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            if (isTesting)
            {
                // في بيئة الاختبار: ننشئ قاعدة البيانات فقط (بدون حذف)
                dbContext.Database.EnsureCreated();
                
                // إضافة بيانات اختبارية أساسية (مثل مستخدم admin)
                if (!dbContext.Users.Any())
                {
                    dbContext.Users.Add(new Models.User
                    {
                        Username = "admin",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                        Email = "admin@example.com",
                        Role = Models.UserRole.Admin,
                        CreatedAt = DateTime.UtcNow
                    });
                    dbContext.SaveChanges();
                }
            }
            else
            {
                // في البيئة العادية: حذف وإنشاء (أو استخدام Migrate حسب الحاجة)
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }
        }

        app.Run();
    }
}