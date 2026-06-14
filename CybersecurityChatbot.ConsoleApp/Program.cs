using System;
using System.Threading;
using CybersecurityChatbot.Core.Services;

namespace CybersecurityChatbot.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChatbotEngine engine = new ChatbotEngine();

            ShowHeader();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Please enter your name: ");
            Console.ResetColor();

            string name = Console.ReadLine() ?? "";
            while (string.IsNullOrWhiteSpace(name))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Name cannot be empty. Please enter your name: ");
                Console.ResetColor();
                name = Console.ReadLine() ?? "";
            }

            engine.SetUserName(name);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nHello, {name}! Welcome to the Cybersecurity Awareness Assistant.");
            Console.WriteLine("Ask me about password safety, phishing, scams, privacy, or safe browsing.");
            Console.WriteLine("Type 'exit' to close the chatbot.\n");
            Console.ResetColor();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("You: ");
                Console.ResetColor();

                string userInput = Console.ReadLine() ?? "";

                if (userInput.Trim().ToLower() == "exit" || userInput.Trim().ToLower() == "bye")
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Bot: Goodbye! Stay safe online.");
                    Console.ResetColor();
                    break;
                }

                string response = engine.ProcessInput(userInput);

                Console.ForegroundColor = ConsoleColor.Magenta;
                TypeEffect("Bot: " + response);
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        static void ShowHeader()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(AsciiArt.GetLogo());
            Console.ResetColor();
        }

        static void TypeEffect(string message)
        {
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(15);
            }
            Console.WriteLine();
        }
    }
}