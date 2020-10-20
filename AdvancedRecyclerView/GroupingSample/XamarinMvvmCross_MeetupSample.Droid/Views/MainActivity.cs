using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using AndroidX.AppCompat.Widget;
using MvvmCross;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using XamarinMvvmCross_MeetupSample.Core;
using XamarinMvvmCross_MeetupSample.Core.ViewModels;

namespace XamarinMvvmCross_MeetupSample.Droid.Views
{
	[Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, ClearTaskOnLaunch = true, LaunchMode = LaunchMode.SingleTask)]
	public class MainActivity : BaseMvxActivity<MainViewModel>
	{
		public MainActivity()
		{

		}

		protected override int LayoutResource => Resource.Layout.main;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetupToolbar();
			// this step should be usually done by some kind of ViewModel Command
			Mvx.Resolve<IMvxViewDispatcher>()
			   .ShowViewModel(MvxViewModelRequest.GetDefaultRequest(typeof(PeopleListViewModel)));
		}

		protected override System.Collections.Generic.IEnumerable<Core.MessageObserver.IMessageObserver> GetMessageObservers()
		{
			foreach (var observer in base.GetMessageObservers())
				yield return observer;

			yield return new ToastMvxMessageObserver(() => FindViewById(Resource.Id.layoutRoot));
		}

		private void SetupToolbar()
		{
			Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			toolbar.Title = "InsaneLab Sample";
			toolbar.SetTitleTextColor(Color.White.ToArgb());
			SetSupportActionBar(toolbar);
			SupportActionBar.SetDisplayHomeAsUpEnabled(false);
		}

		public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.main_menu, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
		{
			if (item.ItemId == Resource.Id.logout)
			{
				ViewModel.LogoutCommand.Execute();
				return true;
			}

			return base.OnOptionsItemSelected(item);
		}
	}
}
