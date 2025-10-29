using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO; // for output 
using System.Data.SqlClient; // for SQL server
using System.Diagnostics;
using System.Xml;

namespace Classes
{

    class MyClass
    {
        protected int x;
        protected int y;
    }

    class MyDerivedC : MyClass
    {

        FileStream fs_OutputFile;
        StreamWriter m_oOutputFile;
        private int i;
        double d;   // private access by default Must be Declared outside of Main.
        private double salary = 100.0;

        protected int InstantiatedAddTwoNumbers(int a, int b)
        {
            return a + b;
        }

        public delegate int DeladdTwoNumbers(int a, int b);  // THIS IS A DELEGATE DECLARATION 

        static int addTwoNumbers(int a, int b)
        {
         //   int c = 0;
            return a + b;        
        }

        // A DELEGATE ENCAPSULATES A FUNCTION 
        
//        static void Main(string[] args)
        static int Main()
        {
            MyDerivedC Skel = new MyDerivedC();

            Skel.d = 55;
            Skel.x = 100;
            Skel.y = 300;

            Console.WriteLine(" writing a protected var {0} ", Skel.y);

        //    Skel.InstantiatedAddTwoNumbers

            DeladdTwoNumbers Dadd = new DeladdTwoNumbers(addTwoNumbers);

            DeladdTwoNumbers DaddInstantiated = new DeladdTwoNumbers(Skel.InstantiatedAddTwoNumbers);

          //  DeladdTwoNumbers Dadd;

            int result = 0;

            result = Dadd(Skel.x, Skel.y);

            Console.WriteLine(" Using a delegate for teh first time " + result);

            result = DaddInstantiated(3,105); /// this is a DELEGATE FOR AND INSTANTIATED FUNCTION.. 

            Console.WriteLine(" Using a delegate instantiated woo hoo " + result);


            // Since Program Skel is derived from MyClass
         //   MyDerivedC mC = new MyDerivedC();

            // Direct access to protected members: THESE ARE PROTECTED CAN ONLY BE USED
            // BY DERIVED CLASSES !!!!

            Skel.parseAString();

            nick n = new nick();

            n.nickCount();

         //   Nick3 n3 = new Nick3(); cannot derive and abstract class

            


            nickDerived nd = new nickDerived();

            nd.nickCount();
            Console.WriteLine("next line");
            nd.nickCountBackwards();





            return 0;

            Skel.nickReadAFile();

            Skel.x = 10;
            Skel.y = 15;
            Console.WriteLine("x = {0}, y = {1}", Skel.x, Skel.y);

            
            //Other types of collection classes include map, tree, and stack; each one has its own advantages.//
            int t = 4;
  //          int v = 6;
            Console.WriteLine("NIck was here return stops code execute ");

            t = Skel.NicksIntMethodTest();

            Skel.runSQLConnection();
            Skel.OpenOutputFiles();

            Skel.nicksXML();


            object obj =100;
            object objStr = null;

            if (obj is Provider)
                Console.WriteLine(" Provider object found ");
            else
                Console.WriteLine("this object is not a Provider object");



            int o = (int)obj;

            if (objStr == null)
                Console.WriteLine("object is NULL");

                


            Skel.nicksCasting(o);

            Skel.nickNumbers();

            Skel.nicksArrays();



            Console.WriteLine(" object {1} number {0} ", t, "sarah");
            Console.WriteLine(" object {0} number {1} ", t, "sarah");
            //            Console.WriteLine(" object {0} ", t );


            nick ni = new nick();

            ni.Number1 = 55;

            

            Provider pr = new Provider();
            pr.Address1 = "41 rosemont drive";
            pr.Name = "nick";

            Console.WriteLine(" addy {0} name {1}", pr.Address1, pr.Name);

       

   //         return 0;            /// this return stopps all the code



            Provider[] prov = new Provider[10000];  //create an array of providers.

            for (int i = 0; i < 10; i++)
            {
                //   Provider[]  p[i] = new Provider();

                prov[i] = new Provider();

         
                prov[i].ProviderId = 1;
                prov[i].Name = "Dr. Smith";
                prov[i].Question1 = 3;
                prov[i].Address1 = "1019 Bailey Avenue";

               //       Console.WriteLine("provider id {0}, provider name {1}", p.ProviderId, p.Name);

                Providers ps = new Providers();
                //                ps.Add(p);

                Console.WriteLine("providers {0}, provider id {1}, provider addy {2}, provider question {3} ", prov[i].Name, prov[i].ProviderId,prov[i].Address1 ,prov[i].Question1 );

                Console.WriteLine("object number {0}", i);
              

            }
            int total = 0;
            for (int j = 0; j < 10; j++)
            {
                total += prov[j].Question1;
            }
            Console.WriteLine("total of all question 1's {0}", total);
            //    Provider pr = new Provider();

            //     foreach (int j in ps  )
            //   {
            //     Console.WriteLine(" here we go {0}, {1} ", Providers );
            //  }

            List<Person> people = new List<Person>();  // using a list to add new objects of Person

            people.Add(new Person(50, "Fred"));   /// adding to an array !!! 
            people.Add(new Person(30, "John"));
            people.Add(new Person(26, "Andrew"));
            people.Add(new Person(24, "Xavier"));
            people.Add(new Person(5, "Mark"));
            people.Add(new Person(6, "Cameron"));

            Console.WriteLine("Unsorted list");
            people.ForEach(delegate(Person p) { Console.WriteLine(String.Format("{0} {1}", p.age, p.name)); });

            Console.WriteLine(" count of all array {0}", people.Count);

            List<Person> young = people.FindAll(delegate(Person p) { return p.age < 25; });
            Console.WriteLine("Age is less than 25");
            young.ForEach(delegate(Person p) { Console.WriteLine(String.Format("{0} {1}", p.age, p.name)); });

            Console.WriteLine("Sorted list, by name");
            people.Sort(delegate(Person p1, Person p2) { return p1.name.CompareTo(p2.name); });
            people.ForEach(delegate(Person p) { Console.WriteLine(String.Format("{0} {1}", p.age, p.name)); });

            people.Sort(delegate(Person p1, Person p2) { return p1.age.CompareTo(p2.age); });
            Console.WriteLine("Sorted list, by age");
            people.ForEach(delegate(Person p) { Console.WriteLine(String.Format("{0} {1}", p.age, p.name)); });

            //   people.Find(1, "Fred");

            //   List<t> strings = new List<Strings>();
            /*
                        Student s = new Student();
                        studentBody[0] = s;
                        // Reuse s!
                        s = new Student();
                        studentBody[1] = s;
              */


            ChildClass child = new ChildClass(); // this shows how to derive a class!

            child.print();

            return 1;
        }  //////////////////////////// END MAIN... 
    
    
    int NicksIntMethodTest()
    {
    


      return 777;   
        
       
    
    }
        void runSQLConnection() {
            try
            {
                //  strConnection = _T("Driver={SQL Server};Server=MyServerName;Database=myDatabaseName;Uid=;Pwd=;");

                SqlConnection dataConnection = new SqlConnection();

                //       dataConnection.ConnectionString = "Integrated Security = true; Initial Catalog= NorthwindCS; Data Source =NICK-PC\\SQLEXPRESS;";
                dataConnection.ConnectionString = "Integrated Security = true; Initial Catalog= Northwind; Data Source =BLUE-CAA985E75B\\SQLEXPRESS;";

                dataConnection.Open();
                SqlCommand dataCommand = new SqlCommand();
                dataCommand.Connection = dataConnection;

                dataCommand.CommandText = "SELECT OrderId, CustomerId FROM ORDERS WHERE ORDERID = 10611 ";
                //                dataCommand.CommandText = "SELECT OrderId, CustomerId FROM ORDERS  ";

                SqlDataReader dataReader = dataCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    int oid = dataReader.GetInt32(0);
                    string cid = dataReader.GetString(1);

                      Console.WriteLine("Order id {0} Order {1}\n", oid, cid);
                   // textBox1.Text = cid;

                }

                dataReader.Close();
                dataConnection.Close();

                //  }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error has occured dummy  -->> " + ex.Message);

            }
            finally
                {
                    Console.WriteLine("Yo Adrian !!!! We Read the DATABASE \n");

                }
        }

      
    static void nick (){
        Console.WriteLine("does nothing");
    }

        void OpenOutputFiles() { 

            string m_outputFile = "c:\\temp\\nicksCrap_01.txt";
            int m = 55;
            string n2 = "nick was here";

        //    fs_OutputFile = new FileStream(m_outputFile, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            fs_OutputFile = new FileStream(m_outputFile, FileMode.Create, FileAccess.Write, FileShare.None);
            m_oOutputFile = new StreamWriter(fs_OutputFile);

            for (int n = 0; n < 5; n++)
            {
                m_oOutputFile.WriteLine(n + " " + n2);
                Console.WriteLine(n+ " " + n2);

            }

            if (m_oOutputFile != null)
                m_oOutputFile.Close();

            System.Console.WriteLine("numerics {0} " , m);  // HOW TO WRITE TO CONSOLE WITHOUT STREAM WRITER

        }



        void nicksCasting(object obj)
        {

            double x = 1234.7;
            int a;
            a = (int)x;  // cast double to int
            System.Console.WriteLine(a);  // HOW TO WRITE TO CONSOLE WITHOUT STREAM WRITER


          

         //   obj = 100;

            if (obj is int)
            {
                int i = (int)obj;
                i.ToString();
                Console.WriteLine(" obj " + i);

            }

        }
        string PadLeftLimitString(string pstrStr, int pnSize, char pchPad)
        {
            string lstrRes;

            if (pstrStr.Length < pnSize)
                lstrRes = pstrStr.PadLeft(pnSize, pchPad);
            else
                lstrRes = pstrStr.Substring(0, pnSize);

            return lstrRes;
        }

        string PadRightLimitString(string pstrStr, int pnSize, char pchPad)
        {
            string lstrRes;

            if (pstrStr.Length < pnSize)
                lstrRes = pstrStr.PadRight(pnSize, pchPad);
            else
                lstrRes = pstrStr.Substring(0, pnSize);

            return lstrRes;
        }

        void nickNumbers()
        {
            int j = 0;
            j++;
            j += 15;
            j -= 3;
            j *= 4;
            j += 3;
            Console.WriteLine("nick Numbers " + j );


        
        //    j = checked(j*2);
            
            //DateTime today = new DateTime();
            DateTime now = DateTime.Now;
       

            DateTime today = DateTime.Today;


            DateTime tommorrow = today.AddDays(1);


            Console.WriteLine("   Now         " + now);
            Console.WriteLine("   today        " + today);
            Console.WriteLine("   tommorrow       " + tommorrow);



            switch (j) { 
                case 0:
                    Console.WriteLine(" Case Zero");
                    break;
                case 55:
                    Console.WriteLine(" Case ffifty five");
                    break;
                default:
                    break;
            
            }


            for (int f=0; f <= 10; f++)
                Console.WriteLine(" counts {0} times ", f);
            int g = 0;

            while (g <= 10)
            {
                Console.WriteLine(" counts {0} times ", g);
                g++;
            }


            
        }

        void nicksArrays()
        {
        
       //     int[] iarr = new int[15];
            //OR FOR DEFINING ARRAYS

            int[] iarr2 = { 1, 2, 4, 5 };

            foreach(int prop in iarr2)
            {
                Console.WriteLine( "array --> " + prop );
            }

            for (int g = 0; g < iarr2.Length; g++)
            {
                Console.WriteLine(" you got it " + iarr2[g]);
            
            }

            /* Hashtable array */

            Hashtable ages = new Hashtable();

            ages["John"] = 41;
            ages["Diana"] = 42;
            ages["James"] = 13;
            ages["Francesca"] = 11;


            foreach (DictionaryEntry element in ages)
            {

                string name = (string)element.Key;
                int age = (int)element.Value;
                Console.WriteLine(" Hash Table name {0} and age {1} ", name, age);

            }


            /* Sorted List SORTED BY NAME NOT AGE !!!!  */

            SortedList ages2 = new SortedList();

            ages2["John"] = 41;
            ages2["Diana"] = 42;
            ages2["James"] = 13;
            ages2["Francesca"] = 11;


            foreach (DictionaryEntry element in ages2)
            {

                string name = (string)element.Key;
                int age = (int)element.Value;
                Console.WriteLine(" Sorted List BY NAME name {0} and age {1} ", name, age);

            }

            /*
            List<cds> cd = new List<cds>();
            cd.Add(new cd(20, "zepplin"));
            cd.Add(new cd(20, "beatles"));
            cd.Add(new cd(20, "who"));
            */



         //   foreach(cd in cds)
         //   {
           //     Console.WriteLine("Cds in collecion here --->>> " , cd)
           // }


                 List<Person> people = new List<Person>();  // using a list to add new objects of Person

            people.Add(new Person(50, "Fred"));   /// adding to an array !!! 
            people.Add(new Person(30, "John"));
            people.Add(new Person(26, "Andrew"));
            people.Add(new Person(24, "Xavier"));
            people.Add(new Person(5, "Mark"));
            people.Add(new Person(6, "Cameron"));

            Console.WriteLine("Unsorted list");
            people.ForEach(delegate(Person p) { Console.WriteLine(String.Format("{0} {1}", p.age, p.name)); });

            Console.WriteLine(" count of all array {0}", people.Count);

            List<Person> young = people.FindAll(delegate(Person p) { return p.age < 25; });
            Console.WriteLine("Age is less than 25");
            young.ForEach(delegate(Person p) { Console.WriteLine(String.Format("{0} {1}", p.age, p.name)); });

            Console.WriteLine("Sorted list, by name");
            people.Sort(delegate(Person p1, Person p2) { return p1.name.CompareTo(p2.name); });
            people.ForEach(delegate(Person p) { Console.WriteLine(String.Format("{0} {1}", p.age, p.name)); });

            people.Sort(delegate(Person p1, Person p2) { return p1.age.CompareTo(p2.age); });
            Console.WriteLine("Sorted list, by age");
            people.ForEach(delegate(Person p) { Console.WriteLine(String.Format("{0} {1}", p.age, p.name)); });




        }


       void nicksXML(){

           using (XmlWriter xw = XmlWriter.Create(@"C:\\temp\\names.xml"))
           {
           xw.WriteRaw("<names>");
           xw.WriteRaw("<name>Bob</name>");
           xw.WriteRaw("<name>David</name>");
           xw.WriteRaw("</names>");

       }

    //       XmlDocument doc = new XmlDocument();
      //     doc.Load(@"C:\\temp\\names.xml");
        //   foreach( XmlNode in doc.SelectNodes("//name/text()"))
       //    {
           
       //    Console.WriteLine(node.value);
       //    }


        }

        void nickReadAFile() {

            StreamReader re = File.OpenText("c:\\temp\\nick.txt");
            string input = null;
            while ((input = re.ReadLine()) != null)
            {
                Console.WriteLine(input);
            }
        //    re.close;
        }
        void parseAString()
        {

            int a1 = 0, b1 = 0, c1 = 0, d1 = 0, e1 = 0, f1 = 0, g1 = 0, h1 = 0, i1 = 0, j1 = 0, k1 = 0, l1 = 0, m1 = 0, n1 = 0, o1 = 0, p1 = 0, q1 = 0, r1 = 0, s1 = 0, t1 = 0, u1 = 0, v1 = 0, w1 = 0, x1 = 0, y1 = 0, z1 = 0;


            string[] a = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            string m = "AAAbbcccddmqzuxxll///bbbd";
            // string[15] n ={ };
            string letter = "";
            SortedList cntChars = new SortedList();
            // letter = m.Substring(i, 1);
            for (int i = 0; i < m.Length; i++)
            {
                letter = m.Substring(i, 1);
                for (int ap = 0; ap <= 51; ap++)
                {
                    // Console.WriteLine(" alphabet {0} ", a[ap].ToString());
                    if (letter == a[ap])
                    {
                        if (letter == "a" || letter == "A")
                            a1++;
                        else
                            if (letter == "b" || letter == "B")
                                b1++;
                            else
                                if (letter == "c" || letter == "C")
                                    c1++;
                                else
                                    if (letter == "d" || letter == "D")
                                        d1++;
                                    else
                                        if (letter == "e" || letter == "E")
                                            e1++;
                                        else
                                            if (letter == "f" || letter == "F")
                                                f1++;
                                            else
                                                if (letter == "g" || letter == "G")
                                                    g1++;
                                                else
                                                    if (letter == "h" || letter == "H")
                                                        h1++;
                                                    else
                                                        if (letter == "i" || letter == "I")
                                                            i1++;
                                                        else
                                                            if (letter == "j" || letter == "J")
                                                                j1++;
                                                            else
                                                                if (letter == "k" || letter == "K")
                                                                    k1++;
                                                                else
                                                                    if (letter == "l" || letter == "L")
                                                                        l1++;
                                                                    else
                                                                        if (letter == "m" || letter == "M")
                                                                            m1++;
                                                                        else
                                                                            if (letter == "o" || letter == "O")
                                                                                o1++;
                                                                            else
                                                                                if (letter == "p" || letter == "P")
                                                                                    p1++;
                                                                                else
                                                                                    if (letter == "q" || letter == "Q")
                                                                                        q1++;
                                                                                    else
                                                                                        if (letter == "r" || letter == "R")
                                                                                            r1++;
                                                                                        else
                                                                                            if (letter == "s" || letter == "S")
                                                                                                s1++;
                                                                                            else
                                                                                                if (letter == "t" || letter == "T")
                                                                                                    t1++;
                                                                                                else
                                                                                                    if (letter == "u" || letter == "U")
                                                                                                        u1++;
                                                                                                    else
                                                                                                        if (letter == "v" || letter == "V")
                                                                                                            v1++;
                                                                                                        else
                                                                                                            if (letter == "w" || letter == "W")
                                                                                                                w1++;
                                                                                                            else
                                                                                                                if (letter == "x" || letter == "X")
                                                                                                                    x1++;
                                                                                                                else
                                                                                                                    if (letter == "y" || letter == "Y")
                                                                                                                        y1++;
                                                                                                                    else
                                                                                                                        if (letter == "z" || letter == "Z")
                                                                                                                            z1++;
                                                                                                                     Console.WriteLine("hey this is a match {0} ", letter);
                        if (cntChars.Contains(letter))
                        {
                            Console.Write("Already there");
                            cntChars.ContainsValue("new");
                        }
                        else
                            cntChars.Add(letter, 1);
                    }


                }



            }// FOR LOOP
            Console.WriteLine("\n\n\n The String was {0} ", m);
            if(a1 > 0)
            Console.WriteLine("\n There are {0} A ", a1);
        if (b1 > 0)
            Console.WriteLine("\n There are {0} B ", b1);
        if (c1 > 0)
            Console.WriteLine("\n There are {0} C ", c1);
        if (d1 > 0)
            Console.WriteLine("\n There are {0} D ", d1);
        if (e1 > 0)
            Console.WriteLine("\n There are {0} E ", e1);
        if (f1 > 0)
            Console.WriteLine("\n There are {0} F ", f1);
        if (g1 > 0)
            Console.WriteLine("\n There are {0} G ", g1);
        if (h1 > 0)
            Console.WriteLine("\n There are {0} H ", h1);
        if (i1 > 0)
            Console.WriteLine("\n There are {0} I ", i1);
        if (j1 > 0)
            Console.WriteLine("\n There are {0} J ", j1);
        if (k1 > 0)
            Console.WriteLine("\n There are {0} K ", k1);
        if (l1 > 0)
            Console.WriteLine("\n There are {0} L ", l1);
        if (m1 > 0)
            Console.WriteLine("\n There are {0} M ", m1);
        if (n1 > 0)
            Console.WriteLine("\n There are {0} N ", n1);
        if (o1 > 0)
            Console.WriteLine("\n There are {0} O ", o1);
        if (p1 > 0)
            Console.WriteLine("\n There are {0} P ", p1);
        if (q1 > 0)
            Console.WriteLine("\n There are {0} Q ", q1);
        if (r1 > 0)
            Console.WriteLine("\n There are {0} R ", r1);
        if (s1 > 0)
            Console.WriteLine("\n There are {0} S ", s1);
        if(t1 > 0)
            Console.WriteLine("\n There are {0} T ", t1);
        if (u1 > 0)
            Console.WriteLine("\n There are {0} U ", u1);
        if (v1 > 0)
            Console.WriteLine("\n There are {0} V ", v1);
        if (w1 > 0)
            Console.WriteLine("\n There are {0} W ", w1);
        if (x1 > 0)
            Console.WriteLine("\n There are {0} X ", x1);
        if (y1 > 0)
            Console.WriteLine("\n There are {0} Y ", y1);
        if (z1 > 0)
            Console.WriteLine("\n There are {0} Z ", z1);

    

            // foreach (DictionaryEntry element in cntChars)
            // {
            // // Console.Write(" characters {0} and val {1} ", cntChars.Keys, cntChars.Values);
            // Console.WriteLine(" characters {0} ", cntChars.Values.ToString() );
            // }
            // else
            // Console.WriteLine("no matches");

        }
    }  // END OF CLASS NO METHODS PAST HERE !!! 
}

public class Provider  // individual provider class Encapsulation!!!
{ //Properties can be made read-only. 
    //This is accomplished by having only a get accessor in the property implementation

    private int providerid;  // Property of Provider class

    public int ProviderId  // for access to provider in pgm.
    {
        get { return providerid; }
        set { providerid = value; }
    }


    private int question1;   // question1 property of Provider class

    public int Question1
    {
         get { return question1; }  // properties allow you to change a private variable... 
         set { question1 = value; } 

      //  get;
     //   set;
    }

    private string name;

    public string Name
    {
        get { return name; }  // properties allow you to change a private variable... 
        set { name = value; }
    }

    private string address1;

    public string Address1
    {
        get
        { return address1; }
        set { address1 = value; }
    }

}

public class Providers : System.Collections.CollectionBase  // collection of providers built by provider class.
{


    public int Add(Provider value)
    {
        return (List.Add(value));
    }

    // public Provider Item (int index){
    //   return List[index];
    //}


    public int IndexOf(Int16 value)
    {
        return (List.IndexOf(value));
    }

    public void Insert(int index, Int16 value)
    {
        List.Insert(index, value);
    }

    public void Remove(Int16 value)
    {
        List.Remove(value);
    }

    public bool Contains(Int16 value)
    {
        // If value is not of type Int16, this will return false.
        return (List.Contains(value));
    }



}
public class Question
{
    //  private int question1;

    public int Question1
    {

        get { return Question1; }
        set { Question1 = value; }
    }

}

public class nick
{
    private int number1;

    public int Number1
    {
        get
        {
            return number1;
        }
        set {number1 = value;      }
    }

    public virtual void nickCount() {
        for (int i = 0; i < 10; i++)
            Console.WriteLine(" running the count {0} ", i);

    }
}
public class nickDerived : nick {

    public void nickCountBackwards() {

        for (int i = 0; i < 10; i++ )
            Console.WriteLine(" running the count Backward {0} ", i);

    }

}
public class test
{
    private int n;

    public int n2
    {
        get
        {

            return n;
        }

        set { n2 = value; }
    }   
        
}
public class Person
{
    public int age;
    public string name;

    public Person(int age, string name)
    {
        this.age = age;
        this.name = name;
    }
}

public class Student
{
    private int studentID;

 public int StudentId
{   
        get{  return studentID;
            }

        set
        {
        studentID = value;
        }

}       
 }
         
public class Provider2  // individual provider class
{
    private int providerid;  // manipulated by functions in the class itself.

    public int ProviderId  // for access to provider in pgm.
    {
        get { return providerid; }
        set { providerid = value; }
    }

}
public class ParentClass
{
    public ParentClass()
    {
        Console.WriteLine("Parent Constructor.");
    }

    public void print()
    {
        Console.WriteLine("I'm a Parent Class.");
    }
}

public class ChildClass : ParentClass
{

//    The protected keyword is a member access modifier. A protected member is accessible from within the class in which it is declared, and from within any class derived from the class that declared this member.


    public ChildClass()
    {
        Console.WriteLine("Child Constructor.");
    }

    
}
public abstract class Nick3 
{
    private int n1;

    public int N1
    {
        get { return n1; }
        set { value = n1; }
    
    }


}

