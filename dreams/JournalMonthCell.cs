using System;

using Xamarin.Forms;

namespace dreams
{
    public class JournalMonthCell : ViewCell
    {
        #region Constructors
        public JournalMonthCell()
        {
            //Content = new Label { Text = "Hello ContentView" };

            Label monYr = new Label()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                HorizontalTextAlignment = TextAlignment.Start
            };
            monYr.SetBinding(Label.TextProperty, new Binding("Date") { StringFormat = "{0:MMMM - yyyy}" });

            Label entryCount = new Label()
            {
                HorizontalOptions = LayoutOptions.End,
                HorizontalTextAlignment = TextAlignment.End
            };
            entryCount.SetBinding(Label.TextProperty, new Binding("RecordCount") 
                { StringFormat = "Records: {0}" });

            StackLayout layout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                    Padding = new Thickness(5,0,5,0),
                Children = 
                    {
                        monYr,
                        entryCount
                    }
            };

            View = layout;
        }
        #endregion
    }
}


