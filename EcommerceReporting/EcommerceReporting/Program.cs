using EcommerceReporting.Services;

namespace EcommerceReporting
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            const string CONNECTION_STRING =
                "Server=DESKTOP-G88OCVU;Database=EcommerceAnalytics;Trusted_Connection=True;TrustServerCertificate=True;";

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<QueryService>(_ => new QueryService(CONNECTION_STRING));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReact", policy =>
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowReact");
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
