using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using SQLite;

namespace dreams
{
    public class DreamsDatabase
    {
        #region Private Vars
        static object _locker = new object ();

        SQLiteConnection _database;
        #endregion

        #region Constructors
        public DreamsDatabase()
        {
            _database = new SQLiteConnection(DatabasePath);
            _database.CreateTable<DreamRecord>();
        }
        #endregion

        #region Methods
        public IEnumerable<DreamRecord> GetItems () 
        {
            lock(_locker)
                return (from i in _database.Table<DreamRecord>() select i).ToList();
        }
        public DreamRecord GetItem (int id)
        {
            lock(_locker)
                return _database.Table<DreamRecord>().FirstOrDefault(x => x.ID == id);
        }
        public int DeleteItem(int id)
        {
            lock(_locker)
                return _database.Delete<DreamRecord>(id);
        }
        public int SaveItem (DreamRecord item) 
        {
            lock (_locker) {
                if (item.ID != 0) {
                    _database.Update(item);
                    return item.ID;
                } else {
                    return _database.Insert(item);
                }
            }
        }
        #endregion

        #region Properties
        string DatabasePath 
        {
            get {
                var sqliteFilename = "Dreams.db3";
                #if __IOS__
                string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
                string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
                var path = Path.Combine(libraryPath, sqliteFilename);
                #else
                #if __ANDROID__
                string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
                var path = Path.Combine(documentsPath, sqliteFilename);
                #else
                // WinPhone
                var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);;
                #endif
                #endif
                return path;
            }
        }
        #endregion
    }
}

