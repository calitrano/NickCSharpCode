using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

namespace IniReader
{
    /// <summary>
    /// The IniFileReader class is used to read and process the values inside
    /// any standard format INI file.  It contains functions to retrieve
    /// the values from any section in the 
    /// </summary>
    class IniFileReader
    {
        private string _iniFilePath = "";
        private bool _hasFileLoaded = false;
        Hashtable _sectionFileHash = null;

        public IniFileReader()
        {
            _iniFilePath = "";
            _hasFileLoaded = false;
        }

        public IniFileReader(string pIniFilePath)
        {
            LoadFile(pIniFilePath);
        }

        public int LoadFile(string pIniFilePath)
        {
            int errorVal = -1;
            string errorMessage;

            _iniFilePath = "";
            _hasFileLoaded = false;

            errorVal = processIniFile(pIniFilePath, out errorMessage);
            if (errorVal == 0)
            {
                _iniFilePath = pIniFilePath;
                _hasFileLoaded = true;
            }

            return errorVal;
        }

        public string GetParameter(string pFileSectionHdr, string pParameterName)
        {
            Hashtable sectionParameters;
            string paramValue = null;

            pFileSectionHdr = pFileSectionHdr.ToLower();
            pParameterName = pParameterName.ToLower();

            if (!_hasFileLoaded)
                throw new System.Exception("There is no valid file loaded");

            if (_sectionFileHash.Contains(pFileSectionHdr))
            {
                sectionParameters = (Hashtable)_sectionFileHash[pFileSectionHdr];

                if (sectionParameters.Contains(pParameterName))
                    paramValue = sectionParameters[pParameterName].ToString();
            }
            else
                throw new System.Exception("Section header does not exist in this ini file");

            return paramValue;
        }

        public Hashtable GetFileSection(string pFileSectionHdr)
        {
            Hashtable sectionParameters = null;

            pFileSectionHdr = pFileSectionHdr.ToLower();

            if (!_hasFileLoaded)
                throw new System.Exception("There is no valid file loaded");

            if (_sectionFileHash.Contains(pFileSectionHdr))
                sectionParameters = (Hashtable)_sectionFileHash[pFileSectionHdr];

            return sectionParameters;
        }

        // Private members
        private int processIniFile(string pIniFilePath, out string pErrorMessage)
        {
            int errorVal = 0;
            int lineType, equalsIndex;
            System.IO.StreamReader iniFile = null;
            string currentLine;
            string sectionName;
            string namePart, valuePart;
            char[] sectHeaderDelims = { '[', ']' };
            string[] sectHeaderSplitLine;
            Hashtable sectionParameters = new Hashtable();

            _sectionFileHash = null;
            _sectionFileHash = new Hashtable();
            _sectionFileHash.Add("", sectionParameters);

            pErrorMessage = "";

            try
            {
                iniFile = File.OpenText(pIniFilePath);
            }
            catch (System.Exception e)
            {
                errorVal = -1;
                pErrorMessage = e.Message;
            }

            if (errorVal == 0)
            {
                while (!iniFile.EndOfStream)
                {
                    currentLine = iniFile.ReadLine();
                    currentLine = currentLine.Trim();

                    if (currentLine.Length == 0)
                        lineType = 0;
                    else if (currentLine[0] == '[')
                        lineType = 1;
                    else if (currentLine[0] == ';')
                        lineType = 0;
                    else
                        lineType = 2;

                    if (lineType == 1)
                    {
                        // Process the section header
                        sectHeaderSplitLine = currentLine.Split(
                            sectHeaderDelims);

                        if (sectHeaderSplitLine.Length > 1)
                        {
                            sectionName = sectHeaderSplitLine[1];
                            sectionName = sectionName.ToLower();
                            sectionParameters = new Hashtable();

                            if (!_sectionFileHash.Contains(sectionName))
                                _sectionFileHash.Add(sectionName, sectionParameters);
                            else
                            {
                                errorVal = -2;
                                break;
                            }
                        }

                    }
                    else if (lineType == 2)
                    {
                        // Process the name value pair
                        equalsIndex = currentLine.IndexOf("=");
                        if (equalsIndex > 0)
                        {
                            namePart = currentLine.Substring(0, equalsIndex).Trim();
                            namePart = namePart.ToLower();
                            valuePart = currentLine.Substring(equalsIndex + 1,
                                currentLine.Length - (equalsIndex + 1)).Trim();

                            if (valuePart.Length == 2 &&
                                valuePart[0] == '"' &&
                                valuePart[1] == '"')
                                valuePart = "";
                            else if (valuePart.Length == 2 &&
                                valuePart[0] == '\'' &&
                                valuePart[1] == '\'')
                                valuePart = "";
                            else if (valuePart.Length > 2)
                            {
                                if (valuePart[0] == '"' &&
                                    valuePart[valuePart.Length - 1] == '"')
                                    valuePart = valuePart.Substring(1,
                                        valuePart.Length - 2);
                                else if (valuePart[0] == '\'' &&
                                    valuePart[valuePart.Length - 1] == '\'')
                                    valuePart = valuePart.Substring(1,
                                        valuePart.Length - 2);
                            }

                            if (sectionParameters.Contains(namePart))
                                sectionParameters[namePart] = valuePart;
                            else
                            {
                                sectionParameters.Add(namePart, valuePart);
                            }
                        }

                    }
                }

                iniFile.Close();
            }

            return errorVal;
        }

        public string CreateODBCStringForDSNLess(string p_strServer, string p_strDatabase, string p_strDatabaseUserId, string p_strDatabasePassword)
        {
            const string lstrPath = "c:\\sybase\\odbcconf.ini";
            LoadFile(lstrPath);

            string lstrDriver = GetParameter("SybaseDriver", "PARM01");

            string lstrODBCString;
            lstrODBCString = "Driver={" + lstrDriver + "}";
            lstrODBCString += " ; ServerName=" + p_strServer;

            for (int i = 3; i <= 20; i++)
            {
                string lstrParmDesc = String.Format("PARM{0:00}", i);

                string lstrParmValue = GetParameter("SybaseDriver", lstrParmDesc);

                if (lstrParmValue != "")
                    lstrODBCString += " ; " + lstrParmValue;
            }


            lstrODBCString += ";DB=" + p_strDatabase + " ; "
                           + "Uid=" + p_strDatabaseUserId + " ; "
                           + "Pwd=" + p_strDatabasePassword;

            return lstrODBCString;
        }

    }
}
