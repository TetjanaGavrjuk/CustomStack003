using System;
using System.Collections;

namespace CustomStack
{
    class CstmStack : IEnumerable
    {
        // начальный размер массива элементов
        private const long InitialLength = 3;

        private int[] Items;
        private int HeadIndx = -1;

        public int Count
        {
            get { return (HeadIndx + 1); }
        }

        #region constructors
        public CstmStack()
        {
            Items = new int[InitialLength];
        }

        public CstmStack(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            Items = new int[capacity];
          }
        #endregion


        #region Public Methods
        public void Push(int item)
        {
            // увеличить размер массива, если он заполнен более, чем на 80%
            if (FullnessPrc() > 80)
            {
                EnlargeArray();
            }
            HeadIndx++;
            Items[HeadIndx] = item;
        }

        public int Pop()
        {
            if (HeadIndx < 0)
            {
                throw new NotImplementedException(); // подобрать более подходящий класс исключения
            }
            //Возвращаем элемент из головы и передвигаем указатель головы
            return Items[HeadIndx--];
        }

        public int Peek()
        {
            if (HeadIndx < 0)
            {
                throw new NotImplementedException(); // подобрать более подходящий класс исключения
            }
            return Items[HeadIndx];
        }

        public void ShowItems()
        {
            for (int i = HeadIndx; i >= 0; i--)
            {
                Console.Write($"{Items[i]}  ");
            }
            Console.WriteLine("\n");
        }
        #endregion

        #region Private Methods
        private int FullnessPrc()
        {
            return (int) Math.Round( (decimal) ( (HeadIndx < 1 ? 1 : HeadIndx+1) / (Items.GetUpperBound(0) + 1) * 100), 0);
        }

        private void EnlargeArray()
        {
            //1.создать новый массив с размером вдвое большим, чем существующий
            int[] NewItems = new int[this.Items.Length*2];

            //2.скопировать в новый массив значения из старого
            for (int i = 0; i < this.Items.Length; i++)
            {
                NewItems[i] = this.Items[i];
            }

            //3.удалить старый массив (может быть просто полагаемся на сборщик мусора???)
            // ??

            //4.ссылку на новый массив засунуть в ссылку на старый- просто переприсваисваем переменные
            //  при этом переменная Items будет указывать на область памяти с НОВЫМ массивом
            this.Items = NewItems;
        }
        #endregion

        #region IEnumerable 
        public IEnumerator GetEnumerator()
        {
            return new CustomStackEnumerator(this);
        }
        #endregion

        #region IEnumerator 
        private class CustomStackEnumerator : IEnumerator
        {
            private readonly CstmStack Stack;
            private int CurrentIndex;

            public CustomStackEnumerator(CstmStack stack)
            {
                this.Stack = stack;
                this.Reset();
            }
            public void Reset()
            {
                CurrentIndex = this.Stack.HeadIndx + 1;
            }

            public bool MoveNext()
            {
                //Если позиция елемента, лежит в пределах длины массива
                if ((CurrentIndex > 0) & (CurrentIndex <= this.Stack.HeadIndx + 1))
                {
                    CurrentIndex--;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public int Current => this.Stack.Items[CurrentIndex];

            object IEnumerator.Current => this.Current;

            public void Dispose() { }
        }
        #endregion

    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            CstmStack MyStack = new CstmStack();
            // Stack myStack = new Stack();

            MyStack.Push(1);
            MyStack.Push(2);
            MyStack.Push(3);

            MyStack.ShowItems();

            //------------------------------------------

            // Проверяем работу перечислителя
            Console.WriteLine("----foreach-1:");

            foreach (int Item in MyStack)
            {
                Console.WriteLine($"Item: {Item}");
            }

            //------------------------------------------

            MyStack.Push(4);
            MyStack.Push(5);
            MyStack.Push(6);

            Console.WriteLine("----foreach-2:");
            foreach (int Item in MyStack)
            {
                Console.WriteLine($"Item: {Item}");
            }
            //------------------------------------------

            Console.ReadLine();
        }
    }
}
