using System;
using NAudio.Wave;

class Program
{
    static void Main(string[] args)
    {
        // Play voice greeting with NAudio
        try
        {
            using (var audioFile = new AudioFileReader("Staying Safe in Digital World.wav"))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100); // Wait for playback to finish
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error playing audio: {ex.Message}");
        }

        // Display new ASCII art
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("   +-------------------+");
        Console.WriteLine("   | Raihaan's Chatbot |");
        Console.WriteLine("   +-------------------+");
        Console.WriteLine("         _______");
        Console.WriteLine("        /       \\");
        Console.WriteLine("       /_________\\");
        Console.WriteLine("       |  Cyber  |");
        Console.WriteLine("       |  Safe   |");
        Console.WriteLine("       |_________|");
        Console.ResetColor();

        // Welcome message with user name
        Console.WriteLine("\nHello! I'm Raihaans Chatbot! What's your name?");
        string userName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(userName))
        {
            userName = "User"; // Default name if input is empty
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("========================================");
        TypeText($"\tWelcome, {userName}!");
        Console.WriteLine("========================================");
        Console.ResetColor();

        // Main interaction loop
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nWhat would you like to know? (Type 'exit' to quit)");
            Console.ResetColor();
            string input = Console.ReadLine()?.ToLower();

            if (input == "exit")
            {
                TypeText("Goodbye! Stay safe online!");
                break;
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please type something!");
                Console.ResetColor();
                continue;
            }

            // Flexible response system using keyword matching
            if (input.Contains("how") && input.Contains("you"))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("I'm doing great, thanks for asking! How about you?");
                Console.ResetColor();
            }
            else if (input.Contains("purpose") || (input.Contains("what") && input.Contains("do")))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("I'm here to educate you on staying safe online!");
                Console.ResetColor();
            }
            else if (input.Contains("what") && input.Contains("ask"))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("You can ask about password safety, phishing, or safe browsing.");
                Console.ResetColor();
            }
            else if (input.Contains("password") || input.Contains("pass"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Use strong passwords with letters, numbers, and symbols. Don’t reuse them!");
                Console.ResetColor();
            }
            else if (input.Contains("phishing") || input.Contains("email"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Phishing emails trick you into giving info. Check sender addresses carefully!");
                Console.ResetColor();
            }
            else if (input.Contains("browsing") || input.Contains("safe") || input.Contains("link"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Avoid clicking suspicious links and use HTTPS websites.");
                Console.ResetColor();
            }
            else if (input.Contains("malware") || input.Contains("virus") || input.Contains("hack") || input.Contains("infect"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Malware can steal your data or harm your device. Install antivirus software and don’t download files from untrusted sources!");
                Console.ResetColor();
            }
            else if (input.Contains("social") || input.Contains("media") || input.Contains("facebook") || input.Contains("twitter") || input.Contains("instagram"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("On social media, avoid oversharing personal info and use privacy settings to protect your accounts.");
                Console.ResetColor();
            }
            else if (input.Contains("wifi") || input.Contains("network") || input.Contains("public"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Avoid using public Wi-Fi for sensitive tasks unless you’re on a VPN. It’s easy for hackers to snoop on unsecured networks!");
                Console.ResetColor();
            }
            else if (input.Contains("update") || input.Contains("software") || input.Contains("patch"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Keep your software and devices updated—updates often fix security holes that hackers exploit.");
                Console.ResetColor();
            }
            else if (input.Contains("identity") || input.Contains("theft") || input.Contains("personal") || input.Contains("info"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Protect your personal info to avoid identity theft. Don’t share things like your ID number or bank details online carelessly!");
                Console.ResetColor();
            }
            else if (input.Contains("south") && input.Contains("africa"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("In South Africa, cyberattacks like phishing are on the rise. Stay vigilant and report suspicious activity to authorities!");
                Console.ResetColor();
            }
            else if (input.Contains("help") || input.Contains("tip") || input.Contains("advice"))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Need help? I can give tips on passwords, phishing, browsing, and more—just ask!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("I didn’t quite understand that. Try asking about password safety, phishing, or safe browsing!");
                Console.ResetColor();
            }
        }
    }

    // Typing effect method for conversational feel
    static void TypeText(string text)
    {
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(50); // 50ms delay per character
        }
        Console.WriteLine();
    }
}