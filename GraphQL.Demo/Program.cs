using FirebaseAdmin;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using FirebaseAdminAuthentication.DependencyInjection.Models;
using FluentValidation.AspNetCore;
using AppAny.HotChocolate.FluentValidation;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFluentValidation();
builder.Services.AddTransient<CourseTypeInputValidator>();

//Configure GraphQL Server
builder.Services.AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddSubscriptionType<Subscription>()
                .AddType<CourseType>()
                .AddType<InstructorType>()
                .AddTypeExtension<CourseQuery>()
                .AddFiltering()
                .AddSorting()
                .AddProjections()
                .AddAuthorization()
                .AddFluentValidation(o =>
                {
                    o.UseDefaultErrorMapper();
                });

builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(builder.Configuration.GetValue<string>("FIREBASE_CONFIG"))
}));

builder.Services.AddFirebaseAuthentication();
builder.Services.AddAuthorization(opt => {
    opt.AddPolicy("IsAdmin", policy=> {
        policy.RequireClaim(FirebaseUserClaimType.EMAIL, "ismail27.dec@gmail.com");
    });
});

builder.Services.AddInMemorySubscriptions();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddPooledDbContextFactory<SchoolDbContext>(
    opt => opt.UseSqlite(connectionString).LogTo(Console.WriteLine));

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddScoped<CoursesRepository>();
builder.Services.AddScoped<InstructorRepository>();
builder.Services.AddScoped<InstructorLoader>();
builder.Services.AddScoped<UserDataLoader>();

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
