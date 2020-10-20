using Android.Content;
using AndroidX.RecyclerView.Widget;
using Android.Util;
using Com.H6ah4i.Android.Widget.Advrecyclerview.Swipeable;
using Com.H6ah4i.Android.Widget.Advrecyclerview.Touchguard;
using MvvmCross.AdvancedRecyclerView.Extensions;
using MvvmCross.DroidX.RecyclerView.ItemTemplates;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MvvmCross.AdvancedRecyclerView.Adapters.NonExpandable
{
    public class MvxAdvancedRecyclerViewNonExpandableAdapterController : MvxAdvancedRecyclerViewAdapterController
    {
        private RecyclerViewTouchActionGuardManager _mRecyclerViewTouchActionGuardManager;
        private RecyclerViewSwipeManager _mRecyclerViewSwipeManager;

        public MvxAdvancedRecyclerViewNonExpandableAdapterController(Context context, IAttributeSet attrs,
            RecyclerView recyclerView, IMvxAndroidBindingContext bindingContext) : base(context, attrs, recyclerView,
            bindingContext)
        {
            
        }

        protected override RecyclerView.Adapter BuildWrappedAdapter(IMvxTemplateSelector templateSelector)
        {
            var advancedRecyclerViewAdapter = new MvxNonExpandableAdapter(BindingContext);
            RecyclerView.Adapter adapter = advancedRecyclerViewAdapter;

            var itemUniqueIdProvider =
                MvxAdvancedRecyclerViewAttributeExtensions.BuildUniqueItemIdProvider(Context, Attrs);
            advancedRecyclerViewAdapter.UniqueIdProvider = itemUniqueIdProvider;
            advancedRecyclerViewAdapter.ItemTemplateSelector = templateSelector;

            AdvancedRecyclerViewAdapter = advancedRecyclerViewAdapter;

            bool isSwipeEnabled = MvxAdvancedRecyclerViewAttributeExtensions.IsSwipeSupported(Context, Attrs);

            if (isSwipeEnabled)
            {
                var swipeableTemplate =
                    MvxAdvancedRecyclerViewAttributeExtensions.BuildSwipeableTemplate(Context, Attrs);
                advancedRecyclerViewAdapter.SwipeableTemplate = swipeableTemplate;

                _mRecyclerViewTouchActionGuardManager = new RecyclerViewTouchActionGuardManager();
                _mRecyclerViewTouchActionGuardManager.SetInterceptVerticalScrollingWhileAnimationRunning(true);
                _mRecyclerViewTouchActionGuardManager.Enabled = true;

                _mRecyclerViewSwipeManager = new RecyclerViewSwipeManager();
                return _mRecyclerViewSwipeManager.CreateWrappedAdapter(advancedRecyclerViewAdapter);
            }

            return advancedRecyclerViewAdapter;
        }

        public override void AttachRecyclerView()
        {
            _mRecyclerViewTouchActionGuardManager?.AttachRecyclerView(RecyclerView);
            _mRecyclerViewSwipeManager?.AttachRecyclerView(RecyclerView);
        }

        public override void Dispose()
        {
            base.Dispose();
            _mRecyclerViewTouchActionGuardManager?.Release();
            _mRecyclerViewSwipeManager?.Release();
            _mRecyclerViewTouchActionGuardManager = null;
            _mRecyclerViewSwipeManager = null;
        }
    }
}