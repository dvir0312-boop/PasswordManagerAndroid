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

        // ================= PASSWORD TABLE =================
        private const string TABLE_RECORD = "PasswordData";

        private const string COLUMN_ID = "_id";
        private const string COLUMN_USERNAME = "username"; // site username
        private const string COLUMN_PASSWORD = "password";
        private const string COLUMN_SITE = "site";
        private const string COLUMN_OWNER = "owner";       // logged-in user

        private static readonly string[] allColumns =
        {
            COLUMN_ID, COLUMN_USERNAME, COLUMN_PASSWORD, COLUMN_SITE, COLUMN_OWNER
        };

        private const string CREATE_TABLE_PWD =
            "CREATE TABLE IF NOT EXISTS " + TABLE_RECORD + " (" +
            COLUMN_ID + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
            COLUMN_USERNAME + " TEXT, " +
            COLUMN_PASSWORD + " TEXT, " +
            COLUMN_SITE + " TEXT, " +
            COLUMN_OWNER + " TEXT)";

        // ================= USERS TABLE =================
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

        public DBHelper(Context context)
            : base(context, DATABASE_NAME, null, DATABASE_VERSION)
        {
        }

        // ================= ON CREATE =================
        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL(CREATE_TABLE_PWD);
            db.ExecSQL(CREATE_TABLE_USERS);
        }

        // ================= ON UPGRADE =================
        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            db.ExecSQL("DROP TABLE IF EXISTS " + TABLE_RECORD);
            db.ExecSQL("DROP TABLE IF EXISTS " + TABLE_USERS);
            OnCreate(db);
        }

        // ================= PASSWORD CRUD =================
        public void Create(PasswordInfo info)
        {
            var db = WritableDatabase;

            ContentValues values = new ContentValues();
            values.Put(COLUMN_USERNAME, info.Username);
            values.Put(COLUMN_PASSWORD, info.Password);
            values.Put(COLUMN_SITE, info.Site);
            values.Put(COLUMN_OWNER, info.Owner);

            info.Id = db.Insert(TABLE_RECORD, null, values);
            db.Close();
        }

        public void Update(PasswordInfo info)
        {
            var db = WritableDatabase;

            ContentValues values = new ContentValues();
            values.Put(COLUMN_USERNAME, info.Username);
            values.Put(COLUMN_PASSWORD, info.Password);
            values.Put(COLUMN_SITE, info.Site);
            values.Put(COLUMN_OWNER, info.Owner);

            db.Update(TABLE_RECORD, values, COLUMN_ID + " = ?",
                new string[] { info.Id.ToString() });

            db.Close();
        }

        public void DeleteById(long id)
        {
            var db = WritableDatabase;
            db.Delete(TABLE_RECORD, COLUMN_ID + " = ?", new[] { id.ToString() });
            db.Close();
        }

        public List<PasswordInfo> ReadAll(string owner)
        {
            var list = new List<PasswordInfo>();
            var db = ReadableDatabase;

            ICursor cursor = db.Query(
                TABLE_RECORD,
                allColumns,
                COLUMN_OWNER + " = ?",
                new[] { owner },
                null, null,
                COLUMN_ID + " ASC"
            );

            if (cursor.MoveToFirst())
            {
                do
                {
                    list.Add(new PasswordInfo(
                        cursor.GetLong(0),
                        cursor.GetString(1),
                        cursor.GetString(2),
                        cursor.GetString(3),
                        cursor.GetString(4)
                    ));
                } while (cursor.MoveToNext());
            }

            cursor.Close();
            db.Close();
            return list;
        }

        // ================= USERS =================
        public void InsertUser(User user)
        {
            var db = WritableDatabase;

            ContentValues values = new ContentValues();
            values.Put(COLUMN_USER_USERNAME, user.Username);
            values.Put(COLUMN_USER_PASSWORDHASH, user.PasswordHash);
            values.Put(COLUMN_USER_SALT, user.Salt);

            db.Insert(TABLE_USERS, null, values);
            db.Close();
        }

        public User GetUser(string username)
        {
            var db = ReadableDatabase;

            ICursor cursor = db.Query(
                TABLE_USERS,
                null,
                COLUMN_USER_USERNAME + " = ?",
                new[] { username },
                null, null, null
            );

            User user = null;

            if (cursor.MoveToFirst())
            {
                user = new User(
                    cursor.GetLong(cursor.GetColumnIndexOrThrow(COLUMN_USER_ID)),
                    cursor.GetString(cursor.GetColumnIndexOrThrow(COLUMN_USER_USERNAME)),
                    cursor.GetString(cursor.GetColumnIndexOrThrow(COLUMN_USER_PASSWORDHASH)),
                    cursor.GetString(cursor.GetColumnIndexOrThrow(COLUMN_USER_SALT))
                );
            }

            cursor.Close();
            db.Close();
            return user;
        }
    }
}
