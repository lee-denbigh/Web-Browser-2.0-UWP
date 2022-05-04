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
                    "SearchTerm VARCHAR(2048) NOT NULL," +
                    "DateSearched DATE, " +
                    "TermSearchedAmount INTEGER)";

                SqliteCommand createTable = new SqliteCommand(searchTermsTableCommand, conn);

                createTable.ExecuteReader();
            }
        }

        public static void AddSearchTermToTable(string SearchTerm, DateTime DateSearched, int TermSearchedAmount)
        {
            string dp = Path.Combine(ApplicationData.Current.LocalFolder.Path, dbPath);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dp}"))
            {
                conn.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = conn;

                insertCommand.CommandText = "INSERT INTO searchterms VALUES(NULL, @SearchTerm, @DateSearched, @TermSearchedAmount)";
                insertCommand.Parameters.AddWithValue("@SearchTerm", SearchTerm);
                insertCommand.Parameters.AddWithValue("@DateSearched", DateSearched);
                insertCommand.Parameters.AddWithValue("@TermSearchedAmount", TermSearchedAmount);

                insertCommand.ExecuteReader();

                conn.Close();
            }
        }

        public static List<string> GetAllSearchedTerms()
        {
            List<string> terms = new List<string>();

            string dp = Path.Combine(ApplicationData.Current.LocalFolder.Path, dbPath);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dp}"))
            {
                conn.Open();

                SqliteCommand selectTermsCommand = new SqliteCommand("SELECT SearchTerm FROM searchterms", conn);

                SqliteDataReader reader = selectTermsCommand.ExecuteReader();
                while (reader.Read())
                {
                    terms.Add(reader.GetString(0));
                }

                conn.Close();
            }

            return terms;
        }
    }
}
