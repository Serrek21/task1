using System.Data.SqlClient;

namespace task1
{
    internal class Program
    {
        static string connectionString = "Server=YOUR_SERVER_NAME;Database=FirmKanc;Trusted_Connection=True;";
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Меню:");
                Console.WriteLine("1. Підключення до бази даних");
                Console.WriteLine("2. Відображення всіх канцтоварів");
                Console.WriteLine("3. Відображення всіх типів канцтоварів");
                Console.WriteLine("4. Відображення всіх менеджерів");
                Console.WriteLine("5. Канцтовари з максимальною кількістю одиниць");
                Console.WriteLine("6. Канцтовари з мінімальною кількістю одиниць");
                Console.WriteLine("7. Канцтовари з мінімальною собівартістю одиниці");
                Console.WriteLine("8. Канцтовари з максимальною собівартістю одиниці");
                Console.WriteLine("9. Канцтовари заданого типу");
                Console.WriteLine("10. Продажі певного менеджера");
                Console.WriteLine("11. Продажі певній фірмі");
                Console.WriteLine("12. Інформація про нещодавній продаж");
                Console.WriteLine("13. Середня кількість товарів за типами");
                Console.WriteLine("0. Вихід");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine();
                Console.Clear();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        switch (choice)
                        {
                            case "1":
                                Console.WriteLine("Підключення до бази даних успішне!");
                                break;
                            case "2":
                                DisplayAllStationery(connection);
                                break;
                            case "3":
                                DisplayAllTypes(connection);
                                break;
                            case "4":
                                DisplayAllManagers(connection);
                                break;
                            case "5":
                                DisplayStationeryWithMaxQuantity(connection);
                                break;
                            case "6":
                                DisplayStationeryWithMinQuantity(connection);
                                break;
                            case "7":
                                DisplayStationeryWithMinCostPrice(connection);
                                break;
                            case "8":
                                DisplayStationeryWithMaxCostPrice(connection);
                                break;
                            case "9":
                                Console.Write("Введіть тип канцтоварів: ");
                                string type = Console.ReadLine();
                                DisplayStationeryByType(connection, type);
                                break;
                            case "10":
                                Console.Write("Введіть ID менеджера: ");
                                int managerId = int.Parse(Console.ReadLine());
                                DisplaySalesByManager(connection, managerId);
                                break;
                            case "11":
                                Console.Write("Введіть назву фірми: ");
                                string buyer = Console.ReadLine();
                                DisplaySalesByBuyer(connection, buyer);
                                break;
                            case "12":
                                DisplayMostRecentSale(connection);
                                break;
                            case "13":
                                DisplayAverageQuantityByType(connection);
                                break;
                            case "0":
                                return;
                            default:
                                Console.WriteLine("Неправильний вибір!");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Помилка: " + ex.Message);
                    }
                }

                Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
                Console.ReadKey();
            }
        }

        static void DisplayAllStationery(SqlConnection connection)
        {
            string query = "SELECT * FROM Stationery";
            ExecuteReaderQuery(connection, query, reader =>
            {
                Console.WriteLine($"Id: {reader["Id"]}, Назва: {reader["Name"]}, Тип: {reader["Type"]}, " +
                                  $"Кількість: {reader["Quantity"]}, Собівартість: {reader["CostPrice"]}");
            });
        }

        static void DisplayAllTypes(SqlConnection connection)
        {
            string query = "SELECT DISTINCT Type FROM Stationery";
            ExecuteReaderQuery(connection, query, reader =>
            {
                Console.WriteLine($"Тип: {reader["Type"]}");
            });
        }

        static void DisplayAllManagers(SqlConnection connection)
        {
            string query = "SELECT * FROM Managers";
            ExecuteReaderQuery(connection, query, reader =>
            {
                Console.WriteLine($"Id: {reader["Id"]}, Ім'я: {reader["Name"]}");
            });
        }

        static void DisplayStationeryWithMaxQuantity(SqlConnection connection)
        {
            string query = "SELECT TOP 1 * FROM Stationery ORDER BY Quantity DESC";
            ExecuteReaderQuery(connection, query, reader =>
            {
                Console.WriteLine($"Id: {reader["Id"]}, Назва: {reader["Name"]}, Кількість: {reader["Quantity"]}");
            });
        }

        static void DisplayStationeryWithMinQuantity(SqlConnection connection)
        {
            string query = "SELECT TOP 1 * FROM Stationery ORDER BY Quantity ASC";
            ExecuteReaderQuery(connection, query, reader =>
            {
                Console.WriteLine($"Id: {reader["Id"]}, Назва: {reader["Name"]}, Кількість: {reader["Quantity"]}");
            });
        }

        static void DisplayStationeryWithMinCostPrice(SqlConnection connection)
        {
            string query = "SELECT TOP 1 * FROM Stationery ORDER BY CostPrice ASC";
            ExecuteReaderQuery(connection, query, reader =>
            {
                Console.WriteLine($"Id: {reader["Id"]}, Назва: {reader["Name"]}, Собівартість: {reader["CostPrice"]}");
            });
        }

        static void DisplayStationeryWithMaxCostPrice(SqlConnection connection)
        {
            string query = "SELECT TOP 1 * FROM Stationery ORDER BY CostPrice DESC";
            ExecuteReaderQuery(connection, query, reader =>
            {
                Console.WriteLine($"Id: {reader["Id"]}, Назва: {reader["Name"]}, Собівартість: {reader["CostPrice"]}");
            });
        }

        static void DisplayStationeryByType(SqlConnection connection, string type)
        {
            string query = "SELECT * FROM Stationery WHERE Type = @Type";
            ExecuteReaderQuery(connection, query, reader =>
            {
                Console.WriteLine($"Id: {reader["Id"]}, Назва: {reader["Name"]}, Кількість: {reader["Quantity"]}");
            }, ("@Type", type));
        }

        static void DisplaySalesByManager(SqlConnection connection, int managerId)
        {
            string query = "SELECT * FROM Sales WHERE ManagerId = @ManagerId";
            ExecuteReaderQuery(connection, query, reader =>
            {
                Console.WriteLine($"Id: {reader["Id"]}, Компанія: {reader["BuyerCompany"]}, Кількість: {reader["QuantitySold"]}");
            }, ("@ManagerId", managerId));
        }

        static void DisplaySalesByBuyer(SqlConnection connection, string buyer)
        {
            string query = "SELECT * FROM Sales WHERE BuyerCompany = @BuyerCompany";
            ExecuteReaderQuery(connection, query, reader =>
            {
                Console.WriteLine($"Id: {reader["Id"]}, Товар: {reader["StationeryId"]}, Кількість: {reader["QuantitySold"]}");
            }, ("@BuyerCompany", buyer));
        }

        static void DisplayMostRecentSale(SqlConnection connection)
        {
            string query = "SELECT TOP 1 * FROM Sales ORDER BY SaleDate DESC";
            ExecuteReaderQuery(connection, query, reader =>
            {
                Console.WriteLine($"Id: {reader["Id"]}, Товар: {reader["StationeryId"]}, Дата: {reader["SaleDate"]}");
            });
        }

        static void DisplayAverageQuantityByType(SqlConnection connection)
        {
            string query = "SELECT Type, AVG(Quantity) AS AvgQuantity FROM Stationery GROUP BY Type";
            ExecuteReaderQuery(connection, query, reader =>
            {
                Console.WriteLine($"Тип: {reader["Type"]}, Середня кількість: {reader["AvgQuantity"]}");
            });
        }

        static void ExecuteReaderQuery(SqlConnection connection, string query, Action<SqlDataReader> readAction, params (string, object)[] parameters)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Item1, param.Item2);
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        readAction(reader);
                    }
                }
            }
        }
    }   
}
