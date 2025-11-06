using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using EmptyProject2025Extended.Models;

namespace EmptyProject2025Extended.Data
{
    internal class DBHelper : SQLiteOpenHelper
    {
        // DBHelper is a class to handle SQLite:
        // (1) Creates an SQLite file on phone storage.
        // (2) Insert new listing in SQLite.
        // (3) Delete a listing by Id from SQLite.
        // (4) Update a listing in sQLite by Id.
        // (5) Return all rows from SQLite.
        // (6) Return a specific row from SQLite. Search by any criteria.
        private const string DATABASE_NAME = "SQLiteExample.db";
        private const int DATABASE_VERSION = 1;
        private const string TABLE_RECORD = "PasswordData";

        private const string COLUMN_ID = "_id";
        private const string COLUMN_USERNAME = "username";
        private const string COLUMN_PASSWORD = "password";
        private const string COLUMN_SITE = "site";

        private static readonly string[] allColumns = { COLUMN_ID, COLUMN_USERNAME, COLUMN_PASSWORD, COLUMN_SITE };

        private const string CREATE_TABLE_USER = "CREATE TABLE IF NOT EXISTS " + TABLE_RECORD + "(" +
            COLUMN_ID + " INTEGER PRIMARY KEY AUTOINCREMENT," +
            COLUMN_USERNAME + " TEXT ," +
            COLUMN_PASSWORD + " TEXT," +
            COLUMN_SITE + " TEXT)";

        private DBHelperService dbHelperService;
        private SQLiteDatabase database; // access to SQLite
        private Context context;

        // DBHelper constructor
        public DBHelper(Context context) : base(context, DATABASE_NAME, null, DATABASE_VERSION)
        {
            this.context = context;
            dbHelperService = new DBHelperService();
        }

        public override void OnCreate(SQLiteDatabase db)
        {

            db.ExecSQL(CREATE_TABLE_USER);
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            // this database is only a cache for online data, so its upgrade policy is
            // to simply to discard the data and start over
            db.ExecSQL("DROP TABLE IF EXISTS " + TABLE_RECORD);
            OnCreate(db);
        }
        //*****************************************************************************************************************************************************
        /// <param name="score"></param>
        public void Create(Score score)
        {
            // insert a new row in database.
            database = WritableDatabase; // Get access to write to database

            // Create a new map of values, where column names are the keys
            ContentValues values = new ContentValues();

            values.Put(COLUMN_USERNAME, PasswordManaging.GetUserName());
            values.Put(COLUMN_PASSWORD, PasswordManaging.GetPassword());
            values.Put(COLUMN_SITE, PasswordManaging.GetSite());
            // insert a new row, returning the primary key value of the new row
            long id = database.Insert(TABLE_RECORD, null, values);
            PasswordManaging.SetId(id);
            values.Put(COLUMN_ID, PasswordManaging.GetId());
            database.Close(); // Close the database
        }



        public void Update(PasswordManaging passwordManaging)
        {
            // update a listing in SQLite.
            // receives an object.
            database = ReadableDatabase; // Get access to read the database
            string[] lookUpValue = { passwordManaging.GetId().ToString() };
            ContentValues values = new ContentValues();
            values.Put(COLUMN_ID, passwordManaging.GetId());
            values.Put(COLUMN_USERNAME, passwordManaging.GetUserUserName());
            values.Put(COLUMN_PASSWORD, passwordManaging.GetPassword());
            values.Put(COLUMN_SITE, passwordManaging.GetSite());
            database.Update(TABLE_RECORD, values, COLUMN_ID + " = ? ", lookUpValue);
            database.Close(); // Close the database
        }



        public List<Score> Read(string column, string[] values)
        {
            // return a specific row in table as an array list.
            // select by any criteria.
            database = ReadableDatabase; // Get access to read the database
            List<Score> PasswordManagingList = new List<PasswordManaging>();

            ICursor cursor = database.Query(TABLE_RECORD, allColumns, column + " = ? ", values, null, null, null); // cursor points at a certain row
            if (cursor.Count > 0)
            {
                while (cursor.MoveToNext())
                {
                    long id = cursor.GetLong(cursor.GetColumnIndex(COLUMN_ID));
                    string username = cursor.GetBlob(cursor.GetColumnIndex(COLUMN_USERNAME));
                    string password = cursor.GetString(cursor.GetColumnIndex(COLUMN_PASSWORD));
                    string site = cursor.GetString(cursor.GetColumnIndex(COLUMN_SITE));
                    Score temp = new Score(id, dbHelperService.ByteToImage(image), name, level, score);
                    scoreList.Add(temp);
                }
            }
            cursor.Close();
            database.Close(); // close the database
            return PasswordManagingList;
        }
        public List<PasswordManaging> ReadAll()
        {
            // return all rows in table as an array list.
            database = ReadableDatabase; // Get access to read the database
            List<PasswordManaging> PasswordManagingList = new List<PasswordManaging>();

            ICursor cursor = database.Query(TABLE_RECORD, allColumns, null, null, null, null, COLUMN_ID + " ASC"); // cursor points at a certain row
            if (cursor.Count > 0)
            {
                while (cursor.MoveToNext())
                {
                    long id = cursor.GetLong(cursor.GetColumnIndex(COLUMN_ID));
                    string username = cursor.GetBlob(cursor.GetColumnIndex(COLUMN_USERNAME));
                    string password = cursor.GetString(cursor.GetColumnIndex(COLUMN_PASSWORD));
                    string site = cursor.GetString(cursor.GetColumnIndex(COLUMN_SITE));
                    Score temp = new Score(id, dbHelperService.ByteToImage(image), name, level, score);
                    scoreList.Add(temp);
                }
            }
            cursor.Close();
            database.Close(); // close the database
            return PasswordManagingList;
        }
        public void DeleteById(long id)
        {
            // delete a row from database.
            // receive an id as long.
            database = ReadableDatabase; // Get access to read the database
            database.Delete(TABLE_RECORD, COLUMN_ID + " = " + id, null);
            database.Close(); // Close the database
        }
        public int DeleteAll()
        {
            // delete all rows from database.
            // return the number of rows deleted.
            database = ReadableDatabase; // Get access to read the database
            int rows = database.Delete(TABLE_RECORD, null, null);
            database.Close(); // Close the database
            return rows;
        }

    }
}