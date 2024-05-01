using System;

namespace l3
{
    // Класс Program для тестирования функциональности списков
    class Program
    {
        // Метод Main, точка входа в программу
        static void Main(string[] args)
        {
            // Создание экземпляров Arr_list и Arr_chain_list
           int arr_count_except = 0;
            int chain_count_except = 0;
            Base_list<char> array = new Arr_list<char>();
            Base_list<char> chain = new Arr_chain<char>();
            Random rnd = new Random();

            // Генерация случайных операций над списками
            for (int i = 0; i < 10000; i++)
            {
                int operation = rnd.Next(5);
                char item = (char)('a' + rnd.Next(0, 30));
                int pos = rnd.Next(100);
                switch (operation)
                {
                    case 0:
                        array.Add(item);
                        chain.Add(item);
                        break;
                    case 1:
                    try
                        {
                            array.Delete(pos);
                        }
                        catch (BadIndexException)
                        {
                            arr_count_except++;
                        }
                        try
                        {
                            chain.Delete(pos);
                        }
                        catch (BadIndexException)
                        {
                            chain_count_except++;
                        }
                        break;
                    case 2:
                        try
                        {
                            array.Insert(pos, item);
                        }
                        catch (BadIndexException)
                        {
                            arr_count_except++;
                        }
                        try
                        {
                            chain.Insert(pos, item);
                        }
                        catch (BadIndexException)
                        {
                            chain_count_except++;
                        }
                        break;
                    case 3:
                        array.Clear();
                        chain.Clear();
                        break;
                    case 4:
                        try
                        {
                            array[pos] = item;
                        }
                        catch (BadIndexException)
                        {
                            arr_count_except++;
                        }
                        try
                        {
                            chain[pos] = item;
                        }
                        catch (BadIndexException)
                        {
                            chain_count_except++;
                        }
                        break;
                }
            }

            bool flag = true;
            if (array.Count == chain.Count)
            {
                for (int i = 0; i < chain.Count; i++)
                {
                    if (array[i] != chain[i])
                    {
                        Console.WriteLine("Test error");
                        flag = false;
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Test error");
                flag = false;
            }
            if (flag == true)
            {
                Console.WriteLine("Test successfull");
            }
            Console.WriteLine(arr_count_except);
            Console.WriteLine(chain_count_except);   
    }
    }        
}
