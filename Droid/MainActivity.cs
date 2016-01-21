using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Parse;

namespace dreams.Droid
{
	[Activity (Label = "dreams.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

            ParseObject.RegisterSubclass<ParseDreamRecord>();
            ParseClient.Initialize("w8Z85h1vGjhm17upt4rxKYZxnrnHdEhRoun9q4lm", 
                "nQ0KYwYcTYcwEAFX6TRw0G7TuE6uEIcCQY8KbT8a");

			LoadApplication (new App ());
		}
	}
}

