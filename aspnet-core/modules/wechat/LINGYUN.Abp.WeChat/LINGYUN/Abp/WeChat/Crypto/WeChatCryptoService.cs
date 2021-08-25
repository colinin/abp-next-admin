using System;
using System.Security.Cryptography;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WeChat.Crypto
{
    public class WeChatCryptoService : IWeChatCryptoService, ITransientDependency
    {
        public virtual string Decrypt(string encryptedData, string iv, string sessionKey)
        {
            using var aes = new AesCryptoServiceProvider();
            aes.Mode = CipherMode.CBC;
            aes.BlockSize = 128;
            // 对称解密使用的算法为 AES-128-CBC，数据采用PKCS#7填充。
            aes.Padding = PaddingMode.PKCS7;

            //格式化待处理字符串
            // 对称解密的目标密文为 Base64_Decode(encryptedData)。
            byte[] byte_encryptedData = Convert.FromBase64String(encryptedData);
            // 对称解密算法初始向量 为Base64_Decode(iv)，其中iv由数据接口返回。
            byte[] byte_iv = Convert.FromBase64String(iv);
            // 对称解密秘钥 aeskey = Base64_Decode(session_key), aeskey 是16字节。
            byte[] byte_sessionKey = Convert.FromBase64String(sessionKey);

            //根据设置好的数据生成解密器实例
            using var transform = aes.CreateDecryptor(byte_iv, byte_sessionKey);
            //解密
            byte[] final = transform.TransformFinalBlock(byte_encryptedData, 0, byte_encryptedData.Length);

            //生成结果
            string result = Encoding.UTF8.GetString(final);
            return result;
        }
    }
}
