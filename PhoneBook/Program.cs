using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

namespace Телефонен_указател
{
    internal class Program
    {
        //const int Count = 100;
        class Node
        {
            public long Next;
            public long Contact;
        }
        class StreamLinkedList
        {
            FileStream fileStream;
            long firstNode;
            BinaryWriter bw;
            BinaryReader br;
            
            public StreamLinkedList(string filePath)
            {
                FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                bw = new BinaryWriter(fileStream, System.Text.Encoding.ASCII);
                br = new BinaryReader(fileStream, System.Text.Encoding.ASCII);
                if (fileStream.Length == 0)
                    bw.Write(-1L);
                else
                    firstNode = br.ReadInt64();
            }
            public void Add(string name, string num)
            {
                long newNode = fileStream.Length;
                // Kazvame che shte dobavqme nai otzad
                fileStream.Position = newNode; 
                //Добавя новия възел и го насочва към първия досега
                bw.Write(firstNode);
                bw.Write(name);
                bw.Write(num);
                //променя първия да сочи към новия
                fileStream.Position = 0;
                bw.Write(newNode);
                firstNode = newNode;
            }
            public string Find (string name)
            {
                fileStream.Position = 0;
                var node = br.ReadInt64();

                
                while (node != -1)
                {
                    fileStream.Position = node;
                    var nodeNext = br.ReadInt64();
                    var contactName = br.ReadString();

                    if (contactName == name)
                    {
                        return br.ReadString();
                    }
                    else
                        node = nodeNext;
                }
                return null;

            }
            public void Dispose()
            {
                bw.Dispose();
                br.Dispose();
                fileStream.Dispose();
            }
        }
        
        static StreamLinkedList streamLinkedList;

        /*static void AddContact(string name, string num)
        {
            var newNode = new Node
            {
                Contact = new Contact
                {
                    Name = name,
                    Num = num
                },
                Next = null
            };
            newNode.Next = firstNode;
            firstNode = newNode;
            
        }
        static string Find (string name)
        {
            var node = firstNode;
            while(node != null && node.Contact.Name != name)
            {
                node = node.Next;
            }

            return node ?.Contact.Num;
        }*/

        static void Main(string[] args)
        {
            streamLinkedList = new StreamLinkedList("contacts");
            while(true)
            {
                Console.Clear();
                Console.WriteLine("a - add; f - find, q - quit");
                var command = Console.ReadKey().KeyChar;
                Console.Clear();
                switch (command)
                {
                    case 'a':
                        {
                            Console.Write("Enter name: ");
                            var name = Console.ReadLine();
                            Console.Write("Enter number: ");
                            var num = Console.ReadLine();
                            streamLinkedList.Add(name, num);
                        }
                        break;
                    case 'f':
                        {
                            Console.Write("Enter name: ");
                            var name = Console.ReadLine();
                            var num = streamLinkedList.Find(name);
                            if (num != null)
                                Console.WriteLine($"Num: {num}");
                            else
                                Console.WriteLine("Not found!");
                        }
                        Console.WriteLine();
                        break;
                    case 'q':
                        return;
                }
            }
        }
    }
}