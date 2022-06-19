using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Windows.Storage;
using DataAccessLibrary.Classes;

namespace DataAccessLibrary
{
    public static class DataAccess
    {
        public static async Task<List<HistoryDetails>> GetHistoryDetails()
        {
            List<HistoryDetails> historyDetails = new List<HistoryDetails>();

            StorageFolder EBFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("EBWebView");
            StorageFolder DefaultFolder = await EBFolder.GetFolderAsync("Default");
            StorageFile HistoryFile = await DefaultFolder.GetFileAsync("History");

            string DBPath = HistoryFile.Path;

            using (SqliteConnection conn = new SqliteConnection($"Filename={DBPath}"))
            {
                conn.Open();

                SqliteCommand selectHistoryCommand = new SqliteCommand("SELECT url, title FROM urls", conn);

                SqliteDataReader query = selectHistoryCommand.ExecuteReader();

                while (query.Read())
                {
                    HistoryDetails hd = new HistoryDetails();
                    hd.Url = query.GetString(0);
                    hd.Title = query.GetString(1);

                    historyDetails.Add(hd);
                }

                conn.Close();
            }

            return historyDetails;
        }
    }
}
