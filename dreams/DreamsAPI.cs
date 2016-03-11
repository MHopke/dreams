using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using Newtonsoft.Json;

using Xamarin.Forms;

namespace dreams
{
    public class DreamsAPI
    {
        #region Constants
        const string UPDATED_AT = "updatedAt";
        #endregion

        #region Private Vars
        static DateTime _installDate;
        static DreamsDatabase _database;
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
            _emotionColors = new Dictionary<Emotion, Color>();
            _emotionColors.Add(Emotion.None, Color.Gray);
            _emotionColors.Add(Emotion.Confused, Colors.Confused);
            _emotionColors.Add(Emotion.Upset, Colors.Upset);
            _emotionColors.Add(Emotion.Energized, Colors.Energized);
            _emotionColors.Add(Emotion.Sad, Colors.Sad);
            _emotionColors.Add(Emotion.Scared, Colors.Scared);

            _database = new DreamsDatabase();
        }
        public static void SetupInstallDate()
        {
            if (App.Current.Properties.ContainsKey(UPDATED_AT))
            {
                _installDate = JsonConvert.DeserializeObject<DateTime>
                    ((string)App.Current.Properties[UPDATED_AT]);
            }
            else
            {
                _installDate = DateTime.Now;
                App.Current.Properties.Add(UPDATED_AT, 
                    JsonConvert.SerializeObject(_installDate));

                App.Current.SavePropertiesAsync();
            }
        }
        public static IEnumerable<DreamRecord> GetRecords()
        {
            return _database.GetItems();
        }
        public static void SaveRecord(DreamRecord record)
        {
            _database.SaveItem(record);
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
                list.AddRange(new List<DreamRecord>(from rec in GetRecords()
                      where rec.Tags.Contains(tag) && !list.Contains(rec)
                      select rec));
            }
            return list;
        }
        public static List<Emotion> GetEmotions()
        {
            return Enum.GetValues(typeof(Emotion)).Cast<Emotion>().ToList();
        }
        #endregion

        #region Properties
        public static DateTime InstallDate
        {
            get { return _installDate; }
        }
        #endregion
    }
}

