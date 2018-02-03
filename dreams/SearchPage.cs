using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;

namespace dreams
{
    public class SearchPage : ContentPage
    {
        #region Private Vars
        string[] _tags;
        ObservableCollection<DreamRecord> _list;
        #endregion

        #region Constructors
        public SearchPage()
        {
            Title = "Search For Dream";
            /*Label monthLabel = new Label()
                {
                    Text = "Search For Dream",
                    FontAttributes = FontAttributes.Bold,
                    HeightRequest = 30,
                    HorizontalTextAlignment = TextAlignment.Center
                };*/

            SearchBar search = new SearchBar()
            {
                Placeholder = "Enter tags..."
            };
            search.SearchButtonPressed += OnSearchBarButtonPressed;

            Label label = new Label()
            {
                Text = "Multiple tags should be separated by commas",
                HorizontalTextAlignment = TextAlignment.Center
            };

            ListView list = new ListView(ListViewCachingStrategy.RecycleElement)
                {
                    RowHeight = 40
                };
            
            _list = new ObservableCollection<DreamRecord>();

            list.ItemsSource = _list;
            list.ItemTemplate = new DataTemplate(typeof(DreamRecordCell));
            list.ItemTapped +=  ItemTapped;

            StackLayout layout = new StackLayout()
                {
                    Children = 
                        {
                            search,
                            label,
                            list
                        }
                    };

            Content = layout;

            MessagingCenter.Subscribe<DreamRecordPage,DreamRecord>(this, "UpdatedRecord", UpdatedRecord);
        }
        #endregion

        #region UI Event Listeners
        void OnSearchBarButtonPressed(object sender, EventArgs args)
        {
            SearchBar bar = (SearchBar)sender;
            string text = bar.Text;

            _tags = text.Split(',');

            if (_list.Count > 0)
                _list.Clear();
            
            List<DreamRecord> records = DreamsAPI.GetDreamsWithTag(text);

            for (int index = 0; index < records.Count; index++)
                _list.Add(records[index]);
        }
        void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new DreamRecordPage((e.Item as DreamRecord),false));
        }
        void UpdatedRecord(DreamRecordPage sender, DreamRecord record)
        {
            Console.WriteLine("received updated record");

            if (record.HasNoTags(_tags))
                _list.Remove(record);
        }
        #endregion
    }
}


