using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using StreamingMusic.Interfaces;
using StreamingMusic.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(StreamingMusic.Droid.Services.DatabaseService))]
namespace StreamingMusic.Droid.Services
{
    public class DatabaseService : IDatabaseService
    {
        SQLiteAsyncConnection db;
        async Task Init()
        {
            if (db != null)
                return;

            // Get an absolute path to the database file
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<MyData>();
        }

        public async Task Add(string name)
       {
            await Init();
            var MyData = new MyData
            {
                Name = name,
            };

            var id = await db.InsertAsync(MyData);
        }

        public async Task Remove(int id)
        {

            await Init();

            await db.DeleteAsync<MyData>(id);
        }

        public async Task<IEnumerable<MyData>> GetData()
        {
            await Init();

            var mydata = await db.Table<MyData>().ToListAsync();
            return mydata.ToList();
        }

        public async Task<MyData> GetData(int id)
        {
            await Init();

            var coffee = await db.Table<MyData>()
                .FirstOrDefaultAsync(c => c.Id == id);

            return coffee;
        }

    }
}