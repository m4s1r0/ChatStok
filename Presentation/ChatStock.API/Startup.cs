using Serilog;

namespace ChatStock.API
{
    public class Startup
    {
        private Serilog.ILogger _logger;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //_logger = ConfigureLogger(configuration);
            //_logger.Information("Logger configured");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // Enable CORS
            app.UseCors(options =>
                options.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json?e=prd", "ChatStock.API v1"));

            app.UseRouting();

            app.UseAuthorization();
            

            // global error handler
            //app.UseMiddleware<ErrorHandlerMiddleware>();

            var logger = loggerFactory.CreateLogger("ChatStockLogging");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
