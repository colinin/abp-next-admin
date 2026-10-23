namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;

public static class FormFieldTypes
{
	public const string SingleLineText = "single-line-text";
	public const string MultiLineText = "multi-line-text";
	public const string Number = "number";
	public const string Date = "date";
	public const string DateRange = "date-range";
	public const string Select = "select";
	public const string Radio = "radio";
	public const string MultipleSelect = "multiple-select";
	public const string Switch = "switch";
	public const string Attachment = "attachment";
	public const string Description = "description";
	public const string Calculated = "calculated";

	public static readonly IReadOnlyList<string> All = new string[12]
	{
        SingleLineText,
        MultiLineText,
        Number,
        Date,
        DateRange,
        Select,
        Radio,
        MultipleSelect,
        Switch,
        Attachment,
        Description,
        Calculated
    };

	public static bool IsKnown(string? type)
	{
		return type != null && All.Contains(type);
	}
}
