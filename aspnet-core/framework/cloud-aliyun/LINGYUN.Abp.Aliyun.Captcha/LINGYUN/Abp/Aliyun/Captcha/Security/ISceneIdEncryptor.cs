namespace LINGYUN.Abp.Aliyun.Captcha.Security;

public interface ISceneIdEncryptor
{
    string Encrypt(string sceneId, string ekeyStr, int expireTimeSec);
}
