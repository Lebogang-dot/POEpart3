using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityAwarenessChatbot.Services
{
    public class NLPService
    {

        private readonly Dictionary<string, List<string>> _intentKeywords =
            new Dictionary<string, List<string>>
        {
            {
                "Greeting",
                new List<string>
                {
                    "hello",
                    "hi",
                    "hey",
                    "good morning",
                    "good afternoon",
                    "good evening"
                }
            },

            {
                "Help",
                new List<string>
                {
                    "help",
                    "assist",
                    "commands",
                    "what can you do",
                    "support"
                }
            },

            {
                "Password",
                new List<string>
                {
                    "password",
                    "strong password",
                    "secure password",
                    "password safety",
                    "forgot password"
                }
            },

            {
                "Phishing",
                new List<string>
                {
                    "phishing",
                    "fake email",
                    "email scam",
                    "scam",
                    "fraud email"
                }
            },

            {
                "SafeBrowsing",
                new List<string>
                {
                    "browser",
                    "safe browsing",
                    "safe website",
                    "internet safety",
                    "https"
                }
            },

            {
                "Reminder",
                new List<string>
                {
                    "reminder",
                    "remind me",
                    "remember this",
                    "add reminder",
                    "set reminder",
                    "create reminder",
                    "new reminder",
                    "task"
                }
            },

            {
                "Quiz",
                new List<string>
                {
                    "quiz",
                    "start quiz",
                    "begin quiz",
                    "test me",
                    "questions"
                }
            },

            {
                "ActivityLog",
                new List<string>
                {
                    "activity",
                    "activity log",
                    "show activity",
                    "history",
                    "recent activity"
                }
            },

            {
                "TwoFactor",
                new List<string>
                {
                    "2fa",
                    "two factor",
                    "two-factor",
                    "authentication"
                }
            },

            {
                "Goodbye",
                new List<string>
                {
                    "bye",
                    "goodbye",
                    "see you",
                    "exit"
                }
            }
        };

        public string DetectIntent(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "Unknown";

            string input = userInput.ToLower().Trim();

            foreach (var intent in _intentKeywords)
            {
                if (intent.Value.Any(keyword => input.Contains(keyword)))
                {
                    return intent.Key;
                }
            }

            return "Unknown";
        }
    }
    using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityAwarenessChatbot.Services
    {
        public class SentimentService
        {
            /
            private readonly Dictionary<string, List<string>> _sentimentWords =
                new Dictionary<string, List<string>>
            {
            {
                "Happy",
                new List<string>
                {
                    "happy",
                    "great",
                    "awesome",
                    "excellent",
                    "good",
                    "fantastic",
                    "amazing",
                    "excited"
                }
            },

            {
                "Sad",
                new List<string>
                {
                    "sad",
                    "depressed",
                    "unhappy",
                    "down",
                    "upset",
                    "crying"
                }
            },

            {
                "Angry",
                new List<string>
                {
                    "angry",
                    "mad",
                    "annoyed",
                    "furious",
                    "irritated"
                }
            },

            {
                "Worried",
                new List<string>
                {
                    "worried",
                    "nervous",
                    "afraid",
                    "anxious",
                    "scared",
                    "concerned"
                }
            },

            {
                "Confused",
                new List<string>
                {
                    "confused",
                    "don't understand",
                    "lost",
                    "unsure",
                    "what"
                }
            }
            };

            public string DetectSentiment(string userInput)
            {
                if (string.IsNullOrWhiteSpace(userInput))
                    return "Neutral";

                string input = userInput.ToLower();

                foreach (var sentiment in _sentimentWords)
                {
                    if (sentiment.Value.Any(word => input.Contains(word)))
                    {
                        return sentiment.Key;
                    }
                }

                return "Neutral";
            }

            
            public string GetSentimentResponse(string sentiment)
            {
                switch (sentiment)
                {
                    case "Happy":
                        return "😊 I'm glad you're feeling positive today! Let's continue learning about cybersecurity.";

                    case "Sad":
                        return "😔 I'm sorry you're feeling down. I'll do my best to make cybersecurity easier to understand.";

                    case "Angry":
                        return "😐 I understand that you're frustrated. Let's work through the problem together.";

                    case "Worried":
                        return "🛡️ It's normal to feel concerned about online security. I'm here to help keep you informed.";

                    case "Confused":
                        return "🤔 No problem! I'll explain everything step by step.";

                    default:
                        return string.Empty;
                }
            }
        }
        using CyberSecurityAwarenessChatbot.Database;
using CyberSecurityAwarenessChatbot.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace CyberSecurityAwarenessChatbot.Services
        {
            public class ReminderService
            {
             
                private readonly DatabaseHelper databaseHelper;

                public ReminderService()
                {
                    databaseHelper = new DatabaseHelper();
                }
                public bool AddReminder(Reminder reminder)
                {
                    try
                    {
                        using (MySqlConnection connection =
                            databaseHelper.GetConnection())
                        {
                            connection.Open();

                            string query =
                            @"INSERT INTO Reminders
                    (Title, Description, ReminderDate, Completed)
                    VALUES
                    (@Title,@Description,@ReminderDate,@Completed)";

                            MySqlCommand command =
                                new MySqlCommand(query, connection);

                            command.Parameters.AddWithValue("@Title",
                                reminder.Title);

                            command.Parameters.AddWithValue("@Description",
                                reminder.Description);

                            command.Parameters.AddWithValue("@ReminderDate",
                                reminder.ReminderDate);

                            command.Parameters.AddWithValue("@Completed",
                                reminder.Completed);

                            command.ExecuteNonQuery();

                            return true;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
                public List<Reminder> GetAllReminders()
                {
                    List<Reminder> reminders = new List<Reminder>();

                    using (MySqlConnection connection =
                        databaseHelper.GetConnection())
                    {
                        connection.Open();

                        string query = "SELECT * FROM Reminders";

                        MySqlCommand command =
                            new MySqlCommand(query, connection);

                        MySqlDataReader reader =
                            command.ExecuteReader();

                        while (reader.Read())
                        {
                            Reminder reminder = new Reminder
                            {
                                ReminderID = Convert.ToInt32(reader["ReminderID"]),
                                Title = reader["Title"].ToString(),
                                Description = reader["Description"].ToString(),
                                ReminderDate = Convert.ToDateTime(reader["ReminderDate"]),
                                Completed = Convert.ToBoolean(reader["Completed"])
                            };

                            reminders.Add(reminder);
                        }
                    }

                    return reminders;
                }
                public bool DeleteReminder(int reminderID)
                {
                    try
                    {
                        using (MySqlConnection connection =
                            databaseHelper.GetConnection())
                        {
                            connection.Open();

                            string query =
                                "DELETE FROM Reminders WHERE ReminderID=@ID";

                            MySqlCommand command =
                                new MySqlCommand(query, connection);

                            command.Parameters.AddWithValue("@ID",
                                reminderID);

                            command.ExecuteNonQuery();

                            return true;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }

                public bool CompleteReminder(int reminderID)
                {
                    try
                    {
                        using (MySqlConnection connection =
                            databaseHelper.GetConnection())
                        {
                            connection.Open();

                            string query =
                            @"UPDATE Reminders
                      SET Completed = TRUE
                      WHERE ReminderID=@ID";

                            MySqlCommand command =
                                new MySqlCommand(query, connection);

                            command.Parameters.AddWithValue("@ID",
                                reminderID);

                            command.ExecuteNonQuery();

                            return true;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }
    }
}
