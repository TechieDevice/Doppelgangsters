//using System;
//using Microsoft.Data.Sqlite;

//namespace Doppelgangsters.Server
//{
//    public class DB
//    {
//        private SQLiteConnection DBConn;

//        public void Connect()
//        {
//            DBConn = new SQLiteConnection(@"Data Sourse=D:\KeyNet\KeyNetServer\KeyNetServer\db\DB.db; Version=3");
//            DBConn.Open();
//        }

//        public void Close()
//        {
//            DBConn.Close();
//        }

//        public void Add(string Name, string Key, string Mark)
//        {
//            SQLiteCommand CMD = DBConn.CreateCommand();
//            CMD.CommandText = "insert into signatures(name, key, mark) values('"+Name+"','"+Key+"','"+Mark+"')";
//        }

//        public string[] Search(string param, string value)
//        {
//            string[] data = new string[3];
//            SQLiteCommand CMD = DBConn.CreateCommand();
//            CMD.CommandText = "select * from signatures where "+param+" like "+value;
//            SQLiteDataReader reader = CMD.ExecuteReader();
//            if (reader.HasRows)
//            {
//                reader.Read();
//                string name = reader["name"].ToString();
//                string Key = reader["key"].ToString();
//                string Mark = reader["mark"].ToString();
//            }
//            return data;
//        }
//    }
//}
