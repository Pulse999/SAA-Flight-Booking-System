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
}
catch (Exception ex)
{
    Console.WriteLine("Database connection failed.");
    Console.WriteLine(ex.Message);
}