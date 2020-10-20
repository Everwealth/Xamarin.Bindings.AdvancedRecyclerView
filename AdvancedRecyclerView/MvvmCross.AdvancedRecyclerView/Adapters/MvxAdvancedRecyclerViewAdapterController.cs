using System;
using Android.Content;
using Android.OS;
using AndroidX.RecyclerView.Widget;
using Android.Util;
using Com.H6ah4i.Android.Widget.Advrecyclerview.Utils;
using MvvmCross.AdvancedRecyclerView.Extensions;
using MvvmCross.AdvancedRecyclerView.TemplateSelectors;
using MvvmCross.DroidX.RecyclerView.ItemTemplates;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MvvmCross.AdvancedRecyclerView.Adapters
{
    public abstract class MvxAdvancedRecyclerViewAdapterController : IDisposable
    {
        protected readonly Android.Content.Context Context;
        protected readonly Android.Util.IAttributeSet Attrs;
        protected readonly RecyclerView RecyclerView;
        protected readonly IMvxAndroidBindingContext BindingContext;
        private RecyclerView.Adapter wrappedAdapter;
        
        
        protected MvxAdvancedRecyclerViewAdapterController(Context context, IAttributeSet attrs, RecyclerView recyclerView, IMvxAndroidBindingContext bindingContext)
        {
            this.Context = context;
            this.Attrs = attrs;
            this.RecyclerView = recyclerView;
            this.BindingContext = bindingContext;
        }

        public RecyclerView.Adapter BuildAdapter()
        {
            var templateSelector = BuildTemplateSelector();
            wrappedAdapter = BuildWrappedAdapter(templateSelector);

            return HeaderFooterWrapperAdapter = new MvxHeaderFooterWrapperAdapter(wrappedAdapter, BindingContext)
            {
                HeaderFooterDetails = BuildHeaderFooterDetails(templateSelector)
            };
        }
        
        private Data.MvxHeaderFooterDetails BuildHeaderFooterDetails(IMvxTemplateSelector templateSelector)
        {
            var headerFooterDetails = new Data.MvxHeaderFooterDetails();
            var headerTemplate = templateSelector as IMvxHeaderTemplate;
            var footerTemplate = templateSelector as IMvxFooterTemplate;

            if (headerTemplate != null)
                headerFooterDetails.HeaderLayoutId = headerTemplate.HeaderLayoutId;
            if (footerTemplate != null)
                headerFooterDetails.FooterLayoutId = footerTemplate.FooterLayoutId;

            return headerFooterDetails;
        }

        protected virtual IMvxTemplateSelector BuildTemplateSelector() => MvxAdvancedRecyclerViewAttributeExtensions.BuildItemTemplateSelector(Context, Attrs);

        protected abstract RecyclerView.Adapter BuildWrappedAdapter(IMvxTemplateSelector templateSelector);

        public abstract void AttachRecyclerView();
        
        public IMvxAdvancedRecyclerViewAdapter AdvancedRecyclerViewAdapter { get; protected set; }
        
        public MvxHeaderFooterWrapperAdapter HeaderFooterWrapperAdapter { get; private set; }

        public virtual void RestoreFromBundle(Bundle bundle)
        {
        }

        public virtual void SaveToBundle(Bundle bundle)
        {
        }
        
        public virtual void Dispose()
        {
            if (wrappedAdapter != null)
                WrapperAdapterUtils.ReleaseAll(wrappedAdapter);
            wrappedAdapter = null;
        }
    }
}
