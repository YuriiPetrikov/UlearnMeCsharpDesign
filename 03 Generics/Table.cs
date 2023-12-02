// Вставьте сюда финальное содержимое файла Table.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.Tables
{
    public class Table<T1, T2, T3>
    {
        private readonly Dictionary<Tuple<T1, T2>, T3> dictionary;
        public List<T1> Rows { get; }
        public List<T2> Columns;
        public ExistedClass Existed;
        public OpenClass Open;

        public Table()
        {
            dictionary = new Dictionary<Tuple<T1, T2>, T3>();
            Rows = new List<T1>();
            Columns = new List<T2>();
            Existed = new ExistedClass(this);
            Open = new OpenClass(this);
        }

        public void AddRow(T1 t1)
        {
            if(!Rows.Contains(t1))
                Rows.Add(t1);
        }

        public void AddColumn(T2 t2)
        {
            if(!Columns.Contains(t2))
                Columns.Add(t2);
        }

        public class ExistedClass
        {
            private readonly Table<T1, T2, T3> table;
            private Tuple<T1, T2> Key;
            public ExistedClass(Table<T1, T2, T3> table)
			{
                this.table = table;
			}

            public T3 this[T1 t1, T2 t2]
            {
                set
                {
                    Key = Tuple.Create(t1, t2);
                    if (!table.Rows.Contains(t1) || !table.Columns.Contains(t2))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        table.dictionary[Key] = value;
                    }
                }
                get
                {
                    Key = Tuple.Create(t1, t2);
                    if (table.Rows.Contains(t1) && table.Columns.Contains(t2))
                    {
                        if (table.dictionary.ContainsKey(Key))
                            return table.dictionary[Key];
                        else
                            return default;
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
            }
        }

        public class OpenClass
        {
            private readonly Table<T1, T2, T3> table;
            private Tuple<T1, T2> Key;

            public OpenClass(Table<T1, T2, T3> table)
			{
                this.table = table;
			}

            public T3 this[T1 t1, T2 t2]
            {
                set
                {
                    Key = Tuple.Create(t1, t2);
                    if (table.dictionary.ContainsKey(Key))
                    {
                        table.dictionary[Key] = value;
                    }
                    else
                    {
                        if (!table.Rows.Contains(t1))
                            table.AddRow(t1);
                        if (!table.Columns.Contains(t2))
                            table.AddColumn(t2);
                        table.dictionary.Add(Key, value);
                    }
                }
                get
                {
                    Key = Tuple.Create(t1, t2);
                    if (table.dictionary.ContainsKey(Key))
                        return table.dictionary[Key];
                    else
                        return default;
                }
            }
        }     
    }
}
