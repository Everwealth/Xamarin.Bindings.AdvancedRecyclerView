using System;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Com.H6ah4i.Android.Widget.Advrecyclerview.Animator;
using MvvmCross.AdvancedRecyclerView.Adapters;
using MvvmCross.AdvancedRecyclerView.Adapters.Expandable;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MvvmCross.AdvancedRecyclerView
{
    [Register("mvvmcross.advancedrecyclerview.MvxAdvancedExpandableRecyclerView")]
    public class MvxAdvancedExpandableRecyclerView : MvxAdvancedRecyclerView
    {
        public MvxAdvancedExpandableRecyclerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public MvxAdvancedExpandableRecyclerView(Context context) : this(context, null)
        {
        }

        public MvxAdvancedExpandableRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, 0)
        {
        }

        public MvxAdvancedExpandableRecyclerView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            AdapterController = new MvxAdvancedRecyclerViewExpandableAdapterController(context, attrs, this, MvxAndroidBindingContextHelpers.Current()); 
            SetItemAnimator(new RefactoredDefaultItemAnimator());
        }

        public MvxExpandableItemAdapter ExpandableItemAdapter => AdapterController.AdvancedRecyclerViewAdapter as MvxExpandableItemAdapter;
        
        public ICommand GroupItemClick
        {
            get => ExpandableItemAdapter.GroupItemClickCommand;
            set => ExpandableItemAdapter.GroupItemClickCommand = value;
        }

        public ICommand GroupItemLongClick
        {
            get => ExpandableItemAdapter.GroupItemLongClickCommand;
            set => ExpandableItemAdapter.GroupItemLongClickCommand = value;
        }

        public ICommand ChildItemClick
        {
            get => ExpandableItemAdapter.ChildItemClickCommand;
            set => ExpandableItemAdapter.ChildItemClickCommand = value;
        }

        public ICommand ChildItemLongClick
        {
            get => ExpandableItemAdapter.ChildItemLongClickCommand;
            set => ExpandableItemAdapter.ChildItemLongClickCommand = value;
        }

        protected override MvxAdvancedRecyclerViewAdapterController AdapterController { get; }
    }
}
