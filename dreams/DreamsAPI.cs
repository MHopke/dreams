using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using Parse;

using Newtonsoft.Json;

using Xamarin;
using Xamarin.Forms;

namespace dreams
{
    public class DreamsAPI
    {
        #region Constants
        public const string FIRST_NAME = "firstName";
        public const string LAST_NAME = "lastName";
        const string ENTRY_COUNT = "entryCount";
        const string RECORDS = "records";
        const string UPDATED_AT = "updatedAt";
        #endregion

        #region Private Vars
        static DateTime _updatedDate;
        static Dictionary<Emotion,Color> _emotionColors;
        #endregion

        #region Constructors
        public DreamsAPI()
        {
        }
        #endregion

        #region Methods
        public static void Initialize()
        {
            Emotions = new List<Emotion>();
            for (int index = 1; index < DreamRecord.NUM_EMOTIONS; index++)
            {
                Emotions.Add((Emotion)index);
            }

            _emotionColors = new Dictionary<Emotion, Color>();
            _emotionColors.Add(Emotion.None, Color.Gray);
            _emotionColors.Add(Emotion.Confused, Colors.Confused);
            _emotionColors.Add(Emotion.Upset, Colors.Upset);
            _emotionColors.Add(Emotion.Energized, Colors.Energized);
            _emotionColors.Add(Emotion.Sad, Colors.Sad);
            _emotionColors.Add(Emotion.Scared, Colors.Scared);

            if (App.Current.Properties.ContainsKey(RECORDS))
            {
                Records = JsonConvert.DeserializeObject<ObservableCollection<DreamRecord>>
                    ((string)App.Current.Properties[RECORDS]);
            }
            else
                Records = new ObservableCollection<DreamRecord>();

            if (App.Current.Properties.ContainsKey(UPDATED_AT))
                _updatedDate = JsonConvert.DeserializeObject<DateTime>(
                    (string)App.Current.Properties[UPDATED_AT]);
            else
                _updatedDate = DateTime.MinValue;
        }
        public static void Save()
        {
            if (App.Current.Properties.ContainsKey(RECORDS))
                App.Current.Properties[RECORDS] = JsonConvert.SerializeObject(Records);
            else
                App.Current.Properties.Add(RECORDS, JsonConvert.SerializeObject(Records));

            if (App.Current.Properties.ContainsKey(UPDATED_AT))
                App.Current.Properties[UPDATED_AT] = JsonConvert.SerializeObject(_updatedDate);
            else
                App.Current.Properties.Add(UPDATED_AT, JsonConvert.SerializeObject(_updatedDate));

            App.Current.SavePropertiesAsync();
        }

        public static async Task PullRecords()
        {
            ParseQuery<ParseDreamRecord> query = 
                new ParseQuery<ParseDreamRecord>().WhereGreaterThan(UPDATED_AT, _updatedDate);

            query = query.Limit(1000);

            IEnumerable<ParseDreamRecord> records = await query.FindAsync();

            foreach (ParseDreamRecord pRecord in records)
                Records.Add(new DreamRecord(pRecord));

            Save();

            return;//MessagingCenter.Send<DreamsAPI>(this, "DataPulled");
        }

        public static Task AddRecord(DreamRecord record)
        {
            Records.Add(record);

            if (PUser.ContainsKey(ENTRY_COUNT))
                PUser[ENTRY_COUNT] = PUser.Get<int>(ENTRY_COUNT) + 1;
            else
                PUser.Add(ENTRY_COUNT, 1);

            Save();

            return PUser.SaveAsync();
        }
        public static Color GetEmotionColor(Emotion emotion)
        {
            if (_emotionColors.ContainsKey(emotion))
                return _emotionColors[emotion];
            else
                return Color.White;
        }
        public static List<DreamRecord> GetDreamsWithTag(string tag)
        {
            List<DreamRecord> list = new List<DreamRecord>();

            string[] tags = tag.Split(',');
            for (int index = 0; index < tags.Length; index++)
            {
                list.AddRange(new List<DreamRecord>(from rec in Records
                      where rec.Tags.Contains(tag) && !list.Contains(rec)
                      select rec));
            }
            return list;
        }
        #endregion

        #region Properties
        public static List<Emotion> Emotions;

        public static ParseUser PUser
        {
            get { return ParseUser.CurrentUser; }
        }

        public int EntryCount
        {
            get { return PUser.Get<int>(ENTRY_COUNT); }
            set { PUser[ENTRY_COUNT] = value; }
        }

        public static ObservableCollection<DreamRecord> Records;
        #endregion
    }
}

