using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CalculatorLibrary
{


    public class getCalculations
    {

        public double num1 { get; set; }
        public double num2 { get; set; }
        public double result { get; set; }
        public string? userChoise { get; set; }



    }


    public class Calculator
    {
       


        public double doOperation(double firstNr, double secondNr, string userChoice, int calcUses = 0)
        {
            double result = 0;




            switch (userChoice)
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
                    Console.WriteLine($"{userChoice} is not a valid choice.");

                    break;

            }

            

            return result;

        }
        
       

    }

}
