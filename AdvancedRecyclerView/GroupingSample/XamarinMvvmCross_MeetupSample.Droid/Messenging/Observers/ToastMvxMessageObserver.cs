using System;
using Android.Views;
using Google.Android.Material.Snackbar;
using XamarinMvvmCross_MeetupSample.Core;
using XamarinMvvmCross_MeetupSample.Core.MessageObserver;

namespace XamarinMvvmCross_MeetupSample.Droid
{
	public class ToastMvxMessageObserver : MessageObserver<ToastMvxMessage>
	{
		readonly Func<View> snackbarRootViewProvider;

		public ToastMvxMessageObserver(Func<View> snackbarRootViewProvider)
		{
			this.snackbarRootViewProvider = snackbarRootViewProvider;
		}

		protected override void OnMessageArrived(ToastMvxMessage messageToHandle)
		{
			Snackbar.Make(snackbarRootViewProvider(), messageToHandle.Content, Snackbar.LengthShort)
					.Show();
		}
	}
}
