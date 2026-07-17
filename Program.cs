using System;
using System.Collections.Generic;

namespace OOPFinalExam
{
    /// <summary>
    /// The entry point class of the application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Demonstrates all the functionality required in PART A of the exam.
        /// </summary>
        /// <param name="theargs">The command line arguments.</param>
        public static void Main(string[] theargs)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("==================================================");
            Console.WriteLine("SKYLINKS AIRWAYS - PART A, Q8 AND Q9 DEMONSTRATION");
            Console.WriteLine("==================================================");

            // Initialize Manager
            SkyLinkManager aManager = new SkyLinkManager();

            // --------------------------------------------------
            // 1. Add at least 3 airlines
            // --------------------------------------------------
            Console.WriteLine("\n--- Adding Airlines ---");
            Airline aAirlineVn = aManager.AddAirline("Vietnam Airlines", "VN", "Vietnam", 1956);
            Airline aAirlineVj = aManager.AddAirline("VietJet Air", "VJ", "Vietnam", 2007);
            Airline aAirlineQh = aManager.AddAirline("Bamboo Airways", "QH", "Vietnam", 2017);

            foreach (Airline aAirline in aManager.Airlines)
            {
                Console.WriteLine(aAirline.GetInfo());
            }

            // Demonstrate duplicate airline validation
            Console.WriteLine("\n--- Testing Duplicate Airline Validation ---");
            try
            {
                aManager.AddAirline("VietJet Air Duplicate", "VJ", "Vietnam", 2026);
            }
            catch (Exception aEx)
            {
                Console.WriteLine($"Caught expected exception: {aEx.Message}");
            }

            // --------------------------------------------------
            // 2. Add at least 6 flights (min 2 per airline)
            // --------------------------------------------------
            Console.WriteLine("\n--- Adding Flights (including Business Flights) ---");
            DateTime aFutureDate = DateTime.Now.AddDays(10);

            // VN Flights
            Flight aFlightVn1 = aManager.AddFlight(aAirlineVn.Id, "HAN", "SGN", aFutureDate, 120, 100, 1500000m);
            Flight aFlightVn2 = aManager.AddBusinessFlight(aAirlineVn.Id, "SGN", "DAD", aFutureDate.AddHours(2), 80, 20, 3000000m, true, true, 500000m);

            // VJ Flights
            Flight aFlightVj1 = aManager.AddFlight(aAirlineVj.Id, "HAN", "CXR", aFutureDate.AddDays(1), 100, 180, 1200000m);
            Flight aFlightVj2 = aManager.AddFlight(aAirlineVj.Id, "CXR", "HAN", aFutureDate.AddDays(1).AddHours(3), 100, 1, 1200000m); // Capacity of 1 for testing

            // QH Flights
            Flight aFlightQh1 = aManager.AddFlight(aAirlineQh.Id, "HAN", "UIH", aFutureDate.AddDays(2), 90, 120, 1300000m);
            Flight aFlightQh2 = aManager.AddFlight(aAirlineQh.Id, "UIH", "HAN", aFutureDate.AddDays(2).AddHours(4), 90, 120, 1300000m);

            foreach (Flight aFlight in aManager.Flights)
            {
                Console.WriteLine(aFlight.GetInfo());
            }

            // Demonstrate Flight validation (past date)
            Console.WriteLine("\n--- Testing Past Departure Date Validation ---");
            try
            {
                aManager.AddFlight(aAirlineVn.Id, "HAN", "SGN", DateTime.Now.AddDays(-1), 120, 100, 1500000m);
            }
            catch (Exception aEx)
            {
                Console.WriteLine($"Caught expected exception: {aEx.Message}");
            }

            // Demonstrate Flight validation (origin == destination)
            Console.WriteLine("\n--- Testing Origin equals Destination Validation ---");
            try
            {
                aManager.AddFlight(aAirlineVn.Id, "HAN", "HAN", aFutureDate, 120, 100, 1500000m);
            }
            catch (Exception aEx)
            {
                Console.WriteLine($"Caught expected exception: {aEx.Message}");
            }

            // --------------------------------------------------
            // 3. Demonstrate FlightCode uniqueness & collision resolution
            // --------------------------------------------------
            Console.WriteLine("\n--- Testing FlightCode Generation & Collision Resolution ---");
            // Add two flights with same airline and origin. The natural auto-increment IDs will make them different.
            // But we can manually call GenerateFlightCode to demonstrate suffixing when the code already exists.
            string asFirstCode = aFlightVn1.FlightCode;
            Console.WriteLine($"Existing Flight Code: {asFirstCode}");
            
            // Generate flight code for same airline (VN) and origin (HAN) with ID = 1 (which matches aFlightVn1's ID and base code)
            string asCollidedCode = aManager.GenerateFlightCode("VN", "HAN", aFlightVn1.Id);
            Console.WriteLine($"Generating code for same parameters (ID {aFlightVn1.Id}) results in: {asCollidedCode}");

            // --------------------------------------------------
            // 4. Add at least 12 bookings (mix of statuses)
            // --------------------------------------------------
            Console.WriteLine("\n--- Adding 12 Bookings ---");
            Booking aFullFlightBooking = null;
            try
            {
                // Flight 1 (VN1) Bookings (Capacity 100)
                aManager.AddBooking(aFlightVn1.Id, "Nguyen Van A", "PP001", "01A", BookingStatus.Confirmed);
                aManager.AddBooking(aFlightVn1.Id, "Tran Thi B", "PP002", "01B", BookingStatus.Pending);
                aManager.AddBooking(aFlightVn1.Id, "Le Van C", "PP003", "01C", BookingStatus.Cancelled);

                // Flight 2 (VN2 - Business) Bookings (Capacity 20)
                aManager.AddBooking(aFlightVn2.Id, "Pham Van D", "PP004", "02A", BookingStatus.Confirmed);
                aManager.AddBooking(aFlightVn2.Id, "Hoang Thi E", "PP005", "02B", BookingStatus.Confirmed);

                // Flight 3 (VJ1) Bookings (Capacity 180)
                aManager.AddBooking(aFlightVj1.Id, "Ngo Van F", "PP006", "03A", BookingStatus.Confirmed);
                aManager.AddBooking(aFlightVj1.Id, "Vu Thi G", "PP007", "03B", BookingStatus.Pending);

                // Flight 4 (VJ2) Bookings (Capacity 1)
                aFullFlightBooking = aManager.AddBooking(
                    aFlightVj2.Id,
                    "Doan Van H",
                    "PP008",
                    "04A",
                    BookingStatus.Confirmed); // Fills flight

                // Flight 5 (QH1) Bookings (Capacity 120)
                aManager.AddBooking(aFlightQh1.Id, "Bui Thi I", "PP009", "05A", BookingStatus.Confirmed);
                aManager.AddBooking(aFlightQh1.Id, "Dang Van J", "PP010", "05B", BookingStatus.Pending);
                aManager.AddBooking(aFlightQh1.Id, "Dinh Thi K", "PP011", "05C", BookingStatus.Cancelled);

                // Flight 6 (QH2) Bookings (Capacity 120)
                aManager.AddBooking(aFlightQh2.Id, "Lam Van L", "PP012", "06A", BookingStatus.Confirmed);

                Console.WriteLine("12 bookings added successfully.");
            }
            catch (Exception aEx)
            {
                Console.WriteLine($"Error adding bookings: {aEx.Message}");
            }

            foreach (Booking aBooking in aManager.Bookings)
            {
                Console.WriteLine(aBooking.GetInfo());
            }

            // Demonstrate duplicate PassportNumber validation (globally unique)
            Console.WriteLine("\n--- Testing Duplicate Passport Validation ---");
            try
            {
                aManager.AddBooking(aFlightVn1.Id, "Duplicate Passport Passenger", "PP001", "10A", BookingStatus.Confirmed);
            }
            catch (Exception aEx)
            {
                Console.WriteLine($"Caught expected exception: {aEx.Message}");
            }

            // Demonstrate duplicate SeatNumber validation (unique per flight)
            Console.WriteLine("\n--- Testing Duplicate Seat Validation ---");
            try
            {
                aManager.AddBooking(aFlightVn1.Id, "Duplicate Seat Passenger", "PP099", "01A", BookingStatus.Confirmed);
            }
            catch (Exception aEx)
            {
                Console.WriteLine($"Caught expected exception: {aEx.Message}");
            }

            // --------------------------------------------------
            // 5. Q8 - Sorting algorithms and binary search
            // --------------------------------------------------
            Console.WriteLine("\n--- Q8: Sorting Algorithms and Binary Search ---");
            List<Flight> aSortingSource = new List<Flight>(aManager.Flights);
            aSortingSource.Add(
                new Flight(101, aAirlineVn.Id, "TEST101", "HAN", "PQC",
                    aFutureDate.AddHours(7), 125, 50, 950000m));
            aSortingSource.Add(
                new Flight(102, aAirlineVj.Id, "TEST102", "SGN", "HUI",
                    aFutureDate.AddHours(1), 85, 60, 2100000m));
            aSortingSource.Add(
                new Flight(103, aAirlineQh.Id, "TEST103", "DAD", "HAN",
                    aFutureDate.AddDays(4), 80, 70, 1100000m));
            aSortingSource.Add(
                new Flight(104, aAirlineVn.Id, "TEST104", "CXR", "SGN",
                    aFutureDate.AddDays(3), 70, 80, 1750000m));

            List<Flight> aPriceSortedFlights = new List<Flight>(aSortingSource);
            aManager.BubbleSortByPrice(aPriceSortedFlights);
            Console.WriteLine("Bubble Sort by Price (ascending):");
            foreach (Flight aFlight in aPriceSortedFlights)
            {
                Console.WriteLine($"  {aFlight.FlightCode}: {aFlight.PricePerSeat:N0}");
            }
            Console.WriteLine(
                $"Bubble Sort comparisons: {aManager.BubbleSortComparisonCount}");

            List<Flight> aDepartureSortedFlights = new List<Flight>(aSortingSource);
            aManager.MergeSortByDeparture(aDepartureSortedFlights);
            Console.WriteLine("Merge Sort by DepartureTime (ascending):");
            foreach (Flight aFlight in aDepartureSortedFlights)
            {
                Console.WriteLine(
                    $"  {aFlight.FlightCode}: {aFlight.DepartureTime:yyyy-MM-dd HH:mm}");
            }
            Console.WriteLine(
                $"Merge Sort comparisons: {aManager.MergeSortComparisonCount}");

            DateTime aSearchDeparture = aSortingSource[7].DepartureTime;
            int aiFoundIndex = aManager.BinarySearchByDeparture(
                aDepartureSortedFlights,
                aSearchDeparture);
            Console.WriteLine(
                aiFoundIndex >= 0
                    ? $"Binary Search found {aDepartureSortedFlights[aiFoundIndex].FlightCode} at index {aiFoundIndex}."
                    : "Binary Search did not find the requested departure time.");

            // --------------------------------------------------
            // 6. Q9 - Min-heap standby queue and promotion
            // --------------------------------------------------
            Console.WriteLine("\n--- Q9: Standby Priority Queue ---");
            aManager.AddBooking(
                aFlightVj2.Id,
                "Regular Standby",
                "PP100",
                "04B",
                BookingStatus.Confirmed,
                3);
            aManager.AddBooking(
                aFlightVj2.Id,
                "VIP Standby",
                "PP101",
                "04C",
                BookingStatus.Confirmed,
                1);
            aManager.AddBooking(
                aFlightVj2.Id,
                "Frequent Flyer Standby",
                "PP102",
                "04D",
                BookingStatus.Confirmed,
                2);

            StandbyQueue aStandbyQueue = aManager.GetStandbyQueue(aFlightVj2.Id);
            Console.WriteLine(
                $"Flight {aFlightVj2.FlightCode} standby queue ({aStandbyQueue.Count} passengers):");
            foreach (StandbyPassenger aPassenger in aStandbyQueue.GetPassengersInPriorityOrder())
            {
                Console.WriteLine($"  {aPassenger}");
            }

            if (aFullFlightBooking != null)
            {
                aManager.CancelBooking(aFullFlightBooking.Id);
                Console.WriteLine(
                    $"Cancelled booking {aFullFlightBooking.Id}; status is now {aFullFlightBooking.Status}.");

                Booking aPromotedBooking = aManager.PromoteFromStandby(aFlightVj2.Id);
                Console.WriteLine(
                    $"Promoted: {aPromotedBooking.PassengerName}, Priority 1, " +
                    $"Seat {aPromotedBooking.SeatNumber}, Status {aPromotedBooking.Status}.");
                Console.WriteLine(
                    $"Passengers still on standby: {aStandbyQueue.Count}");
            }

            // Demonstrate Abstraction and Polymorphism
            Console.WriteLine("\n--- Demonstration of Abstraction and Polymorphism ---");
            List<SkyEntity> aEntities = new List<SkyEntity>();
            aEntities.Add(aAirlineVn);
            aEntities.Add(aFlightVn1);
            aEntities.Add(aFlightVn2); // BusinessFlight
            aEntities.Add(aManager.Bookings[0]);

            foreach (SkyEntity aEntity in aEntities)
            {
                Console.WriteLine($"Entity Type: {aEntity.EntityType}");
                Console.WriteLine($"Summary: {aEntity.GetSummary()}");
                Console.WriteLine($"Info: {aEntity.GetInfo()}");
                Console.WriteLine("--------------------------------------------------");
            }

            Console.WriteLine("\nPART A, Q8 AND Q9 DEMONSTRATION COMPLETE.");
        }
    }
}
