using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using EmptyProject2025Extended.Models;
using System.Collections.Generic;

namespace EmptyProject2025Extended.Data
{
    public class DBHelper : SQLiteOpenHelper
    {
        private const string DATABASE_NAME = "SQLiteExample.db";
        private const int DATABASE_VERSION = 1;

        //**********************************************************
        // PASSWORD TABLE
        //**********************************************************
        private const string TABLE_RECORD = "PasswordData";

        private const string COLUMN_ID = "_id";
        private const string COLUMN_USERNAME = "username";
        private const string COLUMN_PASSWORD = "password";
        private const string COLUMN_SITE = "site";

        private static readonly string[] allColumns =
        {
            COLUMN_ID, COLUMN_USERNAME, COLUMN_PASSWORD, COLUMN_SITE
        };

        private const string CREATE_TABLE_PWD =
            "CREATE TABLE IF NOT EXISTS " + TABLE_RECORD + " (" +
            COLUMN_ID + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
            COLUMN_USERNAME + " TEXT, " +
            COLUMN_PASSWORD + " TEXT, " +
            COLUMN_SITE + " TEXT)";

        //**********************************************************
        // USERS TABLE
        //**********************************************************
        private const string TABLE_USERS = "Users";

        private const string COLUMN_USER_ID = "id";
        private const string COLUMN_USER_USERNAME = "username";
        private const string COLUMN_USER_PASSWORDHASH = "passwordHash";
        private const string COLUMN_USER_SALT = "salt";

        private const string CREATE_TABLE_USERS =
            "CREATE TABLE IF NOT EXISTS " + TABLE_USERS + " (" +
            COLUMN_USER_ID + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
            COLUMN_USER_USERNAME + " TEXT UNIQUE, " +
            COLUMN_USER_PASSWORDHASH + " TEXT, " +
            COLUMN_USER_SALT + " TEXT)";

        private SQLiteDatabase database;
        private readonly Context context;

        public DBHelper(Context context)
            : base(context, DATABASE_NAME, null, DATABASE_VERSION)
        {
            this.context = context;
        }

        //**********************************************************
        // ON CREATE
        //**********************************************************
        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL(CREATE_TABLE_PWD);
            db.ExecSQL(CREATE_TABLE_USERS);

            // Insert default admin user
            string defaultSalt = "static_salt";
            string defaultHash = "1234" + defaultSalt;

            ContentValues admin = new ContentValues();
            admin.Put(COLUMN_USER_USERNAME, "admin");
            admin.Put(COLUMN_USER_PASSWORDHASH, defaultHash);
            admin.Put(COLUMN_USER_SALT, defaultSalt);

            db.Insert(TABLE_USERS, null, admin);
        }

        //**********************************************************
        // ON UPGRADE
        //**********************************************************
        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            db.ExecSQL("DROP TABLE IF EXISTS " + TABLE_RECORD);
            db.ExecSQL("DROP TABLE IF EXISTS " + TABLE_USERS);
            OnCreate(db);
        }

        //**********************************************************
        // CREATE PASSWORD
        //**********************************************************
        public void Create(PasswordInfo passwordInfo)
        {
            database = WritableDatabase;

            ContentValues values = new ContentValues();
            values.Put(COLUMN_USERNAME, passwordInfo.Username);
            values.Put(COLUMN_PASSWORD, passwordInfo.Password);
            values.Put(COLUMN_SITE, passwordInfo.Site);

            long id = database.Insert(TABLE_RECORD, null, values);
            passwordInfo.Id = id;

            database.Close();
        }

        //**********************************************************
        // UPDATE PASSWORD
        //**********************************************************
        public void Update(PasswordInfo passwordInfo)
        {
            database = WritableDatabase;

            ContentValues values = new ContentValues();
            values.Put(COLUMN_USERNAME, passwordInfo.Username);
            values.Put(COLUMN_PASSWORD, passwordInfo.Password);
            values.Put(COLUMN_SITE, passwordInfo.Site);

            database.Update(
                TABLE_RECORD,
                values,
                COLUMN_ID + " = ?",
                new string[] { passwordInfo.Id.ToString() }
            );

            database.Close();
        }

        //**********************************************************
        // DELETE PASSWORD BY ID
        //**********************************************************
        public void DeleteById(long id)
        {
            database = WritableDatabase;

            database.Delete(
                TABLE_RECORD,
                COLUMN_ID + " = ?",
                new string[] { id.ToString() }
            );

            database.Close();
        }

        //**********************************************************
        // DELETE ALL PASSWORDS
        //**********************************************************
        public int DeleteAll()
        {
            database = WritableDatabase;
            int rows = database.Delete(TABLE_RECORD, null, null);
            database.Close();
            return rows;
        }

        //**********************************************************
        // READ PASSWORD BY FIELD
        //**********************************************************
        public List<PasswordInfo> Read(string column, string[] values)
        {
            database = ReadableDatabase;
            List<PasswordInfo> list = new List<PasswordInfo>();

            ICursor cursor = database.Query(
                TABLE_RECORD,
                allColumns,
                column + " = ?",
                values,
                null,
                null,
                null
            );

            if (cursor.MoveToFirst())
            {
                do
                {
                    long id = cursor.GetLong(cursor.GetColumnIndexOrThrow(COLUMN_ID));
                    string username = cursor.GetString(cursor.GetColumnIndexOrThrow(COLUMN_USERNAME));
                    string password = cursor.GetString(cursor.GetColumnIndexOrThrow(COLUMN_PASSWORD));
                    string site = cursor.GetString(cursor.GetColumnIndexOrThrow(COLUMN_SITE));

                    list.Add(new PasswordInfo(id, username, password, site));

                } while (cursor.MoveToNext());
            }

            cursor.Close();
            database.Close();

            return list;
        }

        //**********************************************************
        // READ ALL PASSWORDS
        //**********************************************************
        public List<PasswordInfo> ReadAll()
        {
            database = ReadableDatabase;
            List<PasswordInfo> list = new List<PasswordInfo>();

            ICursor cursor = database.Query(
                TABLE_RECORD,
                allColumns,
                null,
                null,
                null,
                null,
                COLUMN_ID + " ASC"
            );

            if (cursor.MoveToFirst())
            {
                do
                {
                    long id = cursor.GetLong(cursor.GetColumnIndexOrThrow(COLUMN_ID));
                    string username = cursor.GetString(cursor.GetColumnIndexOrThrow(COLUMN_USERNAME));
                    string password = cursor.GetString(cursor.GetColumnIndexOrThrow(COLUMN_PASSWORD));
                    string site = cursor.GetString(cursor.GetColumnIndexOrThrow(COLUMN_SITE));

                    list.Add(new PasswordInfo(id, username, password, site));

                } while (cursor.MoveToNext());
            }

            cursor.Close();
            database.Close();

            return list;
        }
        //**********************************************************
        // INSERT NEW USER
        //**********************************************************
        public void InsertUser(User user)
        {
            // Open database in write mode to insert new data
            SQLiteDatabase database = WritableDatabase;

            // Prepare a container for the values we want to insert
            ContentValues values = new ContentValues();

            // Put values into the container - these key names MUST match the column names in the database
            values.Put("username", user.Username);
            values.Put("passwordHash", user.PasswordHash);
            values.Put("salt", user.Salt);

            // Insert the values into the Users table
            database.Insert("Users", null, values);

            // Close the database to free resources and avoid memory leaks
            database.Close();
        }

        //**********************************************************
        // GET USER BY USERNAME
        //**********************************************************
        public User GetUser(string username)
        {
            SQLiteDatabase db = ReadableDatabase;

            string[] columns =
            {
                COLUMN_USER_ID,
                COLUMN_USER_USERNAME,
                COLUMN_USER_PASSWORDHASH,
                COLUMN_USER_SALT
            };

            string selection = COLUMN_USER_USERNAME + " = ?";
            string[] selectionArgs = { username };

            ICursor cursor = db.Query(
                TABLE_USERS,
                columns,
                selection,
                selectionArgs,
                null,
                null,
                null
            );

            User user = null;

            if (cursor.MoveToFirst())
            {
                long id = cursor.GetLong(cursor.GetColumnIndexOrThrow(COLUMN_USER_ID));
                string uname = cursor.GetString(cursor.GetColumnIndexOrThrow(COLUMN_USER_USERNAME));
                string hash = cursor.GetString(cursor.GetColumnIndexOrThrow(COLUMN_USER_PASSWORDHASH));
                string salt = cursor.GetString(cursor.GetColumnIndexOrThrow(COLUMN_USER_SALT));

                user = new User(id, uname, hash, salt);
            }

            cursor.Close();
            db.Close();

            return user;
        }
    }
}
