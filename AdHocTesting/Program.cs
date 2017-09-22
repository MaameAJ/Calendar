using System;
using Calendar.Models;

namespace AdHocTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            int year = 2017, month = 10, day1 = 6, day2 = 21;
            DateTime date1 = new DateTime(year, month, day1), date2 = new DateTime(year, month, day2);

            PayDay avanade = new PayDay(PayDay.Type.BiMonthly, date1, date2);
            DateTime date = avanade.Start;

            for (int i = 0; i <= 12; i++)
            {
                DateTime[] dates = avanade.GetPayDates(date.Month, date.Year);

                for(int j = 0; j < dates.Length; j++)
                {
                    Console.WriteLine(dates[j].ToShortDateString());
                }

                date = date.AddMonths(1);

                Console.WriteLine();
            }


            Console.ReadKey();
        }
    }
}
