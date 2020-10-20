using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using AndroidX.RecyclerView.Widget;
using MvvmCross.AdvancedRecyclerView.Adapters;
using MvvmCross.DroidX.RecyclerView.Model;

namespace MvvmCross.AdvancedRecyclerView
{
    [Register("mvvmcross.advancedrecyclerview.MvxAdvancedRecyclerView")]
    public abstract class MvxAdvancedRecyclerView : RecyclerView
    {
        protected MvxAdvancedRecyclerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected MvxAdvancedRecyclerView(Context context) : this(context, null)
        {
        }

        protected MvxAdvancedRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, 0)
        {
        }

        public MvxAdvancedRecyclerView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            var currentLayoutManager = base.GetLayoutManager();

            if (currentLayoutManager == null)
                SetLayoutManager(new LinearLayoutManager(context) { Orientation = LinearLayoutManager.Vertical });
            
            HasFixedSize = false;
        }
        
        protected abstract MvxAdvancedRecyclerViewAdapterController AdapterController {
            get;
        }
        
        public void InitializeAdapter(){
            var adapter = AdapterController.BuildAdapter();

            SetAdapter(adapter);
            AdapterController.AttachRecyclerView();
        }
        
        public IEnumerable ItemsSource
        {
            get => AdvancedRecyclerViewAdapter.ItemsSource;
            set {
                if (GetAdapter() == null)
                    InitializeAdapter();
                AdvancedRecyclerViewAdapter.ItemsSource = value; 
            }
        }
        
        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            AdapterController?.Dispose();
        }

        public void SaveBundleState(Bundle bundle)
        {
            AdapterController.SaveToBundle(bundle);
        }

        public void RestoreBundleState(Bundle bundle)
        {
            AdapterController.RestoreFromBundle(bundle);
        }

        public ICommand HeaderClickCommand
        {
            get => AdapterController.HeaderFooterWrapperAdapter.HeaderClickCommand;
            set => AdapterController.HeaderFooterWrapperAdapter.HeaderClickCommand = value;
        }

        public ICommand HeaderLongClickCommand
        {
            get => AdapterController.HeaderFooterWrapperAdapter.HeaderLongClickCommand;
            set => AdapterController.HeaderFooterWrapperAdapter.HeaderLongClickCommand = value;
        }

        public ICommand FooterClickCommand
        {
            get => AdapterController.HeaderFooterWrapperAdapter.FooterClickCommand;
            set => AdapterController.HeaderFooterWrapperAdapter.FooterClickCommand = value;
        }

        public ICommand FooterLongClickCommand
        {
            get => AdapterController.HeaderFooterWrapperAdapter.FooterLongClickCommand;
            set => AdapterController.HeaderFooterWrapperAdapter.FooterLongClickCommand = value;
        }

        public event Action<MvxViewHolderBoundEventArgs> MvxHeaderViewHolderBound {
            add { AdapterController.HeaderFooterWrapperAdapter.MvxHeaderViewHolderBound += value; }
            remove { AdapterController.HeaderFooterWrapperAdapter.MvxHeaderViewHolderBound -= value; }
        }

        public event Action<MvxViewHolderBoundEventArgs> MvxFooterViewHolderBound {
            add { AdapterController.HeaderFooterWrapperAdapter.MvxFooterViewHolderBound += value; }
            remove { AdapterController.HeaderFooterWrapperAdapter.MvxFooterViewHolderBound -= value; }
        }
        
        public IMvxAdvancedRecyclerViewAdapter AdvancedRecyclerViewAdapter => AdapterController.AdvancedRecyclerViewAdapter;

    }
}