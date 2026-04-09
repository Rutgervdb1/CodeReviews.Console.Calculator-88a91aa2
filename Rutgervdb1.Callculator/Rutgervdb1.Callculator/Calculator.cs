using CalculatorLibrary;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Transactions;


bool stopCalc = false;
int calcUsesAmount = 1;
bool useResults = false;
double cleanNr1 = 0;
double cleanNr2 = 0;

string? numInput1 = "";
string? numInput2 = "";

getCalculations getCalc = new getCalculations();


Console.WriteLine($"Welcome to the C# calculator app!  \n");
Console.WriteLine("-------------------------------- \n \n");

Calculator calc = new Calculator();

// create the json array to store operations

JsonArray jsonArrayCalc = new JsonArray();



// we want the program to continue as long as the user does not want to quit.

while (!stopCalc) {


    double result = 0;
    
    bool validInt = false;
    
    // get a valid input for the first number.

    if(!useResults)
    {
        do
        {
            if (!double.TryParse(numInput1, out cleanNr1) == true)
            {
                Console.WriteLine("Please give a valid integer number.");
                numInput1 = Console.ReadLine();
            }
            else
            {
                validInt = true;

            }

        } while (validInt == false);

    }

   //----------------------------------------------------------------------------------------

    Console.WriteLine("Please write your second number: ");

    numInput2 = Console.ReadLine();
       
    bool validInt2 = false;


    // get a valid second number
    // when using a while(!validint2), it loops one time to much
    do
    {

        if (!double.TryParse(numInput2, out cleanNr2) == true)
        {
            Console.WriteLine("Please give a valid integer number.");
            numInput2 = Console.ReadLine();

        }
        else
        {
            validInt2 = true;
           
          
        }

    } while (validInt2 == false);


    //----------------------------------------------------------------------------------------


    //display the options

    Console.WriteLine("Choose an operator from the following list:");
    Console.WriteLine("\ta - Add");
    Console.WriteLine("\ts - Subtract");
    Console.WriteLine("\tm - Multiply");
    Console.WriteLine("\td - Divide");
    Console.Write("Your option? ");

    // get the user choice
     String? userChoice = Console.ReadLine();


    // if the user didn't enter anything or it does not match and a/s/m/d
    if (userChoice == null || !Regex.IsMatch(userChoice, "[asmd]"))
    {

        Console.WriteLine("This is not a valid input.");

    }
    else
    {
        try
        {
            result = calc.doOperation(cleanNr1, cleanNr2, userChoice);

            if (double.IsNaN(result))
            {
                Console.WriteLine("This operation will not complete and will result in an error");
            }
             else Console.WriteLine("Your result is: {0:0.##}\n", result);
            
        } catch (Exception e)
        {

            Console.WriteLine("An exception ocurred while calculating: \n " + e.Message);
        }

     //----------------------------------------------------------------------------------------

        // Add data to the json Array


        jsonArrayCalc.Add(new JsonObject
        {
            ["Number1"] = cleanNr1.ToString(),
            ["Number2"] = cleanNr2.ToString(),
            ["userChoice"] = userChoice,
            ["Result"] = result.ToString()
        });


        // Write the data to the json file

        File.WriteAllText("jsoncalc.json", jsonArrayCalc.ToJsonString(new JsonSerializerOptions
        {
            WriteIndented = true
        }));


     //----------------------------------------------------------------------------------------

        Console.WriteLine("------------------------\n");
        Console.WriteLine($"You've used the calculator {calcUsesAmount} times.");
        Console.WriteLine("Press enter to make another calculation,write " + "h" + " for you calculation history " + " or write " + "n" + " to quit.");



        // Check if the user wants to quit or wants to see the calculation history

        switch (Console.ReadLine())
        {

            case "n":
                stopCalc = true;
                break;

            case "h":

                String jsonHistory = File.ReadAllText("jsoncalc.json");
                JsonArray jsonShowArray = JsonNode.Parse(jsonHistory)!.AsArray();

                foreach (JsonNode jsonShow in jsonShowArray)
                {

                    Console.WriteLine("------ Calculatie ------");
                    Console.WriteLine($"nummer1: {jsonShow["Number1"]}");
                    Console.WriteLine($"nummer2: {jsonShow["Number2"]}");
                    Console.WriteLine($"Operation: {jsonShow["userChoice"]}");
                    Console.WriteLine($"Result: {jsonShow["Result"]}");
                    Console.WriteLine();

                }
                break;

        }

        //check if the user stopped the calculator so it skips this step

        if(stopCalc == false)
        {

            Console.WriteLine("\n");
            Console.WriteLine("Would you like to use one of the the previous results for a new calculation ? Y/N");


            bool validInput = false;

            do
            {
                string? usePrev = Console.ReadLine().ToLower();
               

                switch (usePrev)
                {

                    case "y":

                             // show history
                        string jsonHistory = File.ReadAllText("jsoncalc.json");
                        JsonArray jsonShowArray = JsonNode.Parse(jsonHistory)!.AsArray();

                        int index = 1;
                        foreach (JsonNode item in jsonShowArray)
                         {
                            Console.WriteLine($"[{index}] {item["Result"]}");
                            index++;
                         }

                        Console.WriteLine("Choose a result number:");
                        int chosenIndex = int.Parse(Console.ReadLine()!);
                        bool rightIndex = false;

                        do
                        {
                            if (chosenIndex > jsonShowArray.Count || chosenIndex < jsonShowArray.Count)
                            {

                                Console.WriteLine($"There is no result for the chosen array. please choose a number between 1 and {jsonShowArray.Count}");
                                chosenIndex = int.Parse(Console.ReadLine());

                            }
                            else
                            {
                                rightIndex = true;
                            }


                        } while (!rightIndex);


                            cleanNr1 = double.Parse(jsonShowArray[chosenIndex - 1]!["Result"]!.ToString());

                            Console.WriteLine($"Using {cleanNr1} as first number.");

                            useResults = true;
                            validInput = true;
 

                        break;

                    case "n":

                        Console.WriteLine("Please write your first number.");
                        numInput1 = Console.ReadLine();

                        while (!double.TryParse(numInput1, out cleanNr1))
                        {
                            Console.WriteLine("Invalid number:");
                            numInput1 = Console.ReadLine();
                        }
                        useResults = false;
                        validInput = true;
                        break;

                    default:

                        Console.WriteLine("This is not a valid input. Please choose Y or N");
                        break;
                }
             

            } while (!validInput);
                 
        }

    }
        Console.WriteLine("\n");      
}































