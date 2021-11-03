using System;
using System.Collections.Generic;
using System.Linq;

namespace LINGYUN.Abp.UI.Navigation
{
    public class ApplicationMenuList : List<ApplicationMenu>
    {
        public ApplicationMenuList()
        {

        }

        public ApplicationMenuList(int capacity)
            : base(capacity)
        {

        }

        public ApplicationMenuList(IEnumerable<ApplicationMenu> collection)
            : base(collection)
        {

        }

        public void Normalize()
        {
            RemoveEmptyItems();
            Order();
        }

        private void RemoveEmptyItems()
        {
            RemoveAll(item => item.IsLeaf && item.Url.IsNullOrEmpty());
        }

        private void Order()
        {
            //TODO: Is there any way that is more performant?
            var orderedItems = this.OrderBy(item => item.Order).ToArray();
            Clear();
            AddRange(orderedItems);
        }
    }
}
