using System;
using System.Collections.Generic;
using System.Text;

namespace Logging
{
    public class CEndlLog
    {
        public override string ToString()
        {
            string lstrRes;

            lstrRes = "\r\n";

            for (int i = 0; i < CStep.m_nLevel; i++)
                lstrRes += "\t";

            return lstrRes;
        }
    }
    public class CStep : IDisposable
    {
        public CStep(String pstrStepDesc)
        {
            m_dtStartDt = System.DateTime.Now;
            m_nLevel++;
            m_strStepDesc = pstrStepDesc;
            m_strLastStep = pstrStepDesc;
            m_oSteps.Push(pstrStepDesc);
            m_oOut.Write("==>\t{0}...{1}", m_strStepDesc, CStep.EndlLog);
        }
        public void Dispose()
        {
            TimeSpan loTSWork = System.DateTime.Now - m_dtStartDt;
            m_nLevel--;
            m_oOut.Write("{0} took {1}{2}", m_strStepDesc, ElapseDesc(loTSWork.TotalSeconds), CStep.EndlLog);

            if (!m_oPerfMap.ContainsKey(m_strStepDesc))
                m_oPerfMap.Add(m_strStepDesc, new CPerfInfo());

            m_oPerfMap[m_strStepDesc].AddPerf(loTSWork.TotalSeconds);

            m_oSteps.Pop();

            if (m_oSteps.Count > 0)
                m_strLastStep = m_oSteps.Peek();
            else
                m_strLastStep = "";

            if (m_nLevel == 0)
            {
                m_oOut.Write("\t{0,-38}{1,-10}{2,-10}{3,-10}{4,-10}{5}",
                    "Step Name", "#Runs", "Tot Time", "Min Time", "Max Time", CStep.EndlLog);

                foreach (KeyValuePair<string, CPerfInfo> kvp in m_oPerfMap)
                    m_oOut.Write("\t{0,-38}{1,-10}{2,-10}{3,-10}{4,-10}{5}",
                        kvp.Key, kvp.Value.m_nCount, kvp.Value.m_nTotElapsedSeconds, kvp.Value.m_nMinElapsedSeconds,
                        kvp.Value.m_nMaxElapsedSeconds, CStep.EndlLog);
            }
        }
        public static string ElapseDesc(double pnSeconds)
        {
            double lnSeconds = pnSeconds;

            const int lnSecsPerMinute = 60;
            const int lnSecsPerHour = lnSecsPerMinute * 60;
            const int lnSecsPerDay = lnSecsPerHour * 24;

            int lnNumDays = (int)lnSeconds / lnSecsPerDay;
            lnSeconds = lnSeconds - (double)lnNumDays * lnSecsPerDay;

            int lnNumHours = (int)lnSeconds / lnSecsPerHour;
            lnSeconds = lnSeconds - (double)lnNumHours * lnSecsPerHour;

            int lnNumMinutes = (int)lnSeconds / lnSecsPerMinute;
            lnSeconds = lnSeconds - (double)lnNumMinutes * lnSecsPerMinute;

            string lstrElapseDesc = "";

            if (lnNumDays != 0)
            {
                string lstrDaysDesc;

                lstrDaysDesc = String.Format("{0} day{1}", lnNumDays, lnNumDays == 1 ? "" : "s");

                if (lstrElapseDesc != "")
                    lstrElapseDesc += ", ";

                lstrElapseDesc += lstrDaysDesc;
            }

            if (lnNumHours != 0)
            {
                string lstrHoursDesc;

                lstrHoursDesc = String.Format("{0} hour{1}", lnNumHours, lnNumHours == 1 ? "" : "s");

                if (lstrElapseDesc != "")
                    lstrElapseDesc += ", ";

                lstrElapseDesc += lstrHoursDesc;
            }

            if (lnNumMinutes != 0)
            {
                string lstrMinutesDesc;

                lstrMinutesDesc = String.Format("{0} minute{1}", lnNumMinutes, lnNumMinutes == 1 ? "" : "s");

                if (lstrElapseDesc != "")
                    lstrElapseDesc += ", ";

                lstrElapseDesc += lstrMinutesDesc;
            }

            if ((lnSeconds == 0) || (lstrElapseDesc == ""))
            {
                string lstrSecondsDesc;

                lstrSecondsDesc = String.Format("{0} second{1}", lnSeconds, lnSeconds == 1 ? "" : "s");

                if (lstrElapseDesc != "")
                    lstrElapseDesc += ", ";

                lstrElapseDesc += lstrSecondsDesc;
            }

            return lstrElapseDesc;
        }

        public static int m_nLevel = 0;
        public static Stack<string> m_oSteps = new Stack<string>();
        public static CEndlLog EndlLog = new CEndlLog();
        public static System.IO.TextWriter m_oOut = System.Console.Out;
        public static Dictionary<string, CPerfInfo> m_oPerfMap = new Dictionary<string, CPerfInfo>();
        string m_strStepDesc;
        DateTime m_dtStartDt;
        public static string m_strLastStep;
    }

    public class CPerfInfo
    {
        public CPerfInfo()
        {
            m_nCount = 0;
            m_nTotElapsedSeconds = 0.0;

            m_nMinElapsedSeconds = 999999999.0;
            m_nMaxElapsedSeconds = -1.0;
        }

        public void AddPerf(double pnSeconds)
        {
            m_nTotElapsedSeconds += pnSeconds;
            m_nCount++;

            if (pnSeconds < m_nMinElapsedSeconds)
                m_nMinElapsedSeconds = pnSeconds;

            if (pnSeconds > m_nMaxElapsedSeconds)
                m_nMaxElapsedSeconds = pnSeconds;
        }

        public double m_nMinElapsedSeconds;
        public double m_nMaxElapsedSeconds;
        public double m_nTotElapsedSeconds;
        public int m_nCount;
    }
}


