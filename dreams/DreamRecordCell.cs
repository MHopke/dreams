using System;

using Xamarin.Forms;

namespace dreams
{
    public class DreamRecordCell : ViewCell
    {
        #region Constructors
        public DreamRecordCell()
        {
            /*Button button = new Button()
                {
                    BorderColor = 
                }*/
            Label dateLabel = new Label();
            dateLabel.SetBinding(Label.TextProperty, new Binding("DateRecorded") { StringFormat = "{0:d}" } );

            Label label = new Label();
            label.SetBinding(Label.TextProperty, "Title");

            StackLayout layout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    dateLabel,
                    label
                }
            };
            View = layout;
            //Content = new Label { Text = "Hello ContentView" };
        }
        #endregion
    }
}


