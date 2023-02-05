using Identity.Razor.V1.Authorization;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// We specift the name of the default authentication handler that should be used
// by the authentication middleware.
builder.Services.AddAuthentication("MyCookieAuth")
.AddCookie("MyCookieAuth", opt =>
{
    opt.Cookie.Name = "MyCookieAuth";
    // Specify where the account page is to redirect the user when he hits an authorized page or endpoint.
    opt.LoginPath = "/Account/Login";
    // Specify access denied page
    opt.AccessDeniedPath = "/Account/AccessDenied";
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(4);
});

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireClaim("Admin");
    });

    opt.AddPolicy("MustBeFromHrDepartment", policy =>
    {
        policy.RequireClaim("Department", "HR");
    });

    opt.AddPolicy("HRManagerOnly", policy =>
    {
        policy
        .RequireClaim("Department", "HR")
        .RequireClaim("Manager")
        .Requirements.Add(new HrManagerProbationRequirement(3));
    });
});

// Add the custom authorization handler as a service
builder.Services.AddSingleton<IAuthorizationHandler, HrManagerProbationRequirementHandler>();

builder.Services.AddHttpClient("OurWebApi", clinet =>
{
    clinet.BaseAddress = new Uri("https://localhost:7001/");
});

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
