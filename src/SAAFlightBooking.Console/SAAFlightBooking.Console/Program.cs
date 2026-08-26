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
}
catch (Exception ex)
{
    Console.WriteLine("Database connection failed.");
    Console.WriteLine(ex.Message);
}