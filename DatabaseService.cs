
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

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, DbName);

            _connection = new SQLiteAsyncConnection(dbPath);


            _connection.CreateTableAsync<CarbonRecord>();
        }

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


        public async Task<List<CarbonRecord>> GetHistory()
        {
            return await _connection.Table<CarbonRecord>()
                                    .OrderByDescending(x => x.Date)
                                    .ToListAsync();
        }
    }
}
