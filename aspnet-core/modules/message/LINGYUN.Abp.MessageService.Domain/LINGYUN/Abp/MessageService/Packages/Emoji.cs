using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.MessageService.Packages
{
    public class Emoji : Entity<Guid>
    {
        public virtual string Name { get; protected set; }
        public virtual string Title { get; protected set; }
        public virtual string LinkUrl { get; protected set; }
        protected Emoji() { }
        public Emoji(
            Guid id,
            string name,
            string title,
            string linkUrl)
            :base(id)
        {
            Name = name;
            Title = title;
            LinkUrl = linkUrl;
        }

        public void SetUrl(string linkUrl)
        {
            LinkUrl = linkUrl;
        }
    }
}
