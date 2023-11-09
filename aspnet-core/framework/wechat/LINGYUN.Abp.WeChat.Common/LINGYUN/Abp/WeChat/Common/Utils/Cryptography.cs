using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net;

namespace LINGYUN.Abp.WeChat.Common.Utils
{
    public class Cryptography
    {
        public static uint HostToNetworkOrder(uint inval)
        {
            uint outval = 0;
            for (var i = 0; i < 4; i++)
                outval = (outval << 8) + (inval >> i * 8 & 255);
            return outval;
        }

        public static int HostToNetworkOrder(int inval)
        {
            var outval = 0;
            for (var i = 0; i < 4; i++)
                outval = (outval << 8) + (inval >> i * 8 & 255);
            return outval;
        }
        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="Input">密文</param>
        /// <param name="EncodingAESKey"></param>
        /// <returns></returns>
        /// 
        public static string AES_decrypt(string Input, string EncodingAESKey, ref string corpid)
        {
            byte[] Key;
            Key = Convert.FromBase64String(EncodingAESKey + "=");
            var Iv = new byte[16];
            Array.Copy(Key, Iv, 16);
            var btmpMsg = AES_decrypt(Input, Iv, Key);

            var len = BitConverter.ToInt32(btmpMsg, 16);
            len = IPAddress.NetworkToHostOrder(len);


            var bMsg = new byte[len];
            var bCorpid = new byte[btmpMsg.Length - 20 - len];
            Array.Copy(btmpMsg, 20, bMsg, 0, len);
            Array.Copy(btmpMsg, 20 + len, bCorpid, 0, btmpMsg.Length - 20 - len);
            var oriMsg = Encoding.UTF8.GetString(bMsg);
            corpid = Encoding.UTF8.GetString(bCorpid);


            return oriMsg;
        }

        public static string AES_encrypt(string Input, string EncodingAESKey, string corpid)
        {
            byte[] Key;
            Key = Convert.FromBase64String(EncodingAESKey + "=");
            var Iv = new byte[16];
            Array.Copy(Key, Iv, 16);
            var Randcode = CreateRandCode(16);
            var bRand = Encoding.UTF8.GetBytes(Randcode);
            var bCorpid = Encoding.UTF8.GetBytes(corpid);
            var btmpMsg = Encoding.UTF8.GetBytes(Input);
            var bMsgLen = BitConverter.GetBytes(HostToNetworkOrder(btmpMsg.Length));
            var bMsg = new byte[bRand.Length + bMsgLen.Length + bCorpid.Length + btmpMsg.Length];

            Array.Copy(bRand, bMsg, bRand.Length);
            Array.Copy(bMsgLen, 0, bMsg, bRand.Length, bMsgLen.Length);
            Array.Copy(btmpMsg, 0, bMsg, bRand.Length + bMsgLen.Length, btmpMsg.Length);
            Array.Copy(bCorpid, 0, bMsg, bRand.Length + bMsgLen.Length + btmpMsg.Length, bCorpid.Length);

            return AES_encrypt(bMsg, Iv, Key);

        }
        private static string CreateRandCode(int codeLen)
        {
            var codeSerial = "2,3,4,5,6,7,a,c,d,e,f,h,i,j,k,m,n,p,r,s,t,A,C,D,E,F,G,H,J,K,M,N,P,Q,R,S,U,V,W,X,Y,Z";
            if (codeLen == 0)
            {
                codeLen = 16;
            }
            var arr = codeSerial.Split(',');
            var code = "";
            var randValue = -1;
            var rand = new Random(unchecked((int)DateTime.Now.Ticks));
            for (var i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);
                code += arr[randValue];
            }
            return code;
        }

        private static string AES_encrypt(string Input, byte[] Iv, byte[] Key)
        {
            var aes = new RijndaelManaged();
            //秘钥的大小，以位为单位
            aes.KeySize = 256;
            //支持的块大小
            aes.BlockSize = 128;
            //填充模式
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            aes.Key = Key;
            aes.IV = Iv;
            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    var xXml = Encoding.UTF8.GetBytes(Input);
                    cs.Write(xXml, 0, xXml.Length);
                }
                xBuff = ms.ToArray();
            }
            var Output = Convert.ToBase64String(xBuff);
            return Output;
        }

        private static string AES_encrypt(byte[] Input, byte[] Iv, byte[] Key)
        {
            var aes = new RijndaelManaged();
            //秘钥的大小，以位为单位
            aes.KeySize = 256;
            //支持的块大小
            aes.BlockSize = 128;
            //填充模式
            //aes.Padding = PaddingMode.PKCS7;
            aes.Padding = PaddingMode.None;
            aes.Mode = CipherMode.CBC;
            aes.Key = Key;
            aes.IV = Iv;
            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;

            #region 自己进行PKCS7补位，用系统自己带的不行
            var msg = new byte[Input.Length + 32 - Input.Length % 32];
            Array.Copy(Input, msg, Input.Length);
            var pad = KCS7Encoder(Input.Length);
            Array.Copy(pad, 0, msg, Input.Length, pad.Length);
            #endregion

            #region 注释的也是一种方法，效果一样
            //ICryptoTransform transform = aes.CreateEncryptor();
            //byte[] xBuff = transform.TransformFinalBlock(msg, 0, msg.Length);
            #endregion

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    cs.Write(msg, 0, msg.Length);
                }
                xBuff = ms.ToArray();
            }

            var Output = Convert.ToBase64String(xBuff);
            return Output;
        }

        private static byte[] KCS7Encoder(int text_length)
        {
            var block_size = 32;
            // 计算需要填充的位数
            var amount_to_pad = block_size - text_length % block_size;
            if (amount_to_pad == 0)
            {
                amount_to_pad = block_size;
            }
            // 获得补位所用的字符
            var pad_chr = chr(amount_to_pad);
            var tmp = "";
            for (var index = 0; index < amount_to_pad; index++)
            {
                tmp += pad_chr;
            }
            return Encoding.UTF8.GetBytes(tmp);
        }
        /**
         * 将数字转化成ASCII码对应的字符，用于对明文进行补码
         * 
         * @param a 需要转化的数字
         * @return 转化得到的字符
         */
        static char chr(int a)
        {

            var target = (byte)(a & 0xFF);
            return (char)target;
        }
        private static byte[] AES_decrypt(string Input, byte[] Iv, byte[] Key)
        {
            var aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.None;
            aes.Key = Key;
            aes.IV = Iv;
            var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    var xXml = Convert.FromBase64String(Input);
                    var msg = new byte[xXml.Length + 32 - xXml.Length % 32];
                    Array.Copy(xXml, msg, xXml.Length);
                    cs.Write(xXml, 0, xXml.Length);
                }
                xBuff = decode2(ms.ToArray());
            }
            return xBuff;
        }
        private static byte[] decode2(byte[] decrypted)
        {
            var pad = (int)decrypted[decrypted.Length - 1];
            if (pad < 1 || pad > 32)
            {
                pad = 0;
            }
            var res = new byte[decrypted.Length - pad];
            Array.Copy(decrypted, 0, res, 0, decrypted.Length - pad);
            return res;
        }
    }
}
