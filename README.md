# API Versioning Guide

## What is Versioning?

It's a technique for managing changes in APIs over time, allowing new features or modifications without disrupting existing clients.

## Why Use Versioning? Benefits

- **Backward Compatibility:** Keeps old clients functioning when new changes are introduced.
- **Flexibility:** Allows API evolution with new features or modifications while maintaining stability.
- **Client Management:** Manages different client needs and ensures they use the API versions that suit their requirements.

## Prerequisites

Before implementing API versioning, ensure you have the following NuGet packages installed:

- `Asp.Versioning.Mvc`
- `Asp.Versioning.Mvc.ApiExplorer`

## Configuration

```csharp
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});
```

- `AssumeDefaultVersionWhenUnspecified:` Uses the default version if none is specified.
- `DefaultApiVersion:` Sets the default version of the API.
- `ReportApiVersions:` Includes API version information in responses.

# Types of API Versioning

### 1. Path-Based Versioning

**Version 1 Controller:**

```c#
namespace Api.Controllers.V1
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GovernmentsController : ControllerBase
    {
        private readonly IJsonService _jsonService;

        public GovernmentsController(IJsonService jsonService)
        {
            _jsonService = jsonService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var cities = _jsonService.GetAll<Government>("governments.json");
            return Ok(cities.Take(5));
        }
    }
}
```

**Version 2 Controller:**

```c#
namespace Api.Controllers.V2
{
    [ApiController]
    [ApiVersion(2)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GovernmentsController : ControllerBase
    {
        private readonly IJsonService _jsonService;

        public GovernmentsController(IJsonService jsonService)
        {
            _jsonService = jsonService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var cities = _jsonService.GetAll<Government>("governments.json");
            return Ok(cities.Take(10));
        }
    }
}
```

### API Calls

> **_NOTE:_** you must specify the version in the URL.

- `/api/v1/Governments`
- `/api/v2/Governments`

### 2. Query-Based Versioning

**Version 1 Controller:**

```c#
namespace Api.Controllers.V1{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/[controller]")]
    public class GovernmentsController : ControllerBase
    {
        private readonly IJsonService _jsonService;

        public GovernmentsController(IJsonService jsonService)
        {
            _jsonService = jsonService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var cities = _jsonService.GetAll<Government>("governments.json");
            return Ok(cities.Take(5));
        }
    }
}
```

**Version 2 Controller:**

```c#
namespace Api.Controllers.V2{
    [ApiController]
    [ApiVersion(2)]
    [Route("api/[controller]")]
    public class GovernmentsController : ControllerBase
    {
        private readonly IJsonService _jsonService;

        public GovernmentsController(IJsonService jsonService)
        {
            _jsonService = jsonService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var cities = _jsonService.GetAll<Government>("governments.json");
            return Ok(cities.Take(10));
        }
    }
}
```

**API Calls:**

- `/api/Governments?api-version=1`
- `/api/Governments?api-version=2`

### 3. Combine Strategies

use both query or headers
**Configuration:**

```c#
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(2);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("version"),  // Query string versioning
        new HeaderApiVersionReader("x-api-version")  // Header versioning
    );
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});
```

**API Calls:**

- `/api/Governments?api-version=1`

or using

```
GET /api/Governments
Headers: x-api-version: 1
```

> **_NOTE:_** If no version is specified (query or header), the default version from configuration is used.

### Swagger Configuration for Versioning
when using swagger with versioning (Path) it conflicts and needs to be configured to work fine , configure it as follows:

![without](https://github.com/user-attachments/assets/75d88f39-fb84-484f-86d7-f2749f17cf84)
```c#
namespace API
{
    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                var openApiInfo = new OpenApiInfo
                {
                    Title = $"API v{description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                };

                options.SwaggerDoc(description.GroupName, openApiInfo);
            }
        }

        public void Configure(string? name, SwaggerGenOptions options)
        {
            Configure(options);
        }
    }
}
```

```c#
// Program.cs
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            string url = $"/swagger/{description.GroupName}/swagger.json";
            string name = description.GroupName.ToUpper();
            o.SwaggerEndpoint(url, name);
        }
    });
}
```
![with](https://github.com/user-attachments/assets/c7382982-4228-4eac-9b50-adda60b7c492)


