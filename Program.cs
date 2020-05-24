using System;
using ClassLibrary;
using System.Collections;
using Tree;
using System.Collections.Generic;



namespace lab13
{
    class Programm
    {
        static void Main(string[] args)
        {


            MyNewCollection mc1 = new MyNewCollection("FIRST", 3);
            MyNewCollection mc2 = new MyNewCollection("SECOND", 5);

            //один объект Journal подписать на события CollectionCountChanged и CollectionReferenceChanged из первой коллекции
            Journal joun1 = new Journal();
            mc1.CollectionCountChanged += new CollectionHandler(joun1.CollectionCountChanged);
            mc1.CollectionReferenceChanged += new CollectionHandler(joun1.CollectionReferenceChanged);


            //второй объект Journal подписать на события CollectionReferenceChanged из обеих коллекций. 
            Journal joun2 = new Journal();
            mc1.CollectionReferenceChanged += new CollectionHandler(joun2.CollectionReferenceChanged);
            mc2.CollectionReferenceChanged += new CollectionHandler(joun2.CollectionReferenceChanged);


            //------------
            mc1.Add(new Person("1", 1));
            mc1.Add(new Person("2", 2));
            mc1.Add(new Person("все", 1));

            Person remove = new Person("55", 55);

            mc1.Remove(remove);
            //------------


            mc2.Add(new Person("66", 55));

            Person changeAge = new Person("2", 2);
            mc2.Change(changeAge, 4);


            Console.WriteLine("________ЖУРНАЛ 1 КОЛЛЕКЦИИ__________");
            joun1.Show();
            Console.WriteLine("________ЖУРНАЛ 2 КОЛЛЕКЦИИ__________");
            joun2.Show(); 
        }
    }



    public delegate void CollectionHandler(object source, CollectionHandlerEventArgs args); //   ДЕЛЕГАТ

    public class CollectionHandlerEventArgs : System.EventArgs
    {
        public string NameCollection { get; set; }
        public string ChangeCollection { get; set; }
        public object Obj { get; set; }

        public CollectionHandlerEventArgs()
        {
            NameCollection = null;
            ChangeCollection = null;
            Obj = default;
        }

        public CollectionHandlerEventArgs(string colName, string changetype, object p)
        {
            NameCollection = colName;
            ChangeCollection = changetype;
            Obj = p;
        }

        public override string ToString()
        {
            return "Коллекция: " + NameCollection + ", " + ChangeCollection + " следующий элемент: " + Obj.ToString();
        }

    }


    //типо моя коллекция
    public class MyNewCollection: SearchTree<Person> 
    {
        SearchTree<Person> tree = new SearchTree<Person>();
        string Name { get; set; }

        public MyNewCollection()
        {
            Name = null; 
        }
        public MyNewCollection(string colName, int size)
        {
            Name = colName;
            tree = new SearchTree<Person>(size);
        }


        //удаление объекта
        public override bool Remove(Person person)
        {
            if (base.Remove(person))
            {
                OnCollectionCountChanged(this, new CollectionHandlerEventArgs(this.Name, "УДАЛЕН", person));
                return true;
            }
            else
            {
                return false;
            }
        }

        //добавление объекта
        public override void Add(Person person)
        {
            OnCollectionCountChanged(this, new CollectionHandlerEventArgs(this.Name, "ДОБАВЛЕН", person));
            base.Add(person);
        }

        //изменение значения
        public override bool Change(Person person, int age)
        {
            if (base.Change(person, age))
            {
                OnCollectionReferenceChanged(this, new CollectionHandlerEventArgs(this.Name, $"ПРИСВОЕНО НОВОЕ ЗНАЧЕНИЕ ВОЗРАСТА ({age})", person));
                return true;
            }
            else
            {
                return false;
            }
        }




        //происходит при добавлении нового элемента или при удалении элемента из коллекции
        public event CollectionHandler CollectionCountChanged;

        //объекту коллекции присваивается новое значение       
        public event CollectionHandler CollectionReferenceChanged;

        


        //обработчик события CollectionCountChanged
        public virtual void OnCollectionCountChanged(object source, CollectionHandlerEventArgs args)
        {
            if (CollectionCountChanged != null)
                CollectionCountChanged(source, args);
        }
        //обработчик события OnCollectionReferenceChanged
        public virtual void OnCollectionReferenceChanged(object source, CollectionHandlerEventArgs args)
        {
            if (CollectionReferenceChanged != null)
                CollectionReferenceChanged(source, args);
        }
    }

    //Записи для журнала
    public class JournalEntry
    {

        string NameCollection { get; set; }
        string ChangeCollection { get; set; }
        object Obj { get; set; }

        public JournalEntry()
        {
            NameCollection = null;
            ChangeCollection = null;
            Obj = default;
        }

        public JournalEntry(string colName, string changetype, object p)
        {
            NameCollection = colName;
            ChangeCollection = changetype;
            Obj = p;
        }

        public override string ToString()
        {
            return "Коллекция: " + NameCollection + ", " + ChangeCollection + " следующий элемент: " + Obj.ToString();
        }
    }

    //Журнал в котором сохраняются все записи об изменениях в моей коллекции
    public class Journal
    {
        private List<JournalEntry> journal = new List<JournalEntry>();

        public void CollectionCountChanged(object sourse, CollectionHandlerEventArgs e)
        {
            JournalEntry je = new JournalEntry(e.NameCollection, e.ChangeCollection, e.Obj.ToString());
            journal.Add(je);

        }
        public void CollectionReferenceChanged(object sourse, CollectionHandlerEventArgs e)
        {
            JournalEntry je = new JournalEntry(e.NameCollection, e.ChangeCollection, e.Obj.ToString());
            journal.Add(je);
        }


        public void Show()
        {
            foreach (JournalEntry item in journal)
                Console.WriteLine(item + "\n" );
        }
        
    }


}
