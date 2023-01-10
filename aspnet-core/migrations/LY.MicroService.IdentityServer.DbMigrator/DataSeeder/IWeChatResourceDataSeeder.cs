namespace LY.MicroService.IdentityServer.DbMigrator.DataSeeder;

public interface IWeChatResourceDataSeeder
{
    Task CreateStandardResourcesAsync();
}
