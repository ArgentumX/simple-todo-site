using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;
using MongoDB.Driver;
using TODO.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// MongoDB
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient("mongodb://localhost:27017"));
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase("TodoDb"));
builder.Services.AddIdentityMongoDbProvider<MongoUser>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
    options.LogoutPath = "/account/logout";
});

// Inner services
builder.Services.AddSingleton<TaskService>();
builder.Services.AddScoped<AccountService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Task}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "home",
    pattern: "{controller=Home}/{action=About}");

app.MapControllerRoute(
    name: "account",
    pattern: "{controller=Account}/{action=Login}");

app.Run();

