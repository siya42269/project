using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Data.SQLite;

namespace RestaurantExpertSystem
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnSuggestClick(object? sender, RoutedEventArgs e)
        {
            string time = (TimeBox.SelectedItem as ComboBoxItem)?.Content as string ?? "";
            string mood = (MoodBox.SelectedItem as ComboBoxItem)?.Content as string ?? "";
            int budget = int.TryParse(BudgetBox.Text, out var b) ? b : 0;

            string suggestion = "", reason = "";

            // Rules for Pakistani Halal food suggestions
            if (time == "Breakfast" && mood == "Light" && budget <= 150)
            {
                suggestion = "Halwa Puri with Chai";
                reason = "Light Pakistani breakfast under Rs. 150";
            }
            else if (time == "Lunch" && mood == "Light" && budget <= 200)
            {
                suggestion = "Chicken Shawarma + Pakola";
                reason = "Light lunch under Rs. 200";
            }
            else if (time == "Lunch" && mood == "Heavy" && budget <= 300)
            {
                suggestion = "Beef Biryani + Raita";
                reason = "Heavy lunch under Rs. 300";
            }
            else if (time == "Dinner" && mood == "Heavy" && budget <= 400)
            {
                suggestion = "Chicken Karahi + Roghni Naan + Kheer";
                reason = "Full desi dinner under Rs. 400";
            }
            else if (time == "Dinner" && mood == "Light" && budget <= 250)
            {
                suggestion = "Seekh Kebab Roll + Lassi";
                reason = "Light dinner under Rs. 250";
            }
            else
            {
                ResultBlock.Text = "⚠️ No suitable meal found for your inputs.";
                return;
            }

            ResultBlock.Text = $"✅ Suggestion: {suggestion}\n💡 Reason: {reason}";

            // Save to SQLite
            SaveToDatabase(time, mood, budget, suggestion, reason);
        }

        private void SaveToDatabase(string time, string mood, int budget, string suggestion, string reason)
        {
            string connStr = "Data Source=menu.db";
            using var conn = new SQLiteConnection(connStr);
            conn.Open();

            var createTable = new SQLiteCommand(@"
                CREATE TABLE IF NOT EXISTS Suggestions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Time TEXT,
                    Mood TEXT,
                    Budget INT,
                    Suggestion TEXT,
                    Reason TEXT
                )", conn);
            createTable.ExecuteNonQuery();

            var insert = new SQLiteCommand(
                "INSERT INTO Suggestions (Time, Mood, Budget, Suggestion, Reason) VALUES (@t, @m, @b, @s, @r)", conn);
            insert.Parameters.AddWithValue("@t", time);
            insert.Parameters.AddWithValue("@m", mood);
            insert.Parameters.AddWithValue("@b", budget);
            insert.Parameters.AddWithValue("@s", suggestion);
            insert.Parameters.AddWithValue("@r", reason);
            insert.ExecuteNonQuery();
        }
    }
}
