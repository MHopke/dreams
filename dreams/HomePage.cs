using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Syncfusion.SfChart.XForms;

namespace dreams
{
	public class HomePage : ContentPage
	{
        #region Private Vars
        Dictionary<Emotion,ObservableCollection<ChartDataPoint>> _seriesData;
        #endregion

        #region Constructors
		public HomePage ()
		{
            _seriesData = new Dictionary<Emotion, ObservableCollection<ChartDataPoint>>();

            Button record = new Button()
            {
                BackgroundColor = Colors.DreamBlue,
                TextColor = Color.White,
                Text = "Record Dream"
            };
            record.Clicked += RecordClicked;

            Button journal = new Button()
                {
                    BackgroundColor = Colors.DreamBlue,
                    TextColor = Color.White,
                    Text = "View Journal"
                };
            journal.Clicked += JournalClicked;

            Button search = new Button()
                {
                    BackgroundColor = Colors.DreamBlue,
                    TextColor = Color.White,
                    Text = "Search For Dream"
                };
            search.Clicked += SearchForDream;

            SfChart chart = CreateWeeklyChart();

            StackLayout layout = new StackLayout()
            {
                Children =
                {
                    chart,
                    record,
                    journal,
                    search
                }
            };
            
            Content = layout;

            MessagingCenter.Subscribe<LoginPage>(this, "DataPulled", DataPulled);
            MessagingCenter.Subscribe<DreamRecordPage,DreamRecord>(this, "NewRecord", NewRecord);
		}
        #endregion

        #region Overridden Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            DreamRecordPage.recordUpdated -= UpdatedRecord;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            DreamRecordPage.recordUpdated += UpdatedRecord;
        }
        #endregion

        #region Methods
        SfChart CreateWeeklyChart()
        {
            FilterIntoEmotions();

            SfChart chart = new SfChart()
            {
                    HeightRequest = 250
            };
            chart.Title.Text = "Your Last Seven Days";

            //Initializing Primary Axis
            DateTimeAxis primaryAxis = new DateTimeAxis()
            {
                Interval = 1,
                IntervalType = DateTimeIntervalType.Days,
                ShowMajorGridLines = false,
                ShowMinorGridLines = false
            };
            primaryAxis.LabelStyle.LabelFormat = "MMM dd";

            primaryAxis.Title  =  new ChartAxisTitle  () 
                {
                    Text  =  "Day",
                };

            chart.PrimaryAxis  =  primaryAxis;

            //Initializing Secondary Axis
            NumericalAxis secondaryAxis = new NumericalAxis()
            {
                Interval = 1,
                ShowMinorGridLines = false
            };

            secondaryAxis.Title  =  new ChartAxisTitle  () 
                {
                    Text  =  "Num. Dreams",
                };

            chart.SecondaryAxis  =  secondaryAxis;

            chart.Legend = new ChartLegend();

            foreach(KeyValuePair<Emotion,ObservableCollection<ChartDataPoint>> pair
                in _seriesData)
            {

                StackingColumnSeries series = new StackingColumnSeries()
                {
                        ItemsSource = pair.Value,
                        XBindingPath = "Day",
                        YBindingPath = "Value",
                        Color = DreamsAPI.GetEmotionColor(pair.Key),
                        Label = pair.Key.ToString()
                };

                chart.Series.Add(series);
            }

            return chart;
        }
        void FilterIntoEmotions()
        {
            int index = 0, sub = 0;

            DateTime now = DateTime.Now.Date;
            DateTime date = now;

            Emotion emotion = Emotion.None;
            for (index = 0; index < DreamRecord.NUM_EMOTIONS; index++)
            {
                emotion = (Emotion)index;
                ObservableCollection<ChartDataPoint> points = new ObservableCollection<ChartDataPoint>();
                for (sub = 6; sub >= 0; sub--)
                {
                    date = now.AddDays(-sub);
                    points.Add(new ChartDataPoint(date, new List<DreamRecord>(from rec in DreamsAPI.Records
                        where rec.DateRecorded.Date == date.Date && rec.Emotion == emotion
                        select rec).Count));
                }

                if (_seriesData.ContainsKey(emotion))
                    _seriesData[emotion] = points;
                else
                    _seriesData.Add(emotion, points);
            }
        }
        void AddNewRecord(DreamRecord record)
        {
            if (_seriesData.ContainsKey(record.Emotion))
            {
                ObservableCollection<ChartDataPoint> list = _seriesData[record.Emotion];

                ChartDataPoint point = null;
                for (int index = 0; index < list.Count; index++)
                {
                    point = list[index];

                    //Console.WriteLine($"{point.XValue}  {record.DateRecorded}");

                    if(point.XValue.Equals(record.DateRecorded))
                    {
                        list.Remove(point);
                        point.YValue++;
                        list.Add(point);
                        break;
                    }
                }
            }
        }
        void RemoveRecord(DreamRecord record)
        {
            if (_seriesData.ContainsKey(record.Emotion))
            {
                ObservableCollection<ChartDataPoint> list = _seriesData[record.Emotion];

                ChartDataPoint point = null;
                for (int index = 0; index < list.Count; index++)
                {
                    point = list[index];

                    //Console.WriteLine($"{point.XValue}  {record.DateRecorded}");

                    if(point.XValue.Equals(record.DateRecorded))
                    {
                        list.Remove(point);
                        point.YValue--;
                        list.Add(point);
                        break;
                    }
                }
            }
        }
        #endregion

        #region UI Event Listeners
        void RecordClicked(object sender, EventArgs args)
        {
            Navigation.PushAsync(new DreamRecordPage(new DreamRecord(),true));
        }
        void JournalClicked(object sender, EventArgs args)
        {
            Navigation.PushAsync(new JournalPage());
        }
        void SearchForDream(object sender, EventArgs args)
        {
            Navigation.PushAsync(new SearchPage());
        }
        #endregion

        #region MessageCenter
        void DataPulled(LoginPage sender)
        {
            //Console.WriteLine("data pulled");

            FilterIntoEmotions();
        }
        void NewRecord(DreamRecordPage sender, DreamRecord record)
        {
            //Console.WriteLine("received new record");
            AddNewRecord(record);
        }
        void UpdatedRecord(DreamRecord previous, DreamRecord record)
        {
            //Console.WriteLine("received updated record");

            RemoveRecord(previous);
            AddNewRecord(record);
        }
        #endregion
	}
}


