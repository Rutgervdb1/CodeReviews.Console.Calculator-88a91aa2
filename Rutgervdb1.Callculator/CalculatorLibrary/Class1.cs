using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CalculatorLibrary
{
   public class Calculation
    {
        public double num1 { get; set; }
        public double num2 { get; set; }
        public double result { get; set; }
        public string? SelectedOperator { get; set; }
    }

    public class Calculator
    {
        public double doOperation(double firstNr, double secondNr, string selectedOperator, int calcUses = 0)
        {
            double result = 0;

            switch (selectedOperator)
            {
                case "a":
                    result = firstNr + secondNr;

                    break;

                case "s":
                    result = firstNr - secondNr;

                    break;

                case "m":
                    result = firstNr * secondNr;

                    break;

                case "d":
                    if (secondNr != 0)
                    {
                        result = firstNr / secondNr;
                    }

                    break;

                default:
                    Console.WriteLine($"{selectedOperator} is not a valid choice.");

                    break;
            }
            return result;
        }

        public double ReadInput(string prompt)  {
            while (true)
            {
                Console.WriteLine(prompt);
                string? input = Console.ReadLine();

                if(double.TryParse(input, out double result))
                {
                    return result;
                }
                else {
                    Console.WriteLine("This is not a valid number.");
                }
            }
        }
        public List<Calculation> DeserializeJson(string jsonFile)
        {
            string json = File.ReadAllText(jsonFile);
            var calculations = new List<Calculation>();

            if (!string.IsNullOrEmpty(json))
            {
                calculations = System.Text.Json.JsonSerializer.Deserialize<List<Calculation>>(json) ?? [];
            }

            return calculations;
        }
        public void SerializeJson(string jsonFile, List<Calculation> calculations)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(calculations);
            File.WriteAllText(jsonFile, json);
        }

        public void checkChosenIndex(int chosenIndex, List<Calculation> history)
        {
            chosenIndex = int.Parse(Console.ReadLine());

            if (chosenIndex > history.Count || chosenIndex < 1) {
                Console.WriteLine($"There is no data at the chosen number. Please choose a number between 1 and {history.Count()}.");
                chosenIndex = int.Parse(Console.ReadLine());
            }
            else
            {
                 chosenIndex;
            }
            


        }

    }
}
