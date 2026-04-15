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
var jsonFile = "jsoncalc.json";

Calculation getCalc = new Calculation();
Calculator calc = new Calculator();
var calculations = new List<Calculation>();

Console.WriteLine($"Welcome to the C# calculator app!  \n");
Console.WriteLine("-------------------------------- \n \n");

while (!stopCalc) {

    double result = 0;
   
    if(!useResults)
    {       
        cleanNr1 = calc.ReadInput("Please give your fist integer number");
    }
    cleanNr2 = calc.ReadInput("Please give your second integer number");
   
    Console.WriteLine("Choose an operator from the following list:");
    Console.WriteLine("\ta - Add");
    Console.WriteLine("\ts - Subtract");
    Console.WriteLine("\tm - Multiply");
    Console.WriteLine("\td - Divide");
    Console.Write("Your option? ");

    String? selectedOperator = Console.ReadLine();

    if (selectedOperator == null || !Regex.IsMatch(selectedOperator, "[asmd]")){
         Console.WriteLine("This is not a valid input.");
    }
    else{
        try
        {
            result = calc.doOperation(cleanNr1, cleanNr2, selectedOperator);

            if (double.IsNaN(result))
            {
                Console.WriteLine("This operation will not complete and will result in an error");
            }
             else Console.WriteLine("Your result is: {0:0.##}\n", result);
            
        } catch (Exception e)
        {

            Console.WriteLine("An exception ocurred while calculating: \n " + e.Message);
        }

        calculations.Add(new Calculation
        {
            num1 = cleanNr1,
            num2 = cleanNr2,
            SelectedOperator = selectedOperator,
            result = result
        });

        calc.SerializeJson(jsonFile, calculations);

        Console.WriteLine("------------------------\n");
        Console.WriteLine($"You've used the calculator {calcUsesAmount} times.");
        Console.WriteLine("Press enter to make another calculation,write " + "h" + " for you calculation history " + " or write " + "n" + " to quit.");

        switch (Console.ReadLine())
        {

            case "n":
                stopCalc = true;
                break;

            case "h":
                List<Calculation> history = calc.DeserializeJson(jsonFile);
                
                foreach (Calculation calculation in history)
                {
                    Console.WriteLine("------ Calculation ------");
                    Console.WriteLine($"number1: {calculation.num1}");
                    Console.WriteLine($"number2: {calculation.num2}");
                    Console.WriteLine($"Operation: {calculation.SelectedOperator}");
                    Console.WriteLine($"Result: {calculation.result}");
                    Console.WriteLine();
                }
                break;
        }

        if(!stopCalc){
            Console.WriteLine("\n");
            Console.WriteLine("Would you like to use one of the the previous results for a new calculation ? Y/N");

            bool validInput = false;

            do{
                string? usePrev = Console.ReadLine()?.ToLower();
               
                switch (usePrev)
                {

                    case "y":
                        List<Calculation> history = calc.DeserializeJson(jsonFile);

                        int index = 1;
                        foreach (Calculation calculation in history)
                         {
                            Console.WriteLine($"[{index}] {calculation.result}");
                            index++;
                         }

                        Console.WriteLine("Choose a result number:");
                        int chosenIndex = int.Parse(Console.ReadLine());

                        chosenIndex = Calculator.CheckChosenIndex(chosenIndex, history);

                        cleanNr1 = history[chosenIndex -1].result;

                        Console.WriteLine($"Using {cleanNr1} as first number.");

                        useResults = true;
                        validInput = true;
 
                        break;

                    case "n":    
                        
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































