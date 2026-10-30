using Microsoft.Extensions.Options;
using System;
using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.BlobManagement;

/// <summary>
/// 基于HMAC-SHA256的下载密钥生成器
/// </summary>
/// <remarks>
/// 密钥格式: Base64Url(载荷 + HMAC-SHA256(载荷)), 其中载荷为:
/// [版本(1)] [标志位(1)] [BlobId(16)] [TenantId(16, 可选)] [过期时间Unix秒(8)]
/// 服务端无需保存会话即可校验密钥的合法性与有效期
/// </remarks>
public class HmacBlobDownloadKeyGenerator : IBlobDownloadKeyGenerator, ITransientDependency
{
    private const byte FormatVersion = 1;
    private const int MacSize = 32;
    private const int FixedPayloadSize = 1 + 1 + 16 + 8;
    private const int TenantIdSize = 16;
    private const int MaxKeyLength = 512;

    private readonly AbpBlobManagementOptions _options;
    private readonly IClock _clock;

    private byte[]? _secretBytes;

    public HmacBlobDownloadKeyGenerator(
        IOptions<AbpBlobManagementOptions> options,
        IClock clock)
    {
        _options = options.Value;
        _clock = clock;
    }

    public virtual string Generate(Blob blob)
    {
        Check.NotNull(blob, nameof(blob));

        var expiresAt = CalculateExpirationTime(_clock.Now.ToUniversalTime());
        var payload = CreatePayload(blob.Id, blob.TenantId, expiresAt);
        var mac = ComputeMac(payload);

        var token = new byte[payload.Length + MacSize];
        Buffer.BlockCopy(payload, 0, token, 0, payload.Length);
        Buffer.BlockCopy(mac, 0, token, payload.Length, MacSize);

        return Base64UrlEncode(token);
    }

    public virtual BlobDownloadKey? Parse(string key)
    {
        if (string.IsNullOrWhiteSpace(key) || key.Length > MaxKeyLength)
        {
            return null;
        }

        byte[] token;
        try
        {
            token = Base64UrlDecode(key);
        }
        catch (FormatException)
        {
            return null;
        }

        // 只接受规范编码: Base64Url 末尾存在未被使用的填充位,
        // 非规范的编码(如末位大小写/填充位不同)会被解码为相同的字节, 但字符串并不相同
        if (!string.Equals(Base64UrlEncode(token), key, StringComparison.Ordinal))
        {
            return null;
        }

        if (token.Length < FixedPayloadSize + MacSize)
        {
            return null;
        }

        var payloadLength = token.Length - MacSize;
        var payload = new byte[payloadLength];
        Buffer.BlockCopy(token, 0, payload, 0, payloadLength);

        var mac = new byte[MacSize];
        Buffer.BlockCopy(token, payloadLength, mac, 0, MacSize);

        // 防篡改: 使用固定时间比较, 避免时序侧信道
        var expectedMac = ComputeMac(payload);
        if (!CryptographicOperations.FixedTimeEquals(mac, expectedMac))
        {
            return null;
        }

        if (payload[0] != FormatVersion)
        {
            return null;
        }

        var hasTenant = (payload[1] & 1) == 1;
        var expectedPayloadLength = FixedPayloadSize + (hasTenant ? TenantIdSize : 0);
        if (payload.Length != expectedPayloadLength)
        {
            return null;
        }

        var blobId = new Guid(payload.AsSpan(2, 16), bigEndian: true);

        var offset = 2 + 16;
        Guid? tenantId = null;
        if (hasTenant)
        {
            tenantId = new Guid(payload.AsSpan(offset, TenantIdSize), bigEndian: true);
            offset += TenantIdSize;
        }

        var expiresAt = DateTimeOffset
            .FromUnixTimeSeconds(BinaryPrimitives.ReadInt64BigEndian(payload.AsSpan(offset, 8)))
            .UtcDateTime;

        // 过期密钥等同于无效密钥
        if (expiresAt <= _clock.Now.ToUniversalTime())
        {
            return null;
        }

        return new BlobDownloadKey(blobId, tenantId, expiresAt);
    }

    /// <summary>
    /// 计算密钥过期时间(按时间窗口对齐)
    /// </summary>
    /// <remarks>
    /// 时间分桶: 将当前时间按 <see cref="AbpBlobManagementOptions.DownloadKeyWindow"/> 对齐到窗口起点,
    /// 过期时间 = 窗口结束时间 + DownloadKeyLifetime, 因此同一窗口内生成的载荷完全一致(密钥相同),
    /// 且剩余有效期不会小于 DownloadKeyLifetime
    /// </remarks>
    protected virtual DateTime CalculateExpirationTime(DateTime now)
    {
        var window = _options.DownloadKeyWindow;
        if (window <= TimeSpan.Zero)
        {
            // 未启用时间分桶时退化为单次生成
            return now.Add(_options.DownloadKeyLifetime);
        }

        // 对齐到窗口起点(UTC ticks 取整)
        var windowStartTicks = now.Ticks - (now.Ticks % window.Ticks);
        var expiresAtTicks = windowStartTicks + window.Ticks + _options.DownloadKeyLifetime.Ticks;

        return new DateTime(expiresAtTicks, DateTimeKind.Utc);
    }

    protected virtual byte[] CreatePayload(Guid blobId, Guid? tenantId, DateTime expiresAt)
    {
        var hasTenant = tenantId.HasValue;
        var payload = new byte[FixedPayloadSize + (hasTenant ? TenantIdSize : 0)];

        payload[0] = FormatVersion;
        payload[1] = (byte)(hasTenant ? 1 : 0);

        blobId.TryWriteBytes(payload.AsSpan(2, 16), bigEndian: true, out _);

        var offset = 2 + 16;
        if (hasTenant)
        {
            tenantId!.Value.TryWriteBytes(payload.AsSpan(offset, TenantIdSize), bigEndian: true, out _);
            offset += TenantIdSize;
        }

        var expiresUnixSeconds = new DateTimeOffset(expiresAt).ToUnixTimeSeconds();
        BinaryPrimitives.WriteInt64BigEndian(payload.AsSpan(offset, 8), expiresUnixSeconds);

        return payload;
    }

    protected virtual byte[] ComputeMac(byte[] payload)
    {
        using var hmac = new HMACSHA256(GetSecretBytes());

        return hmac.ComputeHash(payload);
    }

    protected virtual byte[] GetSecretBytes()
    {
        if (_secretBytes != null)
        {
            return _secretBytes;
        }

        var secret = _options.DownloadKeySecret;
        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new AbpException(
                $"Blob download key secret is not configured. Please configure '{nameof(AbpBlobManagementOptions.DownloadKeySecret)}'.");
        }

        _secretBytes = Encoding.UTF8.GetBytes(secret);

        return _secretBytes;
    }

    protected static string Base64UrlEncode(byte[] bytes)
    {
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    protected static byte[] Base64UrlDecode(string value)
    {
        var base64 = value.Replace('-', '+').Replace('_', '/');
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
            case 1:
                throw new FormatException("Invalid base64url string.");
        }

        return Convert.FromBase64String(base64);
    }
}
