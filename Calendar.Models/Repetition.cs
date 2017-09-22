using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Models
{
    public class Repetition
    {
        public enum Repeats { Hourly, Daily, Weekly, Monthly, Yearly };

        public class DateList : IList<int>
        {
            private const string RepeatError = "Cannot use RepeatOn value of {0} with RType {1}.";
            private Repetition _owner;
            private List<int> _list;

            public int this[int index]
            {
                get => ((IList<int>)_list)[index];
                set
                {
                    if (IsValid(value))
                    {
                        ((IList<int>)_list)[index] = value;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("value", String.Format(RepeatError, value, _owner.RType.ToString()));
                    }
                }
            }

            #region Constructors
            #endregion

            #region Properties

            public int Capacity
            {
                get { return _list.Capacity; }
                private set { _list.Capacity = value; }
            }

            public bool IsReadOnly => ((IList<int>)_list).IsReadOnly;

            int ICollection<int>.Count => ((IList<int>)_list).Count;

            #endregion

            private bool IsValid(int value)
            {
                int start = 1, end = -1;

                switch(_owner.RType)
                {
                    case Repeats.Weekly:
                        end = 7;
                        break;
                    case Repeats.Monthly:
                        end = 31;
                        break;
                    case Repeats.Yearly:
                        end = 365;
                        break;
                    default:
                        return false;
                }

                return value >= start && value <= end;
            }

            public void Add(int item)
            {
                if(IsValid(item))
                {
                    ((IList<int>)_list).Add(item);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("item", String.Format(RepeatError, item, _owner.RType.ToString()));
                }
            }

            public void Clear()
            {
                ((IList<int>)_list).Clear();
            }

            public bool Contains(int item)
            {
                return ((IList<int>)_list).Contains(item);
            }

            public void CopyTo(int[] array, int arrayIndex)
            {
                ((IList<int>)_list).CopyTo(array, arrayIndex);
            }

            public IEnumerator<int> GetEnumerator()
            {
                return ((IList<int>)_list).GetEnumerator();
            }

            public int IndexOf(int item)
            {
                return ((IList<int>)_list).IndexOf(item);
            }

            public void Insert(int index, int item)
            {
                if(IsValid(item))
                {
                    ((IList<int>)_list).Insert(index, item);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("item", String.Format(RepeatError, item, _owner.RType.ToString()));
                }
            }

            public bool Remove(int item)
            {
                return ((IList<int>)_list).Remove(item);
            }

            public void RemoveAt(int index)
            {
                ((IList<int>)_list).RemoveAt(index);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IList<int>)_list).GetEnumerator();
            }
        } //TODO rename

        private static Repeats[] RepeatsOnValidTypes = new Repeats[] { Repeats.Weekly, Repeats.Monthly, Repeats.Yearly };

        private Repeats _type;
        private int _repeatsEvery;

        #region Properties

        public Repeats RType
        {
            get { return _type; }
            set { _type = value; }
        }

        public DateTime Starts { get; private set; }
        
        public int RepeatsEvery { get; private set; }
        
        public DateList RepeatsOn { get; private set; } //should this be a class list

        #endregion

        #region Constructors
        public Repetition(Repeats type, int repeatsEvery, DateTime start)
        {
            Starts = start;
            RType = type;
            RepeatsEvery = repeatsEvery;

            if(RepeatsOnValidTypes.Contains(type))
            {
                RepeatsOn = new DateList();
                RepeatsOn.Add(GetRepeatOnValue(start, type));
            }
            else
            {
                RepeatsOn = null;
            }
        }

        public Repetition(Repeats type, int repeatsEvery, params DateTime[] first)
        {
            RType = type;
            RepeatsEvery = repeatsEvery;
            //TODO set Starts to the earliest date in the list

            if (RepeatsOnValidTypes.Contains(type))
            {
                RepeatsOn = new DateList();
                Array.Sort(first); //TODO check how it's sorted
                foreach(DateTime date in first)
                {
                    RepeatsOn.Add(GetRepeatOnValue(date, type));
                }
            }
            else
            {
                RepeatsOn = null;
            }
        }

        public Repetition(Repeats type, DateTime start) : this(type, 1, start) { }

        public Repetition(Repeats type, DateTime[] first) : this(type, 1, first) { }

        #endregion

        #region Public Methods
        public DateTime[] GetInstancesFor(DateTime start, DateTime end)
        {
            List<DateTime> dates = new List<DateTime>();

            //check if start date is a repeat day
            //if no - set start date to closest repeat date after start

            //add start to dates
            //multiply repeatsevery to onecycle value (which is based on Type)

            //while date != end
                //if repeats on
                    //for reach repeatson value
                        //use either AddDays, AddMonths or AddYears using the product above
                        //add repeatson date
                //else
                //use either AddHours or AddDays
                //addrepeatson date
                

            return dates.ToArray<DateTime>();
        }
        #endregion

        #region Static Methods
        public static int GetRepeatOnValue(DateTime date, Repeats type)
        {
            switch(type)
            {
                case Repeats.Monthly:
                    return date.Day;
                case Repeats.Weekly:
                    throw new NotImplementedException("To do - figure out how to convert day of the week to a number.");
                case Repeats.Yearly:
                    return date.DayOfYear;
            }

            return -1;
        }

        #endregion
        
    }
}
