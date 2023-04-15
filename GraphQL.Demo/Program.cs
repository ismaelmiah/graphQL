using FirebaseAdmin;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Configure GraphQL Server
builder.Services.AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddSubscriptionType<Subscription>()
                .AddFiltering()
                .AddSorting()
                .AddProjections()
                .AddAuthorization();

builder.Services.AddSingleton(FirebaseApp.Create());

builder.Services.AddFirebaseAuthentication();

builder.Services.AddInMemorySubscriptions();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddPooledDbContextFactory<SchoolDbContext>(
    opt => opt.UseSqlite(connectionString).LogTo(Console.WriteLine));

// Add services to the container.
builder.Services.AddRazorPages();


builder.Services.AddScoped<CoursesRepository>();
builder.Services.AddScoped<InstructorRepository>();
builder.Services.AddScoped<InstructorLoader>();

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IDbContextFactory<SchoolDbContext> contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<SchoolDbContext>>();
    using (SchoolDbContext context = contextFactory.CreateDbContext())
    {
        context.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseWebSockets();

app.UseRouting();

app.UseAuthentication();

app.UseEndpoints(endpoints => endpoints.MapGraphQL());
app.MapRazorPages();

app.Run();
