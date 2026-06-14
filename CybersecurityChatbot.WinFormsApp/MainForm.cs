using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CybersecurityChatbot.WinFormsApp
{
    public partial class MainForm : Form
    {
        private readonly DatabaseService databaseService = new();
        private readonly List<string> activityLog = new();
        private readonly List<CyberTask> tasks = new();
        private readonly List<QuizQuestion> quizQuestions = new();

        private int currentQuestionIndex = 0;
        private int quizScore = 0;
        private bool quizActive = false;

        private TextBox chatBox;
        private TextBox userInputBox;

        private Button sendMessageButton;
        private Button completeTaskButton;
        private Button deleteTaskButton;
        private Button showTasksButton;
        private Button startQuizButton;
        private Button showLogButton;

        private Button answerAButton;
        private Button answerBButton;
        private Button answerCButton;
        private Button answerDButton;

        private ListBox taskListBox;
        private Label statusLabel;
        private Label quizLabel;

        public MainForm()
        {
            InitializeComponent();
            BuildInterface();
            LoadQuizQuestions();
            LoadTasksFromDatabase();

            AddBotMessage("Welcome to the Cybersecurity Awareness Chatbot!");
            AddBotMessage("You can ask about phishing, passwords, privacy, 2FA, tasks, quizzes, or activity logs.");
            AddActivity("Application started.");
        }
        private void LoadTasksFromDatabase()
        {
            try
            {
                tasks.Clear();
                tasks.AddRange(databaseService.GetAllTasks());
                RefreshTaskList();

                statusLabel.Text = $"Status: Loaded {tasks.Count} task(s) from database.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not connect to the MySQL database. Make sure Docker is running and the cyber-mysql container is started.\n\n" + ex.Message,
                    "Database Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void BuildInterface()
        {
            Controls.Clear();

            Text = "Cybersecurity Awareness Chatbot - POE Part 3";
            Size = new Size(1180, 760);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(240, 244, 248);

            var titleLabel = new Label
            {
                Text = "Cybersecurity Awareness Chatbot",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 55, 120),
                Location = new Point(25, 15),
                AutoSize = true
            };
            Controls.Add(titleLabel);

            var subtitleLabel = new Label
            {
                Text = "Task Assistant • Cybersecurity Quiz • NLP Simulation • Activity Log",
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.DimGray,
                Location = new Point(30, 55),
                AutoSize = true
            };
            Controls.Add(subtitleLabel);

            chatBox = new TextBox
            {
                Location = new Point(25, 90),
                Size = new Size(690, 460),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 10),
                BackColor = Color.White
            };
            Controls.Add(chatBox);

            userInputBox = new TextBox
            {
                Location = new Point(25, 565),
                Size = new Size(565, 35),
                Font = new Font("Segoe UI", 10)
            };
            userInputBox.KeyDown += UserInputBox_KeyDown;
            Controls.Add(userInputBox);

            sendMessageButton = new Button
            {
                Text = "Send",
                Location = new Point(600, 565),
                Size = new Size(115, 35),
                BackColor = Color.FromArgb(30, 100, 220),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            sendMessageButton.Click += SendMessageButton_Click;
            Controls.Add(sendMessageButton);

            var taskTitleLabel = new Label
            {
                Text = "Task Assistant",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 55, 120),
                Location = new Point(745, 90),
                AutoSize = true
            };
            Controls.Add(taskTitleLabel);

            taskListBox = new ListBox
            {
                Location = new Point(745, 125),
                Size = new Size(390, 210),
                Font = new Font("Segoe UI", 9)
            };
            Controls.Add(taskListBox);
            completeTaskButton = new Button
            {
                Text = "Complete Task",
                Location = new Point(745, 350),
                Size = new Size(190, 38),
                BackColor = Color.FromArgb(23, 162, 184),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            completeTaskButton.Click += CompleteTaskButton_Click;
            Controls.Add(completeTaskButton);

            deleteTaskButton = new Button
            {
                Text = "Delete Task",
                Location = new Point(945, 350),
                Size = new Size(190, 38),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            deleteTaskButton.Click += DeleteTaskButton_Click;
            Controls.Add(deleteTaskButton);

            showTasksButton = new Button
            {
                Text = "Show Tasks",
                Location = new Point(745, 400),
                Size = new Size(190, 38),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            showTasksButton.Click += ShowTasksButton_Click;
            Controls.Add(showTasksButton);

            showLogButton = new Button
            {
                Text = "Activity Log",
                Location = new Point(945, 400),
                Size = new Size(190, 38),
                BackColor = Color.FromArgb(52, 58, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            showLogButton.Click += ShowLogButton_Click;
            Controls.Add(showLogButton);

            quizLabel = new Label
            {
                Text = "Cybersecurity Quiz",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 55, 120),
                Location = new Point(745, 465),
                AutoSize = true
            };
            Controls.Add(quizLabel);

            startQuizButton = new Button
            {
                Text = "Start Quiz",
                Location = new Point(745, 500),
                Size = new Size(390, 38),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            startQuizButton.Click += StartQuizButton_Click;
            Controls.Add(startQuizButton);

            answerAButton = CreateAnswerButton("A", 745, 550);
            answerBButton = CreateAnswerButton("B", 945, 550);
            answerCButton = CreateAnswerButton("C", 745, 600);
            answerDButton = CreateAnswerButton("D", 945, 600);

            Controls.Add(answerAButton);
            Controls.Add(answerBButton);
            Controls.Add(answerCButton);
            Controls.Add(answerDButton);

            statusLabel = new Label
            {
                Text = "Status: Ready",
                Location = new Point(25, 625),
                Size = new Size(1100, 35),
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.DarkSlateGray
            };
            Controls.Add(statusLabel);
        }

        private Button CreateAnswerButton(string answer, int x, int y)
        {
            var button = new Button
            {
                Text = $"Answer {answer}",
                Tag = answer,
                Location = new Point(x, y),
                Size = new Size(190, 38),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(22, 55, 120),
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };

            button.Click += AnswerButton_Click;
            return button;
        }

        private void SendMessageButton_Click(object? sender, EventArgs e)
        {
            ProcessUserInput();
        }

        private void UserInputBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ProcessUserInput();
                e.SuppressKeyPress = true;
            }
        }

        private void ProcessUserInput()
        {
            string input = userInputBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return;
            }

            AddUserMessage(input);
            userInputBox.Clear();

            string lowerInput = input.ToLower();

            if (quizActive && IsQuizAnswer(lowerInput))
            {
                CheckQuizAnswer(lowerInput.ToUpper());
                return;
            }

            if (lowerInput.Contains("add task") || lowerInput.Contains("create task") || lowerInput.Contains("set task"))
            {
                AddTaskFromInput(input);
            }
            else if (lowerInput.Contains("complete task") || lowerInput.Contains("mark task"))
            {
                CompleteSelectedTask();
            }
            else if (lowerInput.Contains("delete task") || lowerInput.Contains("remove task"))
            {
                DeleteSelectedTask();
            }
            else if (lowerInput.Contains("show tasks") || lowerInput.Contains("view tasks") || lowerInput.Contains("my tasks"))
            {
                ShowTasks();
            }
            else if (lowerInput.Contains("quiz") || lowerInput.Contains("game") || lowerInput.Contains("start quiz"))
            {
                StartQuiz();
            }
            else if (lowerInput.Contains("activity log") || lowerInput.Contains("what have you done") || lowerInput.Contains("show log"))
            {
                ShowActivityLog();
            }
            else if (lowerInput.Contains("phishing"))
            {
                AddBotMessage("Phishing is when attackers trick you into revealing private information. Check the sender, avoid suspicious links, and report suspicious emails.");
                AddActivity("User asked about phishing.");
            }
            else if (lowerInput.Contains("password"))
            {
                AddBotMessage("Use strong passwords with letters, numbers, and symbols. Avoid reusing passwords and enable two-factor authentication.");
                AddActivity("User asked about password safety.");
            }
            else if (lowerInput.Contains("privacy"))
            {
                AddBotMessage("Protect your privacy by reviewing app permissions, limiting personal information online, and using secure account settings.");
                AddActivity("User asked about privacy.");
            }
            else if (lowerInput.Contains("2fa") || lowerInput.Contains("two-factor"))
            {
                AddBotMessage("Two-factor authentication adds another security step after your password, making your account much safer.");
                AddActivity("User asked about two-factor authentication.");
            }
            else if (lowerInput.Contains("safe browsing") || lowerInput.Contains("browser"))
            {
                AddBotMessage("Safe browsing means avoiding suspicious websites, checking HTTPS, keeping your browser updated, and not downloading unknown files.");
                AddActivity("User asked about safe browsing.");
            }
            else
            {
                AddBotMessage("I understand you are asking about cybersecurity. Try asking about phishing, passwords, privacy, 2FA, safe browsing, tasks, quizzes, or activity logs.");
                AddActivity("Bot handled general cybersecurity input.");
            }
        }

        private bool IsQuizAnswer(string input)
        {
            return input == "a" || input == "b" || input == "c" || input == "d";
        }

        private void AddTaskFromInput(string input)
        {
            string title = input
                .Replace("add task", "", StringComparison.OrdinalIgnoreCase)
                .Replace("create task", "", StringComparison.OrdinalIgnoreCase)
                .Replace("set task", "", StringComparison.OrdinalIgnoreCase)
                .Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                title = "Review cybersecurity settings";
            }

            string reminder = "No reminder set";

            if (input.ToLower().Contains("tomorrow"))
            {
                reminder = "Reminder: Tomorrow";
            }
            else if (input.ToLower().Contains("3 days"))
            {
                reminder = "Reminder: In 3 days";
            }
            else if (input.ToLower().Contains("7 days"))
            {
                reminder = "Reminder: In 7 days";
            }

            var task = new CyberTask
            {
                Title = title,
                Description = $"Cybersecurity task: {title}",
                ReminderText = reminder,
                IsCompleted = false
            };
            try
            {
                task.TaskId = databaseService.AddTask(task);
                tasks.Insert(0, task);
                RefreshTaskList();
            }
            catch (Exception ex)
            {
                AddBotMessage("Task could not be saved to the database.");
                MessageBox.Show(ex.Message, "Database Error");
                return;
            }
            AddBotMessage($"Task added: {task.Title}. {task.ReminderText}");
            AddActivity($"Task added: {task.Title}");
        }


        private void CompleteTaskButton_Click(object? sender, EventArgs e)
        {
            CompleteSelectedTask();
        }

        private void CompleteSelectedTask()
        {
            if (taskListBox.SelectedIndex < 0)
            {
                AddBotMessage("Please select a task from the task list before marking it as complete.");
                return;
            }

            var task = tasks[taskListBox.SelectedIndex];

            try
            {
                databaseService.CompleteTask(task.TaskId);
                task.IsCompleted = true;

                RefreshTaskList();

                AddBotMessage($"Task completed: {task.Title}");
                AddActivity($"Task completed: {task.Title}");
            }
            catch (Exception ex)
            {
                AddBotMessage("Task could not be marked as complete in the database.");
                MessageBox.Show(ex.Message, "Database Error");
            }
        }

        private void DeleteTaskButton_Click(object? sender, EventArgs e)
        {
            DeleteSelectedTask();
        }

        private void DeleteSelectedTask()
        {
            if (taskListBox.SelectedIndex < 0)
            {
                AddBotMessage("Please select a task from the task list before deleting it.");
                return;
            }

            var task = tasks[taskListBox.SelectedIndex];

            try
            {
                databaseService.DeleteTask(task.TaskId);
                tasks.RemoveAt(taskListBox.SelectedIndex);

                RefreshTaskList();

                AddBotMessage($"Task deleted: {task.Title}");
                AddActivity($"Task deleted: {task.Title}");
            }
            catch (Exception ex)
            {
                AddBotMessage("Task could not be deleted from the database.");
                MessageBox.Show(ex.Message, "Database Error");
            }
        }

        private void ShowTasksButton_Click(object? sender, EventArgs e)
        {
            ShowTasks();
        }

        private void ShowTasks()
        {
            if (!tasks.Any())
            {
                AddBotMessage("You do not have any cybersecurity tasks yet.");
                return;
            }

            AddBotMessage("Here are your current cybersecurity tasks:");

            foreach (var task in tasks)
            {
                string status = task.IsCompleted ? "Completed" : "Pending";
                AddBotMessage($"- {task.Title} | {task.ReminderText} | Status: {status}");
            }

            AddActivity("User viewed cybersecurity tasks.");
        }

        private void RefreshTaskList()
        {
            taskListBox.Items.Clear();

            foreach (var task in tasks)
            {
                string status = task.IsCompleted ? "Completed" : "Pending";
                taskListBox.Items.Add($"{task.Title} | {status} | {task.ReminderText}");
            }

            statusLabel.Text = $"Status: {tasks.Count} task(s) loaded.";
        }

        private void LoadQuizQuestions()
        {
            quizQuestions.Clear();

            quizQuestions.Add(new QuizQuestion(
                "What should you do if you receive an email asking for your password?",
                "Reply with your password",
                "Click the link immediately",
                "Report the email as phishing",
                "Forward it to friends",
                "C",
                "Correct. Reporting phishing emails helps prevent scams."));

            quizQuestions.Add(new QuizQuestion(
                "Which password is the strongest?",
                "password123",
                "Daniel2008",
                "12345678",
                "T!ger#Cloud92",
                "D",
                "Correct. A strong password uses a mix of letters, numbers, and symbols."));

            quizQuestions.Add(new QuizQuestion(
                "What does 2FA stand for?",
                "Two-Factor Authentication",
                "Two-File Access",
                "Fast Firewall Access",
                "Final File Approval",
                "A",
                "Correct. 2FA adds an extra layer of security."));

            quizQuestions.Add(new QuizQuestion(
                "True or False: You should use the same password for all accounts.",
                "True",
                "False",
                "Only for school accounts",
                "Only for social media",
                "B",
                "Correct. Reusing passwords is risky because one breach can affect many accounts."));

            quizQuestions.Add(new QuizQuestion(
                "What is phishing?",
                "A type of computer hardware",
                "A scam that tricks users into giving private information",
                "A secure login method",
                "A backup method",
                "B",
                "Correct. Phishing is a social engineering attack."));

            quizQuestions.Add(new QuizQuestion(
                "What should you check before clicking a link?",
                "The sender and the website address",
                "Only the colour of the email",
                "The number of images",
                "The font size",
                "A",
                "Correct. Always check the sender and URL before clicking links."));

            quizQuestions.Add(new QuizQuestion(
                "What is malware?",
                "Software designed to protect you",
                "Software designed to harm or exploit systems",
                "A type of keyboard",
                "A Wi-Fi password",
                "B",
                "Correct. Malware is malicious software."));

            quizQuestions.Add(new QuizQuestion(
                "Which action improves privacy?",
                "Sharing your location with every app",
                "Posting all personal details online",
                "Reviewing app permissions regularly",
                "Using public Wi-Fi for banking",
                "C",
                "Correct. Reviewing app permissions helps protect your privacy."));

            quizQuestions.Add(new QuizQuestion(
                "What should you do on public Wi-Fi?",
                "Access sensitive accounts without protection",
                "Avoid sensitive transactions or use a VPN",
                "Disable your firewall",
                "Share your passwords",
                "B",
                "Correct. Public Wi-Fi can be risky, so avoid sensitive activity or use a VPN."));

            quizQuestions.Add(new QuizQuestion(
                "Why are software updates important?",
                "They only change colours",
                "They remove all passwords",
                "They fix security vulnerabilities",
                "They make phishing safe",
                "C",
                "Correct. Updates often patch security weaknesses."));

            quizQuestions.Add(new QuizQuestion(
                "What is social engineering?",
                "Using social media for fun",
                "Manipulating people into revealing information",
                "Designing websites",
                "Installing antivirus software",
                "B",
                "Correct. Social engineering targets human behaviour."));

            quizQuestions.Add(new QuizQuestion(
                "What should you do if you suspect your account was hacked?",
                "Ignore it",
                "Change the password and enable 2FA",
                "Post your password online",
                "Delete your computer",
                "B",
                "Correct. Changing the password and enabling 2FA helps secure the account."));
        }

        private void StartQuizButton_Click(object? sender, EventArgs e)
        {
            StartQuiz();
        }

        private void StartQuiz()
        {
            quizActive = true;
            quizScore = 0;
            currentQuestionIndex = 0;

            EnableQuizButtons(true);
            ShowCurrentQuestion();

            AddActivity("Cybersecurity quiz started.");
        }

        private void ShowCurrentQuestion()
        {
            if (currentQuestionIndex >= quizQuestions.Count)
            {
                EndQuiz();
                return;
            }

            var question = quizQuestions[currentQuestionIndex];

            AddBotMessage($"Question {currentQuestionIndex + 1} of {quizQuestions.Count}: {question.QuestionText}");
            AddBotMessage($"A) {question.OptionA}");
            AddBotMessage($"B) {question.OptionB}");
            AddBotMessage($"C) {question.OptionC}");
            AddBotMessage($"D) {question.OptionD}");
            AddBotMessage("Choose A, B, C, or D by typing it or clicking an answer button.");

            statusLabel.Text = $"Quiz active: Question {currentQuestionIndex + 1} of {quizQuestions.Count}";
        }

        private void AnswerButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is string answer)
            {
                CheckQuizAnswer(answer);
            }
        }

        private void CheckQuizAnswer(string selectedAnswer)
        {
            if (!quizActive)
            {
                AddBotMessage("No quiz is active. Click Start Quiz first.");
                return;
            }

            var question = quizQuestions[currentQuestionIndex];

            if (selectedAnswer.Equals(question.CorrectAnswer, StringComparison.OrdinalIgnoreCase))
            {
                quizScore++;
                AddBotMessage($"Correct! {question.Feedback}");
            }
            else
            {
                AddBotMessage($"Incorrect. The correct answer is {question.CorrectAnswer}. {question.Feedback}");
            }

            currentQuestionIndex++;

            if (currentQuestionIndex >= quizQuestions.Count)
            {
                EndQuiz();
            }
            else
            {
                ShowCurrentQuestion();
            }
        }

        private void EndQuiz()
        {
            quizActive = false;
            EnableQuizButtons(false);

            AddBotMessage($"Quiz completed! Your final score is {quizScore}/{quizQuestions.Count}.");

            double percentage = (double)quizScore / quizQuestions.Count * 100;

            if (percentage >= 80)
            {
                AddBotMessage("Excellent work! You have strong cybersecurity awareness.");
            }
            else if (percentage >= 50)
            {
                AddBotMessage("Good effort. Keep learning and reviewing cybersecurity safety tips.");
            }
            else
            {
                AddBotMessage("Keep practising. Cybersecurity awareness improves with regular learning.");
            }

            AddActivity($"Quiz completed with score {quizScore}/{quizQuestions.Count}.");
            statusLabel.Text = "Status: Quiz completed.";
        }

        private void EnableQuizButtons(bool enabled)
        {
            answerAButton.Enabled = enabled;
            answerBButton.Enabled = enabled;
            answerCButton.Enabled = enabled;
            answerDButton.Enabled = enabled;
        }

        private void ShowLogButton_Click(object? sender, EventArgs e)
        {
            ShowActivityLog();
        }

        private void ShowActivityLog()
        {
            try
            {
                var logsFromDatabase = databaseService.GetRecentActivityLogs();

                if (!logsFromDatabase.Any())
                {
                    AddBotMessage("No activity has been recorded yet.");
                    return;
                }

                AddBotMessage("Here is a summary of recent actions:");

                foreach (var log in logsFromDatabase)
                {
                    AddBotMessage($"- {log}");
                }
            }
            catch
            {
                if (!activityLog.Any())
                {
                    AddBotMessage("No activity has been recorded yet.");
                    return;
                }

                AddBotMessage("Here is a summary of recent actions:");

                foreach (var log in activityLog.TakeLast(10))
                {
                    AddBotMessage($"- {log}");
                }
            }
        }

        private void AddUserMessage(string message)
        {
            chatBox.AppendText($"User: {message}{Environment.NewLine}");
        }

        private void AddBotMessage(string message)
        {
            chatBox.AppendText($"Chatbot: {message}{Environment.NewLine}{Environment.NewLine}");
        }

        private void AddActivity(string action)
        {
            string log = $"{DateTime.Now:yyyy-MM-dd HH:mm} - {action}";
            activityLog.Add(log);

            try
            {
                databaseService.AddActivityLog(action);
            }
            catch
            {
                // Keeps the app running even if activity logging fails.
            }
        }
    }

    public class CyberTask
    {
        public int TaskId { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public string ReminderText { get; set; } = "";

        public bool IsCompleted { get; set; }
    }

    public class QuizQuestion
    {
        public string QuestionText { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectAnswer { get; set; }
        public string Feedback { get; set; }

        public QuizQuestion(
            string questionText,
            string optionA,
            string optionB,
            string optionC,
            string optionD,
            string correctAnswer,
            string feedback)
        {
            QuestionText = questionText;
            OptionA = optionA;
            OptionB = optionB;
            OptionC = optionC;
            OptionD = optionD;
            CorrectAnswer = correctAnswer;
            Feedback = feedback;
        }
    }
}