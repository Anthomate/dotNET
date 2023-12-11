namespace consoleProject;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        List<object> myList = new List<object>
            {
                1, // 1
                0, // 1
                1, // 2
                new List<object> { 1, 1, 1 }, // 5
            };

        int result = Sum(myList);

        Console.WriteLine("Sum: " + result);
    }

    public static int Sum(IEnumerable<object> values)
    {
        int sum = 0;
        foreach (object value in values)
        {
            switch (value)
            {
                case 0:
                    break; // Si value = 0 on ne fait rien, on peut le supprimer pour simplifier le code
                case 1:
                    sum += 1; // Si value = 1 on ajoute 1
                    break;
                case IEnumerable<object> subList when subList.Any():
                    sum += Sum(subList); // On fait pareil qu'avant mais on va chercher dans une sous liste
                    break;
            }
        }
        return sum;
    }
}