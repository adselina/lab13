using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ClassLibrary;
using List;

namespace Tree
{
    public class SearchTree<T> : IEnumerable
    {
        public SearchPoint<Person> root = null; //корень

        public SearchTree()
        {
            root = null;
        }                          //пустой конструктор
        public SearchTree(int size)
        {
            Person p = new Person();
            
            
            root = new SearchPoint<Person>(p); //первый элемент
            for (int i = 1; i < size; i++)
            {
                p = new Person();
                Add(p);
            }

        }                  //дерево поиска(размер коллекции)
        public SearchTree(params Person[] arr)
        {
            if (arr.Length == 0)
            {
                Console.WriteLine("введите корень");
            }
            else
            {
                root = new SearchPoint<Person>(arr[0]);//первый элемент
                for (int i = 1; i < arr.Length; i++)
                {
                    Add(arr[i]);
                }
            }
        }       //дерево поиска(массив значений)

        public virtual void Add(Person d)
         {
            if (this.root == null)
            {
                this.root = new SearchPoint<Person>(d);
                return;
            }
            if (d == null)
            {
                return;
            }
            SearchPoint<Person> p = this.root;
            SearchPoint<Person> r = null;
            bool ok = false;
            while (p != null && !ok)
            {
                r = p;
                if (d.CompareTo(p.data) == 0)
                    ok = true;
                else
               if (d.CompareTo(p.data) < 0) p = p.left;
                else p = p.right;
            }
            if (ok) return;
            SearchPoint<Person> NewPoint = new SearchPoint<Person>(d);
            if (d.CompareTo(r.data) < 0) r.left = NewPoint;
            else r.right = NewPoint;
            return;

        } //добавление элемента в дерево поиска
        public void Add(SearchPoint<Person> root, Person[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Add(arr[i]);
            }
        }    //добавление массива элементов в дерево

        private void ShowTree(SearchPoint<Person> p, int l)
        {
            if (p != null)
            {
                ShowTree(p.left, l + 3);
                for (int i = 0; i < l; i++)
                    Console.Write("-");
                Console.WriteLine(p.data);
                ShowTree(p.right, l + 3);
            }
        }        //печать поддерьвьев
        public void Show()
        {
            if (root == null)
            {
                Console.WriteLine("Пустое дерево");
            }
            else
                ShowTree(root, 5);
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }                           //печать всего дерева

        public SearchTree<Person> Clone()
        {
            List<Person> list = new List<Person>();
            
            //Нерекурсивный алгоритм
            //Обход в ширину 

            var queue = new Queue<SearchPoint<Person>>(); // создать новую очередь

           queue.Enqueue(this.root); // поместить в очередь первый уровень

            while (queue.Count != 0) // пока очередь не пуста
            {
                //если у текущей ветви есть листья, их тоже добавить в очередь
                if (queue.Peek().left != null)
                {
                    queue.Enqueue(queue.Peek().left);
                }
                if (queue.Peek().right != null)
                {
                    queue.Enqueue(queue.Peek().right);
                }

                list.Add(queue.Dequeue().data);  // убрать последний элемент очереди и занести его в массив
               
            }

            SearchTree<Person> newTree = new SearchTree<Person>(list.ToArray()); 

            return newTree;
        }       //клон дерева
        public SearchTree<T> ShallowCopy()
        {
            SearchTree<T> newTree = new SearchTree<T>();
            newTree = this;
            return newTree;
        }                                        //копия дерева

        public int Count()
        {
            int i = 0;
            foreach (Person item in this)
            {
                i++;
            }
            return i;
        }
        public void RemoveCollection()
        {
            this.root = null;
        }


        public bool Contains(Person value)
        {
            SearchPoint<Person> parent;
            return FindElementAndParent(value, out parent) != null;
        }
        private SearchPoint<Person> FindElementAndParent(Person value, out SearchPoint<Person> parent)
        {
            // Попробуем найти значение в дереве.
            SearchPoint<Person> current = root;
            parent = null;

            // До тех пор, пока не нашли
            while (current != null)
            {
                int result = current.data.CompareTo(value);

                if (result == 1)
                {
                    // Если искомое значение меньше, идем налево.
                    parent = current;
                    current = current.left;
                }
                else if (result == -1)
                {
                    // Если искомое значение больше, идем направо.
                    parent = current;
                    current = current.right;
                }
                else
                {
                    // Если равны, то останавливаемся
                    break;
                }
            }

            return current;
        }
        public virtual bool Remove(Person value)
        {
            SearchPoint<Person> current, parent;

            // Находим удаляемый узел.
            current = FindElementAndParent(value, out parent);

            if (current == null)
            {
                return false;
            }

            // Случай 1: Если нет потомка справа, левый потомок встает на место удаляемого.
            if (current.right == null)
            {
                if (parent == null)
                {
                    root = current.left;
                }
                else
                {
                    int result = parent.data.CompareTo(current.data);
                    if (result > 0)
                    {
                        // Если значение предка больше текущего,
                        // левый потомок текущего узла становится левым потомком предка.
                        parent.left = current.left;
                    }
                    else if (result < 0)
                    { 
                        // Если значение предка меньше текущего, 
                        // левый потомок текущего узла становится правым потомком предка. 
                        parent.right = current.left; 
                    } 
                } 
            }

            // Случай 2: Если нет потомка слева, правый потомок встает на место удаляемого. 
            else if (current.right.left == null) 
            { 
                current.right.left = current.left;
                if (parent == null)
                { root = current.right; }
                else
                {
                    int result = parent.data.CompareTo(current.data);
                    if (result > 0)
                    {
                        // Если значение предка больше текущего,
                        // левый потомок текущего узла становится левым потомком предка..
                        parent.left = current.right;
                    }
                    else if (result < 0)
                    { 
                        // Если значение предка меньше текущего, 
                        // левый потомок текущего узла становится правым потомком предка.  
                        parent.right = current.right;               
                    } 
                } 
            } 


            // Случай 3: Если у правого потомка есть дети слева, крайний левый ребенок из правого поддерева заменяет удаляемый узел. 
            else 
            { 
                // Найдем крайний левый узел. 
                SearchPoint<Person> leftmost = current.right.left;
                SearchPoint<Person> leftmostParent = current.right; 
                while (leftmost.left != null) 
                { 
                    leftmostParent = leftmost; 
                    leftmost = leftmost.left; 
                } 
                // Левое поддерево родителя становится правым поддеревом крайнего левого узла. 
                leftmostParent.left = leftmost.right; 

                // Левый и правый ребенок текущего узла становится левым и правым ребенком крайнего левого. 
                leftmost.left = current.left; 
                leftmost.right = current.right; 

                if (parent == null) { root = leftmost; }    //если корень - удаляемый элемент

                else { int result = parent.data.CompareTo(current.data); 
                    if (result == 1)
                            {
                                // Если значение родителя больше текущего,
                                // крайний левый узел становится левым ребенком родителя.
                                parent.left = leftmost;
                            }
                    else if (result == -1)
                            {
                                // Если значение родителя меньше текущего,
                                // крайний левый узел становится правым ребенком родителя.
                                parent.right = leftmost;
                            }
                        }
                    }

                    return true;
                }

        public virtual bool Change(Person value, int age)
        {
            // Попробуем найти значение в дереве.
            SearchPoint<Person> current = root;
            bool flag = false;
            // До тех пор, пока не нашли
            while (current != null)
            {
                int result = current.data.CompareTo(value);

                if (result == 1)
                {
                    // Если искомое значение меньше, идем налево.
                    current = current.left;
                }
                else if (result == -1)
                {
                    // Если искомое значение больше, идем направо.
                    current = current.right;
                }
                else
                {
                    // Если равны, то останавливаемся и заменяем возраст
                    current.data.Age = age;
                    flag = true;
                    break;
                }
            }
            
            return flag;
        }
        
        //обобщенный для коллекций
       
        public IEnumerator<Person> GetEnumerator()
        {
            //Нерекурсивный алгоритм
            //Обход в ширину 

            var queue = new Queue<SearchPoint<Person>>(); // создать новую очередь

            queue.Enqueue(root); // поместить в очередь первый уровень

            while (queue.Count != 0) // пока очередь не пуста
            {
                //если у текущей ветви есть листья, их тоже добавить в очередь
                if (queue.Peek().left != null)
                {
                    queue.Enqueue(queue.Peek().left);
                }
                if (queue.Peek().right != null)
                {
                    queue.Enqueue(queue.Peek().right);
                }

                yield return queue.Dequeue().data; // убрать последний элемент очереди 
            }
        }


        //необобщенный нумератор 
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
