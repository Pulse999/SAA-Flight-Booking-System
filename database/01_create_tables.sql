-- ============================================================
-- South African Airways Flight Booking System
-- Database: saa_flight_booking
-- Task 3: Create Database Tables
-- PostgreSQL
-- ============================================================


-- ============================================================
-- 1. AIRPORTS
-- ============================================================

CREATE TABLE airports (
    airport_id INTEGER GENERATED ALWAYS AS IDENTITY,
    airport_code VARCHAR(10) NOT NULL,
    airport_name VARCHAR(100) NOT NULL,
    city VARCHAR(100) NOT NULL,
    country VARCHAR(100) NOT NULL,

    CONSTRAINT pk_airports
        PRIMARY KEY (airport_id),

    CONSTRAINT uq_airports_code
        UNIQUE (airport_code)
);


-- ============================================================
-- 2. PASSENGERS
-- ============================================================

CREATE TABLE passengers (
    passenger_id INTEGER GENERATED ALWAYS AS IDENTITY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(150) NOT NULL,
    phone VARCHAR(20) NOT NULL,

    CONSTRAINT pk_passengers
        PRIMARY KEY (passenger_id),

    CONSTRAINT uq_passengers_email
        UNIQUE (email)
);


-- ============================================================
-- 3. FLIGHTS
-- ============================================================

CREATE TABLE flights (
    flight_id INTEGER GENERATED ALWAYS AS IDENTITY,
    flight_number VARCHAR(20) NOT NULL,
    departure_airport_id INTEGER NOT NULL,
    arrival_airport_id INTEGER NOT NULL,
    departure_date_time TIMESTAMP NOT NULL,
    arrival_date_time TIMESTAMP NOT NULL,
    capacity INTEGER NOT NULL,

    CONSTRAINT pk_flights
        PRIMARY KEY (flight_id),

    CONSTRAINT fk_flights_departure_airport
        FOREIGN KEY (departure_airport_id)
        REFERENCES airports (airport_id)
        ON DELETE RESTRICT,

    CONSTRAINT fk_flights_arrival_airport
        FOREIGN KEY (arrival_airport_id)
        REFERENCES airports (airport_id)
        ON DELETE RESTRICT,

    CONSTRAINT chk_flights_different_airports
        CHECK (departure_airport_id <> arrival_airport_id),

    CONSTRAINT chk_flights_capacity
        CHECK (capacity > 0),

    CONSTRAINT chk_flights_times
        CHECK (arrival_date_time > departure_date_time)
);


-- ============================================================
-- 4. BOOKINGS
-- ============================================================

CREATE TABLE bookings (
    booking_id INTEGER GENERATED ALWAYS AS IDENTITY,
    flight_id INTEGER NOT NULL,
    booking_date DATE NOT NULL,
    booking_status VARCHAR(30) NOT NULL,

    CONSTRAINT pk_bookings
        PRIMARY KEY (booking_id),

    CONSTRAINT fk_bookings_flight
        FOREIGN KEY (flight_id)
        REFERENCES flights (flight_id)
        ON DELETE RESTRICT,

    CONSTRAINT chk_bookings_status
        CHECK (
            booking_status IN (
                'Pending',
                'Confirmed',
                'Cancelled'
            )
        )
);


-- ============================================================
-- 5. BOOKING_PASSENGERS
-- Associative table for the many-to-many relationship
-- between Bookings and Passengers
-- ============================================================

CREATE TABLE booking_passengers (
    booking_id INTEGER NOT NULL,
    passenger_id INTEGER NOT NULL,

    CONSTRAINT pk_booking_passengers
        PRIMARY KEY (booking_id, passenger_id),

    CONSTRAINT fk_booking_passengers_booking
        FOREIGN KEY (booking_id)
        REFERENCES bookings (booking_id)
        ON DELETE RESTRICT,

    CONSTRAINT fk_booking_passengers_passenger
        FOREIGN KEY (passenger_id)
        REFERENCES passengers (passenger_id)
        ON DELETE RESTRICT
);


-- ============================================================
-- 6. TICKETS
-- ============================================================

CREATE TABLE tickets (
    ticket_id INTEGER GENERATED ALWAYS AS IDENTITY,
    booking_id INTEGER NOT NULL,
    ticket_number VARCHAR(30) NOT NULL,
    ticket_status VARCHAR(30) NOT NULL,
    issue_date DATE NOT NULL,

    CONSTRAINT pk_tickets
        PRIMARY KEY (ticket_id),

    CONSTRAINT fk_tickets_booking
        FOREIGN KEY (booking_id)
        REFERENCES bookings (booking_id)
        ON DELETE RESTRICT,

    CONSTRAINT uq_tickets_booking
        UNIQUE (booking_id),

    CONSTRAINT uq_tickets_number
        UNIQUE (ticket_number),

    CONSTRAINT chk_tickets_status
        CHECK (
            ticket_status IN (
                'Issued',
                'Cancelled',
                'Used'
            )
        )
);


-- ============================================================
-- 7. PAYMENTS
-- ============================================================

CREATE TABLE payments (
    payment_id INTEGER GENERATED ALWAYS AS IDENTITY,
    booking_id INTEGER NOT NULL,
    amount NUMERIC(10,2) NOT NULL,
    payment_date DATE NOT NULL,
    payment_status VARCHAR(30) NOT NULL,
    payment_method VARCHAR(30) NOT NULL,

    CONSTRAINT pk_payments
        PRIMARY KEY (payment_id),

    CONSTRAINT fk_payments_booking
        FOREIGN KEY (booking_id)
        REFERENCES bookings (booking_id)
        ON DELETE RESTRICT,

    CONSTRAINT uq_payments_booking
        UNIQUE (booking_id),

    CONSTRAINT chk_payments_amount
        CHECK (amount >= 0),

    CONSTRAINT chk_payments_status
        CHECK (
            payment_status IN (
                'Pending',
                'Paid',
                'Failed',
                'Refunded'
            )
        ),

    CONSTRAINT chk_payments_method
        CHECK (
            payment_method IN (
                'Card',
                'EFT',
                'Cash'
            )
        )
);