using System;
using FileHelpers;

namespace IMB00476
{
    [FixedLengthRecord(FixedMode.ExactLength)]
    public class Control
    {
        [FieldFixedLength(11)]
        public String RECORD_CNT;
    }
}
