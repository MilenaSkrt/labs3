using System.Collections;
namespace l3
{
    // Абстрактный базовый класс для реализации различных типов списков
    public abstract class Base_list <T> : IEnumerable<T> where T: IComparable<T>
    {
        // Поле для отслеживания количества элементов в списке
        protected int count;
        public delegate void ListChangedEventHandler(object sender, ActionEventArgs e);
        public event ListChangedEventHandler ItemAdded;
        public event ListChangedEventHandler ItemInserted;
        public event ListChangedEventHandler ItemDeleted;
        public event ListChangedEventHandler ListCleared;

        
        // Свойство для доступа к количеству элементов в списке

         public int Count 
        {
            get { return count; }
        }
                protected virtual void OnItemAdded(T item)
        {
            ItemAdded?.Invoke(this, new ActionEventArgs("Item Added"));
        }

        protected virtual void OnItemInserted(int pos, T item)
        {
            ItemInserted?.Invoke(this, new ActionEventArgs("Item Inserted at position " + pos));
        }

        protected virtual void OnItemDeleted(int pos)
        {
            ItemDeleted?.Invoke(this, new ActionEventArgs("Item Deleted from position " + pos));
        }

        protected virtual void OnListCleared()
        {
            ListCleared?.Invoke(this, new ActionEventArgs("List Cleared"));
        }

         public abstract void Add(T item);

        public abstract void Insert(int pos, T item);

        public abstract void Delete(int pos);

        public abstract void Clear();

        public abstract T this[int i] { get; set; }

        // Метод для вывода элементов списка на экран
        public void Print()
        {
            foreach (var item in this)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
        }

        // Метод для копирования всех элементов из другого списка в текущий список
        public void Assign(Base_list <T> source)
        {
            Clear();
            for (int i = 0; i < source.count; i++)
            {
                Add(source[i]);
            }
        }

        // Метод для копирования всех элементов из текущего списка в другой список
        public void AssignTo(Base_list <T> dest)
        {
            dest.Assign(this);
        }

        // Метод для создания клонированной копии текущего списка
        public Base_list <T> Clone()
        {
            Base_list <T> clone_list = EmptyClone();
            clone_list.Assign(this);
            return clone_list;
        }

        // Абстрактный метод для создания пустой копии списка
        protected abstract Base_list <T> EmptyClone();

        // Виртуальный метод для сортировки элементов списка (сортировка пузырьком)
        public virtual void Sort()
        {
            for (int i = 0; i < count; i+=2)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (this[i].CompareTo(this[j]) > 0)
                    {
                        T temp = this[i];
                        this[i] = this[j];
                        this[j] = temp;
                    }
                }
            }
        }

        // Метод для проверки равенства текущего списка с другим списком
        public bool IsEqual(Base_list <T>  AnotherOne)
        {
            if (AnotherOne == null)
                return false;

            if (this.Count != AnotherOne.Count)
                return false;

            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].CompareTo(AnotherOne[i]) != 0)
                    return false;
            }
            return true;
        }
         public void SaveToFile(string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    for (int i = 0; i < count; i++)
                    {
                        writer.WriteLine(this[i].ToString());
                    }
                }
            }
            catch (BadFileException)
            {
                ExceptionCounter.ArrayExceptionCounterIncrement();
                return;    
            }
        }
        
        public void LoadFromFile(string fileName)
        {
            try
            {
                Clear();
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Trim() == "")
                        {
                            T item = (T)Convert.ChangeType(line, typeof(T));
                            Add(item);
                        }
                    }
                }
            }
            catch (BadFileException)
            {
                ExceptionCounter.ArrayExceptionCounterIncrement();
                return;
            }
        }
        public static bool operator ==(Base_list<T> list1, Base_list<T> list2)
        {
            return list1.IsEqual(list2);
        }

        public static bool operator !=(Base_list<T> list1, Base_list<T> list2)
        {
            return!list1.IsEqual(list2);
        }
        public static Base_list<T> operator +(Base_list<T> list1, Base_list<T> list2)
        {
            /*Base_list<T> list3 = new Base_list<T>();
            list3.Assign(list1);
            list3.Assign(list2);
            return list3;*/
            
            Base_list<T> list3 = list1.Clone();
            for (int i = 0; i < list2.Count; i++)
            {
                list3.Add(list2[i]);
            }
            return list3;
        }
        public class ExceptionCounter
        {
            protected static int ChainExceptionCount = 0;
            protected static int ArrayExceptionCount = 0;
           // public static int ChainExceptionCounter => ChainExceptionCount;
           // public static int ArrayExceptionCounter => ArrayExceptionCount;
            public static int ChainExceptionCounter
            {
                get { return ChainExceptionCount; }
            }
            public static int ArrayExceptionCounter
            {
                get { return ArrayExceptionCount; }
            }
            public static void ArrayExceptionCounterIncrement()
            {
                ArrayExceptionCount++;
            }
            public static void ChainExceptionCounterIncrement()
            {
                ChainExceptionCount++;
            }
            /*public static void ExceptionCounterReset()
            {
                ChainExceptionCount = 0;
                ArrayExceptionCount = 0;
            }*/
        }

        public IEnumerator<T> GetEnumerator()
        {
            //return new Base_listEnumerator<T>(this);   
            return new Base_listEnumerator(this);
        }
        
         IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

         private class Base_listEnumerator : IEnumerator<T>
        {
            private Base_list<T> list;
            private int currentIndex = -1;

            public Base_listEnumerator(Base_list<T> list)
            {
                this.list = list;
            }

            public T Current => list[currentIndex];

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                currentIndex++;
                return currentIndex < list.Count;
            }

            public void Reset()
            {
                currentIndex = -1;
            }

            public void Dispose()
            {
            }
        }
        }
    }
        public class BadIndexException : Exception
        {
            public BadIndexException() : base("exception")
            {
            }
        }       

        public class BadFileException : Exception
        {
            public BadFileException() : base("exception")
            {
            }
        }

        public class ActionEventArgs : EventArgs
        {
            public string Action { get; private set; }

            public ActionEventArgs(string action)
            {   
                Action = action;
            }
        }        
