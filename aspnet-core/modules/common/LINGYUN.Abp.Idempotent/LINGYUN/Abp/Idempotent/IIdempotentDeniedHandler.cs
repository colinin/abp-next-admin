namespace LINGYUN.Abp.Idempotent;
public interface IIdempotentDeniedHandler
{
    void Denied(IdempotentDeniedContext context);
}
