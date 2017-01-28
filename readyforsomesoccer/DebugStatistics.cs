using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace readyforsomesoccer
{
    public static class DebugStatistics
    {
        public static TimeSpan evaluationTime = new TimeSpan();
        public static TimeSpan analysisTime = new TimeSpan();
        public static TimeSpan wholeTime = new TimeSpan();
        public static TimeSpan timeSearchingDistance = new TimeSpan();
        public static TimeSpan timeDuringChecking = new TimeSpan();
        public static TimeSpan timeEvaluating = new TimeSpan();
        public static int enteredCheck = 0;
        public static int passedHash = 0;
        public static int passedHashWasFalse = 0;
        public static int crawlcounter = 0;
        public static void resetStats()
        {
            wholeTime = new TimeSpan();
            timeSearchingDistance = new TimeSpan();
            timeDuringChecking = new TimeSpan();
            passedHash = 0;
            passedHashWasFalse = 0;
            enteredCheck = 0;
            crawlcounter = 0;
            timeEvaluating = new TimeSpan();
        }
        public static string ToString()
        {
            return @"Czas całkowity" + wholeTime.ToString() + @"
            Czas liczenia odległości = " + timeSearchingDistance.ToString() + @"
Czas sprawdzania " + timeDuringChecking.ToString() + @"
Czas oceniania siły" + timeEvaluating.ToString() + @"
Obliczone układy = " + crawlcounter + @"
ilosc sprawdzeń = " + enteredCheck + @"
ilość pominięć hasha = " + passedHash + @"
ilość pominięć i nieprawdy" + passedHashWasFalse;
        }
    }

}
