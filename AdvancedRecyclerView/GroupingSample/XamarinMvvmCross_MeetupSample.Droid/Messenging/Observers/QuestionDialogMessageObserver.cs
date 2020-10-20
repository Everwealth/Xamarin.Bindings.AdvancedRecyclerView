using System;
using AndroidX.AppCompat.App;
using MvvmCross.Plugin.Messenger;
using XamarinMvvmCross_MeetupSample.Core;
using XamarinMvvmCross_MeetupSample.Core.MessageObserver;

namespace XamarinMvvmCross_MeetupSample.Droid
{

	internal class QuestionDialogMessageObserver : ActivityMessageObserver<QuestionDialogMvxMessage>
	{
		public QuestionDialogMessageObserver(WeakReference<AppCompatActivity> activityReference) : base(activityReference)
		{
		}

		protected override void OnMessageArrived(QuestionDialogMvxMessage messageToHandle)
		{
			var activityResponse = GetActivity();

			activityResponse.Handle(() =>
			{
				activityResponse.Results.ShowQuestionDialog(messageToHandle.Title, messageToHandle.Content, messageToHandle.OnYes, messageToHandle.OnNo);
			}, (error) => { });
		}
	}

}
