namespace LINGYUN.Abp.Cli.Commands
{
    public static class CreateOptions
    {
        public static string[] ExclusionFolder = new string[3] { ".github", ".vs", ".svn" };

        public static class Package
        {
            public const string Short = "pk";
            public const string Long = "package";
        }

        public static class Company
        {
            public const string Short = "cp";
            public const string Long = "company";
        }

        public static class NoRandomPort
        {
            public const string Short = "nrp";
            public const string Long = "no-random-port";
        }
    }
}
