using System;
using System.Collections.Generic;
using CybersecurityChatbot.Core.Models;

namespace CybersecurityChatbot.Core.Services
{
    public delegate string ResponseEnhancer(string userInput, string currentResponse, ChatState state);

    public class ChatbotEngine
    {
        private readonly Random _random = new Random();
        private readonly ChatState _state = new ChatState();

        public ChatState State => _state;

        public ResponseEnhancer? Enhancer { get; set; }

        private readonly Dictionary<string, List<string>> _topicResponses =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["password"] = new List<string>
            {
                "Use strong, unique passwords for each account. Avoid using personal details.",
                "A strong password should be long, unique, and hard to guess. Consider using a passphrase.",
                "Do not reuse the same password on multiple accounts. One breach can expose everything."
            },
                ["phishing"] = new List<string>
            {
                "Be cautious of emails asking for urgent action or personal information.",
                "Always check the sender's email address and avoid clicking suspicious links.",
                "Phishing messages often create panic. Slow down and verify before responding."
            },
                ["privacy"] = new List<string>
            {
                "Review your privacy settings on social media and limit what you share publicly.",
                "Think carefully before posting personal information online.",
                "Use two-factor authentication and regularly check app permissions on your phone."
            },
                ["scam"] = new List<string>
            {
                "Scammers often pretend to be trusted organisations. Verify before acting.",
                "Never send money or personal details because of pressure or fear.",
                "If an offer sounds too good to be true, it probably is."
            },
                ["safe browsing"] = new List<string>
            {
                "Only visit trusted websites and look for HTTPS before entering sensitive information.",
                "Avoid downloading files from unknown sites or pop-up ads.",
                "Keep your browser and antivirus updated for better protection."
            }
            };

        private readonly Dictionary<string, string> _followUpResponses =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["password"] = "Another password tip: use a password manager so you do not need to remember every password yourself.",
                ["phishing"] = "Another phishing tip: hover over links before clicking so you can inspect the real destination.",
                ["privacy"] = "Another privacy tip: regularly review which apps have access to your camera, mic, and location.",
                ["scam"] = "Another scam tip: if someone pressures you to act immediately, pause and verify through an official channel.",
                ["safe browsing"] = "Another safe browsing tip: avoid saving sensitive passwords in browsers on shared computers."
            };

        public void SetUserName(string name)
        {
            _state.User.Name = name.Trim();
        }

        public string ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return ApplyEnhancer(input, "I didn’t quite understand that. Could you rephrase?");
            }

            string lower = input.Trim().ToLower();

            if (TryRememberInterest(lower, out string rememberResponse))
            {
                return ApplyEnhancer(input, rememberResponse);
            }

            if (lower.Contains("what do you remember") || lower.Contains("remember about me"))
            {
                if (!string.IsNullOrWhiteSpace(_state.User.Name) && !string.IsNullOrWhiteSpace(_state.User.FavouriteTopic))
                {
                    return ApplyEnhancer(input,
                        $"I remember that your name is {_state.User.Name} and that you're interested in {_state.User.FavouriteTopic}.");
                }

                if (!string.IsNullOrWhiteSpace(_state.User.Name))
                {
                    return ApplyEnhancer(input,
                        $"I remember that your name is {_state.User.Name}, but you haven’t told me your favourite cybersecurity topic yet.");
                }

                if (!string.IsNullOrWhiteSpace(_state.User.FavouriteTopic))
                {
                    return ApplyEnhancer(input,
                        $"I remember that you're interested in {_state.User.FavouriteTopic}.");
                }

                return ApplyEnhancer(input, "I do not remember much yet. Tell me your name or your favourite cybersecurity topic.");
            }

            if (lower.Contains("what is my name") || lower.Contains("do you know my name"))
            {
                if (!string.IsNullOrWhiteSpace(_state.User.Name))
                {
                    return ApplyEnhancer(input, $"Yes, your name is {_state.User.Name}.");
                }

                return ApplyEnhancer(input, "I do not know your name yet. Please enter it first.");
            }

            if (lower.Contains("how are you"))
            {
                return ApplyEnhancer(input, "I’m doing well, thank you. I’m here to help you stay safe online.");
            }

            if (lower.Contains("what is your purpose") || lower.Contains("what's your purpose"))
            {
                return ApplyEnhancer(input, "My purpose is to help South African citizens learn about cybersecurity in a simple and practical way.");
            }

            if (lower.Contains("what can i ask you about"))
            {
                return ApplyEnhancer(input, "You can ask me about password safety, phishing, scams, privacy, and safe browsing.");
            }

            if (IsFollowUp(lower) && !string.IsNullOrWhiteSpace(_state.CurrentTopic))
            {
                return ApplyEnhancer(input, _followUpResponses[_state.CurrentTopic]);
            }

            if (TryDetectTopic(lower, out string topic))
            {
                _state.CurrentTopic = topic;
                string response = GetRandomResponse(_topicResponses[topic]);
                return ApplyEnhancer(input, response);
            }

            return ApplyEnhancer(input,
                "I’m not sure I understand. Try asking about password safety, phishing, scams, privacy, or safe browsing.");
        }

        private string ApplyEnhancer(string userInput, string response)
        {
            if (Enhancer != null)
            {
                return Enhancer(userInput, response, _state);
            }

            return response;
        }

        private bool TryRememberInterest(string input, out string response)
        {
            response = "";

            if ((input.Contains("i'm interested in") ||
                 input.Contains("i am interested in") ||
                 input.Contains("my favourite topic is") ||
                 input.Contains("my favorite topic is"))
                && TryDetectTopic(input, out string topic))
            {
                _state.User.FavouriteTopic = topic;
                _state.CurrentTopic = topic;
                response = $"Great! I’ll remember that you’re interested in {topic}. {GetRandomResponse(_topicResponses[topic])}";
                return true;
            }

            return false;
        }

        private bool TryDetectTopic(string input, out string topic)
        {
            topic = "";

            if (input.Contains("password"))
            {
                topic = "password";
                return true;
            }

            if (input.Contains("phishing"))
            {
                topic = "phishing";
                return true;
            }

            if (input.Contains("privacy"))
            {
                topic = "privacy";
                return true;
            }

            if (input.Contains("scam"))
            {
                topic = "scam";
                return true;
            }

            if (input.Contains("safe browsing") || input.Contains("browser") || input.Contains("link") || input.Contains("website"))
            {
                topic = "safe browsing";
                return true;
            }

            return false;
        }

        private bool IsFollowUp(string input)
        {
            return input.Contains("tell me more")
                || input.Contains("another tip")
                || input.Contains("give me another tip")
                || input.Contains("explain more")
                || input.Contains("what else");
        }

        private string GetRandomResponse(List<string> responses)
        {
            int index = _random.Next(responses.Count);
            return responses[index];
        }
    }
}