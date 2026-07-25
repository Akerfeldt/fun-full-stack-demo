var builder = DistributedApplication.CreateBuilder(args);

var identityServer = builder.AddProject<Projects.IdentityServer>("identityserver")
    .WithHttpHealthCheck("/health");

var apiService = builder.AddProject<Projects.FunDemo_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(identityServer);

builder.Build().Run();
