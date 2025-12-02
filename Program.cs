using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.IO;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text.RegularExpressions;

using Logging;
using IniReader;
/// Nick DeNora version 11/18/2007
/// This is a Extract of claims.. in an insurance type application.
/// 
namespace imk00003
{
    class Program
    {
        const String m_strProgramID = "IMK00003";

        string m_strServer;
        string m_strDatabase;
        string m_strDatabase2;
        string m_strDatabaseUserId;
        string m_strDatabasePassword;

        string m_strLogFile;
        string m_strOutputFile;
        string m_strInputFile;
        string m_strBeginDate;
        string m_strEndDate;
        string iniFilePath;
        FileStream fs_OutputFile;
        FileStream fs_InputFile;
        StreamWriter m_oCustomerClaimsOutputFile;
        StreamReader m_oCustomerClaimsInputFile;
        FileStream fs_LogFile;
        StreamWriter sw_LogFile;

        IniFileReader iniFileRdr = null;
        OdbcConnection m_oOdbcConnection = new OdbcConnection();
        OdbcConnection m_oOdbcConnection2 = new OdbcConnection();
        Int32 mi_ssn1 = 0;
        Int32 mi_ssn2 = 0;
        Int32 mi_oldSSNfound = 0;
        Int32 mi_oldSSNNotfound = 0;
        Int32 mi_medClaimsReadIn = 0;
        Int32 mi_firstNameFound = 0;



        enum WriteOptions { Con, Log, Both };

        const string CARRIER_IND = "BC";
        const string CLAIM_TYPE = "R";
        const string Customer = "00402650";

        static void Main(string[] args)
        {
            int lnRetCode = 0;
            Program IMK00003 = new Program();

            using (new CStep("Execute"))
            {
                try
                {
                    IMK00003.GetEnvironmentData();
                    IMK00003.OpenOutputFiles();
                    IMK00003.WriteHeaderRecord();
                    IMK00003.OpenInputFile();
                    IMK00003.ReadInputFile();
                    IMK00003.OpenDatabaseConnection();
                    IMK00003.QueryDetail();

                }
                catch (Exception e)
                {
                    String lstrErrorMessage;
                    lstrErrorMessage = String.Format("Exception received : {0} in step {1}", e.Message, CStep.m_strLastStep);

                    CStep.m_oOut.WriteLine("{0}{1}", lstrErrorMessage, CStep.EndlLog);
                    IMK00003.DisplayWriteLog(lstrErrorMessage, WriteOptions.Both);
                    lnRetCode = 1;
                }
            }

            IMK00003.Exit(lnRetCode);
            return;
        }

        void GetEnvironmentData()
        {
            using (new CStep("GetEnvironmentData"))
            {
                iniFilePath = "D:\\FacetsInterfaces\\Files\\" + m_strProgramID + "_01.ini";

                // Get information from the .ini file
                iniFileRdr = new IniFileReader(iniFilePath);

                //Retrieve Log File Name - Status log for this process
                m_strServer = iniFileRdr.GetParameter("Program Parameters", "gcsServer");

                //Retrieve Log File Name - Status log for this process
                m_strDatabase = iniFileRdr.GetParameter("Program Parameters", "gcsDatabase");

                //Retrieve Log File Name - Status log for this process
                m_strDatabase2 = iniFileRdr.GetParameter("Program Parameters", "gcsDatabase2");

                //Retrieve Log File Name - Status log for this process
                m_strLogFile = iniFileRdr.GetParameter("Program Parameters", "gcsLogFile");

                //Retrieve Output File1 Name - Incoming log information from FTP process
                m_strOutputFile = iniFileRdr.GetParameter("Program Parameters", "gcsOutputFile");

                //Retrieve Output File1 Name - Incoming log information from FTP process
                m_strInputFile = iniFileRdr.GetParameter("Program Parameters", "gcsInputFile");

                //Retrieve Begin Date - Incoming log information from FTP process
                m_strBeginDate = iniFileRdr.GetParameter("Program Parameters", "BeginDate");

                //Retrieve End Date - Incoming log information from FTP process
                m_strEndDate = iniFileRdr.GetParameter("Program Parameters", "EndDate");

                string lstrSecurityIni = iniFileRdr.GetParameter("Program Parameters", "SecurityPath");
                iniFileRdr = new IniFileReader(lstrSecurityIni);

                //Retrieve Database UserId
                m_strDatabaseUserId = iniFileRdr.GetParameter("Program Parameters", "gcsDatabaseUserId");

                //Retrieve Database Password
                m_strDatabasePassword = iniFileRdr.GetParameter("Program Parameters", "gcsDatabasePassword");
                DisplayWriteLog("\r\n Successfully read Environment parameters\r\n", 0);


                DisplayWriteLog(" " + m_strProgramID + " Process Begins", WriteOptions.Both);
                DisplayWriteLog(" ", WriteOptions.Both);
                DisplayWriteLog(" Server Name:          " + m_strServer, WriteOptions.Both);
                DisplayWriteLog(" Database Name:        " + m_strDatabase, WriteOptions.Both);
                DisplayWriteLog(" 2nd Database Name:    " + m_strDatabase2, WriteOptions.Both);
                DisplayWriteLog(" Log File Name:        " + m_strLogFile, WriteOptions.Both);
                DisplayWriteLog(" Output File Name:     " + m_strOutputFile, WriteOptions.Both);
                DisplayWriteLog(" UserId:               " + m_strDatabaseUserId, WriteOptions.Both);
                DisplayWriteLog(" Begin Date:           " + m_strBeginDate, WriteOptions.Both);
                DisplayWriteLog(" End Date  :           " + m_strEndDate, WriteOptions.Both);

            }
        }
        void OpenInputFile()
        {
            using (new CStep("OpenInputFiles"))
            {
                fs_InputFile = new FileStream(m_strInputFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                m_oCustomerClaimsInputFile = new StreamReader(m_strInputFile);
            }


        }

        void OpenOutputFiles()
        {
            using (new CStep("OpenOutputFiles"))
            {
                //Open Log File - Status log for this process
                fs_LogFile = new FileStream(m_strLogFile, FileMode.Create, FileAccess.Write, FileShare.None);
                sw_LogFile = new StreamWriter(fs_LogFile);

                //Open Output File - Claims file to Customer
                fs_OutputFile = new FileStream(m_strOutputFile, FileMode.Create, FileAccess.Write, FileShare.None);
                m_oCustomerClaimsOutputFile = new StreamWriter(fs_OutputFile);
            }
        }

        void OpenDatabaseConnection()
        {
            using (new CStep("OpenDatabaseConnection"))
            {
                m_oOdbcConnection.ConnectionString = iniFileRdr.CreateODBCStringForDSNLess(m_strServer, m_strDatabase, m_strDatabaseUserId, m_strDatabasePassword);
                m_oOdbcConnection.ConnectionTimeout = 0;
                m_oOdbcConnection.Open();

                m_oOdbcConnection2.ConnectionString = iniFileRdr.CreateODBCStringForDSNLess(m_strServer, m_strDatabase2, m_strDatabaseUserId, m_strDatabasePassword);
                m_oOdbcConnection2.ConnectionTimeout = 0;
                m_oOdbcConnection2.Open();
            }
        }

        string Format(object poField, int pnSize)
        {
            if (poField is int)
            {
                int lnInt = (int)poField;
                string lstrInt = lnInt.ToString();
                return lstrInt.PadLeft(pnSize, '0');
            }
            else if (poField is DateTime)
            {
                DateTime ldtDt = (DateTime)poField;

                return ldtDt.ToString("yyyyMMdd");
            }
            else if (poField is decimal)
            {
                decimal lnAmt = (decimal)poField;

                string lstrAmt = lnAmt.ToString("#.00");

                lstrAmt = lstrAmt.Remove(lstrAmt.IndexOf('.'), 1);

                lstrAmt = lstrAmt.PadLeft(pnSize, '0');

                return lstrAmt;
            }
            else if (poField is double)
            {
                double lnAmt = (double)poField;

                string lstrAmt = lnAmt.ToString("#.00");

                lstrAmt = lstrAmt.Remove(lstrAmt.IndexOf('.'), 1);

                lstrAmt = lstrAmt.PadLeft(pnSize, '0');

                return lstrAmt;
            }
            else if (poField is char)
            {
                string ls_fixedWidth;
                int li_length = poField.ToString().Length;
                ls_fixedWidth = poField.ToString();
                ls_fixedWidth.Substring(0, li_length);
                return ls_fixedWidth;

            }
            else if (poField is string)
            {
                string ls_fixedWidth = poField.ToString();

                if (ls_fixedWidth.Length > pnSize)
                    ls_fixedWidth = ls_fixedWidth.Substring(0, pnSize);
                else if (ls_fixedWidth.Length < pnSize)
                    ls_fixedWidth = ls_fixedWidth.PadRight(pnSize, ' ');

                return ls_fixedWidth;

            }
            else if (poField == null)
            {
                string lstrSpace = " ";
                return lstrSpace.PadRight(pnSize, ' ');
            }
            else
            {
                string ls_fixedWidth = poField.ToString();

                if (ls_fixedWidth.Length > pnSize)
                    ls_fixedWidth = ls_fixedWidth.Substring(0, pnSize);
                else if (ls_fixedWidth.Length < pnSize)
                    ls_fixedWidth = ls_fixedWidth.PadRight(pnSize, ' ');

                return ls_fixedWidth;
            }
        }


        void ReadInputFile()
        {

            StreamReader losr = new StreamReader(m_strInputFile);
            String line;
            while ((line = losr.ReadLine()) != null)
            {
                if ( (line.Length > 0) && line.Substring(0, 2) == "BC")
                {
                    mi_medClaimsReadIn++;
                    WriteDetailOutput(line);
                    if ((mi_medClaimsReadIn % 10) == 0)
                        CStep.m_oOut.Write(".");

                    if ((mi_medClaimsReadIn % 100) == 0)
                        CStep.m_oOut.Write("{0} Medical records processed {1}", mi_medClaimsReadIn, CStep.EndlLog);

                }
            }

            losr.Close();
        }
        void QueryDetail()
        {

            using (new CStep("QueryDetailRx"))
            {
                string lstrSQL;
                            

                lstrSQL = "SELECT " +
                "SUBR_CONT_NO, " +
                "DRUG_PROCESS_MBR_CD, " +
                "DOCM_CTL_NO, " +
                "DRUG_ORIG_AMT, " +
                "DRUG_ALLOW_AMT, " +
                "CLM_BAL_AMT, " +
                "CLM_PAY_DTE, " +
                "PHAR_DISPEN_DTE, " +
                "GRGR_ID " +

                           " FROM ANT017_DRUG_CLAIMS ANT017 " +
                   "WHERE " +
                " ANT017.GRGR_ID = '" + Customer + "' AND " +
                " ANT017.CLM_PAY_DTE > ? " +
                " AND ANT017.CLM_PAY_DTE < ? " +
                " AND ANT017.DRUG_ORIG_AMT >=0 " +  
               " AND ANT017.DRUG_ALLOW_AMT >= 0 " +
                " AND ANT017.CLM_BAL_AMT >= 0 " 
                

                      ;

                using (OdbcCommand loCommand = new OdbcCommand(lstrSQL, m_oOdbcConnection))
                {
                    loCommand.Parameters.Add("@BeginDate", OdbcType.DateTime);
                    loCommand.Parameters["@BeginDate"].Value = DateTime.Parse(m_strBeginDate);

                    loCommand.Parameters.Add("@EndDate", OdbcType.DateTime);
                    loCommand.Parameters["@EndDate"].Value = DateTime.Parse(m_strEndDate);

                    loCommand.CommandTimeout = 0;

                    OdbcDataReader loReader = loCommand.ExecuteReader();

                    int lnRecordCount = 0;

                    while (loReader.Read())
                    {
                        string lstrReport = "";
                        string ls_AdjustmentInd = " ";
                        string ls_MEME_FIRST_NAME;
                        string ls_Filler = "";
                        ls_Filler = Format(ls_Filler,61);
                        string ls_PHAR_DISPEN_DTE;
                        string ls_CLM_PAY_DTE;
                        string ls_COB_IND = "";
                        string ls_COB_AMOUNT;

                        string ls_SUBR_CONT_NO;
                        string ls_DRUG_PROCESS_MBR_CD;
                        string ls_SubscriberSSN;
                        string ls_DRUG_ORIG_AMT;
                        string ls_DRUG_ALLOW_AMT;
                        string ls_CLM_BAL_AMT;
                        string ls_DOCM_CTL_NO;
                        string ls_GRGR_ID;
                        Int32 li_sbsbCK = 0;

                        ls_SUBR_CONT_NO = Format(loReader["SUBR_CONT_NO"], 9);
                        ls_DRUG_PROCESS_MBR_CD = Format(loReader["DRUG_PROCESS_MBR_CD"], 2);
                        ls_DOCM_CTL_NO = Format(loReader["DOCM_CTL_NO"], 10);
                        ls_DRUG_ORIG_AMT = Format(loReader["DRUG_ORIG_AMT"], 7);
                        ls_DRUG_ALLOW_AMT = Format(loReader["DRUG_ALLOW_AMT"], 7);
                        ls_CLM_BAL_AMT = Format(loReader["CLM_BAL_AMT"], 7);
                        ls_CLM_PAY_DTE = Format(loReader["PHAR_DISPEN_DTE"], 8);
                        ls_PHAR_DISPEN_DTE = Format(loReader["PHAR_DISPEN_DTE"], 8);
                        ls_GRGR_ID = Format(loReader["GRGR_ID"], 8);
                                            


                        li_sbsbCK = QueryDeidentifiedSubscriberId(ls_SUBR_CONT_NO, ls_GRGR_ID);
                        if (li_sbsbCK == 0)
                        {
                            li_sbsbCK = QueryDeidentifiedSubscriberId2(ls_SUBR_CONT_NO, ls_GRGR_ID);
                        }

                        int li_DRUG_PROCESS_MBR_CD = Convert.ToInt16(ls_DRUG_PROCESS_MBR_CD);
                        ls_MEME_FIRST_NAME = QueryMemberName(li_sbsbCK, li_DRUG_PROCESS_MBR_CD);

                        ls_SubscriberSSN = QuerySubscriberSSN(li_sbsbCK, ls_GRGR_ID);

                        ls_COB_IND = "N";

                        ls_COB_AMOUNT = Format(0, 7);

                        lstrReport = CARRIER_IND + "       " + ls_SubscriberSSN + " " + ls_DRUG_PROCESS_MBR_CD + " " + ls_DOCM_CTL_NO + CLAIM_TYPE + ls_AdjustmentInd + ls_MEME_FIRST_NAME + ls_DRUG_ORIG_AMT;
                        lstrReport += ls_DRUG_ALLOW_AMT + ls_CLM_BAL_AMT + ls_COB_IND + ls_COB_AMOUNT + ls_PHAR_DISPEN_DTE + ls_PHAR_DISPEN_DTE + ls_Filler
                        ;

                        WriteDetailOutput(lstrReport);
                        lnRecordCount++;

                        if ((lnRecordCount % 10) == 0)
                            CStep.m_oOut.Write(".");

                        if ((lnRecordCount % 100) == 0)
                            CStep.m_oOut.Write("{0} Drug records processed {1}", lnRecordCount, CStep.EndlLog);


                    }
                    WriteTrailerRecord(lnRecordCount);

                }
            }
        }


        int QueryDeidentifiedSubscriberId(string pstrSUBR_CONT_NO, string pstrGRGR_ID)
        {
        
            string lstrSQL;
            int li_sbsbCK = 0;

            lstrSQL =
            "SELECT IT369.SBSB_CK " +
                   "FROM IT369_SUBR_ID_HISTORY IT369 " +
               "WHERE " +
            " IT369.REC_ACT_IND = 'Y' AND " +
            " IT369.CONV_STATUS_CD = '03' AND " +
            " IT369.GRGR_ID = ? " +
           " AND IT369.SBIH_SBSB_ID_ORIG = ? " 
  
             ;

            using (OdbcCommand loCommand = new OdbcCommand(lstrSQL, m_oOdbcConnection2))
            {


                loCommand.Parameters.Add("@GRGR_ID", OdbcType.Text);
                loCommand.Parameters["@GRGR_ID"].Value = pstrGRGR_ID;

                loCommand.Parameters.Add("@SUBR_CONT_NO", OdbcType.Text);
                loCommand.Parameters["@SUBR_CONT_NO"].Value = pstrSUBR_CONT_NO;

                loCommand.CommandTimeout = 0;

                OdbcDataReader loReader = loCommand.ExecuteReader();

                if (loReader.Read())
                {
                    li_sbsbCK = (int) loReader["SBSB_CK"];
                    mi_ssn1++;
                }

                while (loReader.Read()) ;

                loReader = null;
            }

            return li_sbsbCK;
        }

        int QueryDeidentifiedSubscriberId2(string pstrSUBR_CONT_NO, string pstrGRGR_ID)
        {
            int li_sbsbCK = 0;
            string lstrSQL;

            lstrSQL =
            "SELECT IT369.SBSB_CK " +
               "FROM IT369_SUBR_ID_HISTORY IT369 " +
               "WHERE " +
            " IT369.REC_ACT_IND = 'Y' AND " +
            " IT369.GRGR_ID = ? " +
             " AND IT369.SBIH_SBSB_ID_NEW = ? " 
              ;

            OdbcCommand loCommand = new OdbcCommand(lstrSQL, m_oOdbcConnection2);

            loCommand.Parameters.Add("@GRGR_ID", OdbcType.Text);
            loCommand.Parameters["@GRGR_ID"].Value = pstrGRGR_ID;

            loCommand.Parameters.Add("@SUBR_CONT_NO", OdbcType.Text);
            loCommand.Parameters["@SUBR_CONT_NO"].Value = pstrSUBR_CONT_NO;


            loCommand.CommandTimeout = 0;

            OdbcDataReader loReader = loCommand.ExecuteReader();

            if (loReader.Read())
            {
                li_sbsbCK = (int) loReader["SBSB_CK"];
                mi_ssn2++;
            }

            while (loReader.Read()) ;

            loReader = null;

            return li_sbsbCK;


        }

        string QuerySubscriberSSN(int piSbsbCK, string pstrGRGR_ID)
        {

            string ls_SUBSCRIBER_SSN = "";
            string ls_GRGR_ID = "";
            ls_GRGR_ID = pstrGRGR_ID;
            string lstrSQL;

            lstrSQL =
                       "SELECT MEME.MEME_SSN " +
                        "FROM CMC_MEME_MEMBER MEME," +
                  " CMC_SBSB_SUBSC SBSB, " +
                  " CMC_GRGR_GROUP GRGR " +
               "WHERE " +
            " MEME.GRGR_CK = GRGR.GRGR_CK AND " +
            " MEME.GRGR_CK = SBSB.GRGR_CK AND " +
            " SBSB.GRGR_CK = GRGR.GRGR_CK AND " +
            " MEME.SBSB_CK = SBSB.SBSB_CK AND " +
            " MEME.MEME_REL = 'M' AND " +
            " GRGR.GRGR_ID = ? " +
            " AND SBSB.SBSB_CK = ?" 
         
            ;

            OdbcCommand loCommand = new OdbcCommand(lstrSQL, m_oOdbcConnection2);


            loCommand.Parameters.Add("@GRGR_ID", OdbcType.Text);
            loCommand.Parameters["@GRGR_ID"].Value = pstrGRGR_ID;

            loCommand.Parameters.Add("@SbsbCK", OdbcType.Int);
            loCommand.Parameters["@SbsbCK"].Value = piSbsbCK;
            loCommand.CommandTimeout = 0;

            OdbcDataReader loReader = loCommand.ExecuteReader();

            if (loReader.Read())
            {
                ls_SUBSCRIBER_SSN = Format(loReader["MEME_SSN"], 9);
                mi_oldSSNfound++;
            }
            else
                mi_oldSSNNotfound++;

            while (loReader.Read()) ;

            loReader = null;

            if (ls_SUBSCRIBER_SSN == "")
                ls_SUBSCRIBER_SSN = ls_SUBSCRIBER_SSN.PadLeft(9, ' ');

            return ls_SUBSCRIBER_SSN;

        }
        string QueryMemberName(int piSbsbCK, int pi_DRUG_PROCESS_MBR_CD)
        {

            string ls_MEME_FIRST_NAME = "";
            string lstrSQL;

            lstrSQL =
                          " SELECT MEME.MEME_FIRST_NAME " +
                            "     	FROM " +
                             "    CMC_MEME_MEMBER MEME, " +
                             "   CMC_GRGR_GROUP GRGR, " +
                             " CMC_SBSB_SUBSC SBSB " +
                            "    WHERE " +
                            " GRGR.GRGR_CK = MEME.GRGR_CK AND " +
                            " SBSB.SBSB_CK = MEME.SBSB_CK AND " +
                            " SBSB.GRGR_CK = GRGR.GRGR_CK AND " +
                            " MEME.SBSB_CK = SBSB.SBSB_CK AND " +
                            " MEME.GRGR_CK = GRGR.GRGR_CK AND " +
                            "  MEME.MEME_SFX = ? "+
                               " AND " +
                            " SBSB.SBSB_CK = ? "

                               ;


            using (OdbcCommand loCommand = new OdbcCommand(lstrSQL, m_oOdbcConnection2))
            {
                loCommand.Parameters.Add("@DRUG_PROCESS_MBR_CD", OdbcType.Int);
                loCommand.Parameters["@DRUG_PROCESS_MBR_CD"].Value = pi_DRUG_PROCESS_MBR_CD;

                loCommand.Parameters.Add("@SbsbCK", OdbcType.Int);
                loCommand.Parameters["@SbsbCK"].Value = piSbsbCK;
 
                loCommand.CommandTimeout = 0;

                OdbcDataReader loReader = loCommand.ExecuteReader();

                if (loReader.Read())
                {
                    ls_MEME_FIRST_NAME = Format(loReader["MEME_FIRST_NAME"], 14);
                    ls_MEME_FIRST_NAME = ls_MEME_FIRST_NAME.ToUpper();
                    mi_firstNameFound++;
                }

                while (loReader.Read()) ;

                loReader = null;
            }
            if (ls_MEME_FIRST_NAME == "")
                ls_MEME_FIRST_NAME = ls_MEME_FIRST_NAME.PadLeft(14, ' ');

            return ls_MEME_FIRST_NAME;
        }
        string PadLeftLimitString(string pstrStr, int pnSize, char pchPad)
        {
            string lstrRes;

            if (pstrStr.Length < pnSize)
                lstrRes = pstrStr.PadLeft(pnSize, pchPad);
            else
                lstrRes = pstrStr.Substring(0, pnSize);

            return lstrRes;
        }

        string PadRightLimitString(string pstrStr, int pnSize, char pchPad)
        {
            string lstrRes;

            if (pstrStr.Length < pnSize)
                lstrRes = pstrStr.PadRight(pnSize, pchPad);
            else
                lstrRes = pstrStr.Substring(0, pnSize);

            return lstrRes;
        }

        void WriteHeaderRecord()
        {

            string ls_headerLine;
            string ls_carrierCode = "companyCode ";
            string ls_filler1 = "";

            ls_filler1 = PadLeftLimitString(ls_filler1, 20, '0');

            string ls_currentDate = Format(DateTime.Now, 8);

            ls_headerLine = ls_filler1 + ls_carrierCode + ls_currentDate;

            m_oCustomerClaimsOutputFile.WriteLine(ls_headerLine);
        }

        void WriteTrailerRecord(int p_irecordCount)
        {

            string ls_TrailerFiller = "99999999999999999999";
            string ls_TrailerRecord;
            int li_recordCount = 0;
            string ls_TrailerSpaces = "";
            ls_TrailerSpaces = PadLeftLimitString(ls_TrailerSpaces, 60, ' ');


            li_recordCount = p_irecordCount + mi_medClaimsReadIn;
            ls_TrailerRecord = ls_TrailerFiller + li_recordCount + ls_TrailerSpaces;
            m_oCustomerClaimsOutputFile.WriteLine(ls_TrailerRecord);
        }

        void WriteDetailOutput(string p_strBuffer)
        {
            m_oCustomerClaimsOutputFile.WriteLine(p_strBuffer);
        }

        void DisplayWriteLog(string p_strBuffer, WriteOptions p_iWriteOpt)
        {
            if (p_iWriteOpt == WriteOptions.Con || p_iWriteOpt == WriteOptions.Both)
                Console.WriteLine(p_strBuffer);

            if ((p_iWriteOpt == WriteOptions.Log || p_iWriteOpt == WriteOptions.Both) && sw_LogFile != null)
                sw_LogFile.WriteLine(p_strBuffer);
        }

        void Exit(int pnRetCode)
        {
            using (new CStep("Exit"))
            {
                string lstrExitStatus = String.Format("\r\n\r\nexit code is: {0}{1}", pnRetCode, CStep.EndlLog);

                DisplayWriteLog(lstrExitStatus, WriteOptions.Both);


                string ls_totals = "";
                ls_totals = "SSNs Found 1st query  " + mi_ssn1.ToString() + "\n" + "SSNS Found 2nd query " + mi_ssn2.ToString() +
                    "\n Old SSNs Found " + mi_oldSSNfound.ToString() + "\n SSNS NOT FOUND !!! " + mi_oldSSNNotfound +
                    "\n Medical Claims Written " + mi_medClaimsReadIn + "\n First Names found " + mi_firstNameFound + "\n\n"; 
                Console.Write(ls_totals);



                if (sw_LogFile != null)
                    sw_LogFile.Close();

                if (m_oCustomerClaimsOutputFile != null)
                    m_oCustomerClaimsOutputFile.Close();
            }
        }
    }
}
