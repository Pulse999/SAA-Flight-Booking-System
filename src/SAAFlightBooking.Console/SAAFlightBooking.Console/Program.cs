using Npgsql;

string? password = Environment.GetEnvironmentVariable("SAA_DB_PASSWORD");

if (string.IsNullOrWhiteSpace(password))
{
    Console.WriteLine("Database password environment variable is not configured.");
    return;
}

string connectionString =
    "Host=localhost;" +
    "Port=5432;" +
    "Database=SAA_flight_booking;" +
    "Username=postgres;" +
    $"Password={password};";
// Connect to the PostgreSQL database using Npgsql and retrieve passenger records

try
{
    using NpgsqlConnection connection = new NpgsqlConnection(connectionString);

    connection.Open();

    Console.WriteLine("======================================");
    Console.WriteLine(" SAA Flight Booking System");
    Console.WriteLine("======================================");
    Console.WriteLine("Database connection successful!");
    Console.WriteLine("Connected to: saa_flight_booking");
    Console.WriteLine();

    bool running = true;

    // Data basic operations(CRUD) MENU 

    while (running)
    {
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("MAIN MENU");
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("1. Insert Data");
        Console.WriteLine("2. Retrieve Data");
        Console.WriteLine("3. Update Data");
        Console.WriteLine("4. Delete Data");
        Console.WriteLine("5. Reports & Joins");
        Console.WriteLine("6. Exit");
        Console.WriteLine("--------------------------------------------------");
        Console.Write("Select an option: ");

        string? choice = Console.ReadLine();

        Console.WriteLine();

        switch (choice)
        {
            case "1":
                Console.WriteLine("Insert Data functionality will be added in Task 6.");
                break;

            case "2":
                RetrieveData(connection);
                break;

            case "3":
                Console.WriteLine("Update Data functionality will be added in Task 8.");
                break;

            case "4":
                Console.WriteLine("Delete Data functionality will be added in Task 9.");
                break;

            case "5":
                Console.WriteLine("Reports & Joins functionality will be added in Task 10.");
                break;

            case "6":
                running = false;
                Console.WriteLine("Thank you for using the SAA Flight Booking System.");
                break;

            default:
                Console.WriteLine("Invalid option. Please select a number from 1 to 6.");
                break;
        }

        Console.WriteLine();
    }
}

catch (Exception ex)
{
    Console.WriteLine("Database connection failed.");
    Console.WriteLine(ex.Message);
}

string query = @"
        SELECT
            passenger_id,
            first_name,
            last_name,
            email,
            phone
        FROM passengers
        ORDER BY passenger_id;
";

    //Reading and logging the passengers in the database

    using NpgsqlCommand command = new NpgsqlCommand(query, connection);
    using NpgsqlDataReader reader = command.ExecuteReader();

    Console.WriteLine("Passenger Records");
    Console.WriteLine("--------------------------------------");

    while (reader.Read())
    {
        Console.WriteLine(
            $"ID: {reader["passenger_id"]} | " +
            $"Name: {reader["first_name"]} {reader["last_name"]} | " +
            $"Email: {reader["email"]} | " +
            $"Phone: {reader["phone"]}"
        );
    }

    Console.WriteLine("--------------------------------------");
    Console.WriteLine("Passenger records retrieved successfully!");
}
catch (Exception ex)
{
    Console.WriteLine("Database connection failed.");
    Console.WriteLine(ex.Message);
}

void RetrieveData(NpgsqlConnection connection)
{
    throw new NotImplementedException();
}