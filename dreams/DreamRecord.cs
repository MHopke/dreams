using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

using Parse;
using Newtonsoft.Json;

namespace dreams
{
    public enum Emotion { None = 0, Scared, Energized, Confused, Sad, Upset }

    [ParseClassName("DreamRecord")]
    public class ParseDreamRecord : ParseObject
    {
        #region Constructors
        public ParseDreamRecord ()
        {
        }
        #endregion

        #region Methods
        public void SetData(DreamRecord record)
        {
            Title = record.Title;
            Description = record.Description;
            DateRecorded = record.DateRecorded;
            Emotion = record.Emotion;
            Tags = record.Tags;
        }
        #endregion

        #region Properties
        [ParseFieldName("title")]
        public string Title
        {
            get { return GetProperty<string>("Title"); }
            set { SetProperty<string>(value, "Title"); }
        }

        [ParseFieldName("description")]
        public string Description
        {
            get { return GetProperty<string>("Description"); }
            set { SetProperty<string>(value, "Description"); }
        }

        [ParseFieldName("tags")]
        public string Tags
        {
            get { return GetProperty<string>("Tags"); }
            set { SetProperty<string>(value, "Tags"); }
        }

        [ParseFieldName("emotion")]
        public Emotion Emotion
        {
            get { return (Emotion)GetProperty<int>("Emotion"); }
            set { SetProperty<int>((int)value, "Emotion"); }
        }

        [ParseFieldName("dateRecorded")]
        public DateTime DateRecorded
        {
            get { return GetProperty<DateTime>("DateRecorded"); }
            set { SetProperty<DateTime>(value, "DateRecorded"); }
        }
        #endregion
    }
        
    public class DreamRecord
	{
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constants
        public const int NUM_EMOTIONS = 6;
        #endregion

        #region Private Vars
        string _description, _title, _tags;
        Emotion _emotion;
        DateTime _dateRecorded;
        #endregion

		#region Constructors
		public DreamRecord ()
		{
		}
        public DreamRecord(ParseDreamRecord record)
        {
            ObjectId = record.ObjectId;
            Description = record.Description;
            Title = record.Title;

            DateRecorded = record.DateRecorded;

            Emotion = record.Emotion;

            if (record.Tags != null)
                Tags = record.Tags;
        }
        public DreamRecord(DreamRecord other)
        {
            ObjectId = other.ObjectId;
            Title = other.Title;
            Description = other.Description;
            DateRecorded = other.DateRecorded;
            Emotion = other.Emotion;

            Tags = other.Tags;
        }
		#endregion

        #region Methods
        public bool HasNoTags(string[] searchingTags)
        {
            List<string> tags = Tags.Split(',').ToList();

            for (int index = 0; index < searchingTags.Length; index++)
            {
                if (tags.Contains(searchingTags[index]))
                    return false;
            }

            return true;
        }
        #endregion

		#region Properties
        public string ObjectId;

        public string Title
        {
            get { return _title; }
            set
            {
                if(_title != value)
                {
                    _title = value;

                    if(PropertyChanged != null)
                        PropertyChanged(this,new PropertyChangedEventArgs("Title"));
                }
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if(_description != value)
                {
                    _description = value;

                    if(PropertyChanged != null)
                        PropertyChanged(this,new PropertyChangedEventArgs("Description"));
                }
            }
        }

        public string Tags
        {
            get { return _tags; }
            set
            {
                if (_tags != value)
                {
                    _tags = value;

                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Tags"));
                }
            }
        }

        public Emotion Emotion
        {
            get { return _emotion; }
            set
            {
                if (_emotion != value)
                {
                    _emotion = value;

                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Emotion"));
                }
            }
        }

        public DateTime DateRecorded
        {
            get { return _dateRecorded; }
            set
            {
                if (_dateRecorded != value)
                {
                    _dateRecorded = value;

                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("DateRecorded"));
                }
            }
        }
		#endregion
	}
}

