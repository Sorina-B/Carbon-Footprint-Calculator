
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proiect2
{
    public class DatabaseService
    {
        private const string DbName = "CarbonHistory_v1.db3";
        private readonly SQLiteAsyncConnection _connection;

        public DatabaseService()
        {
            // This gets the valid folder path for Android/iOS/Windows
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, DbName);

            _connection = new SQLiteAsyncConnection(dbPath);

            // This creates the table if it doesn't exist yet
            _connection.CreateTableAsync<CarbonRecord>();
        }

        // 2. Method to Add a Record
        public async Task AddRecord(double tons)
        {
            var record = new CarbonRecord
            {
                Date = DateTime.Now,
                TotalTons = tons,
                Rating = tons < 2.0 ? "Sustainable" : "High"
            };

            await _connection.InsertAsync(record);
        }

        // 3. Method to Get All Records (for history view)
        public async Task<List<CarbonRecord>> GetHistory()
        {
            return await _connection.Table<CarbonRecord>()
                                    .OrderByDescending(x => x.Date)
                                    .ToListAsync();
        }
    }
}
