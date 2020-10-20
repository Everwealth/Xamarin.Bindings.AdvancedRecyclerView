using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;
using XamarinMvvmCross_MeetupSample.Core;
using XamarinMvvmCross_MeetupSample.Core.ViewModels;

namespace XamarinMvvmCross_MeetupSample.Droid
{
	[MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame)]
	public class PeopleListFragment : MvxFragment<PeopleListViewModel>
	{
		public PeopleListFragment()
		{
		}

		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);

			var inflatedView = this.BindingInflate(Resource.Layout.people_list_fragment, container, false);
			ViewModel.Load();
 
			return inflatedView;
		}
	}
}
