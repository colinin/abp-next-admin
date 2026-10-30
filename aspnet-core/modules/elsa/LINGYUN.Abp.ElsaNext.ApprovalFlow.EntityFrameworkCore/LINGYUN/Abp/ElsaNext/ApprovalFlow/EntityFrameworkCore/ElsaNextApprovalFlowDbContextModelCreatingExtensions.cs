using LINGYUN.Abp.ElsaNext.ApprovalFlow.CarbonCopies;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Delegations;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDatas;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.OrganizationUnitManagers;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore;

public static class ElsaNextApprovalFlowDbContextModelCreatingExtensions
{
	public static void ConfigureElsaNextApprovalFlow(this ModelBuilder builder)
	{
		Check.NotNull(builder, nameof(builder));
		var prefix = ElsaNextApprovalFlowDbProperties.DbTablePrefix;
		var schema = ElsaNextApprovalFlowDbProperties.DbSchema;

		builder.Entity<ApprovalTemplate>(b =>
		{
			b.ToTable(prefix + "ApprovalTemplates", schema);
			b.ConfigureByConvention();
			b.Property(x => x.Name).HasMaxLength(ApprovalTemplateConsts.MaxNameLength).IsRequired(true);
			b.Property(x => x.DisplayName).HasMaxLength(ApprovalTemplateConsts.MaxDisplayNameLength).IsRequired(true);
			b.Property(x => x.Description).HasMaxLength(ApprovalTemplateConsts.MaxDescriptionLength);
			b.Property(x => x.Category).HasMaxLength(ApprovalTemplateConsts.MaxCategoryLength);
			b.Property(x => x.Icon).HasMaxLength(ApprovalTemplateConsts.MaxIconLength);
			b.Property(x => x.BusinessKeyHint).HasMaxLength(ApprovalTemplateConsts.MaxBusinessKeyHintLength);
			b.Property(x => x.FlowDefinitionJson);
			b.Property(x => x.ElsaDefinitionId).HasMaxLength(ApprovalTemplateConsts.MaxElsaDefinitionIdLength);
			b.Property(x => x.ElsaDefinitionKey).HasMaxLength(ApprovalTemplateConsts.MaxElsaDefinitionKeyLength);
			b.HasIndex(x => new { x.TenantId, x.Name }).IsUnique(true);
			b.HasIndex(x => new { x.TenantId, x.IsPublished, x.IsEnabled });
		});
		builder.Entity<ApprovalInstance>(b =>
		{
			b.ToTable(prefix + "ApprovalInstances", schema);
			b.ConfigureByConvention();
			b.Property(x => x.TemplateName).HasMaxLength(ApprovalInstanceConsts.MaxTemplateNameLength).IsRequired(true);
			b.Property(x => x.TemplateCategory).HasMaxLength(ApprovalInstanceConsts.MaxTemplateCategoryLength);
			b.Property(x => x.Title).HasMaxLength(ApprovalInstanceConsts.MaxTitleLength).IsRequired(true);
			b.Property(x => x.BusinessKey).HasMaxLength(ApprovalInstanceConsts.MaxBusinessKeyLength);
			b.Property(x => x.Outcome).HasMaxLength(ApprovalInstanceConsts.MaxOutcomeLength);
			b.Property(x => x.CurrentNodeId).HasMaxLength(ApprovalInstanceConsts.MaxCurrentNodeIdLength);
			b.Property(x => x.CurrentNodeName).HasMaxLength(ApprovalInstanceConsts.MaxCurrentNodeNameLength);
			b.Property(x => x.ElsaWorkflowInstanceId).HasMaxLength(ApprovalInstanceConsts.MaxElsaWorkflowInstanceIdLength);
			b.Property(x => x.Status).HasConversion<string>().HasMaxLength(ApprovalInstanceConsts.MaxStatusLength)
				.IsRequired(true);
			b.Property(x => x.InitiatorProvider).HasMaxLength(ApprovalInstanceConsts.MaxParticipantProviderLength);
			b.Property(x => x.InitiatorType).HasMaxLength(ApprovalInstanceConsts.MaxParticipantTypeLength);
			b.Property(x => x.InitiatorId).HasMaxLength(ApprovalInstanceConsts.MaxParticipantIdLength);
			b.Property(x => x.InitiatorDisplayName).HasMaxLength(ApprovalInstanceConsts.MaxParticipantDisplayNameLength);
			b.HasIndex(x => new { x.TenantId, x.Status });
			b.HasIndex(x => new { x.TenantId, x.TemplateId });
			b.HasIndex(x => new { x.TenantId, x.InitiatorId });
			b.HasIndex(x => x.ElsaWorkflowInstanceId);
		});
		builder.Entity<ApprovalRecord>(b =>
		{
			b.ToTable(prefix + "ApprovalRecords", schema);
			b.ConfigureByConvention();
			b.Property(x => x.ActionKey).HasMaxLength(ApprovalRecordConsts.MaxActionKeyLength).IsRequired(true);
			b.Property(x => x.ActionLabel).HasMaxLength(ApprovalRecordConsts.MaxActionLabelLength);
			b.Property(x => x.Effect).HasConversion<string>().HasMaxLength(ApprovalRecordConsts.MaxActionEffectLength)
				.IsRequired(true);
			b.Property(x => x.Comment).HasMaxLength(ApprovalRecordConsts.MaxCommentLength);
			b.Property(x => x.NodeId).HasMaxLength(ApprovalRecordConsts.MaxNodeIdLength);
			b.Property(x => x.NodeName).HasMaxLength(ApprovalRecordConsts.MaxNodeNameLength);
			b.Property(x => x.NodeInstanceId).HasMaxLength(ApprovalRecordConsts.MaxNodeInstanceIdLength);
			b.Property(x => x.TaskId).HasMaxLength(ApprovalRecordConsts.MaxTaskIdLength);
			b.Property(x => x.ActorProvider).HasMaxLength(ApprovalRecordConsts.MaxActorProviderLength);
			b.Property(x => x.ActorType).HasMaxLength(ApprovalRecordConsts.MaxActorTypeLength);
			b.Property(x => x.ActorId).HasMaxLength(ApprovalRecordConsts.MaxActorIdLength);
			b.Property(x => x.ActorDisplayName).HasMaxLength(ApprovalRecordConsts.MaxActorDisplayNameLength);
			b.HasIndex(x => new { x.InstanceId, x.OccurredAt });
		});
		builder.Entity<ApprovalCc>(b =>
		{
			b.ToTable(prefix + "ApprovalCcs", schema);
			b.ConfigureByConvention();
			b.Property(x => x.Title).HasMaxLength(ApprovalCcConsts.MaxTitleLength).IsRequired(true);
			b.Property(x => x.Summary).HasMaxLength(ApprovalCcConsts.MaxSummaryLength);
			b.Property(x => x.InstanceReference).HasMaxLength(ApprovalCcConsts.MaxInstanceReferenceLength);
			b.Property(x => x.NodeId).HasMaxLength(ApprovalCcConsts.MaxNodeIdLength);
			b.Property(x => x.NodeInstanceId).HasMaxLength(ApprovalCcConsts.MaxNodeInstanceIdLength);
			b.Property(x => x.RecipientProvider).HasMaxLength(ApprovalCcConsts.MaxParticipantProviderLength);
			b.Property(x => x.RecipientType).HasMaxLength(ApprovalCcConsts.MaxParticipantTypeLength);
			b.Property(x => x.RecipientId).HasMaxLength(ApprovalCcConsts.MaxParticipantIdLength);
			b.Property(x => x.RecipientDisplayName).HasMaxLength(ApprovalCcConsts.MaxParticipantDisplayNameLength);
			b.Property(x => x.SenderProvider).HasMaxLength(ApprovalCcConsts.MaxParticipantProviderLength);
			b.Property(x => x.SenderType).HasMaxLength(ApprovalCcConsts.MaxParticipantTypeLength);
			b.Property(x => x.SenderId).HasMaxLength(ApprovalCcConsts.MaxParticipantIdLength);
			b.Property(x => x.SenderDisplayName).HasMaxLength(ApprovalCcConsts.MaxParticipantDisplayNameLength);
			b.HasIndex(x => new { x.TenantId, x.RecipientId, x.IsRead });
			b.HasIndex(x => x.InstanceId);
		});
		builder.Entity<ApprovalNodeTask>(b =>
		{
			b.ToTable(prefix + "ApprovalNodeTasks", schema);
			b.ConfigureByConvention();
			b.Property(x => x.NodeInstanceId).HasMaxLength(ApprovalNodeTaskConsts.MaxNodeInstanceIdLength).IsRequired(true);
			b.Property(x => x.NodeId).HasMaxLength(ApprovalNodeTaskConsts.MaxNodeIdLength);
			b.Property(x => x.NodeName).HasMaxLength(ApprovalNodeTaskConsts.MaxNodeNameLength);
			b.Property(x => x.TaskId).HasMaxLength(ApprovalNodeTaskConsts.MaxTaskIdLength).IsRequired(true);
			b.Property(x => x.SignMode).HasConversion<string>().HasMaxLength(ApprovalNodeTaskConsts.MaxSignModeLength)
				.IsRequired(true);
			b.Property(x => x.Status).HasConversion<string>().HasMaxLength(ApprovalNodeTaskConsts.MaxStatusLength)
				.IsRequired(true);
			b.Property(x => x.Origin).HasConversion<string>().HasMaxLength(ApprovalNodeTaskConsts.MaxOriginLength)
				.IsRequired(true);

			b.Property(x => x.FirstViewedAt);

			b.Property(x => x.CompletedAt);
			b.Property(x => x.ApproverProvider).HasMaxLength(ApprovalNodeTaskConsts.MaxApproverProviderLength);
			b.Property(x => x.ApproverType).HasMaxLength(ApprovalNodeTaskConsts.MaxApproverTypeLength);
			b.Property(x => x.ApproverId).HasMaxLength(ApprovalNodeTaskConsts.MaxApproverIdLength);
			b.Property(x => x.ApproverDisplayName).HasMaxLength(ApprovalNodeTaskConsts.MaxApproverDisplayNameLength);
			b.HasIndex(x => x.TaskId).IsUnique(true);
			b.HasIndex(x => new { x.NodeInstanceId, x.Status });
			b.HasIndex(x => x.InstanceId);
		});
		builder.Entity<ApprovalDelegation>(b =>
		{
			b.ToTable(prefix + "ApprovalDelegations", schema);
			b.ConfigureByConvention();
			b.Property(x => x.DelegatorProvider).HasMaxLength(ApprovalDelegationConsts.MaxParticipantProviderLength);
			b.Property(x => x.DelegatorType).HasMaxLength(ApprovalDelegationConsts.MaxParticipantTypeLength);
			b.Property(x => x.DelegatorId).HasMaxLength(ApprovalDelegationConsts.MaxParticipantIdLength);
			b.Property(x => x.DelegatorDisplayName).HasMaxLength(ApprovalDelegationConsts.MaxParticipantDisplayNameLength);
			b.Property(x => x.DelegateProvider).HasMaxLength(ApprovalDelegationConsts.MaxParticipantProviderLength);
			b.Property(x => x.DelegateType).HasMaxLength(ApprovalDelegationConsts.MaxParticipantTypeLength);
			b.Property(x => x.DelegateId).HasMaxLength(ApprovalDelegationConsts.MaxParticipantIdLength);
			b.Property(x => x.DelegateDisplayName).HasMaxLength(ApprovalDelegationConsts.MaxParticipantDisplayNameLength);
			b.Property(x => x.Remark).HasMaxLength(ApprovalDelegationConsts.MaxRemarkLength);
			b.HasIndex(x => new { x.TenantId, x.DelegatorId, x.IsEnabled });
			b.HasIndex(x => new { x.TenantId, x.DelegateId, x.IsEnabled });
		});
		builder.Entity<FormDefinition>(b =>
		{
			b.ToTable(prefix + "FormDefinitions", schema);
			b.ConfigureByConvention();
			b.Property(x => x.Name).HasMaxLength(FormDefinitionConsts.MaxNameLength).IsRequired(true);
			b.Property(x => x.Code).HasMaxLength(FormDefinitionConsts.MaxCodeLength).IsRequired(true);
			b.Property(x => x.Description).HasMaxLength(FormDefinitionConsts.MaxDescriptionLength);
			b.HasIndex(x => new { x.TenantId, x.Code, x.Version }).IsUnique(true);
			b.HasIndex(x => new { x.TenantId, x.IsPublished });
			b.HasMany(x => x.Fields).WithOne().HasForeignKey(x => x.FormDefinitionId)
				.OnDelete((DeleteBehavior)3);
			b.Navigation(x => x.Fields).UsePropertyAccessMode(PropertyAccessMode.Field);
		});
		builder.Entity<FormFieldDefinition>(b =>
		{
			b.ToTable(prefix + "FormFieldDefinitions", schema);
			b.ConfigureByConvention();
			b.Property(x => x.Key).HasMaxLength(FormFieldDefinitionConsts.MaxKeyLength).IsRequired(true);
			b.Property(x => x.Label).HasMaxLength(FormFieldDefinitionConsts.MaxLabelLength).IsRequired(true);
			b.Property(x => x.Type).HasMaxLength(FormFieldDefinitionConsts.MaxTypeLength).IsRequired(true);
			b.Property(x => x.Permission).HasMaxLength(FormFieldDefinitionConsts.MaxPermissionLength).IsRequired(true);
			b.Property(x => x.Hint).HasMaxLength(FormFieldDefinitionConsts.MaxHintLength);
			b.Property(x => x.OptionsJson).HasMaxLength(FormFieldDefinitionConsts.MaxOptionsJsonLength);
			b.HasIndex(x => new { x.FormDefinitionId, x.Key }).IsUnique(true);
		});
		builder.Entity<FormData>(b =>
		{
			b.ToTable(prefix + "FormDatas", schema);
			b.ConfigureByConvention();
			b.Property(x => x.FormCode).HasMaxLength(FormDataConsts.MaxFormCodeLength).IsRequired(true);
			b.Property(x => x.InstanceId).HasMaxLength(FormDataConsts.MaxInstanceIdLength).IsRequired(true);
			b.Property(x => x.DataJson).IsRequired(true);
			b.HasIndex(x => new { x.FormDefinitionId, x.InstanceId }).IsUnique(true);
			b.HasIndex(x => new { x.TenantId, x.FormCode });
		});
		builder.Entity<OrganizationUnitManagerAssignment>(b =>
		{
			b.ToTable(prefix + "OrganizationUnitManagerAssignments", schema);
			b.ConfigureByConvention();
			b.Property(x => x.Remark).HasMaxLength(OrganizationUnitManagerConsts.MaxRemarkLength);
			b.HasIndex(x => new { x.TenantId, x.OrganizationUnitId, x.UserId }).IsUnique(true);
			b.HasIndex(x => new { x.TenantId, x.OrganizationUnitId, x.IsPrimary, x.Order });
			b.HasIndex(x => new { x.TenantId, x.UserId });
		});
	}
}
