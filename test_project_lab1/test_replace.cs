using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using AnalaizerClassLibrary; // Простір імен бібліотеки з твоїм класом

namespace AnalaizerTests
{
    [TestClass]
    public class SeparateTests
    {
        // Рядок підключення до MySQL
        private string connectionString = "server=localhost;port=3306;database=calculator_tests;user=tester;password=1234;";

        /// <summary>
        /// Тест із використанням даних із MySQL
        /// </summary>
        [TestMethod]
        public void Separate_FromDatabaseTestCases()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Input, ExpectedTokens FROM separate_tests";
                // ExpectedTokens у БД можна зберігати як строку з роздільником, наприклад: "12,+,3"

                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string input = reader.GetString("Input");
                        string expectedTokensRaw = reader.GetString("ExpectedTokens");
                        string[] expected = expectedTokensRaw.Split(','); // розділяємо у масив

                        List<string> actual = new List<string>(AnalaizerClass.Separate(input));

                        CollectionAssert.AreEqual(expected, actual, $"Помилка для вхідного виразу: {input}");
                    }
                }
            }
        }

        /// <summary>
        /// Прості тести в коді
        /// </summary>

        [TestMethod]
        public void Separate_SimpleExpression_ReturnsTokens()
        {
            string input = "12+3";
            string[] expected = { "12", "+", "3" };

            List<string> result = new List<string>(AnalaizerClass.Separate(input));

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Separate_VariablesAndNumbers_ReturnsTokens()
        {
            string input = "x1+25";
            string[] expected = { "x1", "+", "25" };

            List<string> result = new List<string>(AnalaizerClass.Separate(input));

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Separate_ExpressionWithParentheses_ReturnsTokens()
        {
            string input = "(a+5)";
            string[] expected = { "(", "a", "+", "5", ")" };

            List<string> result = new List<string>(AnalaizerClass.Separate(input));

            CollectionAssert.AreEqual(expected, result);
        }
    }
}
