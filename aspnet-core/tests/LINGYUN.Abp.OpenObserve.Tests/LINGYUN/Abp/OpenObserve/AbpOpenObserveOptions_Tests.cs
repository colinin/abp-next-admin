using LINGYUN.Abp.OpenObserve.Models;
using Shouldly;
using System;
using System.Text;
using Xunit;

namespace LINGYUN.Abp.OpenObserve;

public class AbpOpenObserveOptions_Tests
{
    [Fact]
    public void Should_Create_Basic_Authorization_Header_From_UserName_And_Password()
    {
        var options = new AbpOpenObserveOptions
        {
            UserName = "admin@abp.io",
            Password = "ww1Z5L%6",
        };

        var authorization = options.CreateAuthorizationHeader();

        authorization.ShouldNotBeNull();
        authorization!.Scheme.ShouldBe("Basic");
        authorization.Parameter.ShouldBe(
            Convert.ToBase64String(Encoding.UTF8.GetBytes("admin@abp.io:ww1Z5L%6")));
    }

    [Fact]
    public void Should_Prefer_AccessToken_Over_UserName_And_Password()
    {
        var options = new AbpOpenObserveOptions
        {
            UserName = "admin@abp.io",
            Password = "ww1Z5L%6",
            AccessToken = "Bearer service-account-token",
        };

        var authorization = options.CreateAuthorizationHeader();

        authorization.ShouldNotBeNull();
        authorization!.Scheme.ShouldBe("Bearer");
        authorization.Parameter.ShouldBe("service-account-token");
    }

    [Fact]
    public void Should_Return_Null_When_No_Credentials_Configured()
    {
        var options = new AbpOpenObserveOptions();

        options.CreateAuthorizationHeader().ShouldBeNull();
    }

    [Fact]
    public void Should_Use_Expected_Defaults()
    {
        var options = new AbpOpenObserveOptions();

        options.Endpoint.ShouldBe("http://localhost:5080");
        options.Organization.ShouldBe("default");
        options.TimeoutSeconds.ShouldBe(600);

        OpenObserveConstants.DefaultOrganization.ShouldBe("default");
        OpenObserveConstants.MetaOrganization.ShouldBe("_meta");
        OpenObserveConstants.NdJsonContentType.ShouldBe("application/x-ndjson");
        OpenObserveConstants.BulkActions.Index.ShouldBe("index");
    }
}
