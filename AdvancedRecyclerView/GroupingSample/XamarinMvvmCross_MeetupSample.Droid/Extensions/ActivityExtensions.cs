using System;
using Android.Content;
using AndroidX.AppCompat.App;

namespace XamarinMvvmCross_MeetupSample.Droid
{
	public static class ActivityExtensions
	{
		public static AppCompatActivity ShowQuestionDialog(this AppCompatActivity activity, string title, string content,
		Action onOk, Action onCancel)
		{
			new AlertDialog.Builder(activity)
			    .SetIcon(Resource.Drawable.Icon)
				.SetTitle(title)
				.SetMessage(content)
				.SetPositiveButton("ok", (e, a) => { onOk?.Invoke(); })
				.SetNegativeButton("no, thanks", (e, a) => { onCancel?.Invoke(); })
				.Show();

			return activity;
		}

		public static AppCompatActivity ShowMessageDialog(this AppCompatActivity activity, string title, string content,
		Action onMessageClosed = null)
		{
			Action onClosedAction = onMessageClosed ?? (() => { });

			new AlertDialog.Builder(activity)
			    .SetIcon(Resource.Drawable.Icon)
				.SetTitle(title)
				.SetMessage(content)
				.SetPositiveButton("ok", (e, a) => { })
				.SetOnDismissListener(new DismissActionListenerForDialogWrapper(onClosedAction))
				.Show();

			return activity;
		}


		class DismissActionListenerForDialogWrapper : Java.Lang.Object, IDialogInterfaceOnDismissListener
		{
			Action onClosedAction;

			public DismissActionListenerForDialogWrapper(Action onClosedAction)
			{
				this.onClosedAction = onClosedAction;
			}

			public void OnDismiss(IDialogInterface dialog)
			{
				onClosedAction?.Invoke();
			}
		}
	}
}
