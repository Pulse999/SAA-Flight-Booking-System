\# Database Relationships



\## Task 4 – Relationships (PA0103)



\### Overview



The South African Airways Flight Booking System uses relational database relationships to connect the different entities within the PostgreSQL database.



The relationships are implemented using primary keys and foreign keys to maintain referential integrity and ensure that related records remain consistent.



\## Database Relationships



\### Airports and Flights



The `flights` table contains two foreign keys referencing the `airports` table:



\- `departure\_airport\_id` → `airports.airport\_id`

\- `arrival\_airport\_id` → `airports.airport\_id`



This allows each flight to have both a departure airport and an arrival airport.



\### Flights and Bookings



The `bookings` table contains:



\- `flight\_id` → `flights.flight\_id`



This associates each booking with a specific flight.



\### Bookings and Payments



The `payments` table contains:



\- `booking\_id` → `bookings.booking\_id`



This associates payment records with their corresponding bookings.



\### Bookings and Tickets



The `tickets` table contains:



\- `booking\_id` → `bookings.booking\_id`



This associates tickets with the bookings from which they were generated.



\### Bookings and Passengers



The relationship between bookings and passengers is many-to-many.



A booking can contain multiple passengers, while a passenger can have multiple bookings.



This relationship is implemented using the `booking\_passengers` junction table.



The table contains:



\- `booking\_id` → `bookings.booking\_id`

\- `passenger\_id` → `passengers.passenger\_id`



\## Referential Integrity



Foreign key constraints are used throughout the database to ensure that related records reference existing records.



The implemented relationships were verified using PostgreSQL and pgAdmin 4.

