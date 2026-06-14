
Cybersecurity Awareness Chatbot - PROG6221 POE
Project Overview

This project is a GUI-based Cybersecurity Awareness Chatbot developed for the PROG6221 Portfolio of Evidence. The purpose of the application is to educate users about important cybersecurity topics while allowing them to interact with the chatbot through a graphical user interface.

The chatbot provides information about phishing, password safety, two-factor authentication, privacy, malware, safe browsing, and social engineering. The final version also includes a task assistant, a cybersecurity quiz, simulated Natural Language Processing, MySQL database storage, and an activity log.

This project is not a console-only application. The final POE version uses a Windows Forms GUI with buttons, text boxes, task lists, quiz controls, and activity log functionality.

Project Structure

The solution contains three main projects:

CybersecurityChatbot.ConsoleApp
CybersecurityChatbot.Core
CybersecurityChatbot.WinFormsApp

CybersecurityChatbot.ConsoleApp

This project contains the earlier console-based chatbot work from the previous parts of the assignment.

CybersecurityChatbot.Core

This project contains reusable chatbot logic and supporting classes used from the earlier chatbot implementation.

CybersecurityChatbot.WinFormsApp

This is the final GUI application for the POE. It contains the Windows Forms interface, chatbot interaction, task assistant, quiz feature, MySQL database integration, NLP simulation, and activity log.

Main Features
1. GUI-Based Chatbot

The application uses a Windows Forms graphical user interface. The user can type messages into an input box and receive chatbot responses in a chat display area.

The GUI includes:

Chat display area
User input box
Send button
Task list
Complete task button
Delete task button
Quiz start button
Quiz answer buttons
Activity log button
Status label

The GUI makes the chatbot easier to use and meets the POE requirement that the final application must be graphical and not console-based.

2. Cybersecurity Task Assistant

The task assistant helps users create and manage cybersecurity-related tasks.

Users can enter commands such as:

add task enable two-factor authentication tomorrow
add task review privacy settings in 7 days
show tasks
complete task
delete task

The task assistant supports:

Adding cybersecurity tasks
Viewing existing tasks
Setting reminder text
Marking tasks as complete
Deleting tasks
Saving tasks to the database
Loading saved tasks when the app starts again

Example task:

Title: Enable two-factor authentication
Description: Cybersecurity task: Enable two-factor authentication
Reminder: Tomorrow
Status: Pending

3. MySQL Database Integration

The application uses a MySQL database to store cybersecurity tasks and activity logs. MySQL is run through Docker, so MySQL Workbench is not required.

The database name is:

cybersecurity_chatbot_db

The application uses two main database tables:

CyberTasks
ActivityLogs

The CyberTasks table stores:

Task ID
Task title
Task description
Reminder text
Completion status
Created date

The ActivityLogs table stores:

Log ID
Action description
Date and time of the action

This means that tasks and logs remain available even after the application is closed and reopened.

4. Cybersecurity Mini-Game Quiz

The chatbot includes a cybersecurity quiz that tests the user’s knowledge of basic cybersecurity concepts.

The quiz includes more than 10 questions and covers topics such as:

Phishing
Password safety
Two-factor authentication
Malware
Public Wi-Fi safety
Software updates
Social engineering
Privacy settings
Safe browsing

The quiz provides:

One question at a time
Multiple-choice answers
Immediate feedback after each answer
Score tracking
Final score at the end
Encouraging feedback based on the user’s score

Example question:

Question: What should you do if you receive an email asking for your password?

A) Reply with your password
B) Click the link immediately
C) Report the email as phishing
D) Forward it to friends

Correct answer: C

Feedback: Correct. Reporting phishing emails helps prevent scams.

5. NLP Simulation

The chatbot includes a simple Natural Language Processing simulation. It does not use advanced artificial intelligence, but it recognises keywords and different user phrases using string matching.

The chatbot can detect phrases such as:

add task
create task
set task
show tasks
view tasks
my tasks
complete task
delete task
start quiz
play game
show activity log
what have you done
phishing
password
privacy
2FA
safe browsing

This allows the chatbot to respond to different wording instead of only one exact command.

Example:

User: Can you add task review privacy settings tomorrow?
Chatbot: Task added: review privacy settings tomorrow.

6. Activity Log Feature

The activity log records important actions that happen in the chatbot.

The activity log records actions such as:

Application started
Task added
Task completed
Task deleted
Quiz started
Quiz completed
User asked about phishing
User asked about password safety
User viewed tasks

The user can view the activity log by clicking the Activity Log button or by typing:

show activity log
what have you done

The chatbot then displays the most recent actions in a clear list.

7. Cybersecurity Awareness Responses

The chatbot responds to common cybersecurity topics.

Phishing

The chatbot explains that phishing is when attackers trick users into revealing private information, such as passwords or banking details.

Password Safety

The chatbot advises users to use strong passwords, avoid reusing passwords, and enable two-factor authentication.

Privacy

The chatbot explains the importance of reviewing app permissions and limiting the amount of personal information shared online.

Two-Factor Authentication

The chatbot explains that two-factor authentication adds an extra security step after the password, making accounts more secure.

Safe Browsing

The chatbot advises users to check website links, avoid suspicious downloads, and keep browsers updated.

How to Run the Project
Requirements

To run this application, you need:

Visual Studio
.NET installed
Docker Desktop
MySql.Data NuGet package installed in the WinForms project
Running MySQL with Docker

The project uses a MySQL Docker container for database storage.

Start Docker Desktop first.

Then open Command Prompt and run:

docker start cyber-mysql

If the container does not exist yet, create it using:

docker run --name cyber-mysql -e MYSQL_ROOT_PASSWORD=CyberPass123! -e MYSQL_DATABASE=cybersecurity_chatbot_db -p 3306:3306 -d mysql:8.0

To check that the MySQL container is running, use:

docker ps

You should see a container named:

cyber-mysql

Running the WinForms Application
Open the solution in Visual Studio.
Right-click CybersecurityChatbot.WinFormsApp.
Select Set as Startup Project.
Make sure Docker Desktop is running.
Make sure the cyber-mysql container is running.
Press the green Run button in Visual Studio.

The GUI chatbot window should open.

How to Use the Chatbot

Example commands:

What is phishing?
Tell me about password safety
What is 2FA?
How can I protect my privacy?
add task enable two-factor authentication tomorrow
show tasks
start quiz
show activity log

Testing the Database

To test that the database is working:

Start the MySQL Docker container.
Run the WinForms application.
Add a task using the chatbot.
Close the application.
Open the application again.
Check that the task is still displayed.

If the task is still there, the MySQL database persistence is working correctly.

GitHub Requirements

This repository includes:

Complete source code
GUI application
Core chatbot logic
README file
MySQL database integration
Task assistant
Quiz feature
NLP simulation
Activity log feature
Meaningful commits
Release tags

Required release tags:

v1.0-gui-task-assistant
v2.0-quiz-nlp
v3.0-final-poe

Video Presentation

The video demonstration should show:

The GitHub repository.
The project structure in Visual Studio.
The WinForms GUI running.
The chatbot responding to cybersecurity questions.
Adding a task.
Saving and loading tasks from the MySQL database.
Completing a task.
Deleting a task.
Starting the quiz.
Answering quiz questions.
Showing the final quiz score.
Demonstrating NLP keyword detection.
Showing the activity log.
Explaining how the database works.
Explaining the code structure.
Challenges Faced

One challenge in this project was moving from a console-based chatbot to a full graphical user interface. The GUI needed to include controls for chatting, task management, quiz interaction, and activity logging while still keeping the chatbot easy to use.

Another challenge was database integration. Instead of using MySQL Workbench, MySQL was run through Docker. The application had to connect to the Docker MySQL container, create the database and tables, and then save and load tasks correctly.

The quiz and NLP simulation also required careful logic so that the chatbot could understand different user inputs and provide useful cybersecurity responses.
