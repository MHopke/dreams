using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;

using Syncfusion.SfChart.XForms;

namespace dreams
{
    public class MonthPage : ContentPage
    {
        #region Private Vars
        JournalMonth _month;
        ObservableCollection<ChartDataPoint> _points;
        #endregion

        #region Constructors
        public MonthPage(JournalMonth month)
        {
            _month = month;

            Title = month.Date.ToString("MMMM - yyyy");
            SfChart chart = CreateChart(month);

            ListView list = new ListView(ListViewCachingStrategy.RecycleElement)
                {
                    RowHeight = 40
                };

            list.ItemsSource = month.Records;
            list.ItemTemplate = new DataTemplate(typeof(DreamRecordCell));
            list.ItemTapped +=  ItemTapped;

            StackLayout layout = new StackLayout()
            {
                Children = 
                        {
                            chart,
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

        #region Methods
        SfChart CreateChart(JournalMonth month)
        {
            _points = new ObservableCollection<ChartDataPoint>();

            SfChart chart = new SfChart()
                {
                    HeightRequest = 250
                };
            chart.Title.Text = "Monthly Emotion Overview";

            PieSeries series = new PieSeries()
            {
                XBindingPath = "Emotion",
                YBindingPath = "Value"
            };
            series.ColorModel.Palette = ChartColorPalette.Custom;
            series.DataMarker = new ChartDataMarker();
            series.DataMarkerPosition = CircularSeriesDataMarkerPosition.OutsideExtended;
            series.DataMarker.LabelContent = LabelContent.Percentage;

            series.ColorModel.CustomBrushes.Clear();
            chart.Legend = new ChartLegend();

            Emotion emotion = Emotion.None;
            for (int index = 0; index < DreamRecord.NUM_EMOTIONS; index++)
            {
                emotion = (Emotion)index;
                List<DreamRecord> records = new List<DreamRecord>(from rec in month.Records
                                                                                 where rec.Emotion == emotion
                                                                                 select rec);
                _points.Add(new ChartDataPoint(emotion.ToString(), records.Count));
                series.ColorModel.CustomBrushes.Add(DreamsAPI.GetEmotionColor(emotion));
            }

            series.ItemsSource = _points;

            chart.Series.Add(series);

            return chart;
        }
        void AddNewRecord(DreamRecord record)
        {
            string emotion = record.Emotion.ToString();
            ChartDataPoint point = null;
            for (int index = 0; index < _points.Count; index++)
            {
                point = _points[index];

                //Console.WriteLine($"{point.XValue}  {record.DateRecorded}");

                if(point.XValue.Equals(emotion))
                {
                    _points.Remove(point);
                    point.YValue++;
                    _points.Add(point);
                    break;
                }
            }
        }
        void RemoveRecord(DreamRecord record)
        {
            string emotion = record.Emotion.ToString();
            ChartDataPoint point = null;
            for (int index = 0; index < _points.Count; index++)
            {
                point = _points[index];

                //Console.WriteLine($"{point.XValue}  {record.DateRecorded}");

                if(point.XValue.Equals(emotion))
                {
                    _points.Remove(point);
                    point.YValue--;
                    _points.Add(point);
                    break;
                }
            }
        }
        #endregion

        #region UI Event Listeners
        void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new DreamRecordPage((e.Item as DreamRecord),false));
        }
        void RecordUpdated(DreamRecord previous, DreamRecord record)
        {
            RemoveRecord(previous);

            if (record.DateRecorded.Month == _month.Date.Month)
                AddNewRecord(record);
        }
        #endregion
    }
}


