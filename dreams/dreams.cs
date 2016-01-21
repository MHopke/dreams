using System;

using Xamarin.Forms;

namespace dreams
{
	public class App : Application
	{
        #region Constants
        const string USER_KEY = "user";
        #endregion

        #region Constructors
		public App ()
        {
            DreamsAPI.Initialize();
			// The root page of your application
			MainPage = new NavigationPage (new HomePage ()) 
			{
                    BarBackgroundColor = Colors.DreamBlue,
                    BarTextColor = Color.White
			};
		}
        #endregion

        #region App Lifecycle Methods
		protected override void OnStart ()
		{
			// Handle when your app starts
            if (DreamsAPI.PUser != null)
            {
                /*User.CurrentUser.SetData(JsonConvert.DeserializeObject<User>((string)
                    Application.Current.Properties[USER_KEY]));

                if (string.IsNullOrEmpty(User.CurrentUser.AccessToken))
                {
                    (MainPage as NavigationPage).Navigation.PushModalAsync(new LoginPage());
                }
                else
                {
                    //do some database stuff
                    User.CurrentUser.SynchronizeData();
                }*/
            }
            else
            {
                (MainPage as NavigationPage).Navigation.PushModalAsync(new LoginPage());
            }
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
        #endregion
	}

    #region Color Extensions
    public class Colors
    {
        public static Color DreamBlue = Color.FromHex("66ccff");
        public static Color Upset = Color.FromHex("ff5050");
        public static Color Energized = Color.FromHex("33cc33");
        public static Color Scared = Color.FromHex("ffcc00");
        public static Color Sad = Color.FromHex("6666ff");
        public static Color Confused = Color.FromHex("999966");
    }
    #endregion
}

