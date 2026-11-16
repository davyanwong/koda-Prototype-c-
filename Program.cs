using System.Globalization;
using KodaPrototype.Services;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddSingleton(new DataStore(Path.Combine(builder.Environment.ContentRootPath, "data")));
builder.Services.AddSingleton<PdfTextReader>();
builder.Services.AddSingleton<InvoiceService>();
builder.Services.AddSingleton<PlannerService>();
builder.Services.AddSingleton<AiService>();
builder.Services.AddSingleton<AppState>();   // one user at a time: fine for a prototype demo

var app = builder.Build();
app.UseRequestLocalization(o => o.SetDefaultCulture("en-US"));
app.UseStaticFiles();
app.MapRazorPages();
app.Run();
