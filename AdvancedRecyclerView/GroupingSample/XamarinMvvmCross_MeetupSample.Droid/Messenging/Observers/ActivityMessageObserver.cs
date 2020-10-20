using System;
using AndroidX.AppCompat.App;
using MvvmCross.Plugin.Messenger;
using XamarinMvvmCross_MeetupSample.Core;
using XamarinMvvmCross_MeetupSample.Core.MessageObserver;

namespace XamarinMvvmCross_MeetupSample.Droid
{
	abstract class ActivityMessageObserver<TMessage> : MessageObserver<TMessage> where TMessage : MvxMessage
	{
		private readonly WeakReference<AppCompatActivity> _activityReference;

		protected ActivityMessageObserver(WeakReference<AppCompatActivity> activityReference)
		{
			_activityReference = activityReference;

		}

		protected LayerResponse<AppCompatActivity> GetActivity()
		{
			AppCompatActivity activity;
			_activityReference.TryGetTarget(out activity);

			if (activity != null && activity.Handle != IntPtr.Zero)
				return new LayerResponse<AppCompatActivity>(activity);

			return new LayerResponse<AppCompatActivity>().AddErrorMessage("Activity is not available now.");
		}
	}
}
