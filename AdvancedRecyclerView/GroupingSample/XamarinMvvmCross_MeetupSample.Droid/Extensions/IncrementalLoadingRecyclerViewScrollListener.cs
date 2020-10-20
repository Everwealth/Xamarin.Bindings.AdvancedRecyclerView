using System;
using Android.Runtime;
using AndroidX.RecyclerView.Widget;
using XamarinMvvmCross_MeetupSample.Core;

namespace XamarinMvvmCross_MeetupSample.Droid
{
	internal class IncrementalLoadingRecyclerViewScrollListener : RecyclerView.OnScrollListener
	{
		private readonly IIncrementalLoading _incrementalLoading;
		private const int visibleThreshold = 5;
		private int firstVisibleItem;
		private int visibleItemCount;
		private int totalItemCount;

		public IncrementalLoadingRecyclerViewScrollListener(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
		{
		}

		public IncrementalLoadingRecyclerViewScrollListener(IIncrementalLoading incrementalLoading)
		{
			_incrementalLoading = incrementalLoading;
		}

		public override async void OnScrolled(RecyclerView recyclerView, int dx, int dy)
		{
			base.OnScrolled(recyclerView, dx, dy);

			visibleItemCount = recyclerView.ChildCount;
			totalItemCount = recyclerView.GetLayoutManager().ItemCount;
			firstVisibleItem = (recyclerView.GetLayoutManager() as LinearLayoutManager).FindFirstVisibleItemPosition();


			if (totalItemCount > 0 && totalItemCount - visibleItemCount
				<= firstVisibleItem + visibleThreshold)
			{
				if (_incrementalLoading != null && _incrementalLoading.ShouldLoadMoreData())
					await _incrementalLoading.LoadMoreData();
			}
		}
	}
}
