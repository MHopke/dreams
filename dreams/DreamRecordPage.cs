using System;

using Xamarin.Forms;

using Parse;

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
                HeightRequest = 300,
                BackgroundColor = Color.Gray,
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

            for (int index = 0; index < DreamsAPI.Emotions.Count; index++)
                picker.Items.Add((DreamsAPI.Emotions[index]).ToString());

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
                        ParseDreamRecord pRecord = new ParseDreamRecord();

                        pRecord.SetData(record);
                        pRecord.ACL = new Parse.ParseACL(DreamsAPI.PUser);

                        await pRecord.SaveAsync();

                        record.ObjectId = pRecord.ObjectId;
                        await DreamsAPI.AddRecord(record);

                        MessagingCenter.Send<DreamRecordPage, DreamRecord>(this,"NewRecord",record);
                    }
                    else
                    {
                        ParseDreamRecord pRecord = ParseObject.CreateWithoutData
                            <ParseDreamRecord>(record.ObjectId);

                        try
                        {
                            await pRecord.FetchAsync();
                            pRecord.SetData(record);

                            await pRecord.SaveAsync();

                            DreamsAPI.Save();

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

            StackLayout layout = new StackLayout()
            {
                    Children = 
                        {
                            title,
                            descLabel,
                            description,
                            datePicker,
                            picker,
                            tags,
                            submit
                        }
            };

            Content = layout;
        }
        #endregion
    }
}


