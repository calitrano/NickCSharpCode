using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace ORACLE_EXTRACT_DESKTOP_AUG_21
{
    class Program
    {
        static void Main(string[] args)
        {
            //            C# / .NET
            //• // Connection string format: User Id=[username];Password=[password];Data Source=[hostname]:[port]/[DB service name];
            //• OracleConnection con = new OracleConnection("User Id=system;Password=GetStartedWithXE;Data Source=localhost:1521/XEPDB1;");
            //• con.Open();
            //• OracleCommand cmd = con.CreateCommand();
            //• cmd.CommandText = "SELECT \'Hello World!\' FROM dual";

            //• OracleDataReader reader = cmd.ExecuteReader();
            //• reader.Read();
            //            Console.WriteLine(reader.GetString(0));

            try
            {
                OraTest ot = new OraTest();
                ot.Connect();

                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = "select * from EVENTS";
                cmd.Connection = ot.con;
             //    ot.con.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    Console.WriteLine("hey nick WE HAVE ROWS ! ");
                    while (dr.Read())
                    {
                                         

                        Console.WriteLine(dr["type"].ToString() + " " + dr["event_date"].ToString() + " " + dr["description"].ToString());
                        //Console.Write("<td>" + dr["event_date"].ToString() + "</td>");
                        // Console.Write("</tr>");
                    }
                    Console.Write("closing ");
                    ot.Close();
                    }
                else
                    {
                        Console.Write("No Data In DataBase");
              
                    }
        

        

            }
            catch (Exception ex)
            {
                Console.WriteLine(" Blowing up in Main " + ex.ToString());
            }
            Console.WriteLine(" the program works !!! ");




        }
        class OraTest
        {

            public OracleConnection con;
            public void Connect()
            {
                try
                {
                    con = new OracleConnection();
                    //        con.ConnectionString = "User Id=IIQReference;Password=IIQ#ref123;Data Source=IIQD;Integrated Security=no";
                    con.ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XEPDB1))); User Id = system; Password = nick1234; ";
                    con.Open();
                    Console.WriteLine("Yo Man !!!! Connected to Oracle  ---> " + con.ServerVersion);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" Cannot Connect Nick " + ex.ToString());


                }

            }

            public void Close()
            {
                con.Close();
                con.Dispose();
            }


        }

    }
}
