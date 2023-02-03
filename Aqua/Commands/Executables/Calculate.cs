using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aqua.Commands.Executables
{
    public class Calculate : Command
    {
        public Calculate(string name, string description) : base(name, description) { }
        int firstNumber = 0, secondNumber = 0, calc;

        public override string Execute(string[] args)
        {
            switch (args[0])
            {
                case "add":
                    Int32.TryParse(args[1], out firstNumber);
                    Int32.TryParse(args[2], out secondNumber);

                    calc = firstNumber + secondNumber;
                    return calc.ToString();

                case "min":
                    Int32.TryParse(args[1], out firstNumber);
                    Int32.TryParse(args[2], out secondNumber);

                    calc = firstNumber - secondNumber;
                    return calc.ToString();

                case "mult":
                    Int32.TryParse(args[1], out firstNumber);
                    Int32.TryParse(args[2], out secondNumber);

                    calc = firstNumber * secondNumber;
                    return calc.ToString();

                case "div":
                    Int32.TryParse(args[1], out firstNumber);
                    Int32.TryParse(args[2], out secondNumber);

                    calc = firstNumber / secondNumber;
                    return calc.ToString();

                case "perc":
                    Int32.TryParse(args[1], out firstNumber);
                    Int32.TryParse(args[2], out secondNumber);

                    calc = (100 * secondNumber) / firstNumber;
                    return calc.ToString() + "%";
            }

            return null;
        }
    }
}
