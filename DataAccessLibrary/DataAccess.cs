using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace DataAccessLibrary
{
    public static class DataAccess
    {
        static string dbPath = "webbrowser2.db";

        // This will create the database and tables.
        public static async void InitialiseDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync(dbPath, CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, dbPath);

            using (SqliteConnection conn =
                new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                String searchTermsTableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS searchterms (searchtermID INTEGER PRIMARY KEY," +
                    "searchterm VARCHAR(2048) NOT NULL)";

                SqliteCommand createTable = new SqliteCommand(searchTermsTableCommand, conn);

                createTable.ExecuteReader();
            }
        }
    }
}
