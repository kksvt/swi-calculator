using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace swi_calculator
{
    class SwiMathOperation
    {
        [JsonPropertyName("operator")]
        public string Op { get; set; } = string.Empty;

        [JsonPropertyName("value1")]
        public double? Value1 { get; set; }

        [JsonPropertyName("value2")]
        public double? Value2 { get; set; }

        public double Calculate()
        {
            if (!Value1.HasValue)
            {
                throw new InvalidOperationException("Operator \"" + Op + "\" requires value 1");
            }
            switch (Op)
            {
                case "add":
                    if (!Value2.HasValue)
                    {
                        throw new InvalidOperationException("Operator \"" + Op + "\" requires two operands");
                    }
                    return Value1.Value + Value2.Value;
                case "sub":
                    if (!Value2.HasValue)
                    {
                        throw new InvalidOperationException("Operator \"" + Op + "\" requires two operands");
                    }
                    return Value1.Value - Value2.Value;
                case "mul":
                    if (!Value2.HasValue)
                    {
                        throw new InvalidOperationException("Operator \"" + Op + "\" requires two operands");
                    }
                    return Value1.Value * Value2.Value;
                case "sqrt":
                    return Math.Sqrt(Value1.Value);
                default:
                    throw new NotImplementedException("Operator \"" +  Op + "\" is not implemented");
            }
        }
    }

    class SwiMathObject { 
        public string Name { get; set; }
        public SwiMathOperation Operation { get; set; }

        public double Result { get; set; }

        public SwiMathObject(string name, SwiMathOperation operation)
        {
            this.Name = name;
            this.Operation = operation;
            this.Result = operation.Calculate();
        }

        public override string ToString()
        {
            return (Name + ": " + Result);
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            const string inputFile = "input.json";
            const string outputFile = "output.txt";
            try
            {
                using (FileStream fs = File.OpenRead(inputFile))
                {
                    using (JsonDocument document = JsonDocument.Parse(fs))
                    {
                        List<SwiMathObject> calculations = new();
                        JsonElement root = document.RootElement;
                        foreach (var operation in root.EnumerateObject())
                        {
                            try
                            {
                                calculations.Add(new SwiMathObject(operation.Name, JsonSerializer.Deserialize<SwiMathOperation>(operation.Value)));
                            } catch (InvalidOperationException e)
                            {
                                Console.WriteLine(e.Message + ", skipping.");
                            } catch (NotImplementedException e)
                            {
                                Console.WriteLine(e.Message + ", skipping.");
                            }
                        }

                        calculations.Sort((calculation1, calculation2) => calculation1.Result.CompareTo(calculation2.Result));
                        using (StreamWriter sw = new StreamWriter(outputFile))
                        {
                            foreach (var calculation in calculations)
                            {
                                sw.WriteLine(calculation.ToString());   
                            }
                        }
                    }
                }
            } catch (FileNotFoundException)
            {
                Console.WriteLine("File \"" + inputFile + "\" does not exist.");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}