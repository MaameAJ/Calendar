using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Models
{
    public class PayDay
    {
        public enum Type { Weekly, Biweekly, Monthly, BiMonthly };

        public Type PType { get; private set; }

        private DateTime _start;

        public DateTime Start
        {
            get { return _start; }
            set { _start = value; }
        }

        public int[] DaysOfMonth { get; private set; }

        public PayDay(DateTime start, Type type)
        {
            _start = start;
            PType = type;
        }

        public PayDay(Type type, DateTime first, DateTime? second = null)
        {
            if(type == Type.Monthly)
            {
                DaysOfMonth = new int[] { first.Day };
            }
            else if(type == Type.BiMonthly && second != null)
            {
                _start = first;
                DaysOfMonth = new int[] { first.Day, second.Value.Day };
            }
        }

        public DateTime[] GetPayDates(int month, int year)
        {
            List<DateTime> dates = new List<DateTime>();

            for(int i = 0; i < DaysOfMonth.Length; i++)
            {
                dates.Add(GetLastBusinessDay(new DateTime(year, month, DaysOfMonth[i])));
            }

            return dates.ToArray<DateTime>();
        }

        private static DateTime GetLastBusinessDay(DateTime date)
        {
            //TODO check if holiday
            while (!date.IsBusinessDay())
            {
                date = date.AddDays(-1);
            }

            return date;
        }
    }
}
