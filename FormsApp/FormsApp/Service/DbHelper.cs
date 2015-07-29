using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FormsApp.Model;
//using FormsApp.ViewModel;
using SQLite;

namespace FormsApp.Service
{
    public static class DbHelper
    {
        private static SQLiteAsyncConnection AsyncConnection;
        private const string DbName = "foo.sq3";
        private static SQLiteConnection Connection;
        private static string dbFolder;
        public static void SetDbFolder(string dbFolder)
        {
            DbHelper.dbFolder = dbFolder;
        }
        public static void Init()
        {
            Connection = new SQLiteConnection(Path.Combine(dbFolder, DbName));
            Connection.CreateTable<TimeSet>();
            //connection.CreateTable<Stage>();
            Connection.CreateTable<Settings>();
            
            //Connection.Close();
            //Connection.Dispose();
            AsyncConnection = new SQLiteAsyncConnection(Path.Combine(dbFolder, DbName));

        }

        public static void CloseConnection()
        {
            Connection.Close();
        }
        public static Task<int> AddTimeSet(TimeSet timeSet)
        {
            return AsyncConnection.InsertAsync(timeSet);
        }
        public static Task<int> UpdateTimeSet(TimeSet timeSet)
        {
            return AsyncConnection.UpdateAsync(timeSet);
        }
        static public Task<int> DeleteTimeSet(TimeSet timeSet)
        {
            return AsyncConnection.DeleteAsync(timeSet);
        }

        static public Task<List<TimeSet>> GetTimeSets()
        {
            return AsyncConnection.Table<TimeSet>().ToListAsync();
        }

        static public Settings GetSettings()
        {
            return Connection.Table<Settings>().FirstOrDefault();
        }
        public static Task<int> AddSettings(Settings settings)
        {
            return AsyncConnection.InsertAsync(settings);
        }
        //static public Task<List<Stage>> GetStageByTimeSet(int timeSet)
        //{
            
        //}
        //static public async Task<List<TimeSetViewModel>> GetTimeSetViewModels()
        //{
        //    var sets = await GetTimeSets();
        //    Task.Run(() =>
        //    {
        //        foreach (var timeSet in sets)
        //        {
                    
        //        }
        //    });
        //}
    }
}
