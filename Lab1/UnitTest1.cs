using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using AnalaizerClassLibrary; // Простір імен бібліотеки з методом Separate

namespace AnalaizerTests
{
    [TestClass]
    public class SeparateTests
    {
        // Рядок підключення до MySQL (підстав свої дані)
        private string connectionString = "server=localhost;port=3306;database=calculator_tests;user=telebot1game;password=1012141618aa;";

        /// <summary>
        /// Тестування методу Separate з використанням даних із бази.
        /// Таблиця має вигляд:
        /// CREATE TABLE separate_tests (
        ///     Id INT AUTO_INCREMENT PRIMARY KEY,
        ///     Input VARCHAR(255),
        ///     ExpectedResult VARCHAR(255)
        /// );
        /// ExpectedResult зберігається як токени через пробіл, наприклад: "2 + 2"
        /// </summary>
        [TestMethod]
        public void Separate_FromDatabaseTestCases()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Input, ExpectedResult FROM separate_tests";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string input = reader.GetString("Input");
                        string expected = reader.GetString("ExpectedResult");

                        // Виклик методу
                        IEnumerable<string> actualTokens = AnalaizerClass.Separate(input);
                        string actual = string.Join(" ", actualTokens);

                        Assert.AreEqual(expected, actual, $"Помилка для виразу: {input}");
                    }
                }
            }
        }

        /// <summary>
        /// Базовий тест: простий вираз "2+2"
        /// </summary>
        [TestMethod]
        public void Separate_SimpleExpression_ReturnsTokens()
        {
            string input = "2+2";
            string expected = "2 + 2";

            var result = AnalaizerClass.Separate(input);

            Assert.AreEqual(expected, string.Join(" ", result));
        }

        /// <summary>
        /// Вираз із дужками "(3-1)*5"
        /// </summary>
        [TestMethod]
        public void Separate_WithParentheses_ReturnsTokens()
        {
            string input = "(3-1)*5";
            string expected = "( 3 - 1 ) * 5";

            var result = AnalaizerClass.Separate(input);

            Assert.AreEqual(expected, string.Join(" ", result));
        }

        /// <summary>
        /// Вираз із пробілами "  4   +7 "
        /// </summary>
        [TestMethod]
        public void Separate_WithSpaces_IgnoresSpaces()
        {
            string input = "  4   +7 ";
            string expected = "4 + 7";

            var result = AnalaizerClass.Separate(input);

            Assert.AreEqual(expected, string.Join(" ", result));
        }

        /// <summary>
        /// Вираз із літерними токенами "mod"
        /// </summary>
        [TestMethod]
        public void Separate_WithLetters_ReturnsCorrectToken()
        {
            string input = "10mod3";
            string expected = "10 mod 3";

            var result = AnalaizerClass.Separate(input);

            Assert.AreEqual(expected, string.Join(" ", result));
        }
    }
}
