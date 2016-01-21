using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using Xamarin.Forms;

namespace dreams
{
    public class JournalPage : ContentPage
    {
        #region Overridden Methods
        ObservableCollection<JournalMonth> _months;
        #endregion

        #region Constructors
        public JournalPage()
        {
            Title = "Dream Journal";
            NavigationPage.SetBackButtonTitle(this, "Journal");

            DateTime now = DateTime.UtcNow;
            DateTime start = DreamsAPI.PUser.CreatedAt.Value;

            _months = new ObservableCollection<JournalMonth>();
            //Console.WriteLine(start);
            while (start <= now)
            {
                JournalMonth jMonth = new JournalMonth() { Date = start };
                jMonth.Records = new ObservableCollection<DreamRecord>(from rec in DreamsAPI.Records
                     where rec.DateRecorded.Month == start.Month && rec.DateRecorded.Year == start.Year
                     select rec);
                jMonth.RecordCount = jMonth.Records.Count;
                _months.Add(jMonth);

                start = start.AddMonths(1);
            }

            JournalMonth curMonth = new JournalMonth() { Date = now };
            curMonth.Records = new ObservableCollection<DreamRecord>(from rec in DreamsAPI.Records
                where rec.DateRecorded.Month == now.Month && rec.DateRecorded.Year == now.Year
                select rec);
            curMonth.RecordCount = curMonth.Records.Count;
            _months.Add(curMonth);

            ListView list = new ListView()
            {
                    SeparatorVisibility = SeparatorVisibility.None
            };

            list.ItemsSource = _months;
            list.ItemTemplate = new DataTemplate(typeof(JournalMonthCell));
            list.ItemTapped += MonthTapped;

            StackLayout layout = new StackLayout()
            {
                Children =
                {
                    list
                }
            };

            Content = layout;
        }
        #endregion

        #region Overridden Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            DreamRecordPage.recordUpdated -= RecordUpdated;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            DreamRecordPage.recordUpdated += RecordUpdated;
        }
        #endregion

        #region UI Event Listeners
        void MonthTapped (object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new MonthPage(e.Item as JournalMonth));
        }
        void RecordUpdated(DreamRecord previous, DreamRecord record)
        {
            if (record.DateRecorded.Month != previous.DateRecorded.Month)
            {
                JournalMonth month = null;
                for (int index = 0; index < _months.Count; index++)
                {
                    month = _months[index];

                    if (month.Date.Month == previous.DateRecorded.Month)
                    {
                        month.Records.Remove(record);
                        month.RecordCount--;
                    }
                    else if (month.Date.Month == record.DateRecorded.Month)
                    {
                        month.Records.Add(record);
                        month.RecordCount++;
                    }
                }
            }
        }
        #endregion
    }
}


