using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using diary.Entities;
using diary.BLL;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.IO;

namespace diary.DAL
{
   public class UserAccess
    {
       private string AppointmentTitle="";
       private string AppointmentTime="";

       public string getAppointmentTitle() { return AppointmentTitle; }
       public string getAppointmentTime() { return AppointmentTime; }

       public bool checkDate(string Username, string Date)
       {
           string temp=" ";
           BaseDLL da = new BaseDLL();
           SqlCommand cmd = da.GetCommand("SELECT date from [Diary] where date='" + Date +
                "' and username='" + Username + "'");
           cmd.Connection.Open();
           SqlDataReader reader = cmd.ExecuteReader();
           using (reader)
           {
               while (reader.Read())
               {
                   temp = reader.GetString(0);
               }
               reader.Close();
           }
           cmd.Connection.Close();
           if(temp==Date)
           {
               return true;
           }
           else
           {
               return false;
           }
       }

       public string loadData(string Username, string Date, ref byte[] Image)
       {
           string temp = null; 
           BaseDLL da = new BaseDLL();
           SqlCommand cmd = da.GetCommand("SELECT page from [Diary] where (username='"+Username+"' and date='"+Date+"')");
           cmd.Connection.Open();
           SqlDataReader reader = cmd.ExecuteReader();
           using (reader)
           {
               while (reader.Read())
               {
                   temp = reader.GetString(0);
               }
               reader.Close();
           }
           
           cmd = da.GetCommand("SELECT image FROM [Diary] WHERE (username='" + Username + "' and date='" + Date + "')");
           cmd.Connection.Open();
           Image = (byte[])cmd.ExecuteScalar();
           cmd = da.GetCommand("SELECT appointmentTitle FROM [Diary] WHERE (username='" + Username + "' and date='" + Date + "')");
           cmd.Connection.Open();
           reader = cmd.ExecuteReader();
           using (reader)
           {
               while (reader.Read())
               {
                   AppointmentTitle = reader.GetString(0);
               }
               reader.Close();
           }
           cmd = da.GetCommand("SELECT appointmentTime FROM [Diary] WHERE (username='" + Username + "' and date='" + Date + "')");
           cmd.Connection.Open();
           reader = cmd.ExecuteReader();
           using (reader)
           {
               while (reader.Read())
               {
                   AppointmentTime = reader.GetString(0);
               }
               reader.Close();
           }
           cmd.Connection.Close();
           return Decrypt(temp);
       }
       public bool Insert(string Username, string Date, string Page, ref byte[] Image, string AppointmentTitle, string AppointmentTime, string AppointmentMode)
       {
           
               BaseDLL da = new BaseDLL();
               SqlCommand cmd = da.GetCommand("INSERT INTO [Diary] (username,date,page,image,appointmentTitle,appointmentTime,appointmentMode)" +
                    "VALUES (@Username, @Date, @Page, @Pic, @AppointmentTitle, @AppointmentTime,@AppointmentMode)");

               //  SqlParameter p = new SqlParameter("@ID", SqlDbType.Int);
               //  p.Value = obj.ID;

               SqlParameter p = new SqlParameter("@Username", SqlDbType.VarChar, 50);
               p.Value = Username;

               SqlParameter p1 = new SqlParameter("@Date", SqlDbType.VarChar, 50);
               p1.Value = Date;

               SqlParameter p2 = new SqlParameter("@Page", SqlDbType.Text);
               p2.Value = Encrypt(Page);
               try
               {
                   SqlParameter p3 = new SqlParameter("@Pic", SqlDbType.VarBinary, Image.Length, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, Image);
                   p3.Value = Image;

                   cmd.Parameters.Add(p3);
               }
               
               catch(Exception)
               {
                   Image = new byte[10];
                   SqlParameter p3 = new SqlParameter("@Pic", SqlDbType.VarBinary, Image.Length, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, Image);
                   p3.Value = Image;

                   cmd.Parameters.Add(p3);
               }
               SqlParameter p4 = new SqlParameter("@AppointmentTitle", SqlDbType.VarChar, 50);
               p4.Value = AppointmentTitle;

               SqlParameter p5 = new SqlParameter("@AppointmentTime", SqlDbType.VarChar, 50);
               p5.Value = AppointmentTime;

               SqlParameter p6 = new SqlParameter("@AppointmentMode", SqlDbType.VarChar, 50);
               p6.Value = AppointmentTime;


               cmd.Parameters.Add(p);
               cmd.Parameters.Add(p1);
               cmd.Parameters.Add(p2);
               cmd.Parameters.Add(p4);
               cmd.Parameters.Add(p5);
               cmd.Parameters.Add(p6);
               

               cmd.Connection.Open();

               int val = cmd.ExecuteNonQuery();

               cmd.Connection.Close();
               return val > 0;          
           
       }

       public bool Update(string Username, string Date, string Page, byte[] Image, string AppointmentTitle, string AppointmentTime, string AppointmentMode)
       {
           BaseDLL da = new BaseDLL();
           SqlCommand cmd = da.GetCommand("UPDATE [Diary] SET page='" +Encrypt(Page) + "' where (username='" + Username + "' and date='" + Date + "')" );
           cmd.Connection.Open();
           int val = cmd.ExecuteNonQuery();
           cmd.Connection.Close();

           cmd = da.GetCommand("UPDATE [Diary] SET appointmentTitle='" + AppointmentTitle + "' where (username='" + Username + "' and date='" + Date + "')");
           cmd.Connection.Open();
           cmd.ExecuteNonQuery();
           cmd.Connection.Close();

           cmd = da.GetCommand("UPDATE [Diary] SET appointmentTime='" + AppointmentTime + "' where (username='" + Username + "' and date='" + Date + "')");
           cmd.Connection.Open();
           cmd.ExecuteNonQuery();
           cmd.Connection.Close();

           cmd = da.GetCommand("UPDATE [Diary] SET appointmentMode='" + AppointmentMode + "' where (username='" + Username + "' and date='" + Date + "')");
           cmd.Connection.Open();
           cmd.ExecuteNonQuery();
           cmd.Connection.Close();

           cmd = da.GetCommand("UPDATE [Diary] SET image=@pic WHERE (username='" + Username + "' and date='" + Date + "')");
           try
           {
               SqlParameter prm = new SqlParameter("@pic", SqlDbType.VarBinary, Image.Length, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, Image);
               cmd.Parameters.Add(prm);
           }
           catch(Exception)
           {
               Image = new byte[10];
               SqlParameter prm = new SqlParameter("@pic", SqlDbType.VarBinary, Image.Length, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, Image);
               cmd.Parameters.Add(prm);

           }
           cmd.Connection.Open();
           cmd.ExecuteNonQuery();
           cmd.Connection.Close();

           return val > 0;
       }

       public bool checkReminder(string Username, string Date,int hour, int minute, int second,int msecond)
       {
           BaseDLL da = new BaseDLL();
           string time="";
           string mode = "";
           SqlCommand cmd = da.GetCommand("SELECT appointmentTime FROM [Diary] WHERE (username='" + Username + "' and date='" + Date + "')");
           cmd.Connection.Open();
           SqlDataReader reader = cmd.ExecuteReader();
           using (reader)
           {
               while (reader.Read())
               {
                    time = reader.GetString(0);
               }
               reader.Close();
           }
           string[] tokens = time.Split(':');
           int Hour = Convert.ToInt32(tokens[0]);
           int Minute = Convert.ToInt32(tokens[1]);
           cmd.Connection.Close();

           cmd = da.GetCommand("SELECT appointmentMode FROM [Diary] WHERE (username='" + Username + "' and date='" + Date + "')");
           cmd.Connection.Open();
            reader = cmd.ExecuteReader();
           using (reader)
           {
               while (reader.Read())
               {
                   mode = reader.GetString(0);
               }
               reader.Close();
           }
           cmd.Connection.Close();

           //int test=0;
           if(mode=="In Time")
           {
               if (hour - Hour == 0 && minute - Minute == 0 && second == 1)
               {
                   return true;
               }
               else
               {
                   return false;
               }
           }
           else if (mode == "Before 1 Hour")
           {
               if (hour - Hour == -1 && minute - Minute == 0 && second == 1)
               {
                   return true;
               }
               else
               {
                   return false;
               }
           }
           else
           {
               return false;
           }

          
       }

       public string getAppointmentDescription(string Username, string Date)
       {
           BaseDLL da = new BaseDLL();
           string description = "";
           SqlCommand cmd = da.GetCommand("SELECT appointmentTitle FROM [Diary] WHERE (username='" + Username + "' and date='" + Date + "')");
           cmd.Connection.Open();
           SqlDataReader reader = cmd.ExecuteReader();
           using (reader)
           {
               while (reader.Read())
               {
                   description = reader.GetString(0);
               }
               reader.Close();
           }
           cmd.Connection.Close();
           if (description==null)
           {
               description = " ";
           }
           return description;

       }

       public string getAppointmentTime(string Username, string Date)
       {
           BaseDLL da = new BaseDLL();
           string time = "";
           SqlCommand cmd = da.GetCommand("SELECT appointmentTime FROM [Diary] WHERE (username='" + Username + "' and date='" + Date + "')");
           cmd.Connection.Open();
           SqlDataReader reader = cmd.ExecuteReader();
           using (reader)
           {
               while (reader.Read())
               {
                   time = reader.GetString(0);
               }
               reader.Close();
           }
           cmd.Connection.Close();
           if (time == null)
           {
               time = " ";
           }
           return time;

       }

       private string Encrypt(string clearText)
       {
           string EncryptionKey = "MAKV2SPBNI99212";
           byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
           using (Aes encryptor = Aes.Create())
           {
               Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
               encryptor.Key = pdb.GetBytes(32);
               encryptor.IV = pdb.GetBytes(16);
               using (MemoryStream ms = new MemoryStream())
               {
                   using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                   {
                       cs.Write(clearBytes, 0, clearBytes.Length);
                       cs.Close();
                   }
                   clearText = Convert.ToBase64String(ms.ToArray());
               }
           }
           return clearText;
       }
       private string Decrypt(string cipherText)
       {
           string EncryptionKey = "MAKV2SPBNI99212";
           byte[] cipherBytes = Convert.FromBase64String(cipherText);
           using (Aes encryptor = Aes.Create())
           {
               Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
               encryptor.Key = pdb.GetBytes(32);
               encryptor.IV = pdb.GetBytes(16);
               using (MemoryStream ms = new MemoryStream())
               {
                   using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                   {
                       cs.Write(cipherBytes, 0, cipherBytes.Length);
                       cs.Close();
                   }
                   cipherText = Encoding.Unicode.GetString(ms.ToArray());
               }
           }
           return cipherText;
       }
    }
}
