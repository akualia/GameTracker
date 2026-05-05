using SQLite;
using GameTracker.Models;

namespace GameTracker.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        // Initializes and opens the database connection
        async Task Init()
        {
            if (_database is not null)
                return;

            // Define the local database file path on the device
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "GameTracker.db3");
            _database = new SQLiteAsyncConnection(dbPath);

            // --- CRUD operations (Create, Read, Update, Delete) ---
            await _database.CreateTableAsync<Game>();
        }

        // Retrieve all saved games

        public async Task<List<Game>> GetGamesAsync()
        {
            await Init();
            return await _database.Table<Game>().ToListAsync();
        }

        // Insert a new game or update it if it already exists
        public async Task<int> SaveGameAsync(Game game)
        {
            await Init();
            // Check if the game already exists in the database
            var existingGame = await _database.Table<Game>().Where(x => x.Id == game.Id).FirstOrDefaultAsync();
            if (existingGame != null)
            {
                // Update existing record
                return await _database.UpdateAsync(game);
            }
            else
            {
                // Insert new record
                return await _database.InsertAsync(game);
            }
        }

        // Delete a game from the database
        public async Task<int> DeleteGameAsync(Game game)
        {
            await Init();
            return await _database.DeleteAsync(game);
        }
    }
}