using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;

namespace PackageName.CompanyName.ProjectName;

public class TestHostEnvironment : IHostEnvironment
{
    public TestHostEnvironment()
    {
        EnvironmentName = "Test";
        ApplicationName = "TestApplication";
        ContentRootPath = AppDomain.CurrentDomain.BaseDirectory;
        ContentRootFileProvider = new PhysicalFileProvider(ContentRootPath);
    }

    public string EnvironmentName { get; set; }
    public string ApplicationName { get; set; }
    public string ContentRootPath { get; set; }
    public IFileProvider ContentRootFileProvider { get; set; }
}