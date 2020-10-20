using System;
using MvvmCross.AdvancedRecyclerView.Data;
using MvvmCross.DroidX.RecyclerView.ItemTemplates;

namespace MvvmCross.AdvancedRecyclerView.TemplateSelectors
{
    public abstract class MvxExpandableTemplateSelector : IMvxTemplateSelector
    {
        private const int GroupViewType = 12345276;
        private int groupLayoutId;

        protected MvxExpandableTemplateSelector(int groupLayoutId)
        {
            this.groupLayoutId = groupLayoutId;
        }

        public int GetItemViewType(object forItemObject)
        {
            if (forItemObject is MvxGroupedData)
                return GroupViewType;

            return GetChildItemViewType(forItemObject);
        }

        public int GetItemLayoutId(int fromViewType)
        {
            if (fromViewType == GroupViewType)
                return groupLayoutId;

            return GetChildItemLayoutId(fromViewType);
        }

        public int ItemTemplateId { get; set; }

        protected abstract int GetChildItemViewType(object forItemObject);

        protected abstract int GetChildItemLayoutId(int fromViewType);
    }
}