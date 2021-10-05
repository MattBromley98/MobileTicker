using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;


/*
 * This CS file holds the constructor and destructor for instances of a SQLite Database which holds the Stock Data (<Result>)
 */

namespace test
{
    public class stockdatabase
    {
        readonly SQLiteAsyncConnection database;
        public stockdatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Result>().Wait();
        }
        public Task<List<Result>> GetStocksAsync()
        {
            /*
             * Function to return a Table of all Stocks appended in the database.
             */
            return database.Table<Result>().ToListAsync();
        }
        public Task<Result> GetStockAsync(int id)
        {
            /*
            * Function to return an individual stock in the database.
            */
            return database.Table<Result>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }
        public Task<int> SaveStockAsync(Result inStock)
        {
            /*
            * Function which checks if a stock is already in the database and if not add it.
             */
            if (inStock.ID != 0)
            {
                //Updates the Stock
                return database.UpdateAsync(inStock);
            } else
            {
                //Creates a new Stock in the Database
                return database.InsertAsync(inStock);
            }
        }
        public Task<int> DeleteStockAsync(Result inStock)
        {
            /*
            * Function to delete an individual stock in the database.
            */
            return database.DeleteAsync(inStock);
        }
    }
}