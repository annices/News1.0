using System;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using MvcNews.Models;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;

namespace MvcNews.DBAccess
{
    /// <summary>
    /// This class handles the interaction between the application and the database.
    /// </summary>
    public class DBConnect
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="iconfig"></param>
        public DBConnect(IConfiguration iconfig)
        {
            _iconfig = iconfig;
        }

        // Config variables:
        IConfiguration _iconfig;
        string db_table_admin = "a_admin";
        string db_table_news = "a_news";
        string db_table_status = "a_status";


        /// <summary>
        /// Method to get the database connection string from appsettings file.
        /// </summary>
        /// <returns>The db connection string.</returns>
        private string dbConnect()
        {
            return _iconfig.GetSection("ConnectionStrings").GetSection("MvcNewsContext").Value;
        }


        /*********************************************************************
        * The following methods concern db queries related to news objects.  *
        *********************************************************************/

        /// <summary>
        /// This method stores a new entry in db.
        /// </summary>
        /// <param name="model">News object.</param>
        public void createEntry(News model)
        {
            string conn = dbConnect();
            SqlConnection dbConn = new SqlConnection(conn);

            // Insert query to be executed to db:
            string sql = "INSERT INTO " + db_table_news + " VALUES(@title, @created, @entry, @status, @useremail);";

            SqlCommand dbCommand = new SqlCommand(sql, dbConn);

            // Use SqlParameters to prevent SQL injections:
            dbCommand.Parameters.Add(new SqlParameter("@title", model.Title));
            dbCommand.Parameters.Add(new SqlParameter("@created", model.EntryDate));
            dbCommand.Parameters.Add(new SqlParameter("@entry", model.Entry));
            dbCommand.Parameters.Add(new SqlParameter("@status", model.Status));
            dbCommand.Parameters.Add(new SqlParameter("@useremail", model.Email));

            dbConn.Open();
            dbCommand.ExecuteNonQuery();
            dbConn.Close();
        }


        /// <summary>
        /// This method updates a news entry in db.
        /// </summary>
        /// <param name="model">News object.</param>
        public void editEntry(News model)
        {
            string conn = dbConnect();
            SqlConnection dbConn = new SqlConnection(conn);

            // Update query:
            string sql = "UPDATE " + db_table_news +
            " SET Title = @title, Created = @created, Entry = @entry, Status = @status, UserEmail = @useremail" +
            " WHERE EntryID = @entryid";

            SqlCommand dbCommand = new SqlCommand(sql, dbConn);

            // Use SqlParameters to prevent SQL injections:
            dbCommand.Parameters.Add(new SqlParameter("@entryid", model.EntryID));
            dbCommand.Parameters.Add(new SqlParameter("@title", model.Title));
            dbCommand.Parameters.Add(new SqlParameter("@created", model.EntryDate));
            dbCommand.Parameters.Add(new SqlParameter("@entry", model.Entry));
            dbCommand.Parameters.Add(new SqlParameter("@status", model.Status));
            dbCommand.Parameters.Add(new SqlParameter("@useremail", model.Email));

            dbConn.Open();
            dbCommand.ExecuteNonQuery();
            dbConn.Close();
        }


        /// <summary>
        /// This method deletes a news entry from db.
        /// </summary>
        /// <param name="id">Entry ID.</param>
        public void deleteEntry(int id)
        {
            string conn = dbConnect();
            SqlConnection dbConn = new SqlConnection(conn);

            // Update query:
            string sql = "DELETE " + db_table_news + " WHERE EntryID = @entryid";

            SqlCommand dbCommand = new SqlCommand(sql, dbConn);

            // Use SqlParameters to prevent SQL injections:
            dbCommand.Parameters.Add(new SqlParameter("@entryid", id));

            dbConn.Open();
            dbCommand.ExecuteNonQuery();
            dbConn.Close();
        }


        /// <summary>
        /// This method gets a specific news entry from db.
        /// </summary>
        /// <param name="id">Entry ID.</param>
        /// <returns>News object.</returns>
        public News getEntry(int id)
        {
            string conn = dbConnect();
            SqlConnection dbConn = new SqlConnection(conn);

            // Query to get an entry based its ID:
            string sql = "SELECT * FROM " + db_table_news + " WHERE EntryID = @entryid";

            SqlCommand dbCommand = new SqlCommand(sql, dbConn);

            // Use SqlParameter to prevent SQL injection:
            dbCommand.Parameters.Add(new SqlParameter("@entryid", id));

            dbConn.Open();
            SqlDataReader reader = dbCommand.ExecuteReader();

            News model = new News();
            // If we have a match, assign values to corresponding news properties:
            if (reader.Read())
            {
                model.EntryID = Convert.ToInt32(reader["EntryID"]);
                model.Title = reader["Title"].ToString();
                model.EntryDate = reader["Created"].ToString();
                model.Entry = reader["Entry"].ToString();
                model.Status = reader["Status"].ToString();
                model.Email = reader["UserEmail"].ToString();
            }

            dbConn.Close();
            return model;
        }


        /// <summary>
        /// This method gets all news entries from db.
        /// </summary>
        /// <returns>A list of all entries.</returns>
        public List<News> getAllEntries()
        {
            string conn = dbConnect();
            SqlConnection dbconn = new SqlConnection(conn);

            // Create a select query to be executed to db table:
            string sql = "SELECT * FROM " + db_table_news;

            SqlCommand dbcommand = new SqlCommand(sql, dbconn);

            dbconn.Open();

            IDataReader reader = dbcommand.ExecuteReader();

            List<News> newslist = new List<News>();

            // Loop to make sure all table rows are read:
            while (reader.Read())
            {
                if (reader != null)
                {
                    newslist.Add(new News()
                    {
                        EntryID = Convert.ToInt32(reader["EntryID"]),
                        Title = reader["Title"].ToString(),
                        EntryDate = reader["Created"].ToString(),
                        Entry = reader["Entry"].ToString(),
                        Status = reader["Status"].ToString(),
                        Email = reader["UserEmail"].ToString()
                    });
                }
            }

            dbconn.Close();
            return newslist;
        }


        /// <summary>
        /// This method gets the news statuses from db.
        /// </summary>
        /// <returns>A list with news statuses.</returns>
        public List<News> getEntryStatuses()
        {
            string conn = dbConnect();
            SqlConnection dbconn = new SqlConnection(conn);

            // Create a select query to be executed to db table:
            string sql = "SELECT * FROM " + db_table_status;

            SqlCommand dbcommand = new SqlCommand(sql, dbconn);

            dbconn.Open();

            IDataReader reader = dbcommand.ExecuteReader();

            List<News> statuslist = new List<News>();
            // Loop to make sure all table rows are read:
            while (reader.Read())
            {
                var StatusID = reader["Status"].ToString();

                statuslist.Add(new News() { Status = StatusID });
            }

            dbconn.Close();
            return statuslist;
        }


        /*********************************************************************
        * The following methods concern db queries related to admin objects. *
        *********************************************************************/

        /// <summary>
        /// This method stores a new admin user in db.
        /// </summary>
        /// <param name="model">Admin object.</param>
        public void createUser(Admin model)
        {
            string conn = dbConnect();
            SqlConnection dbConn = new SqlConnection(conn);

            // Encrypt password with hash and salt:
            PasswordEncryption passwordHasher = new PasswordEncryption();
            // If you change input value below, ensure it also reflects the supported length in the corresponding db table:
            byte[] salt = passwordHasher.generateSalt(64);
            HSPassword PasswordHash = passwordHasher.generateHashWithSalt(model.Password, salt, SHA256.Create());
            model.PasswordHash = PasswordHash.Digest;
            model.Salt = Convert.ToBase64String(salt);

            // Insert query to be executed to db:
            string sql = "INSERT INTO " + db_table_admin + " VALUES(@firstname, @lastname, @useremail, @passwordhash, @salt)";

            SqlCommand dbCommand = new SqlCommand(sql, dbConn);

            // Prevent conflicts with SqlParameters when null is set for optional fields:
            if (model.Lastname == null)
                model.Lastname = "";

            // Use SqlParameters to prevent SQL injections:
            dbCommand.Parameters.Add(new SqlParameter("@firstname", model.Firstname));
            dbCommand.Parameters.Add(new SqlParameter("@lastname", model.Lastname));
            dbCommand.Parameters.Add(new SqlParameter("@useremail", model.Email));
            dbCommand.Parameters.Add(new SqlParameter("@passwordhash", model.PasswordHash));
            dbCommand.Parameters.Add(new SqlParameter("@salt", model.Salt));

            dbConn.Open();
            dbCommand.ExecuteNonQuery();
            dbConn.Close();
        }


        /// <summary>
        /// This method updates the admin credentials in db.
        /// </summary>
        public void editUser(Admin model)
        {
            string conn = dbConnect();
            SqlConnection dbConn = new SqlConnection(conn);

            // Generate a new password hash if the password is requested to be updated:
            if (!String.IsNullOrWhiteSpace(model.Password))
            {
                PasswordEncryption passwordHasher = new PasswordEncryption();
                // If you change input value below, ensure it also reflects the supported length in the corresponding db table:
                byte[] salt = passwordHasher.generateSalt(64);
                HSPassword PasswordHash = passwordHasher.generateHashWithSalt(model.Password, salt, SHA256.Create());
                model.PasswordHash = PasswordHash.Digest;
                model.Salt = Convert.ToBase64String(salt);
            }

            // Update query with check on password update, else current password is kept:
            string sql = "UPDATE " + db_table_admin +
            " SET Firstname = @firstname, Lastname = @lastname, Email = @useremail, " +
            "PasswordHash = CASE WHEN PasswordHash <> @passwordhash THEN @passwordhash ELSE PasswordHash END, " +
            "Salt = CASE WHEN Salt <> @salt THEN @salt ELSE Salt END " +
            "WHERE Email = @useremail;";

            SqlCommand dbCommand = new SqlCommand(sql, dbConn);

            // Prevent conflicts with SqlParameters when null is set for optional fields:
            if (model.Lastname == null)
                model.Lastname = "";

            // Use SqlParameters to prevent SQL injections:
            dbCommand.Parameters.Add(new SqlParameter("@firstname", model.Firstname));
            dbCommand.Parameters.Add(new SqlParameter("@lastname", model.Lastname));
            dbCommand.Parameters.Add(new SqlParameter("@useremail", model.Email));
            dbCommand.Parameters.Add(new SqlParameter("@passwordhash", model.PasswordHash));
            dbCommand.Parameters.Add(new SqlParameter("@salt", model.Salt));

            dbConn.Open();
            dbCommand.ExecuteNonQuery();
            dbConn.Close();
        }


        /// <summary>
        /// This method gets the credentials from db for an admin user.
        /// </summary>
        /// <param name="useremail">User email to get the specific user.</param>
        /// <returns>Admin object.</returns>
        public Admin getUser(string useremail)
        {
            string conn = dbConnect();
            SqlConnection dbConn = new SqlConnection(conn);

            // Query to get a user based on an email:
            string sql = "SELECT * FROM " + db_table_admin + " WHERE Email = @useremail";

            SqlCommand dbCommand = new SqlCommand(sql, dbConn);

            // Use SqlParameter to prevent SQL injection:
            dbCommand.Parameters.Add(new SqlParameter("@useremail", useremail));

            dbConn.Open();
            SqlDataReader reader = dbCommand.ExecuteReader();

            Admin model = new Admin();
            if (reader.Read())
            {
                // Assign values to corresponding admin properties:
                model.Firstname = reader["Firstname"].ToString();
                model.Lastname = reader["Lastname"].ToString();
                model.Email = reader["Email"].ToString();
                model.PasswordHash = reader["PasswordHash"].ToString();
                model.Salt = reader["Salt"].ToString();
            }

            dbConn.Close();
            return model;
        }


        /// <summary>
        /// This method gets a user's current password hash and salt to be attached to a user update request.
        /// The purpose of this method is to prevent the hash from being exposed in the view layer by always
        /// handling the hash and salt transportation purely in the back-end.
        /// </summary>
        /// <param name="useremail">Email to get the specific user.</param>
        /// <returns>Admin object.</returns>
        public Admin getPassword(string useremail)
        {
            string conn = dbConnect();
            SqlConnection dbConn = new SqlConnection(conn);

            // Query to get the necessary user data:
            string sql = "SELECT PasswordHash, Salt FROM " + db_table_admin + " WHERE Email = @useremail";

            SqlCommand dbCommand = new SqlCommand(sql, dbConn);

            // Use SqlParameter to prevent SQL injection:
            dbCommand.Parameters.Add(new SqlParameter("@useremail", useremail));

            dbConn.Open();
            SqlDataReader reader = dbCommand.ExecuteReader();

            Admin model = new Admin();
            if (reader.Read())
            {
                // Assign values to corresponding admin properties:
                model.PasswordHash = reader["PasswordHash"].ToString();
                model.Salt = reader["Salt"].ToString();
            }

            dbConn.Close();
            return model;
        }


        /// <summary>
        /// This method validates a user login based on a password match between the user input and the existing credentials in db.
        /// </summary>
        /// <param name="email">User name.</param>
        /// <param name="password">User password.</param>
        /// <returns>Valid or invalid login.</returns>
        public Boolean checkLogin(string email, string password)
        {
            string conn = dbConnect();
            SqlConnection dbConn = new SqlConnection(conn);

            // Get hash and salt password from db for a specific user:
            string sql = "SELECT PasswordHash, Salt FROM " + db_table_admin + " WHERE email = @useremail;";

            SqlCommand dbCommand = new SqlCommand(sql, dbConn);

            // Use SqlParameter to prevent SQL injection:
            dbCommand.Parameters.Add(new SqlParameter("@useremail", email));

            dbConn.Open();
            SqlDataReader reader = dbCommand.ExecuteReader();

            // Loop to make sure all table rows are read:
            while (reader.Read())
            {
                if (new HSPassword().confirmPassword(password, reader["Salt"].ToString(), reader["PasswordHash"].ToString()))
                {
                    return true;
                }
                break;
            }

            dbConn.Close();
            return false;
        }


    } // End class.

} // End namespace.