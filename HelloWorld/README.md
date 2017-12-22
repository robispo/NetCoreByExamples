# Hello World

This is a "Hello World" from scratch in ASP.Net Core MVC

## Steps

1. Create a new project **ASP.Net Core** with an empty template.
2. At the **Starup.cs** in the **ConfigureServices** method add mvc.

```csharp
public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }
```

3. At the **Starup.cs** in the **Configure** method use mvc for default routing and static files to access flat files (Optinal).

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
```

4. Add a folder call **Controlles** and add a default empty controller call **HomeController**, in the default routing is the name that we specific.
5. Inside of **HomeController** rithg click in **Index** action and chouse "add view" option.