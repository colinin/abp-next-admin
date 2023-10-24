using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.LocalizationManagement
{
    public class Language : AuditedEntity<Guid>, ILanguageInfo
    {
        public virtual bool Enable { get; set; }
        public virtual string CultureName { get; protected set; }
        public virtual string UiCultureName { get; protected set; }
        public virtual string DisplayName { get; protected set; }
        public virtual string FlagIcon { get; set; }
        protected Language() { }
        public Language(
            Guid id,
            [NotNull] string cultureName,
            [NotNull] string uiCultureName,
            [NotNull] string displayName,
            string flagIcon = null)
            : base(id)
        {
            CultureName = Check.NotNullOrWhiteSpace(cultureName, nameof(cultureName), LanguageConsts.MaxCultureNameLength);
            UiCultureName = Check.NotNullOrWhiteSpace(uiCultureName, nameof(uiCultureName), LanguageConsts.MaxUiCultureNameLength);
            DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), LanguageConsts.MaxDisplayNameLength);

            FlagIcon = !flagIcon.IsNullOrWhiteSpace() 
                ? Check.Length(flagIcon, nameof(flagIcon), LanguageConsts.MaxFlagIconLength)
                : null;

            Enable = true;
        }

        public virtual void SetDisplayName(string displayName)
        {
            DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), LanguageConsts.MaxDisplayNameLength);
        }

        public virtual void SetFlagIcon(string flagIcon)
        {
            FlagIcon = Check.Length(flagIcon, nameof(flagIcon), LanguageConsts.MaxFlagIconLength);
        }

        public virtual void ChangeCulture(string cultureName, string uiCultureName = null, string displayName = null)
        {
            ChangeCultureInternal(cultureName, uiCultureName, displayName);
        }

        private void ChangeCultureInternal(string cultureName, string uiCultureName, string displayName)
        {
            CultureName = Check.NotNullOrWhiteSpace(cultureName, nameof(cultureName), LanguageConsts.MaxCultureNameLength);

            UiCultureName = !uiCultureName.IsNullOrWhiteSpace()
                ? Check.Length(uiCultureName, nameof(uiCultureName), LanguageConsts.MaxUiCultureNameLength)
                : cultureName;

            DisplayName = !displayName.IsNullOrWhiteSpace()
                ? Check.Length(displayName, nameof(displayName), LanguageConsts.MaxDisplayNameLength)
                : cultureName;
        }
    }
}
