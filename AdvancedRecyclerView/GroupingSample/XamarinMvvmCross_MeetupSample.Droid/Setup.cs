using Android.Content;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.ViewModels;

namespace XamarinMvvmCross_MeetupSample.Droid
{
    public class Setup : MvxAndroidSetup<Core.App>
	{
        public Setup()
        {
        }
         
		protected override IMvxApplication CreateApp()
		{
			return new Core.App();
		}

		protected override IMvxAndroidViewPresenter CreateViewPresenter()
		{
			return new ApplicationPresenter(AndroidViewAssemblies);
		}
	
	}
}
