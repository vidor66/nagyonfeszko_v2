using System.Globalization;

public static class AdvancedVoltageSolver
{
    class VoltageKeyValueObject
    {
        public string key;
        public string value;
    }
    public static string EvaluateVoltage (double voltage)
    {
        if (voltage >= 0 && voltage <= 0.8) return "0";
        if (voltage >= 2.7 && voltage <= 5.8) return "1";
        return "E";
    }

    public static string DecodeInputVoltages (string input)
    {
        if (input.Length % 2 != 0) return "INPUT_LENGTH_ERROR";

        List<string> voltages = new List<string>();
        string result = "";

        for (int i = 0; i < input.Length; i += 2) voltages.Add(input[i] + "." + input[i + 1]);
        foreach (string voltage in voltages) result += EvaluateVoltage(Convert.ToDouble(voltage, CultureInfo.InvariantCulture));

        return result;
    }

    public static string EvaluateGate (string gate, string[] voltage_codes)
    {
        string result = "";
        int[] accepted_length_between = { 2, 2 };

        switch (gate)
        {
            case "NOT":
                accepted_length_between[0] = 1;
                accepted_length_between[1] = 1;
                if (!(voltage_codes.Length >= accepted_length_between[0] && voltage_codes.Length <= accepted_length_between[1])) return "Kapu / változó hiba, próbáld újra!";
                for (int i = 0; i < voltage_codes[0].Length; i++)
                {
                    if (voltage_codes[0][i].ToString().Equals("E")) result += "E";
                    else if (voltage_codes[0][i].ToString().Equals("1")) result += "0";
                    else result += "1";
                }
                break;
            case "AND":
                if (!(voltage_codes.Length >= accepted_length_between[0] && voltage_codes.Length <= accepted_length_between[1])) return "Kapu / változó hiba, próbáld újra!";
                for (int i = 0; i < voltage_codes[0].Length; i++)
                {
                    if (voltage_codes[0][i].ToString().Equals("E") || voltage_codes[1][i].ToString().Equals("E")) result += "E";
                    else if (voltage_codes[0][i].ToString().Equals("1") && voltage_codes[1][i].ToString().Equals("1")) result += "1";
                    else result += "0";
                }
                break;
            case "OR":
                if (!(voltage_codes.Length >= accepted_length_between[0] && voltage_codes.Length <= accepted_length_between[1])) return "Kapu / változó hiba, próbáld újra!";
                for (int i = 0; i < voltage_codes[0].Length; i++)
                {
                    if (voltage_codes[0][i].ToString().Equals("E") || voltage_codes[1][i].ToString().Equals("E")) result += "E";
                    else if (voltage_codes[0][i].ToString().Equals("1") || voltage_codes[1][i].ToString().Equals("1")) result += "1";
                    else result += "0";
                }
                break;
            case "NOR":
                if (!(voltage_codes.Length >= accepted_length_between[0] && voltage_codes.Length <= accepted_length_between[1])) return "Kapu / változó hiba, próbáld újra!";
                for (int i = 0; i < voltage_codes[0].Length; i++)
                {
                    if (voltage_codes[0][i].ToString().Equals("E") || voltage_codes[1][i].ToString().Equals("E")) result += "E";
                    else if (voltage_codes[0][i].ToString().Equals("1") || voltage_codes[1][i].ToString().Equals("1")) result += "0";
                    else result += "1";
                }
                break;
            case "NAND":
                if (!(voltage_codes.Length >= accepted_length_between[0] && voltage_codes.Length <= accepted_length_between[1])) return "Kapu / változó hiba, próbáld újra!";
                for (int i = 0; i < voltage_codes[0].Length; i++)
                {
                    if (voltage_codes[0][i].ToString().Equals("E") || voltage_codes[1][i].ToString().Equals("E")) result += "E";
                    else if (voltage_codes[0][i].ToString().Equals("1") && voltage_codes[1][i].ToString().Equals("1")) result += "0";
                    else result += "1";
                }
                break;
            case "XOR":
                // TODO
                break;
            case "XNOR":
                // TODO
                break;
        }

        return result;
    }
    
    public static void Main (string[] args)
    {
        Console.WriteLine("Üdv, kérlek add meg az elsősorban milyen kaput alkalmazzunk a számításokhoz, majd a változók neveit is. Figyelj arra, hogy ebben a sorban minden információ szóközzel legyen elválasztva! Pl.: NOT A C\r\n");
        Console.WriteLine("A következő sorokban kérlek add meg a fent használt változók értékeit. A helyes formátum: NÉV ÉRTÉK. Pl.: A 1212121212");

        string gate = "";
        List<string> gate_values = new List<string>();
        List<string> keys = new List<string>();
        List<string> input_lines = new List<string>();
        var lines = new List<VoltageKeyValueObject>();
        string line = Console.ReadLine();
        //if (String.IsNullOrWhiteSpace(line)) { Environment.Exit(1); }
        while (!String.IsNullOrWhiteSpace(line))
        {
            input_lines.Add(line);
            line = Console.ReadLine();
        }

        if (input_lines.Count > 0)
        {
            for (int i = 0; i < input_lines.Count; i++)
            {
                if (i == 0)
                {
                    string[] first_line = input_lines[i].ToUpper().Split(" ");
                    gate = first_line[0];
                    for (int j = 1; j < first_line.Length; j++) keys.Add(first_line[j]);
                }
                else
                {
                    string[] row = input_lines[i].ToUpper().Split(" ");
                    lines.Add(new VoltageKeyValueObject { key = row[0], value = row[1] });
                }
            }
        }

        foreach (string key in keys)
        {
            var x = lines.FirstOrDefault(line => line.key == key);
            if (x != null) gate_values.Add(DecodeInputVoltages(x.value));
        }

        Console.WriteLine(EvaluateGate(gate, gate_values.ToArray()));

        Console.ReadKey();
    }
}