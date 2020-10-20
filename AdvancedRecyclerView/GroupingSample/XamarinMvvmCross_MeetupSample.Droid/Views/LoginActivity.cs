using System;
using Android.App;
using Android.Runtime;
using Android.Views;
using XamarinMvvmCross_MeetupSample.Core.ViewModels;
using XamarinMvvmCross_MeetupSample.Droid.Views;

namespace XamarinMvvmCross_MeetupSample.Droid
{
	[Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, ClearTaskOnLaunch = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTask)]
	public class LoginActivity : BaseMvxActivity<LoginViewModel>
	{
		public LoginActivity()
		{
		}

		//protected LoginActivity(IntPtr ptr, JniHandleOwnership transfer) : base(ptr, transfer)
		//{

		//}

		protected override System.Collections.Generic.IEnumerable<Core.MessageObserver.IMessageObserver> GetMessageObservers()
		{
			foreach (var baseObserver in base.GetMessageObservers())
				yield return baseObserver;

			foreach (var viewToBlockObserver in BlockViewOnAsyncLongOperationMessageObserver.BuildObservers(FindViewById<ViewGroup>(Resource.Id.rootView)))
				yield return viewToBlockObserver;
		}

		protected override int LayoutResource => Resource.Layout.login;
	}
}
