var builder = WebApplication.CreateBuilder(args);

//Configure GraphQL Server
builder.Services.AddGraphQLServer()
                .AddQueryType<LibraryQuery>()
                .AddMutationType<Mutation>()
                .AddSubscriptionType<Subscription>();

builder.Services.AddInMemorySubscriptions();

// Add services to the container.
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
app.UseWebSockets();

app.UseRouting().UseEndpoints(endpoints => endpoints.MapGraphQL());

app.UseAuthorization();

app.MapRazorPages();

app.Run();
