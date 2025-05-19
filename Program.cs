using System;
using System.Collections.Generic;
using System.Linq;
using System.Media; // Add this for SoundPlayer
using System.Threading;

class Program
{
    // Delegate for response generation
    delegate string ResponseGenerator(string input);

    // Generic collections for keywords, memory, and sentiment
    private static Dictionary<string, List<string>> keywordResponses;
    private static Dictionary<string, string> userMemory;
    private static Dictionary<string, Dictionary<string, string>> sentimentResponses;
    private static Random random;
    private static string currentTopic;

    static void Main(string[] args)
    {
        // Initialize data for Part 2 features
        InitializeData();

        // This is part of the code that has the directory to the location of the audio WAV file for the greeting message and plays the selected audio when starting the chatbot.
        try
        {
            SoundPlayer player = new SoundPlayer("Staying Safe in Digital World.wav");
            player.PlaySync(); // PlaySync blocks until the audio finishes
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error playing audio: {ex.Message}");
        }

        // The part of the code that displays the ASCII image immediately after the audio has finished been played.
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("  ____      _                ____                     _ ");
        Console.WriteLine(" / ___|   _| |__   ___ _ __ / ___|_   _  __ _ _ __ __| |");
        Console.WriteLine("| |  | | | | '_ \\ / _ \\ '__| |  _| | | |/ _` | '__/ _` |");
        Console.WriteLine("| |__| |_| | |_) |  __/ |  | |_| | |_| | (_| | | | (_| |");
        Console.WriteLine(" \\____\\__, |_.__/ \\___|_|   \\____|\\__,_|\\__,_|_|  \\__,_|");
        Console.WriteLine("      |___/                                             ");
        Console.ResetColor();

        // The chatbot first greets you then asks what’s your name is and prompts you to insert your name into the chatbot.
        Console.WriteLine("\nHello! I'm CyberGuard! What's your name?");
        string userName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(userName))
        {
            userName = "User"; // Default name if input is empty
        }
        // Store name in memory for Part 2
        userMemory["name"] = userName;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("========================================");
        TypeText($"\tWelcome, {userName}!");
        Console.WriteLine("========================================");
        Console.ResetColor();

        // Define delegate for response generation
        ResponseGenerator generateResponse = ProcessInput;

        // The chatbot then asked you what you would like to know about, however if you choose to end the chatbot,
        // type ‘exit’ then a goodbye message will be displayed and the chatbot window will close.
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

            // If you click ‘Enter’ without inserting text, the chatbot will give you a warning message to type something.
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please type something!");
                Console.ResetColor();
                continue;
            }

            // Use delegate to generate response for Part 2
            string response = generateResponse(input);
            Console.ForegroundColor = response.StartsWith("I didn’t") ? ConsoleColor.Red : ConsoleColor.Yellow;
            TypeText(response);
            Console.ResetColor();
        }
    }

    // Initialize data structures for Part 2 features
    static void InitializeData()
    {
        random = new Random();
        userMemory = new Dictionary<string, string>();
        currentTopic = string.Empty;

        // Keyword responses with multiple options for randomness
        keywordResponses = new Dictionary<string, List<string>>
        {
            { "password", new List<string>
                {
                    "Use strong passwords with letters, numbers, and symbols. Don’t reuse them!",
                    "Enable two-factor authentication for extra password security.",
                    "Use a password manager to generate and store complex passwords."
                }
            },
            { "phishing", new List<string>
                {
                    "Phishing emails trick you into giving info. Check sender addresses carefully!",
                    "Hover over links to verify their destination before clicking.",
                    "Be wary of emails asking for personal information unexpectedly."
                }
            },
            { "scam", new List<string>
                {
                    "Scammers often pose as trusted organizations. Verify before sharing info.",
                    "Report suspicious calls or emails to authorities.",
                    "Avoid sending money to unsolicited requests."
                }
            },
            { "privacy", new List<string>
                {
                    "Review privacy settings on your accounts to control what you share.",
                    "Use a VPN on public Wi-Fi to protect your data.",
                    "Avoid sharing sensitive info on social media."
                }
            },
            { "malware", new List<string>
                {
                    "Malware can steal data or harm devices. Install antivirus software!",
                    "Don’t download files from untrusted sources.",
                    "Scan your device regularly for malware."
                }
            },
            { "wifi", new List<string>
                {
                    "Avoid public Wi-Fi for sensitive tasks unless using a VPN.",
                    "Hackers can snoop on unsecured networks. Stay cautious!",
                    "Use strong passwords for your home Wi-Fi."
                }
            },
            { "social", new List<string>
                {
                    "On social media, avoid oversharing personal info.",
                    "Use privacy settings to protect your accounts.",
                    "Be cautious of friend requests from unknown profiles."
                }
            }
        };

        // Sentiment responses with topic-specific empathetic messages
        sentimentResponses = new Dictionary<string, Dictionary<string, string>>
        {
            { "worried", new Dictionary<string, string>
                {
                    { "password", "It's completely understandable to feel that way. Weak passwords can be a big risk. Let me share some tips to help you stay safe." },
                    { "phishing", "It's completely understandable to feel that way. Phishing emails can be very deceptive. Let me share some tips to help you stay safe." },
                    { "scam", "It's completely understandable to feel that way. Scammers can be very convincing. Let me share some tips to help you stay safe." },
                    { "privacy", "It's completely understandable to feel that way. Privacy breaches can feel overwhelming. Let me share some tips to help you stay safe." },
                    { "malware", "It's completely understandable to feel that way. Malware can be a serious threat. Let me share some tips to help you stay safe." },
                    { "wifi", "It's completely understandable to feel that way. Public Wi-Fi can be risky. Let me share some tips to help you stay safe." },
                    { "social", "It's completely understandable to feel that way. Social media can expose your data. Let me share some tips to help you stay safe." },
                    { "default", "It's completely understandable to feel that way. Cybersecurity can be complex. Let me share some tips to help you stay safe." }
                }
            },
            { "curious", new Dictionary<string, string>
                {
                    { "password", "That’s a great mindset! Password security is key to staying safe online. Here’s some info to fuel your curiosity." },
                    { "phishing", "That’s a great mindset! Learning about phishing can keep you safe. Here’s some info to fuel your curiosity." },
                    { "scam", "That’s a great mindset! Understanding scams can protect you. Here’s some info to fuel your curiosity." },
                    { "privacy", "That’s a great mindset! Privacy is crucial online. Here’s some info to fuel your curiosity." },
                    { "malware", "That’s a great mindset! Knowing about malware can save your device. Here’s some info to fuel your curiosity." },
                    { "wifi", "That’s a great mindset! Safe Wi-Fi use is important. Here’s some info to fuel your curiosity." },
                    { "social", "That’s a great mindset! Social media safety is vital. Here’s some info to fuel your curiosity." },
                    { "default", "That’s a great mindset! Cybersecurity is fascinating. Here’s some info to fuel your curiosity." }
                }
            },
            { "frustrated", new Dictionary<string, string>
                {
                    { "password", "I hear you, passwords can be tough to manage. Don’t worry, I’m here to help with some simple tips." },
                    { "phishing", "I hear you, phishing can be confusing. Don’t worry, I’m here to help with some clear tips." },
                    { "scam", "I hear you, scams can be overwhelming. Don’t worry, I’m here to help with some practical tips." },
                    { "privacy", "I hear you, privacy settings can be tricky. Don’t worry, I’m here to help with some easy tips." },
                    { "malware", "I hear you, malware can be a headache. Don’t worry, I’m here to help with some straightforward tips." },
                    { "wifi", "I hear you, Wi-Fi security can be complex. Don’t worry, I’m here to help with some useful tips." },
                    { "social", "I hear you, social media can be tricky to secure. Don’t worry, I’m here to help with some simple tips." },
                    { "default", "I hear you, cybersecurity can be overwhelming. Don’t worry, I’m here to help with some clear tips." }
                }
            }
        };
    }

    // Process user input for Part 2 features
    static string ProcessInput(string input)
    {
        // Store favorite topic for memory feature
        if (input.Contains("interested in", StringComparison.OrdinalIgnoreCase) ||
            input.Contains("like to learn", StringComparison.OrdinalIgnoreCase) ||
            input.Contains("i like", StringComparison.OrdinalIgnoreCase))
        {
            foreach (var kw in keywordResponses.Keys) // Changed 'keyword' to 'kw' to avoid conflict
            {
                if (input.Contains(kw, StringComparison.OrdinalIgnoreCase))
                {
                    userMemory["favoriteTopic"] = kw;
                    currentTopic = kw;
                    return $"Got it, {userMemory["name"]}! I'll remember you're interested in {kw}. {GetRandomResponse(kw)}";
                }
            }
            return "I didn’t catch which topic you’re interested in. Try saying something like 'I’m interested in passwords'!";
        }

        // Detect sentiment for Part 2
        string sentiment = sentimentResponses.Keys.FirstOrDefault(s => input.Contains(s, StringComparison.OrdinalIgnoreCase));
        string keyword = keywordResponses.Keys.FirstOrDefault(k => input.Contains(k, StringComparison.OrdinalIgnoreCase));

        // Handle sentiment with keyword
        if (sentiment != null && keyword != null)
        {
            currentTopic = keyword;
            string sentimentResponse = sentimentResponses[sentiment].ContainsKey(keyword)
                ? sentimentResponses[sentiment][keyword]
                : sentimentResponses[sentiment]["default"];
            return $"{sentimentResponse} {GetRandomResponse(keyword)}";
        }
        // Handle sentiment without keyword
        else if (sentiment != null)
        {
            string sentimentResponse = sentimentResponses[sentiment]["default"];
            return $"{sentimentResponse} Try asking about a specific topic like passwords or scams!";
        }

        // Handle keyword without sentiment
        if (keyword != null)
        {
            currentTopic = keyword;
            return GetRandomResponse(keyword);
        }

        // Handle follow-up questions for conversation flow
        if ((input.Contains("more", StringComparison.OrdinalIgnoreCase) ||
             input.Contains("details", StringComparison.OrdinalIgnoreCase) ||
             input.Contains("rephrase", StringComparison.OrdinalIgnoreCase) ||
             input.Contains("understand", StringComparison.OrdinalIgnoreCase) ||
             input.Contains("clarify", StringComparison.OrdinalIgnoreCase)) &&
             !string.IsNullOrEmpty(currentTopic))
        {
            string prefix = input.Contains("rephrase", StringComparison.OrdinalIgnoreCase) ||
                           input.Contains("understand", StringComparison.OrdinalIgnoreCase) ||
                           input.Contains("clarify", StringComparison.OrdinalIgnoreCase)
                ? "Let me put it another way: "
                : $"Here’s more on {currentTopic}: ";
            return $"{prefix}{GetRandomResponse(currentTopic)}";
        }

        // General inquiries from Part 1
        if (input.Contains("how", StringComparison.OrdinalIgnoreCase) && input.Contains("you", StringComparison.OrdinalIgnoreCase))
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            string response = "I'm doing great, thanks for asking! How about you?";
            Console.ResetColor();
            return response;
        }
        if (input.Contains("purpose", StringComparison.OrdinalIgnoreCase) ||
            (input.Contains("what", StringComparison.OrdinalIgnoreCase) && input.Contains("do", StringComparison.OrdinalIgnoreCase)))
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            string response = "I'm here to educate you on staying safe online!";
            Console.ResetColor();
            return response;
        }
        if (input.Contains("what", StringComparison.OrdinalIgnoreCase) && input.Contains("ask", StringComparison.OrdinalIgnoreCase))
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            string response = "You can ask about password safety, phishing, scams, privacy, or other cybersecurity topics!";
            Console.ResetColor();
            return response;
        }

        // Personalized response using memory
        if (userMemory.ContainsKey("favoriteTopic") && random.Next(0, 3) == 0)
        {
            string topic = userMemory["favoriteTopic"];
            return $"Hi {userMemory["name"]}, since you’re interested in {topic}, here’s a tip: {GetRandomResponse(topic)}";
        }

        // If you ask the chatbot about something not related to cyber security, the chatbot will prompt you to ask something that’s to do with cybersecurity.
        return "I didn’t quite understand that. Try asking about password safety, phishing, or privacy!";
    }

    // Get a random response for a keyword
    static string GetRandomResponse(string keyword)
    {
        var responses = keywordResponses[keyword];
        return responses[random.Next(responses.Count)];
    }

    // To give the chatbot a more realistic feel and look, this code delays the output the speed to make it feel more human like.
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