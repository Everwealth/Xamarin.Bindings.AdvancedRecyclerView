using System;
using System.Collections.Specialized;
using Android.Runtime;
using AndroidX.RecyclerView.Widget;
using Android.Views;
using Com.H6ah4i.Android.Widget.Advrecyclerview.Swipeable;
using Com.H6ah4i.Android.Widget.Advrecyclerview.Swipeable.Action;
using Java.Lang;
using MvvmCross.AdvancedRecyclerView.Data;
using MvvmCross.AdvancedRecyclerView.Data.EventArguments;
using MvvmCross.AdvancedRecyclerView.Data.ItemUniqueIdProvider;
using MvvmCross.AdvancedRecyclerView.Extensions;
using MvvmCross.AdvancedRecyclerView.Swipe.ResultActions;
using MvvmCross.AdvancedRecyclerView.Swipe.ResultActions.ItemManager;
using MvvmCross.AdvancedRecyclerView.Swipe.State;
using MvvmCross.AdvancedRecyclerView.TemplateSelectors;
using MvvmCross.AdvancedRecyclerView.ViewHolders;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using Object = Java.Lang.Object;

namespace MvvmCross.AdvancedRecyclerView.Adapters.NonExpandable
{
    public class MvxNonExpandableAdapter : MvxRecyclerAdapter, ISwipeableItemAdapter, IMvxAdvancedRecyclerViewAdapter
    {
        Lazy<DefaultSwipeableTemplate> _lazyDefaultSwipeableTemplate = new Lazy<DefaultSwipeableTemplate>();

        public MvxNonExpandableAdapter(IMvxAndroidBindingContext bindingContext) : base(bindingContext)
        {
            //HasStableIds = true;
            SetHasStableIds(true); // there is a bug with Xamarin.Android 10.x - MethodNotFoundException for HasStbleIds setter
        }

        public MvxNonExpandableAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }


        IntPtr pointerToSetHasStableId = IntPtr.Zero;
        IntPtr class_ref = IntPtr.Zero;

        [Register("setHasStableIds", "(Z)V", "")]
        public unsafe void SetHasStableIds(bool hasStableIds)
        {
            if (pointerToSetHasStableId == IntPtr.Zero)
            {
                class_ref = JNIEnv.FindClass(typeof(RecyclerView.Adapter));
                pointerToSetHasStableId = JNIEnv.GetMethodID(class_ref, "setHasStableIds", "(Z)V");
            }
            try
            {
                JValue* __args = stackalloc JValue[1];
                __args[0] = new JValue(hasStableIds);
                JNIEnv.CallVoidMethod(this.Handle, pointerToSetHasStableId, __args);
            }
            finally
            {
            }
        }


        private MvxSwipeableTemplate swipeableTemplate;
        public MvxSwipeableTemplate SwipeableTemplate
        {
            get { return swipeableTemplate ?? _lazyDefaultSwipeableTemplate.Value; }
            set { swipeableTemplate = value; }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
            var itemTemplateSelector = ItemTemplateSelector;

            var viewForHolder = InflateViewForHolder(parent, viewType, itemBindingContext);

            var vh = new MvxAdvancedRecyclerViewHolder(viewForHolder,
                                                       SwipeableTemplate.SwipeContainerViewGroupId,
                                                       SwipeableTemplate.UnderSwipeContainerViewGroupId,
                itemBindingContext)
            {
                
            };

            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            base.OnBindViewHolder(holder, position);

            var advancedRecyclerViewHolder = holder as MvxAdvancedRecyclerViewHolder;
            if (SwipeableTemplate != null)
            {
                advancedRecyclerViewHolder.MaxLeftSwipeAmount = SwipeableTemplate.GetMaxLeftSwipeAmount(advancedRecyclerViewHolder.DataContext, advancedRecyclerViewHolder);
                advancedRecyclerViewHolder.MaxRightSwipeAmount = SwipeableTemplate.GetMaxRightSwipeAmount(advancedRecyclerViewHolder.DataContext, advancedRecyclerViewHolder);
                advancedRecyclerViewHolder.MaxDownSwipeAmount = SwipeableTemplate.GetMaxDownSwipeAmount(advancedRecyclerViewHolder.DataContext, advancedRecyclerViewHolder);
                advancedRecyclerViewHolder.MaxUpSwipeAmount = SwipeableTemplate.GetMaxUpSwipeAmount(advancedRecyclerViewHolder.DataContext, advancedRecyclerViewHolder);
                
                SwipeableTemplate.SetupUnderSwipeBackground(advancedRecyclerViewHolder);
                SwipeableTemplate.SetupSlideAmount(advancedRecyclerViewHolder, SwipeItemPinnedStateController);
            }
        }

        public override void OnViewAttachedToWindow(Object holder)
        {
            base.OnViewAttachedToWindow(holder);

            var viewHolder = (IMvxRecyclerViewHolder)holder;
            viewHolder.OnAttachedToWindow();
        }

        public override void OnViewDetachedFromWindow(Object holder)
        {
            var viewHolder = (IMvxRecyclerViewHolder)holder;

            viewHolder.OnDetachedFromWindow();
            base.OnViewDetachedFromWindow(holder);
        }

        public int OnGetSwipeReactionType(Object p0, int p1, int x, int y)
        {
            var viewHolder = p0 as MvxAdvancedRecyclerViewHolder;

            return viewHolder.SwipeableContainerView.HitTest(x, y)
                ? SwipeableTemplate.GetSwipeReactionType(viewHolder.DataContext, viewHolder)
                : SwipeableItemConstants.ReactionCanNotSwipeAny;
        }

        public void OnSetSwipeBackground(Object viewHolder, int position, int type)
        {
            var advancedViewHolder = viewHolder as MvxAdvancedRecyclerViewHolder;
            var args = new MvxSwipeBackgroundSetEventArgs()
            {
                ViewHolder = advancedViewHolder,
                Position = position,
                Type = type
            };
            SwipeBackgroundSet?.Invoke(args);
            SwipeableTemplate?.OnSwipeBackgroundSet(args);
        }

        public SwipeResultAction OnSwipeItem(Object p0, int position, int result)
        {
            switch (result)
            {
                case SwipeableItemConstants.ResultSwipedDown:
                    return SwipeResultActionFactory.GetSwipeDownResultAction(new NonExpandableSwipeResultActionItemManager(this, position));
                case SwipeableItemConstants.ResultSwipedLeft:
                    return SwipeResultActionFactory.GetSwipeLeftResultAction(new NonExpandableSwipeResultActionItemManager(this, position));
                case SwipeableItemConstants.ResultSwipedRight:
                    return SwipeResultActionFactory.GetSwipeRightResultAction(new NonExpandableSwipeResultActionItemManager(this, position));
                case SwipeableItemConstants.ResultSwipedUp:
                    return SwipeResultActionFactory.GetSwipeUpResultAction(new NonExpandableSwipeResultActionItemManager(this, position));
                default:
                    return position != RecyclerView.NoPosition ? 
                        SwipeResultActionFactory.GetUnpinSwipeResultAction(new NonExpandableSwipeResultActionItemManager(this, position)) :
                        new SwipeResultActionDoNothing();
            }
        }

        public override long GetItemId(int position)
        {
            if (UniqueIdProvider == null)
                throw new InvalidOperationException($"You have to assing {nameof(UniqueIdProvider)} property to use AdvancedRecyclerView Adapter");

            var item = GetItem(position);
            return UniqueIdProvider.GetUniqueId(item);
        }
 
        public MvxSwipeResultActionFactory SwipeResultActionFactory => SwipeableTemplate?.SwipeResultActionFactory ?? new MvxSwipeResultActionFactory();

        private IMvxItemUniqueIdProvider uniqueIdProvider;
        public IMvxItemUniqueIdProvider UniqueIdProvider
        {
            get { return uniqueIdProvider; }
            set
            {
                uniqueIdProvider = value;
                SwipeItemPinnedStateController.UniqueIdProvider = value;
            }
        }

        public SwipeItemPinnedStateControllerProvider SwipeItemPinnedStateController { get; } = new SwipeItemPinnedStateControllerProvider();

        public override void NotifyDataSetChanged(NotifyCollectionChangedEventArgs e)
        {
            base.NotifyDataSetChanged(e);

            if (e.Action == NotifyCollectionChangedAction.Reset)
                SwipeItemPinnedStateController.ResetState();
            else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            {
                foreach(var itemsToRemove in e.OldItems)
                    SwipeItemPinnedStateController.SetPinnedForAllStates(itemsToRemove, false);
            }
        }

        public void OnSwipeItemStarted(Object p0, int p1)
        {
            this.NotifyDataSetChanged();
        }

        public event Action<MvxSwipeBackgroundSetEventArgs> SwipeBackgroundSet;
    }
}