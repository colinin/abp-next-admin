using Volo.Abp;

namespace LINGYUN.Abp.Wrapper
{
    public class FakeException: AbpException
    {
        public FakeException()
        {
        }

        public FakeException(string message)
            :   base(message)
        {
        }
    }
}
