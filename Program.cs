using System;
using System.IO;
using System.Collections.Generic;

using System.Data;

class CsvHelper
{
    //CSV CREATE

    public static void CreateCsv(string filePath, string[] headers)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            string headerLine = string.Join(",", headers); //Creates a new empty location to hold appended data
            writer.WriteLine(headerLine);
        }

        Console.WriteLine("CSV file created with headers");
    }

    //CSV APPEND

    public static void AppendToCsv(string filePath, string dataRow)
    {
        using (StreamWriter writer = new StreamWriter(filePath, append: true))
        {
            writer.WriteLine(dataRow);
        }

        Console.WriteLine("Data row appended to CSV file");
    }

    //CSV READ

    //READ ALL
    public static void ReadCsv(string filePath)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
    }

    //READ COLUMN ROWS
    public static void ReadColumns(string filePath, int Column, bool showIndex, bool skipHeaders)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            int Count = 0;
            bool isFirstLine = skipHeaders; //if skip headers is false, the process will be skipped

            while ((line = reader.ReadLine()) != null)
            {
                if (isFirstLine)
                {
                    isFirstLine = false;  // Skip the header line
                    Count++;
                    continue;
                }

                string[] columns = line.Split(',');

                if (showIndex)
                {
                    Console.WriteLine($"{Count}: {columns[Column]}");
                }
                else
                {
                    Console.WriteLine(columns[Column]);
                }
                Count++;
            }
        }
    }

    //CSV GET

    //GET CSV ROW
    public static string GetCsvRow(string filePath, int row) 
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            string Row;
            int currentRow = 0;

            // Loop until we find the requested row or end of file
            while ((Row = reader.ReadLine()) != null)
            {
                if (currentRow == row) // If we've reached the desired row
                {
                    return Row;
                }

                currentRow++;
            }
        }

        // If we reach this point, the row number was beyond the total number of rows
        return null;
    }

    //GET CSV ROW COUNT
    public static int CountRows(string filePath, bool skipHeaders) 
    {
        int count = 0;
        bool isFirstLine = skipHeaders; //if skip headers is false, the process will be skipped

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (isFirstLine)
                {
                    isFirstLine = false;  // Skip the header line
                    count++;
                    continue;
                }

                count++;
            }
        }

        return count;
    }
}

class Program 
{
    static void Main()
    {
        //STARTUP

        //Game Data
        string FishfilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Game", "fishData.csv"); //path of fish data

        //User Data
        string SavefilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "User", "Save.csv"); // path of game save

        //checks if save file exists in path
        if (File.Exists(SavefilePath))
        {
            Console.WriteLine("Found Save");
        }
        else //If not, creates a save file with headers
        {
            Console.WriteLine("Save does not exists");
            Console.WriteLine("Creating a Save");

            string[] saveHeader = {"fishInv", "rodInv", "baitInv", "moneyBal", "curRod", "curBait"};
            CsvHelper.CreateCsv(SavefilePath, saveHeader);
            Console.WriteLine("New save file created");
        }

        //MENU
        //Repeat menu until screen change or closing
        while (true)
        {
            Console.Clear();
            Console.WriteLine("FISH FINAL");
            Console.WriteLine();
            Console.WriteLine("Fishing in your computer. Not to get confused with Phishing");

            Console.WriteLine();
            Console.WriteLine("1. Play\n2. Fish dictionary\n3. Leave.");

            switch (Console.ReadLine())
            {
                case "1": //Plays game
                    Console.Clear();
                    Console.WriteLine("Yippy yay yoooo! :)");
                    break;

                case "2": //Opens fish dictionary
                    Console.Clear();
                    Console.WriteLine("What fish are there..\n");
                    CsvHelper.ReadColumns(FishfilePath, 0, true, true);

                    Console.WriteLine("Enter the number of the fish you would like to view the details of.\nEnter Exit to return to main menu.");
                    while (true)
                    {
                        string action = Console.ReadLine();
                        string searchResult = "";
                        

                        if (action == "Exit")
                        {
                            break;
                        }

                        if (int.TryParse(action, out int result))
                        {
                            searchResult = CsvHelper.GetCsvRow(FishfilePath, int.Parse(action));

                            if (searchResult != null)
                            {
                                string[] fishDetails = searchResult.Split(',');
                                Console.WriteLine();
                                Console.WriteLine(fishDetails[0]); //Writes name of fish
                                Console.WriteLine();

                                Console.WriteLine($"Fish Size: {fishDetails[1]}\nFish Description: {fishDetails[2]}\nFish Value: {fishDetails[3]}");
                            }
                            else
                            {
                                Console.WriteLine("No fish found.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("You must enter a number.");
                        }
                        
                        
                    }
                    break;

                case "3": //closes app
                    Console.Clear();
                    Console.WriteLine("Buh byeee");
                    System.Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("that's not an option >:(");
                    break;
            }
        }
    }
}