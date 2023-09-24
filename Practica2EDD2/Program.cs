using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using Practica2EDD2.model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Practica2EDD2
{
    class Program
    {
        static bool AreBitArraysEqual(BitArray bitArray1, BitArray bitArray2)
        {
            if (bitArray1.Length != bitArray2.Length)
            {
                return false;
            }

            for (int i = 0; i < bitArray1.Length; i++)
            {
                if (bitArray1[i] != bitArray2[i])
                {
                    return false;
                }
            }

            return true;
        }

        public class CsvReader
        {
            public List<List<string>> ReadCsv(string filePath)
            {
                List<List<string>> csvData = new List<List<string>>();

                using (TextFieldParser parser = new TextFieldParser(filePath))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(";");

                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();

                        if (fields != null)
                        {
                            List<string> rowData = new List<string>(fields);
                            csvData.Add(rowData);
                        }
                    }
                }

                return csvData;
            }
        }
        static void Main(string[] args)
        {

            Dictionary<BitArray, HuffmanTree> encripted = new Dictionary<BitArray, HuffmanTree>();

            List<string> listC = new List<string>();

            List<string> listA = new List<string>();
            List<string> listB = new List<string>();
            List<Person> personas = new List<Person>();
            BTree<Person> b = new BTree<Person>(250);

            string filePath = @"C:\Users\Rolando Ziller\Documents\Universidad\2023\segundo ciclo\edd2\inputLAB2.csv";

            CsvReader csvReader = new CsvReader();
            List<List<string>> csvData = csvReader.ReadCsv(filePath);

            // Ahora puedes trabajar con los datos del archivo CSV.
            foreach (List<string> row in csvData)
            {
                listA.Add(row[0]);
                listB.Add(row[1]);


                /*foreach (string cell in row)
                {
                    var line = cell;
                    var values = line.Split(';');

                    listA.Add(values[0]);
                    listB.Add(values[1]);
                }*/



            }
            foreach (var item in listB)
            {
                Person input = JsonConvert.DeserializeObject<Person>(item)!;
                //Console.WriteLine($"Nombre: {input.Name}" + " " + $"Id: {input.Id}" + " " + $"Fecha: {input.BirthDate}" + " " + $"Direccion: {input.Address}" + " ");
                //Console.WriteLine("----------------------");
                //Console.WriteLine("");
                personas.Add(input);
            }



            for (int i = 0; i < listA.Count; i++)
            {
                Console.WriteLine("Accion: " + listA[i] + " " + $"Nombre: {personas[i].Name}" + " " + $"Id: {personas[i].Id}" + " " + $"Fecha: {personas[i].BirthDate}" + " " + $"Direccion: {personas[i].Address}" + " ");
                if (listA[i] == "INSERT")
                {
                    b.Insert(personas[i]);
                    //b.PrintTreeGraph(@"C:\Users\Rolando Ziller\Documents\Universidad\2023\segundo ciclo\edd2");
                }
                else if (listA[i] == "PATCH")
                {

                    b.UpdatePersonInfo(personas[i].Id, personas[i].Address, personas[i].BirthDate, personas[i].Company);
                  
                }
                else if (listA[i] == "DELETE")
                {
                    b.RemovePerson(personas[i].Id);

                    //b.Remove(personas[i]);
                    //b.PrintTreeGraph(@"C:\Users\Rolando Ziller\Documents\Universidad\2023\segundo ciclo\edd2");
                }


            }

            foreach (Person person in personas)
            {
                if (person.Company != null)
                {
                    foreach (var item in person.Company)
                    {
                        if (!listC.Contains(item))
                        {
                            listC.Add(item);
                        }

                    }
                }
                
            }

            //Console.WriteLine(listC);
            //Console.WriteLine("");



            b.code();

            string company = "";

            bool showMenu;
            bool showMenu2;
            bool showMenu3;
            bool showMenu4;
            bool showMenu5;
            //bool showMenu6;
            bool showMenuPrincipal = true;

            while (showMenuPrincipal)
            {
                Console.Clear();
                Console.WriteLine("Company: " + company);
                Console.WriteLine("Choose an option: ");
                Console.WriteLine("1) Choose company");
                Console.WriteLine("2) Coding person");
                Console.WriteLine("3) Decoding person");
                Console.WriteLine("4) Search person");
                Console.WriteLine("5) Show B Tree");
                Console.Write("\r\nEnter an option: ");
                Console.Write("");

                switch (Console.ReadLine())
                {
                    case "1":
                        showMenu = true;
                        while (showMenu)
                        {
                            Console.WriteLine("");
                            for (int i = 0; i < listC.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}) {listC[i]}");
                            }
                            Console.WriteLine("");
                            Console.WriteLine("Enter the id of the company or type exit: ");
                            Console.WriteLine("");
                            string userCExit = Console.ReadLine();

                            if (userCExit == "exit")
                            {
                                break;
                            }
                            else
                            {

                                if ((int.Parse(userCExit) - 1) < listC.Count)
                                {
                                    company = listC[int.Parse(userCExit) - 1];
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("");
                                    Console.WriteLine($"Please enter an id between 1 and {listC.Count}.");
                                    Console.WriteLine("");
                                }

                            }
                        }

                        

                        break;
                    case "2":
                        
                        long codingId;

                        Console.WriteLine("Enter the id of the person or type exit: ");

                        string codingUserId = Console.ReadLine();
                        if (codingUserId == "exit")
                        {
                            break;
                        }
                        else
                        {
                            codingId = long.Parse(codingUserId);
                        }

                        if (company != "")
                        {
                            //Console.WriteLine(b.FindCompanyPerson(company, codingId));
                            if (b.FindCompanyPerson(company, codingId))
                            {
                                string input = codingId +" "+company;
                                HuffmanTree huffmanTree = new HuffmanTree();

                                // Build the Huffman tree
                                huffmanTree.Build(input);

                                // Encode
                                BitArray encoded = huffmanTree.Encode(input);
                                encripted.Add(encoded, huffmanTree);
                                Console.Write("Encoded info: ");
                                foreach (bool bit in encoded)
                                {
                                    Console.Write((bit ? 1 : 0) + "");
                                }
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine($"Sorry no person with id {codingId} is applying to the company {company}.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Sorry you need to choose a company first.");
                            System.Threading.Thread.Sleep(1500);
                            break;
                        }



                        Console.WriteLine("");
                        Console.WriteLine("Type exit to return to menu");
                        Console.WriteLine("");
                        string userExitCoding = Console.ReadLine();

                        if (userExitCoding == "exit")
                        {
                            break;
                        }

                        break;
                    case "3":
                        //Console.WriteLine(encripted);
                        Console.WriteLine("Enter the code of the person you wish to decode or type exit: ");

                        string decodingUserId = Console.ReadLine();
                        
                        if (decodingUserId == "exit")
                        {
                            break;
                        }
                        else
                        {

                            string binaryString = decodingUserId;
                            BitArray bitArrayFromString = new BitArray(binaryString.Select(c => c == '1').ToArray());
                            foreach (bool bit in bitArrayFromString)
                            {
                                Console.Write((bit ? 1 : 0) + "");
                            }
                            Console.WriteLine();

                            //var areEqual = bitArrayFromString.Equals(encripted.ElementAt(0).Key);


                            /*foreach (var item in encripted)
                            {
                                bool areEqual = AreBitArraysEqual(bitArrayFromString, item.Key);
                                if (areEqual)
                                {
                                    string decoded = item.Value.Decode(item.Key);

                                    Console.WriteLine("Decoded: " + decoded);
                                }


                                
                            }*/

                            int indexDecode = -1;
                            for (int i = 0; i < encripted.Count; i++)
                            {
                                bool areEqual = AreBitArraysEqual(bitArrayFromString, encripted.ElementAt(i).Key);

                                if (areEqual)
                                {
                                    indexDecode = i;



                                }
                                
                            }


                            if (indexDecode >= 0)
                            {
                                string decoded = encripted.ElementAt(indexDecode).Value.Decode(encripted.ElementAt(indexDecode).Key);
                                Console.WriteLine("Decoded: " + decoded);
                            }
                            else
                            {
                                Console.WriteLine("No records found");
                            }
                            //string decoded = huffmanTree.Decode(encoded);
                            //Console.WriteLine("Decoded: " + decoded);
                        }




                        Console.WriteLine("");
                        Console.WriteLine("Type exit to return to menu");
                        Console.WriteLine("");
                        string userExitDecoding = Console.ReadLine();

                        if (userExitDecoding == "exit")
                        {
                            break;
                        }
                        break;
                    case "4":

                        showMenu5 = true;

                        while (showMenu5)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Enter id or type exit: ");

                            string userIdSearch = Console.ReadLine();

                            if (userIdSearch == "exit")
                            {
                                break;
                            }
                            else
                            {
                                

                                Person personaResultante = new Person();
                                long searchId = long.Parse(userIdSearch);

                                Console.WriteLine("");
                                Console.WriteLine("Bitacora:");

                                //Console.WriteLine(b.SearchById(searchId));
                                Console.WriteLine("");
                                personaResultante = b.SearchById(searchId);
                                b.decoded(personaResultante);
                                Console.Write($"Nombre: {b.SearchById(searchId).Name}, DPI: {b.SearchById(searchId).Id}, Fecha de Nacimiento: {b.SearchById(searchId).BirthDate}, Dirección: {b.SearchById(searchId).Address}");
                                Console.WriteLine("");
                                //Console.Write(" Empresas: ");
                                //Console.WriteLine("");
                                /*foreach (var item in b.SearchById(searchId).Company)
                                {
                                    Console.WriteLine(" " + item);
                                }*/
                          
                                Console.WriteLine("");
                                personaResultante = null;
                                /*List<Person> searchResults = b.SearchByName(searchName);

                                if (searchResults.Count > 0)
                                {
                                    Console.WriteLine("");
                                    Console.WriteLine("Bitacora:");
                                    foreach (Person person in searchResults)
                                    {

                                        Console.WriteLine($"Nombre: {person.Name}, DPI: {person.Id}, Fecha de Nacimiento: {person.BirthDate}, Dirección: {person.Address}");
                                        Console.WriteLine("");
                                    }

                                    b.PrintTreeGraph(@"C:\Users\Rolando Ziller\Documents\Universidad\2023\segundo ciclo\edd2");
                                }
                                else
                                {
                                    Console.WriteLine("No records found");
                                    Console.WriteLine("");

                                }*/

                            }
                        }



                        break;

                    case "5":
                        Console.WriteLine("");
                        Console.WriteLine("B Tree: ");
                        Console.WriteLine("");
                        b.Show();
                        Console.WriteLine("");
                        Console.WriteLine("Type exit to return to menu");
                        Console.WriteLine("");

                        string userExit = Console.ReadLine();

                        if (userExit == "exit")
                        {
                            break;
                        }
                        break;
                    default:

                        break;
                }
            }
            /*

            Person persona1 = new Person();
            DateTime dateTime = DateTime.Parse("01-01-1900");
            persona1.Name = "diego";
            persona1.Id = 12345678;
            persona1.BirthDate = dateTime;
            persona1.Address = "guatemala";


            Person persona2 = new Person();
            DateTime dateTime2 = DateTime.Parse("02-01-1900");
            persona2.Name = "max";
            persona2.Id = 12345679;
            persona2.BirthDate = dateTime2;
            persona2.Address = "guatemala";

            Person persona3 = new Person();
            DateTime dateTime5 = DateTime.Parse("02-01-1900");
            persona3.Name = "diego";
            persona3.Id = 12345621;
            persona3.BirthDate = dateTime5;
            persona3.Address = "guatemala";


            Person persona4 = new Person();
            DateTime dateTime6 = DateTime.Parse("03-01-1900");
            persona4.Name = "diego";
            persona4.Id = 12345622;
            persona4.BirthDate = dateTime6;
            persona4.Address = "guatemala";

  


            
            int comparisonResult = persona1.CompareTo(persona2);

            if (comparisonResult == 0)
            {
                Console.WriteLine("Las personas son iguales.");
            }
            else if (comparisonResult < 0)
            {
                Console.WriteLine("persona1 es menor que persona2.");
            }
            else
            {
                Console.WriteLine("persona1 es mayor que persona2.");
            }

            

            BTree<int> b = new BTree<int>(3);
            b.Insert(8);
            b.Insert(9);
            b.Insert(10);
            b.Insert(11);
            b.Insert(15);
            b.Insert(20);
            b.Insert(17);

            b.Show();


            if (b.Contain(12))
            {
                Console.WriteLine("\nEncontrado");
            }
            else
            {
                Console.WriteLine("\nNo encontrado");
            }

            BTree<Person> bp = new BTree<Person>(3);

            bp.Insert(persona1);
            bp.Insert(persona2);

            Console.WriteLine("");
            bp.Show();

            bp.Remove(persona9);
            Console.WriteLine("");
            bp.Show();

            bp.Remove(persona8);
            Console.WriteLine("");
            bp.Show();

            bp.Remove(persona7);
            Console.WriteLine("");
            bp.Show();

            bp.Remove(persona6);
            Console.WriteLine("");
            bp.Show();

            bp.Remove(persona5);
            Console.WriteLine("");
            bp.Show();

            bp.Remove(persona4);
            Console.WriteLine("");
            bp.Show();

            bp.Remove(persona3);
            Console.WriteLine("");
            bp.Show();

            bp.Remove(persona2);
            Console.WriteLine("");
            bp.Show();


            Console.WriteLine(bp.root.key[0].Name);

            Console.WriteLine(bp.root.key);

            bp.PrintTreeGraph(@"C:\Users\Rolando Ziller\Documents\Universidad\2023\segundo ciclo\edd2");
            
            if (bp.Contain(persona1))
            {
                Console.WriteLine("\nEncontrado");
            }
            else
            {
                Console.WriteLine("\nNo encontrado");
            }
            

            DateTime dateTime3 = DateTime.Parse("02-02-1900");
            
            int thisId = 12345679;
            string thisName = "max";

            DateTime? thisDay = dateTime3;
            string thisAddress = null;

           bp.UpdatePersonInfo(thisName, thisId, thisAddress, thisDay);

            int thisId2 = 12345679;
            string thisName2 = "max";

            DateTime? thisDay2 = null;
            string thisAddress2 = "el salvador";

            bp.UpdatePersonInfo(thisName2, thisId2, thisAddress2, thisDay2);
            Console.WriteLine("");
            bp.Show();

            bp.Insert(persona3);
            bp.Insert(persona4);

            Console.WriteLine("");
            bp.Show();

            int thisId3 = 12345622;
            string thisName3 = "diego";
            DateTime dateTime4 = DateTime.Parse("05-01-1900");

            DateTime? thisDay3 = dateTime4;
            string thisAddress3 = null;

            bp.UpdatePersonInfo(thisName3, thisId3, thisAddress3, thisDay3);
            Console.WriteLine("");
            bp.Show();

            bp.PrintTreeGraph(@"C:\Users\Rolando Ziller\Documents\Universidad\2023\segundo ciclo\edd2");

            int thisId4 = 12345678;
            string thisName4 = "diego";

            bp.RemovePerson(thisName4, thisId4);
            Console.WriteLine("");
            bp.Show();

            string searchName = "diego";
            List<Person> searchResults = bp.SearchByName(searchName);
            Console.WriteLine("");
            Console.WriteLine("Bitacora:");
            foreach (Person person in searchResults)
            {
                
                Console.WriteLine($"Nombre: {person.Name}, DPI: {person.Id}, Fecha de Nacimiento: {person.BirthDate}, Dirección: {person.Address}");
            }

            bp.PrintTreeGraph(@"C:\Users\Rolando Ziller\Documents\Universidad\2023\segundo ciclo\edd2");
            */
        }
    }
}
