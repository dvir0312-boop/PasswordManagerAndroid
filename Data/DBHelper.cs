using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using EmptyProject2025Extended.Models;
using EmptyProject2025Extended.Security;
using System.Collections.Generic;

namespace EmptyProject2025Extended.Data
{
    public class DBHelper : SQLiteOpenHelper
    {
        private const string DATABASE_NAME = "SQLiteExample.db";
        private const int DATABASE_VERSION = 3;

        // NOTE: Device-based key used for encrypting stored passwords (PasswordData table)
        private readonly string deviceKey = EncryptionUtils.GetDeviceId();

        // ================= PASSWORD TABLE =================
        private const string TABLE_PASSWORDS = "PasswordData";

        private const string COL_P_ID = "_id";
        private const string COL_P_USERNAME = "username";
        private const string COL_P_PASSWORD = "password";
        private const string COL_P_SITE = "site";
        private const string COL_P_OWNER = "owner";

        private const string CREATE_PASSWORD_TABLE =
            "CREATE TABLE IF NOT EXISTS " + TABLE_PASSWORDS + " (" +
            COL_P_ID + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
            COL_P_USERNAME + " TEXT, " +
            COL_P_PASSWORD + " TEXT, " +
            COL_P_SITE + " TEXT, " +
            COL_P_OWNER + " TEXT)";

        // ================= USERS TABLE =================
        private const string TABLE_USERS = "Users";

        private const string COL_U_ID = "id";
        private const string COL_U_USERNAME = "username";
        private const string COL_U_PASSWORD_HASH = "passwordHash";
        private const string COL_U_PASSWORD_SALT = "passwordSalt";
        private const string COL_U_SECURITY_QUESTION = "securityQuestion";
        private const string COL_U_SECURITY_ANSWER_HASH = "securityAnswerHash";
        private const string COL_U_SECURITY_ANSWER_SALT = "securityAnswerSalt";
        private const string COL_U_RECOVERY_WORDS = "recoveryWords"; // recovery phrase (10 words)

        private const string CREATE_USERS_TABLE =
            "CREATE TABLE IF NOT EXISTS " + TABLE_USERS + " (" +
            COL_U_ID + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
            COL_U_USERNAME + " TEXT UNIQUE, " +
            COL_U_PASSWORD_HASH + " TEXT, " +
            COL_U_PASSWORD_SALT + " TEXT, " +
            COL_U_SECURITY_QUESTION + " TEXT, " +
            COL_U_SECURITY_ANSWER_HASH + " TEXT, " +
            COL_U_SECURITY_ANSWER_SALT + " TEXT, " +
            COL_U_RECOVERY_WORDS + " TEXT)";

        public DBHelper(Context context)
            : base(context, DATABASE_NAME, null, DATABASE_VERSION)
        {
        }

        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL(CREATE_PASSWORD_TABLE);
            db.ExecSQL(CREATE_USERS_TABLE);
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            db.ExecSQL("DROP TABLE IF EXISTS " + TABLE_PASSWORDS);
            db.ExecSQL("DROP TABLE IF EXISTS " + TABLE_USERS);
            OnCreate(db);
        }

        // ================= PASSWORD CRUD =================

        public void Create(PasswordInfo info)
        {
            var db = WritableDatabase;

            ContentValues values = new ContentValues();
            values.Put(COL_P_USERNAME, info.Username);

            // Encrypt before saving
            values.Put(COL_P_PASSWORD, EncryptionUtils.Encrypt(info.Password, deviceKey));

            values.Put(COL_P_SITE, info.Site);
            values.Put(COL_P_OWNER, info.Owner);

            info.Id = db.Insert(TABLE_PASSWORDS, null, values);
            db.Close();
        }

        public void Update(PasswordInfo info)
        {
            var db = WritableDatabase;

            ContentValues values = new ContentValues();
            values.Put(COL_P_USERNAME, info.Username);

            // Encrypt before saving
            values.Put(COL_P_PASSWORD, EncryptionUtils.Encrypt(info.Password, deviceKey));

            values.Put(COL_P_SITE, info.Site);
            values.Put(COL_P_OWNER, info.Owner);

            db.Update(
                TABLE_PASSWORDS,
                values,
                COL_P_ID + " = ?",
                new[] { info.Id.ToString() }
            );

            db.Close();
        }

        public void DeleteById(long id)
        {
            var db = WritableDatabase;
            db.Delete(TABLE_PASSWORDS, COL_P_ID + " = ?", new[] { id.ToString() });
            db.Close();
        }

        public List<PasswordInfo> ReadAll(string owner)
        {
            var list = new List<PasswordInfo>();
            var db = ReadableDatabase;

            ICursor cursor = db.Query(
                TABLE_PASSWORDS,
                null,
                COL_P_OWNER + " = ?",
                new[] { owner },
                null, null,
                COL_P_ID + " ASC"
            );

            if (cursor.MoveToFirst())
            {
                do
                {
                    string encrypted = cursor.GetString(cursor.GetColumnIndexOrThrow(COL_P_PASSWORD));
                    string decrypted = EncryptionUtils.Decrypt(encrypted, deviceKey);

                    list.Add(new PasswordInfo(
                        cursor.GetLong(cursor.GetColumnIndexOrThrow(COL_P_ID)),
                        cursor.GetString(cursor.GetColumnIndexOrThrow(COL_P_USERNAME)),
                        decrypted,
                        cursor.GetString(cursor.GetColumnIndexOrThrow(COL_P_SITE)),
                        cursor.GetString(cursor.GetColumnIndexOrThrow(COL_P_OWNER))
                    ));
                }
                while (cursor.MoveToNext());
            }

            cursor.Close();
            db.Close();
            return list;
        }

        // ================= USERS =================

        public void InsertUser(User user, string recoveryWords)
        {
            var db = WritableDatabase;

            ContentValues values = new ContentValues();
            values.Put(COL_U_USERNAME, user.Username);
            values.Put(COL_U_PASSWORD_HASH, user.PasswordHash);
            values.Put(COL_U_PASSWORD_SALT, user.PasswordSalt);
            values.Put(COL_U_SECURITY_QUESTION, user.SecurityQuestion);
            values.Put(COL_U_SECURITY_ANSWER_HASH, user.SecurityAnswerHash);
            values.Put(COL_U_SECURITY_ANSWER_SALT, user.SecurityAnswerSalt);

            // Save recovery words as plain text for MVP (you can hash later)
            values.Put(COL_U_RECOVERY_WORDS, recoveryWords);

            db.Insert(TABLE_USERS, null, values);
            db.Close();
        }

        public User GetUser(string username)
        {
            var db = ReadableDatabase;

            ICursor cursor = db.Query(
                TABLE_USERS,
                null,
                COL_U_USERNAME + " = ?",
                new[] { username },
                null, null, null
            );

            User user = null;

            if (cursor.MoveToFirst())
            {
                user = new User(
                    cursor.GetLong(cursor.GetColumnIndexOrThrow(COL_U_ID)),
                    cursor.GetString(cursor.GetColumnIndexOrThrow(COL_U_USERNAME)),
                    cursor.GetString(cursor.GetColumnIndexOrThrow(COL_U_PASSWORD_HASH)),
                    cursor.GetString(cursor.GetColumnIndexOrThrow(COL_U_PASSWORD_SALT)),
                    cursor.GetString(cursor.GetColumnIndexOrThrow(COL_U_SECURITY_QUESTION)),
                    cursor.GetString(cursor.GetColumnIndexOrThrow(COL_U_SECURITY_ANSWER_HASH)),
                    cursor.GetString(cursor.GetColumnIndexOrThrow(COL_U_SECURITY_ANSWER_SALT))
                );
            }

            cursor.Close();
            db.Close();
            return user;
        }

        public string GetRecoveryWords(string username)
        {
            var db = ReadableDatabase;

            ICursor cursor = db.Query(
                TABLE_USERS,
                new[] { COL_U_RECOVERY_WORDS },
                COL_U_USERNAME + " = ?",
                new[] { username },
                null, null, null
            );

            string words = null;

            if (cursor.MoveToFirst())
            {
                words = cursor.GetString(0);
            }

            cursor.Close();
            db.Close();
            return words;
        }

        public void UpdateUserPassword(string username, string newHash, string newSalt)
        {
            var db = WritableDatabase;

            ContentValues values = new ContentValues();
            values.Put(COL_U_PASSWORD_HASH, newHash);
            values.Put(COL_U_PASSWORD_SALT, newSalt);

            db.Update(
                TABLE_USERS,
                values,
                COL_U_USERNAME + " = ?",
                new[] { username }
            );

            db.Close();
        }
    }
}
