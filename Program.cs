var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

builder.Services.AddControllers();
builder.Services.AddHttpClient<RecaptchaService>();
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAllOrigins",
      builder => builder
          .AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}
else
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}

app.UseRouting();

app.UseCors("AllowAllOrigins");

app.UseEndpoints(endpoints =>
{
  endpoints.MapControllers();
});

app.Run();
