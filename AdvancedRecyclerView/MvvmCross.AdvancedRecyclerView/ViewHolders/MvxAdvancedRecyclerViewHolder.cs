using System;
using System.Windows.Input;
using Android.Views;
using Com.H6ah4i.Android.Widget.Advrecyclerview.Swipeable;
using Com.H6ah4i.Android.Widget.Advrecyclerview.Utils;
using MvvmCross.Base;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.WeakSubscription;

namespace MvvmCross.AdvancedRecyclerView.ViewHolders
{
    public class MvxAdvancedRecyclerViewHolder : AbstractSwipeableItemViewHolder, IMvxRecyclerViewHolder, IMvxBindingContextOwner
    {
        private IMvxBindingContext _bindingContext;

        private object _cachedDataContext;
        private View swipeableView;
        private IDisposable clickSubscription, longClickSubscription;
        private IDisposable itemViewClickSubscription, itemViewLongClickSubscription;
 
        public MvxAdvancedRecyclerViewHolder(View itemView, int swipeableContainerViewId, int underSwipeContainerViewId, IMvxAndroidBindingContext context)
            : base(itemView)
        {
            swipeableView = itemView.FindViewById(swipeableContainerViewId);
            UnderSwipeableContainerView = itemView.FindViewById(underSwipeContainerViewId);
            _bindingContext = context;
        }

        public MvxAdvancedRecyclerViewHolder(IntPtr handle, Android.Runtime.JniHandleOwnership ownership) : base(handle, ownership)
        {
        }


        public event EventHandler<EventArgs> Click;
        public event EventHandler<EventArgs> LongClick;
         
        public override View SwipeableContainerView => swipeableView;

        public View UnderSwipeableContainerView { get; }

        public IMvxBindingContext BindingContext
        {
            get { return _bindingContext; }
            set { throw new NotImplementedException("BindingContext is readonly in the list item"); }
        }

        public object DataContext
        {
            get
            {
                return _bindingContext.DataContext;
            }
            set
            {
                _bindingContext.DataContext = value;

                // This is just a precaution.  If we've set the DataContext to something
                // then we don't need to have the old one still cached.
                if (value != null)
                    _cachedDataContext = null;
            }
        }
               
        public virtual void OnAttachedToWindow()
        {
            if (_cachedDataContext != null && DataContext == null)
                DataContext = _cachedDataContext;

            if (itemViewClickSubscription == null)
                itemViewClickSubscription = ItemView.WeakSubscribe(nameof(View.Click), OnItemViewClick);
            if (itemViewLongClickSubscription == null)
                itemViewLongClickSubscription = ItemView.WeakSubscribe<View, View.LongClickEventArgs>(nameof(View.LongClick), OnItemViewLongClick);
        }

        protected virtual void OnItemViewClick(object sender, EventArgs e)
        {
            Click?.Invoke(this, e);
        }

        protected virtual void OnItemViewLongClick(object sender, EventArgs e)
        {
            LongClick?.Invoke(this, e);
        }
 
        public virtual void OnDetachedFromWindow()
        {
            itemViewClickSubscription?.Dispose();
            itemViewClickSubscription = null;
            itemViewLongClickSubscription?.Dispose();
            itemViewLongClickSubscription = null;

            _cachedDataContext = DataContext;
            DataContext = null;
        }

        public virtual void OnViewRecycled()
        {
            _cachedDataContext = null;
            DataContext = null;
        }

        public int Id { get; set; }

        protected override void Dispose(bool disposing)
        {
            // Clean up the binding context since nothing
            // explicitly Disposes of the ViewHolder.
            itemViewClickSubscription?.Dispose();
            itemViewClickSubscription = null;
            itemViewLongClickSubscription?.Dispose();
            itemViewLongClickSubscription = null;

            _cachedDataContext = null;
            clickSubscription?.Dispose();
            longClickSubscription?.Dispose();

            if (_bindingContext != null)
            {
                _bindingContext.DataContext = null;
                _bindingContext.ClearAllBindings();
                _bindingContext.DisposeIfDisposable();
                _bindingContext = null;
            }

            base.Dispose(disposing);
        }
    }
}
