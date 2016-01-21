using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using Parse;
using Syncfusion.SfChart.XForms.iOS.Renderers;

namespace dreams.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();

            ParseObject.RegisterSubclass<ParseDreamRecord>();
            ParseClient.Initialize("w8Z85h1vGjhm17upt4rxKYZxnrnHdEhRoun9q4lm", 
                "nQ0KYwYcTYcwEAFX6TRw0G7TuE6uEIcCQY8KbT8a");

            new SfChartRenderer ();

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

