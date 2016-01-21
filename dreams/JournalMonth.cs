using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace dreams
{
    public class JournalMonth
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Public Vars
        public ObservableCollection<DreamRecord> Records;
        #endregion

        #region Private Vars
        int _recordCount;

        DateTime _date;
        #endregion

        #region Constructors
        public JournalMonth()
        {
        }
        #endregion

        #region Properties
        public int RecordCount
        {
            get { return _recordCount; }
            set
            {
                if (_recordCount != value)
                {
                    _recordCount = value;

                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("RecordCount"));
                }
            }
        }
        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (_date != value)
                {
                    _date = value;

                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Date"));
                }
            }
        }
        #endregion
    }
}

