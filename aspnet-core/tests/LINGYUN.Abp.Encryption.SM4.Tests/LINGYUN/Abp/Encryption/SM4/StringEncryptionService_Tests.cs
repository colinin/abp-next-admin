using Shouldly;
using Volo.Abp.Security.Encryption;
using Xunit;

namespace LINGYUN.Abp.Encryption.SM4;
public class StringEncryptionService_Tests : AbpEncryptionSM4TestBase
{
    private readonly IStringEncryptionService _stringEncryptionService;

    public StringEncryptionService_Tests()
    {
        _stringEncryptionService = GetRequiredService<IStringEncryptionService>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("This is a plain text!")]
    public void Should_Enrypt_And_Decrpyt_With_Default_Options(string plainText)
    {
        var encryptedText = _stringEncryptionService.Encrypt(plainText);

        var decryptedText = _stringEncryptionService.Decrypt(encryptedText);

        decryptedText.ShouldBe(plainText);
    }
}
