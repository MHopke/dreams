using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace dreams
{
    public class DreamRecordPage : ContentPage
    {
        #region Events
        public static event System.Action<DreamRecord,DreamRecord> recordUpdated;
        #endregion

        #region Constants
        const string UPDATE = "Update Record";
        const string ADD = "Add Record";
        #endregion

        #region Private Vars
        DreamRecord _previousRecord;
        #endregion

        #region Constructors
        public DreamRecordPage(DreamRecord record, bool newRecord)
        {
            _previousRecord = new DreamRecord(record);

            Entry title = new Entry()
                {
                    Placeholder = "Enter title...",
                    HeightRequest = 50
                };
            title.BindingContext = record;
            title.SetBinding(Entry.TextProperty, "Title");

            Label descLabel = new Label()
            {
                Text = "Description",
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
            };
            Editor description = new Editor()
            {
                //HeightRequest = 250,
                BackgroundColor = Color.Gray,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            description.BindingContext = record;
            description.SetBinding(Editor.TextProperty, "Description");

            DatePicker datePicker = new DatePicker()
            {
                Format = "D"
            };
            datePicker.BindingContext = record;
            datePicker.SetBinding(DatePicker.DateProperty, "DateRecorded");

            DateTime now = DateTime.Now;
            if (newRecord)
                datePicker.Date = now;
            else
                datePicker.Date = record.DateRecorded;
            
            datePicker.MinimumDate = datePicker.Date.AddYears(-1);
            datePicker.MaximumDate = now;

            Picker picker = new Picker()
            {
                Title = "Emotion"
            };
            picker.SelectedIndexChanged += (object sender, EventArgs e) => 
                {
                    if(picker.SelectedIndex != -1)
                        record.Emotion = (Emotion)picker.SelectedIndex;
                };

            List<Emotion> emotions = DreamsAPI.GetEmotions();
            for (int index = 0; index < emotions.Count; index++)
                picker.Items.Add((emotions[index]).ToString());

            picker.SelectedIndex = (int)record.Emotion;

            Entry tags = new Entry()
                {
                    Placeholder = "Enter comma separated tags",
                    HeightRequest = 50,
                };
            tags.BindingContext = record;
            tags.SetBinding(Entry.TextProperty, "Tags");

            Button submit = new Button();

            if (newRecord)
                submit.Text = ADD;
            else
                submit.Text = UPDATE;

            submit.Clicked += async (object sender, EventArgs e) => 
                {
                    if(newRecord)
                    {
                        DreamsAPI.SaveRecord(record);

                        MessagingCenter.Send<DreamRecordPage, DreamRecord>(this,"NewRecord",record);
                    }
                    else
                    {
                        try
                        {
                            DreamsAPI.SaveRecord(record);

                            if((_previousRecord.DateRecorded != record.DateRecorded 
                                || _previousRecord.Emotion != record.Emotion) && recordUpdated != null)
                                recordUpdated(_previousRecord,record);
                        }
                        catch(Exception er)
                        {
                            Console.WriteLine(er.Message);
                        }
                    }

                    await Navigation.PopAsync();
                };

            StackLayout descStack = new StackLayout () {
                Children =
                {
                            descLabel,
                            description,
                        },
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            StackLayout bottomStack = new StackLayout () {
                Children =
                        {
                            datePicker,
                            picker,
                            tags,
                            submit
                        },
                VerticalOptions = LayoutOptions.End
            };

            StackLayout layout = new StackLayout()
            {
                    Children = 
                        {
                            title,
                    descStack,
                    bottomStack
                        }
            };

            Content = layout;
        }
        #endregion
    }
}


