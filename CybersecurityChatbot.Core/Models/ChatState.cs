namespace CybersecurityChatbot.Core.Models
{
    public class ChatState
    {
        public UserProfile User { get; set; } = new UserProfile();
        public string CurrentTopic { get; set; } = "";
    }
}