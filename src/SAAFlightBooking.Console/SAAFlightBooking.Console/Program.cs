using Npgsql;
using static System.Net.WebRequestMethods;

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
    Console.WriteLine("Connected to: SAA_flight_booking");
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
        // Place holder cases in the meantime 
        switch (choice)
        {
            case "1":
                Console.WriteLine("Insert Data functionality will be added in Task 6.");
                break;

            case "2":
                RetrieveData(connection);
                break;

            case "3":
                UpdateData(connection);
                break;

            case "4":
                DeleteData(connection);
                break;

            case "5":
                ReportsAndJoins(connection);
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

// reports and joins submenu

// ==================================================
// REPORTS & JOINS
// ==================================================

static void ReportsAndJoins(NpgsqlConnection connection)
{
    bool reporting = true;

    while (reporting)
    {
        Console.WriteLine("==================================================");
        Console.WriteLine("                 REPORTS & JOINS");
        Console.WriteLine("==================================================");
        Console.WriteLine("1. Passenger Booking Report");
        Console.WriteLine("2. Flight Manifest");
        Console.WriteLine("3. Tickets Issued Report");
        Console.WriteLine("4. Bookings Per Flight");
        Console.WriteLine("5. Revenue Per Flight");
        Console.WriteLine("6. Overall Booking Summary");
        Console.WriteLine("7. Back to Main Menu");
        Console.WriteLine("==================================================");
        Console.Write("Select an option: ");

        string? choice = Console.ReadLine();

        Console.WriteLine();

        switch (choice)
        {
            case "1":
                PassengerBookingReport(connection);
                break;

            case "2":
                FlightManifest(connection);
                break;

            case "3":
                TicketsIssuedReport(connection);
                break;

            case "4":
                PassengerBookingSummary(connection);
                break;

            case "5":
                RevenuePerFlight(connection);
                break;

            case "6":
                OverallBookingSummary(connection);
                break;

            case "7":
                reporting = false;
                break;

            default:
                Console.WriteLine("Invalid option. Please select a number from 1 to 7.");
                break;
        }

        Console.WriteLine();
    }
}

// Inner join reports Passenger booking report

static void PassengerBookingReport(NpgsqlConnection connection)
{
    string query = @"
        SELECT
            p.passenger_id,
            p.first_name,
            p.last_name,
            b.booking_id,
            b.booking_date,
            b.booking_status,
            f.flight_number
        FROM passengers p
        INNER JOIN booking_passengers bp
            ON p.passenger_id = bp.passenger_id
        INNER JOIN bookings b
            ON bp.booking_id = b.booking_id
        INNER JOIN flights f
            ON b.flight_id = f.flight_id
        ORDER BY p.passenger_id, b.booking_id;
    ";

    using NpgsqlCommand command = new NpgsqlCommand(query, connection);
    using NpgsqlDataReader reader = command.ExecuteReader();

    Console.WriteLine("==================================================");
    Console.WriteLine("             PASSENGER BOOKING REPORT");
    Console.WriteLine("==================================================");

    while (reader.Read())
    {
        Console.WriteLine(
            $"Passenger ID: {reader["passenger_id"]} | " +
            $"Name: {reader["first_name"]} {reader["last_name"]}"
        );

        Console.WriteLine(
            $"Booking ID: {reader["booking_id"]} | " +
            $"Flight: {reader["flight_number"]} | " +
            $"Date: {reader["booking_date"]} | " +
            $"Status: {reader["booking_status"]}"
        );

        Console.WriteLine("--------------------------------------------------");
    }

    Console.WriteLine("Passenger booking report generated successfully!");
}

// Flight manifest

static void FlightManifest(NpgsqlConnection connection)
{
    string query = @"
        SELECT
            f.flight_number,
            a1.airport_code AS departure_code,
            a1.city AS departure_city,
            a2.airport_code AS arrival_code,
            a2.city AS arrival_city,
            p.first_name,
            p.last_name,
            b.booking_status
        FROM flights f
        INNER JOIN airports a1
            ON f.departure_airport_id = a1.airport_id
        INNER JOIN airports a2
            ON f.arrival_airport_id = a2.airport_id
        INNER JOIN bookings b
            ON f.flight_id = b.flight_id
        INNER JOIN booking_passengers bp
            ON b.booking_id = bp.booking_id
        INNER JOIN passengers p
            ON bp.passenger_id = p.passenger_id
        ORDER BY f.flight_number, p.last_name;
    ";

    using NpgsqlCommand command = new NpgsqlCommand(query, connection);
    using NpgsqlDataReader reader = command.ExecuteReader();

    Console.WriteLine("==================================================");
    Console.WriteLine("                  FLIGHT MANIFEST");
    Console.WriteLine("==================================================");

    while (reader.Read())
    {
        Console.WriteLine(
            $"Flight: {reader["flight_number"]} | " +
            $"Route: {reader["departure_city"]} ({reader["departure_code"]}) -> " +
            $"{reader["arrival_city"]} ({reader["arrival_code"]})"
        );

        Console.WriteLine(
            $"Passenger: {reader["first_name"]} {reader["last_name"]} | " +
            $"Booking Status: {reader["booking_status"]}"
        );

        Console.WriteLine("--------------------------------------------------");
    }

    Console.WriteLine("Flight manifest generated successfully!");
}

// Tickets Issued Report

static void TicketsIssuedReport(NpgsqlConnection connection)
{
    string query = @"
        SELECT
            t.ticket_number,
            t.ticket_status,
            t.issue_date,
            b.booking_id,
            f.flight_number
        FROM tickets t
        INNER JOIN bookings b
            ON t.booking_id = b.booking_id
        INNER JOIN flights f
            ON b.flight_id = f.flight_id
        ORDER BY t.ticket_id;
    ";

    using NpgsqlCommand command = new NpgsqlCommand(query, connection);
    using NpgsqlDataReader reader = command.ExecuteReader();

    Console.WriteLine("==================================================");
    Console.WriteLine("                 TICKETS ISSUED REPORT");
    Console.WriteLine("==================================================");

    while (reader.Read())
    {
        Console.WriteLine(
            $"Ticket: {reader["ticket_number"]} | " +
            $"Booking ID: {reader["booking_id"]} | " +
            $"Flight: {reader["flight_number"]}"
        );

        Console.WriteLine(
            $"Status: {reader["ticket_status"]} | " +
            $"Issue Date: {reader["issue_date"]}"
        );

        Console.WriteLine("--------------------------------------------------");
    }

    Console.WriteLine("Tickets report generated successfully!");
}

// Passenger booking summary

static void PassengerBookingSummary(NpgsqlConnection connection)
{
    string query = @"
        SELECT
            p.passenger_id,
            p.first_name,
            p.last_name,
            b.booking_id,
            b.booking_status
        FROM passengers p
        LEFT JOIN booking_passengers bp
            ON p.passenger_id = bp.passenger_id
        LEFT JOIN bookings b
            ON bp.booking_id = b.booking_id
        ORDER BY p.passenger_id, b.booking_id;
    ";

    using NpgsqlCommand command = new NpgsqlCommand(query, connection);
    using NpgsqlDataReader reader = command.ExecuteReader();

    Console.WriteLine("==================================================");
    Console.WriteLine("            PASSENGER BOOKING SUMMARY");
    Console.WriteLine("==================================================");

    while (reader.Read())
    {
        string bookingId = reader["booking_id"] == DBNull.Value
            ? "No booking"
            : reader["booking_id"].ToString()!;

        string bookingStatus = reader["booking_status"] == DBNull.Value
            ? "No booking"
            : reader["booking_status"].ToString()!;

        Console.WriteLine(
            $"Passenger ID: {reader["passenger_id"]} | " +
            $"Name: {reader["first_name"]} {reader["last_name"]} | " +
            $"Booking ID: {bookingId} | " +
            $"Status: {bookingStatus}"
        );
    }

    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine("Passenger booking summary generated successfully!");
}

// Booking count report

static void BookingCountReport(NpgsqlConnection connection)
{
    string query = @"
        SELECT
            f.flight_number,
            COUNT(b.booking_id) AS total_bookings
        FROM flights f
        LEFT JOIN bookings b
            ON f.flight_id = b.flight_id
        GROUP BY
            f.flight_id,
            f.flight_number
        ORDER BY total_bookings DESC;
    ";

    using NpgsqlCommand command = new NpgsqlCommand(query, connection);
    using NpgsqlDataReader reader = command.ExecuteReader();

    Console.WriteLine("==================================================");
    Console.WriteLine("              BOOKINGS PER FLIGHT");
    Console.WriteLine("==================================================");

    while (reader.Read())
    {
        Console.WriteLine(
            $"Flight: {reader["flight_number"]} | " +
            $"Total Bookings: {reader["total_bookings"]}"
        );
    }

    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine("Booking count report generated successfully!");
}

// Delete submenu

static void DeleteData(NpgsqlConnection connection)
{
    Console.WriteLine("==================================================");
    Console.WriteLine("                  DELETE DATA");
    Console.WriteLine("==================================================");
    Console.WriteLine("1. Delete Booking");
    Console.WriteLine("2. Back to Main Menu");
    Console.WriteLine("==================================================");
    Console.Write("Select an option: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            DeleteBooking(connection);
            break;

        case "2":
            break;

        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}

// Remove booking records

static void DeleteBooking(NpgsqlConnection connection)
{
    Console.Write("Enter booking ID to delete: ");

    if (!int.TryParse(Console.ReadLine(), out int bookingId))
    {
        Console.WriteLine("Invalid booking ID.");
        return;
    }

    string query = @"
        DELETE FROM bookings
        WHERE booking_id = @booking_id;
    ";

    try
    {
        using NpgsqlCommand command = new NpgsqlCommand(query, connection);

        command.Parameters.AddWithValue("@booking_id", bookingId);

        int rowsAffected = command.ExecuteNonQuery();

        if (rowsAffected > 0)
        {
            Console.WriteLine("Booking deleted successfully.");
        }
        else
        {
            Console.WriteLine("No booking was found with that ID.");
        }
    }
    catch (PostgresException ex)
    {
        Console.WriteLine();
        Console.WriteLine("Unable to delete booking.");
        Console.WriteLine("The booking is still referenced by related records.");
        Console.WriteLine($"Database message: {ex.MessageText}");
    }
}

// Update submenu

static void UpdateData(NpgsqlConnection connection)
{
    bool updating = true;

    while (updating)
    {
        Console.WriteLine("==================================================");
        Console.WriteLine("                 UPDATE DATA");
        Console.WriteLine("==================================================");
        Console.WriteLine("1. Update Passenger Email");
        Console.WriteLine("2. Update Booking Status");
        Console.WriteLine("3. Update Payment Status");
        Console.WriteLine("4. Back to Main Menu");
        Console.WriteLine("==================================================");
        Console.Write("Select an option: ");

        string? choice = Console.ReadLine();

        Console.WriteLine();

        switch (choice)
        {
            case "1":
                UpdatePassengerEmail(connection);
                break;

            case "2":
                UpdateBookingStatus(connection);
                break;

            case "3":
                UpdatePaymentStatus(connection);
                break;

            case "4":
                updating = false;
                break;

            default:
                Console.WriteLine("Invalid option. Please select 1 to 4.");
                break;
        }

        Console.WriteLine();
    }
}

// Update passenger emails

static void UpdatePassengerEmail(NpgsqlConnection connection)
{
    Console.Write("Enter passenger ID: ");
    string? input = Console.ReadLine();

    if (!int.TryParse(input, out int passengerId))
    {
        Console.WriteLine("Invalid passenger ID.");
        return;
    }

    Console.Write("Enter new email address: ");
    string? newEmail = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(newEmail))
    {
        Console.WriteLine("Email address cannot be empty.");
        return;
    }

    string query = @"
        UPDATE passengers
        SET email = @email
        WHERE passenger_id = @passengerId;
    ";

    using NpgsqlCommand command = new NpgsqlCommand(query, connection);

    command.Parameters.AddWithValue("@email", newEmail);
    command.Parameters.AddWithValue("@passengerId", passengerId);

    int rowsAffected = command.ExecuteNonQuery();

    if (rowsAffected > 0)
    {
        Console.WriteLine("Passenger email updated successfully!");
    }
    else
    {
        Console.WriteLine("Passenger not found.");
    }
}

// Update passenger booking status 

static void UpdateBookingStatus(NpgsqlConnection connection)
{
    Console.Write("Enter booking ID: ");
    string? input = Console.ReadLine();

    if (!int.TryParse(input, out int bookingId))
    {
        Console.WriteLine("Invalid booking ID.");
        return;
    }

    Console.Write("Enter new booking status (Pending, Confirmed, Cancelled): ");
    string? newStatus = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(newStatus))
    {
        Console.WriteLine("Booking status cannot be empty.");
        return;
    }

    string query = @"
        UPDATE bookings
        SET booking_status = @status
        WHERE booking_id = @bookingId;
    ";

    using NpgsqlCommand command = new NpgsqlCommand(query, connection);

    command.Parameters.AddWithValue("@status", newStatus);
    command.Parameters.AddWithValue("@bookingId", bookingId);

    int rowsAffected = command.ExecuteNonQuery();

    if (rowsAffected > 0)
    {
        Console.WriteLine("Booking status updated successfully!");
    }
    else
    {
        Console.WriteLine("Booking not found.");
    }
}

// Update passenger payment status 

static void UpdatePaymentStatus(NpgsqlConnection connection)
{
    Console.Write("Enter payment ID: ");
    string? input = Console.ReadLine();

    if (!int.TryParse(input, out int paymentId))
    {
        Console.WriteLine("Invalid payment ID.");
        return;
    }

    Console.Write("Enter new payment status (Pending, Paid, Failed, Refunded): ");
    string? newStatus = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(newStatus))
    {
        Console.WriteLine("Payment status cannot be empty.");
        return;
    }

    string query = @"
        UPDATE payments
        SET payment_status = @status
        WHERE payment_id = @paymentId;
    ";

    using NpgsqlCommand command = new NpgsqlCommand(query, connection);

    command.Parameters.AddWithValue("@status", newStatus);
    command.Parameters.AddWithValue("@paymentId", paymentId);

    int rowsAffected = command.ExecuteNonQuery();

    if (rowsAffected > 0)
    {
        Console.WriteLine("Payment status updated successfully!");
    }
    else
    {
        Console.WriteLine("Payment not found.");
    }
}

// Retrieve data menu

static void RetrieveData(NpgsqlConnection connection)
{
    bool retrieving = true;

    while (retrieving)
    {
        Console.WriteLine("==================================================");
        Console.WriteLine("                 RETRIEVE DATA");
        Console.WriteLine("==================================================");
        Console.WriteLine("1. View Passengers");
        Console.WriteLine("2. View Airports");
        Console.WriteLine("3. View Flights");
        Console.WriteLine("4. View Bookings");
        Console.WriteLine("5. View Tickets");
        Console.WriteLine("6. View Payments");
        Console.WriteLine("7. Search / Filter Records");
        Console.WriteLine("8. Back to Main Menu");
        Console.WriteLine("==================================================");
        Console.Write("Select an option: ");

        string? choice = Console.ReadLine();

        Console.WriteLine();

        switch (choice)
        {
            case "1":
                RetrievePassengers(connection);
                break;

            case "2":
                RetrieveAirports(connection);
                break;

            case "3":
                RetrieveFlights(connection);
                break;

            case "4":
                RetrieveBookings(connection);
                break;

            case "5":
                RetrieveTickets(connection);
                break;

            case "6":
                RetrievePayments(connection);
                break;

            case "7":
                SearchAndFilterData(connection);
                break;

            case "8":
                retrieving = false;
                break;

            default:
                Console.WriteLine("Invalid option.");
                break;
        }

        Console.WriteLine();
    }
}


// Retrieve passenger records

static void RetrievePassengers(NpgsqlConnection connection)
{
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

    using NpgsqlCommand command = new NpgsqlCommand(query, connection);
    using NpgsqlDataReader reader = command.ExecuteReader();

    Console.WriteLine("==================================================");
    Console.WriteLine("                PASSENGER RECORDS");
    Console.WriteLine("==================================================");

    while (reader.Read())
    {
        Console.WriteLine(
            $"ID: {reader["passenger_id"]} | " +
            $"Name: {reader["first_name"]} {reader["last_name"]} | " +
            $"Email: {reader["email"]} | " +
            $"Phone: {reader["phone"]}"
        );
    }

    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine("Passenger records retrieved successfully!");
}


// Retrieve Airports records

static void RetrieveAirports(NpgsqlConnection connection)
{
    string query = @"
        SELECT
            airport_id,
            airport_code,
            airport_name,
            city,
            country
        FROM airports
        ORDER BY airport_id;
    ";

    using NpgsqlCommand command = new NpgsqlCommand(query, connection);
    using NpgsqlDataReader reader = command.ExecuteReader();

    Console.WriteLine("==================================================");
    Console.WriteLine("                  AIRPORT RECORDS");
    Console.WriteLine("==================================================");

    while (reader.Read())
    {
        Console.WriteLine(
            $"ID: {reader["airport_id"]} | " +
            $"Code: {reader["airport_code"]} | " +
            $"Airport: {reader["airport_name"]} | " +
            $"City: {reader["city"]} | " +
            $"Country: {reader["country"]}"
        );
    }

    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine("Airport records retrieved successfully!");
}

// Retrieve Flight records 

static void RetrieveFlights(NpgsqlConnection connection)
{
    string query = @"
        SELECT
            f.flight_id,
            f.flight_number,
            dep.airport_code AS departure_code,
            dep.city AS departure_city,
            arr.airport_code AS arrival_code,
            arr.city AS arrival_city,
            f.departure_date_time,
            f.arrival_date_time,
            f.capacity
        FROM flights f
        INNER JOIN airports dep
            ON f.departure_airport_id = dep.airport_id
        INNER JOIN airports arr
            ON f.arrival_airport_id = arr.airport_id
        ORDER BY f.flight_id;
    ";

    using NpgsqlCommand command = new NpgsqlCommand(query, connection);
    using NpgsqlDataReader reader = command.ExecuteReader();

    Console.WriteLine("==================================================");
    Console.WriteLine("                  FLIGHT RECORDS");
    Console.WriteLine("==================================================");

    while (reader.Read())
    {
        Console.WriteLine(
            $"ID: {reader["flight_id"]} | " +
            $"Flight: {reader["flight_number"]} | " +
            $"Route: {reader["departure_city"]} ({reader["departure_code"]}) -> " +
            $"{reader["arrival_city"]} ({reader["arrival_code"]})"
        );

        Console.WriteLine(
            $"Departure: {reader["departure_date_time"]} | " +
            $"Arrival: {reader["arrival_date_time"]} | " +
            $"Capacity: {reader["capacity"]}"
        );

        Console.WriteLine("--------------------------------------------------");
    }

    Console.WriteLine("Flight records retrieved successfully!");
}

// Retrieve Booing records 

static void RetrieveBookings(NpgsqlConnection connection)
{
    string query = @"
        SELECT
            b.booking_id,
            b.booking_date,
            b.booking_status,
            p.first_name,
            p.last_name,
            f.flight_number,
            dep.airport_code AS departure_code,
            dep.city AS departure_city,
            arr.airport_code AS arrival_code,
            arr.city AS arrival_city
        FROM bookings b

        INNER JOIN booking_passengers bp
            ON b.booking_id = bp.booking_id

        INNER JOIN passengers p
            ON bp.passenger_id = p.passenger_id

        INNER JOIN flights f
            ON b.flight_id = f.flight_id

        INNER JOIN airports dep
            ON f.departure_airport_id = dep.airport_id

        INNER JOIN airports arr
            ON f.arrival_airport_id = arr.airport_id

        ORDER BY b.booking_id, p.last_name, p.first_name;
    ";

    using NpgsqlCommand command = new NpgsqlCommand(query, connection);
    using NpgsqlDataReader reader = command.ExecuteReader();

    Console.WriteLine("==================================================");
    Console.WriteLine("                  BOOKING RECORDS");
    Console.WriteLine("==================================================");

    int currentBookingId = -1;

    while (reader.Read())
    {
        int bookingId = Convert.ToInt32(reader["booking_id"]);

        if (bookingId != currentBookingId)
        {
            if (currentBookingId != -1)
            {
                Console.WriteLine("--------------------------------------------------");
            }

            currentBookingId = bookingId;

            Console.WriteLine($"Booking ID: {reader["booking_id"]}");
            Console.WriteLine($"Booking Date: {reader["booking_date"]}");
            Console.WriteLine($"Status: {reader["booking_status"]}");
            Console.WriteLine(
                $"Flight: {reader["flight_number"]} | " +
                $"Route: {reader["departure_city"]} ({reader["departure_code"]}) -> " +
                $"{reader["arrival_city"]} ({reader["arrival_code"]})"
            );

            Console.WriteLine("Passengers:");
        }

        Console.WriteLine(
            $"  - {reader["first_name"]} {reader["last_name"]}"
        );
    }

    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine("Booking records retrieved successfully!");
}

// Retrieve ticket data 

static void RetrieveTickets(NpgsqlConnection connection)
{
    string query = @"
        SELECT
            t.ticket_id,
            t.ticket_number,
            t.ticket_status,
            t.issue_date,
            t.booking_id
        FROM tickets t
        ORDER BY t.ticket_id;
    ";

    using NpgsqlCommand command = new NpgsqlCommand(query, connection);
    using NpgsqlDataReader reader = command.ExecuteReader();

    Console.WriteLine("==================================================");
    Console.WriteLine("                  TICKET RECORDS");
    Console.WriteLine("==================================================");

    while (reader.Read())
    {
        Console.WriteLine(
            $"Ticket ID: {reader["ticket_id"]} | " +
            $"Ticket Number: {reader["ticket_number"]} | " +
            $"Booking ID: {reader["booking_id"]} | " +
            $"Status: {reader["ticket_status"]} | " +
            $"Issue Date: {reader["issue_date"]}"
        );
    }

    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine("Ticket records retrieved successfully!");
}

// Retrieve payment records 

static void RetrievePayments(NpgsqlConnection connection)
{
    string query = @"
        SELECT
            payment_id,
            booking_id,
            amount,
            payment_date,
            payment_status,
            payment_method
        FROM payments
        ORDER BY payment_id;
    ";

    using NpgsqlCommand command = new NpgsqlCommand(query, connection);
    using NpgsqlDataReader reader = command.ExecuteReader();

    Console.WriteLine("==================================================");
    Console.WriteLine("                 PAYMENT RECORDS");
    Console.WriteLine("==================================================");

    while (reader.Read())
    {
        Console.WriteLine(
            $"Payment ID: {reader["payment_id"]} | " +
            $"Booking ID: {reader["booking_id"]} | " +
            $"Amount: R{reader["amount"]} | " +
            $"Date: {reader["payment_date"]} | " +
            $"Status: {reader["payment_status"]} | " +
            $"Method: {reader["payment_method"]}"
        );
    }

    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine("Payment records retrieved successfully!");
}

// Search anf retrival menu

static void SearchAndFilterData(NpgsqlConnection connection)
{
    bool searching = true;

    while (searching)
    {
        Console.WriteLine("==================================================");
        Console.WriteLine("             SEARCH / FILTER RECORDS");
        Console.WriteLine("==================================================");
        Console.WriteLine("1. Search Passenger by Name");
        Console.WriteLine("2. Search Flights by Airport");
        Console.WriteLine("3. Filter Bookings by Status");
        Console.WriteLine("4. Filter Payments by Status");
        Console.WriteLine("5. Back");
        Console.WriteLine("==================================================");
        Console.Write("Select an option: ");

        string? choice = Console.ReadLine();

        Console.WriteLine();

        switch (choice)
        {
            case "1":
                SearchPassengers(connection);
                break;

            case "2":
                SearchFlightsByAirport(connection);
                break;

            case "3":
                FilterBookingsByStatus(connection);
                break;

            case "4":
                FilterPaymentsByStatus(connection);
                break;

            case "5":
                searching = false;
                break;

            default:
                Console.WriteLine("Invalid option.");
                break;
        }

        Console.WriteLine();
    }
}

// Search for passengers by first name or last name

static void SearchPassengers(NpgsqlConnection connection)
{
    Console.Write("Enter passenger first name or last name: ");
    string? search = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(search))
    {
        Console.WriteLine("Search value cannot be empty.");
        return;
    }

    string query = @"
        SELECT
            passenger_id,
            first_name,
            last_name,
            email,
            phone
        FROM passengers
        WHERE first_name ILIKE @search
           OR last_name ILIKE @search
        ORDER BY last_name, first_name;
    ";

    using NpgsqlCommand command = new NpgsqlCommand(query, connection);
    command.Parameters.AddWithValue("@search", $"%{search}%");

    using NpgsqlDataReader reader = command.ExecuteReader();

    Console.WriteLine();
    Console.WriteLine("==================================================");
    Console.WriteLine("              PASSENGER SEARCH RESULTS");
    Console.WriteLine("==================================================");

    bool found = false;

    while (reader.Read())
    {
        found = true;

        Console.WriteLine(
            $"ID: {reader["passenger_id"]} | " +
            $"Name: {reader["first_name"]} {reader["last_name"]} | " +
            $"Email: {reader["email"]} | " +
            $"Phone: {reader["phone"]}"
        );
    }

    if (!found)
    {
        Console.WriteLine("No passengers found.");
    }

    Console.WriteLine("--------------------------------------------------");
}

//Search flights by airport

static void SearchFlightsByAirport(NpgsqlConnection connection)
{
    Console.Write("Enter airport code (e.g. JNB, CPT, DUR): ");
    string? airportCode = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(airportCode))
    {
        Console.WriteLine("Airport code cannot be empty.");
        return;
    }

    string query = @"
        SELECT
            f.flight_id,
            f.flight_number,
            dep.airport_code AS departure_code,
            dep.city AS departure_city,
            arr.airport_code AS arrival_code,
            arr.city AS arrival_city,
            f.departure_date_time,
            f.arrival_date_time,
            f.capacity
        FROM flights f
        INNER JOIN airports dep
            ON f.departure_airport_id = dep.airport_id
        INNER JOIN airports arr
            ON f.arrival_airport_id = arr.airport_id
        WHERE dep.airport_code ILIKE @airport
           OR arr.airport_code ILIKE @airport
        ORDER BY f.departure_date_time;
    ";

    using NpgsqlCommand command = new NpgsqlCommand(query, connection);
    command.Parameters.AddWithValue("@airport", airportCode);

    using NpgsqlDataReader reader = command.ExecuteReader();

    Console.WriteLine();
    Console.WriteLine("==================================================");
    Console.WriteLine("               FLIGHT SEARCH RESULTS");
    Console.WriteLine("==================================================");

    bool found = false;

    while (reader.Read())
    {
        found = true;

        Console.WriteLine(
            $"Flight: {reader["flight_number"]} | " +
            $"Route: {reader["departure_city"]} ({reader["departure_code"]}) -> " +
            $"{reader["arrival_city"]} ({reader["arrival_code"]})"
        );

        Console.WriteLine(
            $"Departure: {reader["departure_date_time"]} | " +
            $"Arrival: {reader["arrival_date_time"]} | " +
            $"Capacity: {reader["capacity"]}"
        );

        Console.WriteLine("--------------------------------------------------");
    }

    if (!found)
    {
        Console.WriteLine("No flights found for that airport.");
    }
}

// Filter bookings by status

static void FilterBookingsByStatus(NpgsqlConnection connection)
{
    Console.Write("Enter booking status (Pending, Confirmed, Cancelled): ");
    string? status = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(status))
    {
        Console.WriteLine("Booking status cannot be empty.");
        return;
    }

    string query = @"
        SELECT
            booking_id,
            flight_id,
            booking_date,
            booking_status
        FROM bookings
        WHERE booking_status ILIKE @status
        ORDER BY booking_date;
    ";

    using NpgsqlCommand command = new NpgsqlCommand(query, connection);
    command.Parameters.AddWithValue("@status", status);

    using NpgsqlDataReader reader = command.ExecuteReader();

    Console.WriteLine();
    Console.WriteLine("==================================================");
    Console.WriteLine("              BOOKING FILTER RESULTS");
    Console.WriteLine("==================================================");

    bool found = false;

    while (reader.Read())
    {
        found = true;

        Console.WriteLine(
            $"Booking ID: {reader["booking_id"]} | " +
            $"Flight ID: {reader["flight_id"]} | " +
            $"Date: {reader["booking_date"]} | " +
            $"Status: {reader["booking_status"]}"
        );
    }

    if (!found)
    {
        Console.WriteLine("No bookings found with that status.");
    }

    Console.WriteLine("--------------------------------------------------");
}

// Filter payments by status

static void FilterPaymentsByStatus(NpgsqlConnection connection)
{
    Console.Write("Enter payment status (Paid, Pending, Failed, Refunded): ");
    string? status = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(status))
    {
        Console.WriteLine("Payment status cannot be empty.");
        return;
    }

    string query = @"
        SELECT
            payment_id,
            booking_id,
            amount,
            payment_date,
            payment_status,
            payment_method
        FROM payments
        WHERE payment_status ILIKE @status
        ORDER BY payment_date;
    ";

    using NpgsqlCommand command = new NpgsqlCommand(query, connection);
    command.Parameters.AddWithValue("@status", status);

    using NpgsqlDataReader reader = command.ExecuteReader();

    Console.WriteLine();
    Console.WriteLine("==================================================");
    Console.WriteLine("              PAYMENT FILTER RESULTS");
    Console.WriteLine("==================================================");

    bool found = false;

    while (reader.Read())
    {
        found = true;

        Console.WriteLine(
            $"Payment ID: {reader["payment_id"]} | " +
            $"Booking ID: {reader["booking_id"]} | " +
            $"Amount: R{reader["amount"]} | " +
            $"Status: {reader["payment_status"]} | " +
            $"Method: {reader["payment_method"]}"
        );
    }

    if (!found)
    {
        Console.WriteLine("No payments found with that status.");
    }

    Console.WriteLine("--------------------------------------------------");
}