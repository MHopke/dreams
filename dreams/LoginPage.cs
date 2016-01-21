using System;

using Xamarin.Forms;

using Parse;

namespace dreams
{
    public class LoginPage : ContentPage
    {
        #region Private Vars
        bool _login = true;

        string _email, _password, _firstName, _lastName;

        Button _loginButton, _signupButton;

        StackLayout _nameStack;

        BoxView _loginUnderline, _signupUnderline;

        StackLayout _indicatorLayout;
        ActivityIndicator _indicator;
        #endregion

        #region Constructors
        public LoginPage()
        {
            BackgroundColor = Colors.DreamBlue;//ColorExtensions.HeadersDark;

            _login = true;

            #region Components
            BoxView left = new BoxView()
                {
                    Color = Color.White
                };

            BoxView right = new BoxView()
                {
                    Color = Color.White
                };

            Label title = new Label()
                {
                    Text = "Dreams",
                    FontFamily = Device.OnPlatform("Arial",null,null),
                    FontSize = 45.0,
                    TextColor = Color.White,
                    XAlign = TextAlignment.Center
                };

            _loginButton = new Button()
                {
                    Text = "LOGIN",
                    TextColor = Color.White,
                    FontFamily = Device.OnPlatform("Arial",null,null),
                    FontSize = 20.0,
                    //IsEnabled = false
                };
            _loginButton.Clicked += Signin;
            _loginUnderline = new BoxView() { Color = Color.White };

            _signupButton = new Button()
                {
                    Text = "SIGN UP",
                    TextColor = Color.White,
                    FontFamily = Device.OnPlatform("Arial",null,null),
                    FontSize = 20.0
                };
            _signupButton.Clicked += Signup;
            _signupUnderline = new BoxView() { Color = Color.White, IsVisible = false };

            Label or = new Label()
                {
                    Text = "OR",
                    TextColor = Color.White,
                    FontFamily = Device.OnPlatform("Arial",null,null),
                    FontSize = 12.0,
                    XAlign = TextAlignment.Center,
                    YAlign = TextAlignment.Center
                };

            Entry username = new Entry()
                {
                    Placeholder = "Email",
                    Keyboard = Keyboard.Email
                };
            username.BindingContext = this;
            username.SetBinding(Entry.TextProperty, "Email");

            Entry password = new Entry()
                {
                    Placeholder = "Password",
                    IsPassword = true
                };
            password.BindingContext = this;
            password.SetBinding(Entry.TextProperty, "Password");

            Entry firstName = new Entry()
                {
                    Placeholder = "First Name"
                };
            firstName.BindingContext = this;
            firstName.SetBinding(Entry.TextProperty, "FirstName");

            Entry lastName = new Entry()
                {
                    Placeholder = "Last Name"
                };
            lastName.BindingContext = this;
            lastName.SetBinding(Entry.TextProperty, "LastName");

            Button confirm = new Button()
                {
                    BorderRadius = 7,
                    BackgroundColor = Color.White,
                    Text = "LET'S GO",
                    TextColor = Colors.DreamBlue,//ColorExtensions.HeadersLight,
                    FontFamily = Device.OnPlatform("Arial", null, null),
                    FontSize = 16
                };
            confirm.Clicked += Confirm;

            _indicator = new ActivityIndicator()
                {
                    InputTransparent = true,
                    Color = Color.Black
                };
            #endregion

            #region Layout Setup
            RelativeLayout layout = new RelativeLayout();

            layout.Children.Add(left,
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Height * 0.18; }),
                Constraint.RelativeToParent((parent) => { return parent.Width * 0.12; }),
                Constraint.RelativeToParent((parent) => { return parent.Height * 0.017; } ));

            layout.Children.Add(right,
                Constraint.RelativeToParent((parent) => { return parent.Width - parent.Width * 0.12; }),
                Constraint.RelativeToParent((parent) => { return parent.Height * 0.18; }),
                Constraint.RelativeToParent((parent) => { return parent.Width * 0.12; }),
                Constraint.RelativeToParent((parent) => { return parent.Height * 0.017; } ));

            layout.Children.Add(title,
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Y + parent.Height * 0.14; }),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height * 0.13; }));

            layout.Children.Add(_loginButton,
                Constraint.RelativeToParent((parent) => { return parent.Width * 0.16; }),
                Constraint.RelativeToParent((parent) => { return parent.Height * 0.32; }),
                Constraint.RelativeToParent((parent) => { return parent.Width * 0.25; }),
                Constraint.RelativeToParent((parent) => { return parent.Height * 0.05; } ));

            layout.Children.Add(_loginUnderline,
                Constraint.RelativeToView(_loginButton,(parent,view) => { return view.X; }),
                Constraint.RelativeToView(_loginButton,(parent,view) => { return view.Y + view.Height; }),
                Constraint.RelativeToView(_loginButton,(parent,view) => { return view.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height * 0.008; }));

            layout.Children.Add(_signupButton,
                Constraint.RelativeToParent((parent) => { return parent.Width - parent.Width * 0.44; }),
                Constraint.RelativeToParent((parent) => { return parent.Height * 0.32; }),
                Constraint.RelativeToParent((parent) => { return parent.Width * 0.28; }),
                Constraint.RelativeToView(_loginButton,(parent,view) => { return view.Height; } ));

            layout.Children.Add(_signupUnderline,
                Constraint.RelativeToView(_signupButton,(parent,view) => { return view.X; }),
                Constraint.RelativeToView(_signupButton,(parent,view) => { return view.Y + view.Height; }),
                Constraint.RelativeToView(_signupButton,(parent,view) => { return view.Width; }),
                Constraint.RelativeToView(_loginUnderline,(parent,view) => { return view.Height; }));

            layout.Children.Add(or,
                Constraint.RelativeToParent((parent) => { return parent.Width * 0.48 - parent.Width * 0.125; }),
                Constraint.RelativeToParent((parent) => { return parent.Height * 0.32; }),
                Constraint.RelativeToParent((parent) => { return parent.Width * 0.25; }),
                Constraint.RelativeToView(_loginButton,(parent,view) => { return view.Height; } ));

            _nameStack = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Children = 
                        {
                            firstName,
                            lastName
                        },
                    IsVisible = false
                };

            StackLayout entryStack = new StackLayout()
                {
                    Children = 
                        {
                            _nameStack,
                            username,
                            password
                        }
                    };

            layout.Children.Add(entryStack,
                Constraint.RelativeToView(_loginButton,(parent,view) => { return view.X; }),
                Constraint.RelativeToParent((parent) => { return parent.Y + parent.Height * 0.46;}),
                Constraint.RelativeToParent((parent) => { return parent.Width * 0.68; }),
                Constraint.RelativeToParent((parent) => { return parent.Height * 0.4; }));

            /*layout.Children.Add(password,
                Constraint.RelativeToView(username,(parent,view) => { return view.X; }),
                Constraint.RelativeToView(username,(parent,view) => { return view.Y + view.Height + parent.Height * 0.02;}),
                Constraint.RelativeToView(username,(parent,view) => { return view.Width; }),
                Constraint.RelativeToView(username,(parent,view) => { return view.Height; }));*/

            layout.Children.Add(confirm,
                Constraint.RelativeToParent((parent) => { return parent.Width / 2.0 - parent.Width * 0.19; }),
                Constraint.RelativeToParent((parent) => { return parent.Y + parent.Height * 0.72; }),
                Constraint.RelativeToParent((parent) => { return parent.Width * 0.38; }),
                Constraint.RelativeToParent((parent) => { return parent.Height * 0.05; }));

            _indicatorLayout = new StackLayout()
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Children = { _indicator },
                    IsVisible = false
                };
            
            Grid grid = new Grid()
                {
                    Children = 
                        {
                            layout,
                            _indicatorLayout
                        }
                    };
            #endregion

            Content = grid;
        }
        #endregion

        #region UI Listeners
        void Signup(object sender, EventArgs args)
        {
            if (_login)
            {
                _signupButton.FontFamily = Device.OnPlatform("Arial", null, null);
                _signupUnderline.IsVisible = true;
                //_signupButton.IsEnabled = false;

                _loginButton.FontFamily = Device.OnPlatform("Arial", null, null);
                _loginUnderline.IsVisible = false;
                //_loginButton.IsEnabled = true;

                _nameStack.IsVisible = true;

                _login = false;
            }
        }
        void Signin(object sender, EventArgs args)
        {
            if (!_login)
            {
                _signupButton.FontFamily = Device.OnPlatform("Arial", null, null);
                _signupUnderline.IsVisible = false;
                //_signupButton.IsEnabled = true;

                _loginButton.FontFamily = Device.OnPlatform("Arial", null, null);
                _loginUnderline.IsVisible = true;
                //_loginButton.IsEnabled = false;

                _nameStack.IsVisible = false;

                _login = true;
            }
        }
        async void Confirm(object sender, EventArgs args)
        {
            //Console.WriteLine(_login);

            _indicatorLayout.IsVisible = true;
            _indicator.IsRunning = true;

            if (_login)
            {
                try
                {
                    await ParseUser.LogInAsync(_email, _password);

                    _indicator.IsRunning = false;
                    _indicatorLayout.IsVisible = false;

                    await DreamsAPI.PullRecords();

                    MessagingCenter.Send<LoginPage>(this, "DataPulled");

                    await  Navigation.PopModalAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine("error in login: " + e.Message);

                    _indicatorLayout.IsVisible = false;
                    _indicator.IsRunning = false;
                }
            }
            else
            {
                ParseUser user = new ParseUser()
                    {
                        Username = _email,
                        Email = _email,
                        Password = _password,
                    };

                user[DreamsAPI.FIRST_NAME] = _firstName;
                user[DreamsAPI.LAST_NAME] = _lastName;

                try
                {
                    await user.SignUpAsync();

                    _indicator.IsRunning = false;
                    _indicatorLayout.IsVisible = false;
                    await Navigation.PopModalAsync();
                }
                catch(Exception e)
                {
                    Console.WriteLine("error in signup: " + e.Message);

                    _indicatorLayout.IsVisible = false;
                    _indicator.IsRunning = false;
                }
            }
        }
        #endregion

        #region Properties
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;

                    OnPropertyChanged();
                }
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;

                    OnPropertyChanged();
                }
            }
        }
        #endregion
    }
}


