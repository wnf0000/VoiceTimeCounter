using SQLite;

namespace FormsApp.Model
{
    
   public class Settings
    {
        [AutoIncrement]
        [PrimaryKey]
        public int Id { set; get; }

        public bool IsFirstRun { set; get; }
    }
}
