﻿using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using Syncfusion.SfChart.XForms.iOS.Renderers;

namespace dreams.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();

            new SfChartRenderer ();

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

