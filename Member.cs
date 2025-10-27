using System;
using FileHelpers;

namespace IMB00476
{
    [FixedLengthRecord(FixedMode.ExactLength)]
    public class Member
    {
        [FieldFixedLength(30)]
        public String SBSB_ID;

        [FieldFixedLength(11)]
        public String MEME_SFX;

        [FieldFixedLength(30)]
        public String FILLER1;

        [FieldFixedLength(25)]
        public String MEMBER_FIRSTNAME;

        [FieldFixedLength(25)]
        public String MEMBER_LASTNAME;

        [FieldFixedLength(25)]
        public String MEMBER_MI;

        [FieldFixedLength(3)]
        public String GENDER;

        [FieldFixedLength(15)]
        public String RELATIONSHIP;

        [FieldFixedLength(40)]
        public String MEMBER_ADDR1;

        [FieldFixedLength(40)]
        public String MEMBER_ADDR2;

        [FieldFixedLength(40)]
        public String MEMBER_ADDR3;

        [FieldFixedLength(30)]
        public String MEMBER_COUNTY;

        [FieldFixedLength(20)]
        public String MEMBER_CITY;

        [FieldFixedLength(20)]
        public String MEMBER_ZIP;

        [FieldFixedLength(12)]
        public String MEMBER_STATE;

        [FieldFixedLength(10)]
        public String DOB;

        [FieldFixedLength(3)]
        public String RACE;

        [FieldFixedLength(3)]
        public String AGE;

        [FieldFixedLength(30)]
        public String PCP;

        [FieldFixedLength(30)]
        public String TREATING_MD;

        [FieldFixedLength(15)]
        public String HOME_PHONE;

        [FieldFixedLength(15)]
        public String WORK_PHONE;

        [FieldFixedLength(3)]
        public String LANGUAGE_IND;

        [FieldFixedLength(10)]
        public String ELIG_START_DT;

        [FieldFixedLength(10)]
        public String ELIG_END_DT;

        [FieldFixedLength(30)]
        public String FILLER2;

        [FieldFixedLength(20)]
        public String PARENT_GROUP_ID;

        [FieldFixedLength(20)]
        public String GROUP_ID;

        [FieldFixedLength(20)]
        public String SUB_GROUP_ID;

        [FieldFixedLength(20)]
        public String CLASS_PLAN;

        [FieldFixedLength(20)]
        public String FILLER3;

        [FieldFixedLength(15)]
        public String FILLER4;

        [FieldFixedLength(1)]
        public String FILLER5;

        [FieldFixedLength(25)]
        public String FILLER6;

        [FieldFixedLength(25)]
        public String FILLER7;

        [FieldFixedLength(25)]
        public String FILLER8;

        [FieldFixedLength(15)]
        public String FILLER9;

        [FieldFixedLength(4)]
        public String PLAN_TYP;

        [FieldFixedLength(2)]
        public String LOB;

        [FieldFixedLength(10)]
        public String BENEFIT_PLAN;

        [FieldFixedLength(255)]
        public String EMAIL;

        [FieldFixedLength(50)]
        public String FILLER10;

        [FieldFixedLength(50)]
        public String FILLER11;

        [FieldFixedLength(50)]
        public String SBU;

        [FieldFixedLength(50)]
        public String CPU;

        [FieldFixedLength(50)]
        public String FILLER12;

        [FieldFixedLength(50)]
        public String FILLER13;

        [FieldFixedLength(50)]
        public String FILLER14;

        [FieldFixedLength(1)]
        public String BITMAP;

        [FieldFixedLength(25)]
        public String HEALTH_SUPPORT_BITMAP;
    }
    
	#region "  Decimal Converter  "

	internal class TwoDecimalConverter: ConverterBase
	{
		public override object StringToField(string from)
		{
			decimal res = Convert.ToDecimal(from);
			return res / 100;
		}

		public override string FieldToString(object from)
		{
			decimal d = (decimal) from;
			return Math.Round(d * 100).ToString();
		}
	}

	#endregion

}
