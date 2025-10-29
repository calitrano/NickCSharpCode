using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
namespace CSharpUpdateSQLServer
{
    class Program   // WHEN DOING PROGRAMMING AT HOME ON YOUR DESKTOP
        // DUDE MAKE SURE SQL SERVER HAS THE NORTHWIND DATABASE INSTALLED OR YOUR FUCT!
    // LOOK IN MY DOC. PROGRAMS sql Server stuff.doc IT WILL TELL YOU HOW TO DOWNLOAD NORTHWIND!

        //DUDE MAKE SURE YOU RUN AS ADMINISTRATOR OR YOU WILL BE GOOGLEING
    // WHAT THE HECK IS The requested operation requires elevation !!! 
        // ITS FLIPPING WINDOWS VISTA AGAIN!!! 
    {
        const String m_strProgramID = "CSharpUpdateSQLServer";

        static void Main(string[] args)
        {
         //   const String m_strProgramID = "Update";

            Program CSharpUpdateSQLServer = new Program();

            CSharpUpdateSQLServer.OpenDatabaseConnection();


        }

        void OpenDatabaseConnection()
        {
            //using (new CStep("OpenDatabaseConnection"))
            //{
            /*
                m_oOdbcConnection.ConnectionString = iniFileRdr.CreateODBCStringForDSNLess(m_strServer, m_strDatabase2, m_strDatabaseUserId, m_strDatabasePassword);
                m_oOdbcConnection.ConnectionTimeout = 0;
                m_oOdbcConnection.Open();
            */




//'     Data Source = NICK-PC\SQL_EXPRESS; Initial Catalog = NorthwindCS; User Id = myUsername; Password = myPassword;
     //Data Source = NICK-PC\SQL_EXPRESS; Initial Catalog = NorthwindCS; Trusted_Connection=Yes;
         //  Data Source= Server=NICK-PC;Database=NorthwindCS;Trusted_Connection=True;
            //string strConnection;

            try
            {
              //  strConnection = _T("Driver={SQL Server};Server=MyServerName;Database=myDatabaseName;Uid=;Pwd=;");

                SqlConnection dataConnection = new SqlConnection();

         //       dataConnection.ConnectionString = "Integrated Security = true; Initial Catalog= NorthwindCS; Data Source =NICK-PC\\SQLEXPRESS;";
                dataConnection.ConnectionString = "Integrated Security = true; Initial Catalog= Northwind; Data Source =BLUE-CAA985E75B\\SQLEXPRESS;";

                dataConnection.Open();
                SqlCommand dataCommand = new SqlCommand();
                dataCommand.Connection = dataConnection;

                dataCommand.CommandText = "SELECT OrderId, CustomerId FROM ORDERS WHERE ORDERID = 10611 ";

                SqlDataReader dataReader = dataCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    int oid = dataReader.GetInt32(0);
                    string cid = dataReader.GetString(1);

                    Console.WriteLine("Order id {0} Order {1}\n",oid,cid );

                
                }

                dataReader.Close();
                dataConnection.Close();

                //  }
            }
            catch(Exception  e)
            {
                Console.WriteLine("An error has occured dummy  -->> " + e.Message);
            
            }

            //for (int i = 0; i < 10000; i++)
            //{
            //    Console.Write("yo nick\n ");
            //}

        }


    }


}
