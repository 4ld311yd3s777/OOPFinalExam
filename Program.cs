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
        /// Demonstrates the implemented airline reservation features, including Q7 and Q8.
        /// </summary>
        /// <param name="theargs">The command line arguments.</param>
        public static void Main(string[] theargs)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("==================================================");
            Console.WriteLine("SKYLINK AIRWAYS - OOP FINAL EXAM DEMONSTRATION");
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
                aManager.AddBooking(aFlightVj2.Id, "Doan Van H", "PP008", "04A", BookingStatus.Confirmed); // Fills flight

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
            // 5. Demonstrate Standby Queue and PromoteFromStandby
            // --------------------------------------------------
            Console.WriteLine("\n--- Testing Priority Queue for Standby Passengers ---");
            // Fill a flight to capacity (VJ2 has capacity 1 and already has 1 confirmed booking from above)
            Console.WriteLine($"Flight VJ2 capacity is {aFlightVj2.TotalSeats}. The flight is already full.");

            // Add 3 more passengers; they should go to standby queue
            aManager.AddBooking(aFlightVj2.Id, "Regular Pass", "PP101", "04B", BookingStatus.Confirmed, 3);
            System.Threading.Thread.Sleep(10); // Ensure distinct registration times
            aManager.AddBooking(aFlightVj2.Id, "VIP Pass", "PP102", "04C", BookingStatus.Confirmed, 1);
            System.Threading.Thread.Sleep(10);
            aManager.AddBooking(aFlightVj2.Id, "Freq Flyer Pass", "PP103", "04D", BookingStatus.Confirmed, 2);

            Console.WriteLine($"Standby queue count for VJ2: {aFlightVj2.StandbyQueue.Count()}");
            Console.WriteLine($"Top passenger expected: VIP Pass (Priority 1)");
            Console.WriteLine($"Actual top: {aFlightVj2.StandbyQueue.Peek().PassengerName} (Priority {aFlightVj2.StandbyQueue.Peek().Priority})");

            Console.WriteLine("\nCancelling the only confirmed booking on VJ2...");
            Booking aBookingVJ2 = null;
            foreach (Booking aBooking in aManager.Bookings)
            {
                 if (aBooking.FlightId == aFlightVj2.Id && aBooking.Status == BookingStatus.Confirmed)
                 {
                     aBookingVJ2 = aBooking;
                     break;
                 }
            }
            if (aBookingVJ2 != null)
            {
                 aManager.CancelBooking(aBookingVJ2.Id);
                 Console.WriteLine($"Cancelled booking for {aBookingVJ2.PassengerName}");
            }

            Console.WriteLine("\nCalling PromoteFromStandby...");
            aManager.PromoteFromStandby(aFlightVj2.Id);

            // --------------------------------------------------
            // 6. Q7 - Demonstrate SkyStack and booking history undo
            // --------------------------------------------------
            Console.WriteLine("\n--- Q7: Custom SkyStack and Booking History ---");
            SkyStack<int> aoTestStack = new SkyStack<int>();
            for (int aiValue = 1; aiValue <= 10; aiValue++)
            {
                aoTestStack.Push(aiValue);
            }
            Console.WriteLine($"SkyStack size after 10 pushes: {aoTestStack.Size()}");
            Console.WriteLine($"SkyStack top before pop: {aoTestStack.Peek()}");
            Console.WriteLine($"SkyStack popped value: {aoTestStack.Pop()}");

            Booking aHistoryBooking1 = aManager.AddBooking(
                aFlightVn1.Id, "History Passenger One", "PP201", "11A", BookingStatus.Confirmed);
            Booking aHistoryBooking2 = aManager.AddBooking(
                aFlightVn1.Id, "History Passenger Two", "PP202", "11B", BookingStatus.Pending);
            Booking aHistoryBooking3 = aManager.AddBooking(
                aFlightVn1.Id, "History Passenger Three", "PP203", "11C", BookingStatus.Confirmed);

            Console.WriteLine(
                $"Added bookings: {aHistoryBooking1.Id}, {aHistoryBooking2.Id}, {aHistoryBooking3.Id}");
            aManager.CancelBooking(aHistoryBooking2.Id);
            Console.WriteLine(
                $"Before undo: booking {aHistoryBooking2.Id} status = {aHistoryBooking2.Status}");

            Booking aUndoneBooking = aManager.UndoLastBookingAction();
            Console.WriteLine(
                $"After undo: booking {aUndoneBooking.Id} status = {aUndoneBooking.Status}");
            Console.WriteLine($"Booking history entries remaining: {aManager.BookingHistorySize}");

            // --------------------------------------------------
            // 7. Q8 - Demonstrate custom sorting and binary search
            // --------------------------------------------------
            Console.WriteLine("\n--- Q8: Flight Sorting Algorithms and Binary Search ---");

            // Add four flights so both algorithms operate on the same 10 flight objects.
            aManager.AddFlight(aAirlineVn.Id, "DAD", "PQC", aFutureDate.AddHours(5), 75, 90, 900000m);
            aManager.AddFlight(aAirlineVj.Id, "SGN", "HPH", aFutureDate.AddDays(12), 125, 150, 2400000m);
            aManager.AddFlight(aAirlineQh.Id, "VCA", "DLI", aFutureDate.AddDays(6), 70, 80, 1100000m);
            aManager.AddFlight(aAirlineVn.Id, "PQC", "HAN", aFutureDate.AddDays(3), 130, 100, 1800000m);

            List<Flight> aoFlightsByPrice = new List<Flight>(aManager.Flights);
            List<Flight> aoFlightsByDeparture = new List<Flight>(aManager.Flights);
            DateTime adtSearchTarget = aFlightVj1.DepartureTime;

            aManager.BubbleSortByPrice(aoFlightsByPrice);
            Console.WriteLine("Bubble Sort by PricePerSeat (ascending):");
            foreach (Flight aFlight in aoFlightsByPrice)
            {
                Console.WriteLine($"{aFlight.FlightCode}: {aFlight.PricePerSeat:N0}");
            }
            Console.WriteLine(
                $"Bubble Sort comparisons: {aManager.BubbleSortComparisonCount}");

            aManager.MergeSortByDeparture(aoFlightsByDeparture);
            Console.WriteLine("\nMerge Sort by DepartureTime (ascending):");
            foreach (Flight aFlight in aoFlightsByDeparture)
            {
                Console.WriteLine(
                    $"{aFlight.FlightCode}: {aFlight.DepartureTime:yyyy-MM-dd HH:mm:ss}");
            }
            Console.WriteLine(
                $"Merge Sort comparisons: {aManager.MergeSortComparisonCount}");

            int aiFoundIndex = aManager.BinarySearchByDeparture(
                aoFlightsByDeparture, adtSearchTarget);
            Console.WriteLine(
                $"\nBinary Search target {adtSearchTarget:yyyy-MM-dd HH:mm:ss}: index {aiFoundIndex}");
            if (aiFoundIndex >= 0)
            {
                Console.WriteLine(
                    $"Found flight: {aoFlightsByDeparture[aiFoundIndex].FlightCode}");
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

            Console.WriteLine("\nQ7 AND Q8 DEMONSTRATION COMPLETE.");
        }
    }
}
