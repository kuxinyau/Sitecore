public class Palindrome
{
    public static bool IsPalindrome(string inputString, string trashSymbolString)
    {
        // check input string is null or empty
        if (string.IsNullOrEmpty(inputString))
        {
            Console.WriteLine("\n\nInput String is null or empty");
            return false;
        }

        int left = 0;   // left most index
        int right = inputString.Length - 1; // right most index

        // loop to compare one by one
        while (left < right)
        {
            while (left < right && trashSymbolString.Contains(inputString[left]))
            {
                left++;     // ignore trash and check next left index
            }

            while (left < right && trashSymbolString.Contains(inputString[right]))
            {
                right--;    // ignore trash and check next right index
            }

            if (char.ToLower(inputString[left++]) != char.ToLower(inputString[right--]))    // ignore case & move to next comparison
            {
                return false;   // left right does not match
            }
        }

        return true;    // true without finding any mismatched character
    }

    public static void Main(string[] args)
    {
        // example
        string inputString1 = "a@b!!b$a";
        Console.WriteLine("Input String: " + inputString1);
        string trashSymbolsString1 = "!@$";
        Console.WriteLine("Trash Symbol String: " + trashSymbolsString1);
        Console.WriteLine("Result: " + IsPalindrome(inputString1, trashSymbolsString1) + "\n\n"); // Output: true

        string inputString2 = "?Aa#c";
        Console.WriteLine("Input String: " + inputString2);
        string trashSymbolsString2 = "#?";
        Console.WriteLine("Trash Symbol String: " + trashSymbolsString2);
        Console.WriteLine("Result: " + IsPalindrome(inputString2, trashSymbolsString2) + "\n\n"); // Output: false

        // check by ourselves
        while (true)
        {
            Console.WriteLine("Input String: ");
            string inputString = Console.ReadLine();

            // stop checking
            if (inputString.ToLower().Equals("stop"))
                break;

            Console.WriteLine("\nTrash Symbol String: ");
            string trashSymbolString = Console.ReadLine();

            Console.WriteLine("\nResult: " + IsPalindrome(inputString, trashSymbolString) + "\n\n");
        }

        Console.ReadLine();
    }
}
