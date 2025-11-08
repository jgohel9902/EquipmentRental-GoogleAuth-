using EquipmentRentalUI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

namespace EquipmentRentalUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IClaimsTransformation, RoleClaimsTransformer>();
            builder.Services.AddAuthorization();

            builder.Services.AddHttpClient<ApiClient>(c =>
            {
                c.BaseAddress = new Uri(builder.Configuration["Api:BaseAddress"]!);
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/auth/login";
                options.LogoutPath = "/auth/logout";
                options.AccessDeniedPath = "/auth/denied";
            })
            .AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
                options.CallbackPath = "/signin-google";

                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.SaveTokens = true;

                options.ClaimActions.MapJsonKey(System.Security.Claims.ClaimTypes.NameIdentifier, "sub");
                options.ClaimActions.MapJsonKey(System.Security.Claims.ClaimTypes.Email, "email");
                options.ClaimActions.MapJsonKey(System.Security.Claims.ClaimTypes.Name, "name");

                options.Events.OnTicketReceived = async ctx =>
                {
                    var email = ctx.Principal?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                    if (string.IsNullOrEmpty(email))
                    {
                        ctx.Response.Redirect("/auth/denied");
                        ctx.HandleResponse();
                        return;
                    }

                    using var httpClient = new HttpClient();
                    httpClient.BaseAddress = new Uri(ctx.HttpContext.RequestServices
                        .GetRequiredService<IConfiguration>()["Api:BaseAddress"]!);

                    var response = await httpClient.GetAsync($"api/Auth/role?email={email}");
                    string role = "User";
                    if (response.IsSuccessStatusCode)
                        role = await response.Content.ReadAsStringAsync();

                    var identity = (System.Security.Claims.ClaimsIdentity)ctx.Principal!.Identity!;
                    identity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role));

                    ctx.Properties.RedirectUri = role == "Admin"
                        ? "/Home/AdminDashboard"
                        : "/Home/UserDashboard";
                };
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
