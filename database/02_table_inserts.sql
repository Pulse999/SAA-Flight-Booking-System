-- ============================================================
-- TASK 6 - INSERT SAMPLE DATA
-- South African Airways Flight Booking System
-- ============================================================

-- ============================================================
-- 1. AIRPORTS
-- ============================================================

INSERT INTO airports
    (airport_code, airport_name, city, country)
VALUES
    ('JNB', 'O.R. Tambo International Airport', 'Johannesburg', 'South Africa'),
    ('CPT', 'Cape Town International Airport', 'Cape Town', 'South Africa'),
    ('DUR', 'King Shaka International Airport', 'Durban', 'South Africa'),
    ('BFN', 'Bram Fischer International Airport', 'Bloemfontein', 'South Africa'),
    ('PLZ', 'Chief Dawid Stuurman International Airport', 'Gqeberha', 'South Africa');

-- ============================================================
-- 2. PASSENGERS
-- ============================================================

INSERT INTO passengers
    (first_name, last_name, email, phone)
VALUES
    ('Thabo', 'Mokoena', 'thabo.mokoena@example.com', '0825551001'),
    ('Lerato', 'Dlamini', 'lerato.dlamini@example.com', '0835551002'),
    ('Sipho', 'Nkosi', 'sipho.nkosi@example.com', '0845551003'),
    ('Naledi', 'Molefe', 'naledi.molefe@example.com', '0725551004'),
    ('Anele', 'Khumalo', 'anele.khumalo@example.com', '0765551005'),
    ('Kagiso', 'Mthembu', 'kagiso.mthembu@example.com', '0795551006'),
    ('Zanele', 'Ndlovu', 'zanele.ndlovu@example.com', '0815551007'),
    ('Mpho', 'Mahlangu', 'mpho.mahlangu@example.com', '0715551008'),
    ('Bongani', 'Sithole', 'bongani.sithole@example.com', '0785551009'),
    ('Ayanda', 'Pillay', 'ayanda.pillay@example.com', '0745551010'),
    ('Karabo', 'Molefe', 'karabo.molefe@example.com', '0825551011'),
    ('Nomsa', 'Mabena', 'nomsa.mabena@example.com', '0835551012');

-- ============================================================
-- 3. FLIGHTS
-- ============================================================

INSERT INTO flights
    (
        flight_number,
        departure_airport_id,
        arrival_airport_id,
        departure_date_time,
        arrival_date_time,
        capacity
    )
VALUES
    ('SA101', 1, 2, '2026-09-02 07:30:00', '2026-09-02 09:45:00', 180),
    ('SA102', 2, 1, '2026-09-05 18:00:00', '2026-09-05 20:15:00', 180),

    ('SA201', 1, 3, '2026-09-03 10:00:00', '2026-09-03 11:10:00', 160),
    ('SA202', 3, 1, '2026-09-07 14:30:00', '2026-09-07 15:45:00', 160),

    ('SA301', 1, 4, '2026-09-04 08:00:00', '2026-09-04 09:00:00', 120),
    ('SA401', 2, 5, '2026-09-06 12:30:00', '2026-09-06 14:00:00', 150);





-- ============================================================
-- 4. BOOKINGS
-- ============================================================

INSERT INTO bookings
    (flight_id, booking_date, booking_status)
VALUES
    (1, '2026-08-10', 'Confirmed'),
    (1, '2026-08-11', 'Confirmed'),
    (2, '2026-08-12', 'Confirmed'),
    (3, '2026-08-13', 'Pending'),
    (3, '2026-08-14', 'Confirmed'),
    (4, '2026-08-15', 'Cancelled'),
    (5, '2026-08-16', 'Confirmed'),
    (5, '2026-08-17', 'Confirmed'),
    (6, '2026-08-18', 'Pending'),
    (6, '2026-08-19', 'Confirmed');

-- ============================================================
-- 5. BOOKING_PASSENGERS
-- ============================================================

INSERT INTO booking_passengers
    (booking_id, passenger_id)
VALUES
    (1, 1),
    (1, 2),

    (2, 3),

    (3, 4),
    (3, 5),

    (4, 6),

    (5, 7),
    (5, 8),

    (6, 9),

    (7, 10),
    (7, 11),

    (8, 12),

    (9, 2),

    (10, 6);

-- ============================================================
-- 6. TICKETS
-- ============================================================

INSERT INTO tickets
    (
        booking_id,
        ticket_number,
        ticket_status,
        issue_date
    )
VALUES
    (1, 'SA-TKT-100001', 'Issued', '2026-08-10'),
    (2, 'SA-TKT-100002', 'Issued', '2026-08-11'),
    (3, 'SA-TKT-100003', 'Used', '2026-08-12'),
    (5, 'SA-TKT-100005', 'Issued', '2026-08-14'),
    (6, 'SA-TKT-100006', 'Cancelled', '2026-08-15'),
    (7, 'SA-TKT-100007', 'Issued', '2026-08-16'),
    (8, 'SA-TKT-100008', 'Issued', '2026-08-17'),
    (10, 'SA-TKT-100010', 'Issued', '2026-08-19');

-- ============================================================
-- 7. PAYMENTS
-- ============================================================

INSERT INTO payments
    (
        booking_id,
        amount,
        payment_date,
        payment_status,
        payment_method
    )
VALUES
    (1, 2450.00, '2026-08-10', 'Paid', 'Card'),
    (2, 1250.00, '2026-08-11', 'Paid', 'EFT'),
    (3, 3100.00, '2026-08-12', 'Paid', 'Card'),
    (4, 1850.00, '2026-08-13', 'Pending', 'EFT'),
    (5, 2200.00, '2026-08-14', 'Paid', 'Card'),
    (6, 1950.00, '2026-08-15', 'Refunded', 'Card'),
    (7, 2750.00, '2026-08-16', 'Paid', 'EFT'),
    (8, 1450.00, '2026-08-17', 'Paid', 'Card'),
    (9, 1650.00, '2026-08-18', 'Failed', 'Card'),
    (10, 2350.00, '2026-08-19', 'Paid', 'EFT');

