using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD40_2
{
    public class Calendar
    {
        public enum Month { Jan = 0, Feb = 1, Mar = 2, Apr = 3, May = 4, Jun = 5, Jul = 6, Aug = 7, Sep = 8, Oct = 9, Nov = 10, Dec = 11 }

        Date currentDate = new Date();
        public bool DateChanged = false;
        Month oldMonth;

        public void Update(double seconds)
        {
            DateChanged = false;
            currentDate.Update(seconds);
            if (currentDate.Month != oldMonth)
            {
                DateChanged = true;
            }
            oldMonth = currentDate.Month;
        }

        public Date GetDate()
        {
            return new Date(currentDate.Value);
        }

        public override string ToString()
        {
            return currentDate.ToString();
        }

        public double MonthsSince(Date date)
        {
            return GetDate().MonthsSince(date);
        }

        public class Date
        {
            private const int SECONDSPERMONTH = 15;
            private double val;

            public Date(double age = 0)
            {
                val = age;
            }

            public Date Update(double seconds)
            {
                val = seconds / SECONDSPERMONTH;
                return this;
            }

            public double Value
            {
                get
                {
                    return val;
                }
            }

            public int Year
            {
                get
                {
                    return 2000 + (int)(val / 12);
                }
            }

            public Month Month
            {
                get
                {
                    return (Month)(val % 12);
                }
            }

            public void AddMonths(int months)
            {
                val += months;
            }

            public void AddYears(int years)
            {
                val += (12 * years);
            }

            public double MonthsSince(Date date)
            {
                return Value - date.Value;
            }

            public string TimeSince(Date date)
            {
                double diff = MonthsSince(date);
                String retVal = "";
                if (diff > 12)
                {
                    retVal += diff / 12 + " years, ";
                }
                retVal += diff % 12 + " months";
                return retVal;
            }

            public override string ToString()
            {
                return Enum.GetName(Month.GetType(), Month) + " " + Year;
            }
        }
    }
}
