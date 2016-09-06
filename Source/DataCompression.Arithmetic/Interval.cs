namespace DataCompression.Common
{
    public class Interval
    {
        public double High { get; set; }
        public double Low { get; set; }
        public double Size { get; set; }
        public Interval()
        {
            
        }

        public Interval(double p_low, double p_high)
        {
            Low = p_low;
            High = p_high;
            Size = High - Low;
        }

        public static Interval UpdateInterval(Interval p_intervalCurrent, Interval p_newCharacter)
        {
            Interval newInterval = new Interval();
            newInterval.Low = p_intervalCurrent.Low + (p_intervalCurrent.High - p_intervalCurrent.Low) * p_newCharacter.Low;
            newInterval.High = p_intervalCurrent.Low + (p_intervalCurrent.High - p_intervalCurrent.Low) * p_newCharacter.High;

            return newInterval; 
        }

        public static void UpdateCurrentInterval(Interval p_intervalCurrent, Interval p_newCharacter)
        {
            p_intervalCurrent.Low = p_intervalCurrent.Low + (p_intervalCurrent.High - p_intervalCurrent.Low) * p_newCharacter.Low;
            p_intervalCurrent.High = p_intervalCurrent.Low + (p_intervalCurrent.High - p_intervalCurrent.Low) * p_newCharacter.High;
        }
    }
}
