using System.Collections.Generic;
using System.Linq;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;

public static class FormFieldPermissions
{
	public const string ReadOnly = "R";

	public const string Editable = "E";

	public const string Hidden = "H";

	public static readonly IReadOnlyList<string> All = new string[3] { "R", "E", "H" };

	public static bool IsKnown(string? permission)
	{
		return permission != null && All.Contains(permission);
	}

	public static bool CanRead(string permission)
	{
		return permission != "H";
	}

	public static bool CanWrite(string permission)
	{
		return permission == "E";
	}
}
