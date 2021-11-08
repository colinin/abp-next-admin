namespace LINGYUN.Abp.DataProtection
{
    public class ResourceGrantedResult
    {
        public bool Succeeded => Resource != null;
        public ProtectedResource Resource { get; }
        public ProtectedField[] Fields { get; }
        public ProtectedFieldRule[] Rules { get; }
        public ResourceGrantedResult(
            ProtectedResource resource,
            ProtectedField[] fields,
            ProtectedFieldRule[] rules)
        {
            Resource = resource;
            Fields = fields;
            Rules = rules;
        }
    }
}
