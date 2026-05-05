using SQLite;
using GameTracker.Models;

namespace GameTracker.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        // ฟังก์ชันสำหรับตั้งค่าและเปิดฐานข้อมูล
        async Task Init()
        {
            if (_database is not null)
                return;

            // กำหนดที่อยู่ไฟล์ฐานข้อมูลในเครื่องมือถือ
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "GameTracker.db3");
            _database = new SQLiteAsyncConnection(dbPath);

            // สร้างตารางเก็บข้อมูลเกมอิงตาม Model ที่เราสร้างไว้ตอนแรก
            await _database.CreateTableAsync<Game>();
        }

        // --- คำสั่ง ดึงข้อมูล เพิ่มข้อมูล และ ลบข้อมูล ---

        public async Task<List<Game>> GetGamesAsync()
        {
            await Init();
            return await _database.Table<Game>().ToListAsync();
        }

        public async Task<int> SaveGameAsync(Game game)
        {
            await Init();
            // ถ้าเกมนี้มีอยู่ในฐานข้อมูลแล้ว ให้อัปเดต ถ้ายังไม่มี ให้สร้างใหม่
            var existingGame = await _database.Table<Game>().Where(x => x.Id == game.Id).FirstOrDefaultAsync();
            if (existingGame != null)
            {
                return await _database.UpdateAsync(game);
            }
            else
            {
                return await _database.InsertAsync(game);
            }
        }

        public async Task<int> DeleteGameAsync(Game game)
        {
            await Init();
            return await _database.DeleteAsync(game);
        }
    }
}