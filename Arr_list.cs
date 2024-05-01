namespace l3
{
    // Класс Arr_list, который расширяет функциональность абстрактного класса Base_list
    public class Arr_list<T> : Base_list<T> where T : IComparable<T>
    {
        private T[] buffer; // Массив для хранения элементов списка

        // Конструктор класса Arr_list
        public Arr_list()
        {
            buffer = new T[0]; // Инициализация массива нулевой длины
            count = 0; // Инициализация счетчика элементов списка
        }

        // Метод для увеличения размера массива при необходимости
        private void Expand()
        {
            if (count == buffer.Length)
            {
                T[] newBuffer;
                if (buffer.Length == 0)
                {
                    newBuffer = new T [2];
                }
                else
                {
                    newBuffer = new T[buffer.Length * 2];
                }

                // Копирование элементов из старого массива в новый
                for (int i = 0; i < buffer.Length; i++)
                {
                    newBuffer[i] = buffer[i];
                }
                buffer = newBuffer; // Присвоение нового массива в качестве буфера
            }
        }

        // Переопределение метода добавления элемента в конец списка
        public override void Add(T item)
        {
            Expand(); // Увеличение размера массива при необходимости
            buffer[count] = item; // Добавление элемента в конец массива
            count++; // Увеличение счетчика элементов списка
        }

        // Переопределение метода вставки элемента по указанной позиции
         public override void Insert(int pos, T item)
        {
            try
            {
                if (pos < 0 || pos > count)
                {
                    ExceptionCounter.ArrayExceptionCounterIncrement();
                    return;
                }

                Expand();

                for (int i = count; i > pos; i--)
                {
                    buffer[i] = buffer[i - 1];
                }

                buffer[pos] = item;
                count++;
            }
            catch(BadIndexException){
                ExceptionCounter.ArrayExceptionCounterIncrement();
                return;
            }
        }

        // Переопределение метода удаления элемента по указанной позиции
        public override void Delete(int pos)
        {
            try{
                if (pos < 0 || pos >= count)
                {
                    ExceptionCounter.ArrayExceptionCounterIncrement();
                    return;
                }

                for (int i = pos; i < count - 1; i++)
                {
                    buffer[i] = buffer[i + 1];
                }

                count--;
            }
            catch(BadIndexException){
                ExceptionCounter.ArrayExceptionCounterIncrement();
                return;
            }
        }
        // Переопределение метода очистки списка
       public override void Clear()
        {
            buffer = new T[0];
            count = 0;
            OnListCleared();
        }

        // Переопределение индексатора для доступа к элементам списка
        public override T this[int index]
        {
            get
            {
                    if (index < 0 || index >= count)
                {
                    throw new BadIndexException();
                }
                return buffer[index]; // Возврат элемента по индексу
            }
            set
            {
                    if (index < 0 || index >= count)
                    {
                        throw new BadIndexException();
                    }
                    buffer[index] = value;

                }
            }


        // Переопределение метода для создания пустой копии списка
        protected override Base_list<T> EmptyClone()
        {
            return new Arr_list<T>();
        }
        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < count; i++)
            {
                str += buffer[i] + " ";
            }
            return str;
            // return string.Join(" ", buffer);
        }
    }
}
