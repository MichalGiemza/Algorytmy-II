using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haszowanie
{
    public class Hash<T>
    {
        const int defaultArrayLenght = 8;
        const int arrayCheckCount = 16;
        readonly double hashConst = (Math.Sqrt(5) - 1) / 2;

        int elCount;
        List<T>[] lists;
        
        public Hash(int arrayLength = defaultArrayLenght)
        {
            lists = CreateNewLists(arrayLength);
            elCount = 0;
        }

        public int GetHash<Tgh>(Tgh el)
        {   // TODO: Rozbic switcha na metody
            double val;
            int pos;

            // Dla null
            if (el == null)
                return 0;

            switch (Type.GetTypeCode(el.GetType()))
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Char:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    pos = Convert.ToInt32(el);

                    if (pos == int.MinValue)
                        pos = int.MaxValue;

                    return (int)(lists.Length * (Math.Abs(pos) * hashConst % 1));

                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    val = Convert.ToDouble(el);

                    if (double.IsNaN(val) || double.IsInfinity(val))
                        return 0;

                    return (int)(lists.Length * (Math.Abs(val * hashConst % 1)));
                    
                case TypeCode.String:
                    string el_str = el as string;
                    val = 1.0;
                    pos = 0;

                    if (el_str.Length == 0)
                        return 0;

                    for (int i = 0; i < arrayCheckCount; i++)
                    {
                        val += (Math.Abs(el_str[(int)pos]) + 1) / val;

                        pos = (int)(el_str.Length * (Math.Abs(val) * hashConst % 1));
                    }
                    return (int)(lists.Length * (Math.Abs(val) * hashConst % 1));

                case TypeCode.Object:
                    
                    if (el.GetType().IsArray)
                    {   // Tablice, sprawdzane po wybranych elementach
                        var ar = el as Array;
                        val = 1.0;
                        pos = 0;

                        if (ar.Length == 0)
                            return 0;

                        for (int i = 0; i < arrayCheckCount; i++)
                        {
                            val += (GetHash(ar.GetValue(pos)) + 1) / val;
                            
                            pos = (int)(ar.Length * (Math.Abs(val) * hashConst % 1));
                        }
                        return (int)(lists.Length * (Math.Abs(val) * hashConst % 1));
                    }

                    if (typeof(T) is IEnumerable)
                    {   // Listy, stosy, sprawdzanie po pierwszych elementach
                        val = 1.0;
                        var e = (el as IEnumerable).GetEnumerator();
                            
                        for (int i = 0; i < arrayCheckCount; i++)
                        {
                            if (e.MoveNext() == false)
                                break;

                            val = GetHash(e.Current) / val;
                        }
                    }
                    else
                    {   // Zwykłe obiekty
                        return GetHash(el.ToString());
                    }
                    return 0;
                default:
                    return 0;
            }
        }

        public void Insert(T el)
        {
            if (Member(el))
                return;

            if (elCount >= lists.Length)
                ResizeArray(lists.Length * 2);

            int hash = GetHash(el);

            lists[hash].Add(el);
            elCount++;
        }

        public void Delete(T el)
        {
            if (elCount < lists.Length / 4 && lists.Length >= defaultArrayLenght * 2)
                ResizeArray(lists.Length / 2);

            int hash = GetHash(el);

            lists[hash].Remove(el);
            elCount--;
        }

        public bool Member(T el)
        {
            int hash = GetHash(el);
            
            return lists[hash].Exists(x => Equals(x, el));
        }

        public int[] GetArrayFill()
        {
            int[] fill = new int[lists.Length];

            for (int i = 0; i < lists.Length; i++)
                fill[i] = lists[i].Count();

            return fill;
        }

        private void ResizeArray(int newLength)
        {
            List<T>[] oldLists = lists;
            lists = CreateNewLists(newLength);
            elCount = 0; // Elementy są dodawane od nowa, zostaną na nowo policzone.

            foreach (var list in oldLists)
            {
                foreach (var el in list)
                {
                    Insert(el);
                }
            }
        }

        private List<T>[] CreateNewLists(int newLength)
        {
            var l = new List<T>[newLength];

            for (int i = 0; i < l.Length; i++)
                l[i] = new List<T>();

            return l;
        }
    }
}
