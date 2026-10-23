namespace LINGYUN.Abp.ElsaNext.ApprovalFlow;

public static class ElsaNextApprovalFlowErrorCodes
{
    public const string Namespace = "ElsaNextApprovalFlow";
    public static class Template
    {
        public const string NameAlreadyExists = Namespace + ":010001";

        public const string NotFound = Namespace + ":010002";

        public const string NotPublished = Namespace + ":010003";

        public const string DesignRequired = Namespace + ":010004";

        public const string WorkflowInvalid = Namespace + ":010005";
    }

    public static class Instance
    {
        public const string NotFound = Namespace + ":020001";

        public const string AlreadyFinished = Namespace + ":020002";

        public const string NoPendingTask = Namespace + ":020003";

        public const string AmbiguousTask = Namespace + ":020004";

        public const string NotInitiator = Namespace + ":020005";


        public const string NodeNotFound = Namespace + ":020007";

        public const string NodeNotAdded = Namespace + ":020008";

        public const string NodeNotPending = Namespace + ":020009";

        public const string ApproverRequired = Namespace + ":020010";

        public const string TransfereeRequired = Namespace + ":020011";

        public const string TransferRejected = Namespace + ":020012";

        public const string OutcomeNotAllowed = Namespace + ":020014";

        public const string StatusCannotBeRunning = Namespace + ":020015";
    }

    public static class Delegation
    {
        public const string NotFound = Namespace + ":030001";

        public const string InvalidPeriod = Namespace + ":030002";

        public const string SelfDelegation = Namespace + ":030003";


    }

    public static class FormDefinition
    {
        public const string CodeAlreadyExists = Namespace + ":040001";

        public const string AlreadyPublished = Namespace + ":040002";

        public const string NoField = Namespace + ":040003";
    }

    public static class FormField
    {
        public const string KeyAlreadyExists = Namespace + ":040101";

        public const string NotFound = Namespace + ":040102";

        public const string InvalidType = Namespace + ":040103";

        public const string InvalidPermission = Namespace + ":040104";
    }

    public static class FormData
    {
        public const string NotFound = Namespace + ":040201";

        public const string DefinitionNotPublished = Namespace + ":040202";

        public const string InvalidJson = Namespace + ":040203";

        public const string FieldHidden = Namespace + ":040204";

        public const string RequiredFieldMissing = Namespace + ":040205";
    }

    public static class Workflow
    {
        public const string DefinitionVersionIdRequired = Namespace + ":080001";

        public const string WorkflowInstanceIdRequired = Namespace + ":080002";
    }

    public static class ApprovalTask
    {
        public const string TooManyApprovers = Namespace + ":100001";

        public const string NoApprovers = Namespace + ":100002";
    }

    public static class OrganizationUnitManager
    {
        public const string AlreadyExists = Namespace + ":101001";

        public const string NotFound = Namespace + ":101002";

        public const string OrganizationUnitNotFound = Namespace + ":101003";

        public const string PrimaryConflict = Namespace + ":101004";
    }

    public static class OrganizationUnitSupervisor
    {
        public const string ChainTooDeep = Namespace + ":101101";
    }
}
