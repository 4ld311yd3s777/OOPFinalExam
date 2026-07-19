using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

            #region PART A: Entity Setup - Airlines, Flights, Bookings

            #region A1: Add Airlines (3 Airlines)

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

            #endregion

            #region A2: Add Flights (6+ Flights including Business Flights)

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

            #endregion

            #region A3: Add Bookings (12+ Bookings with validation)

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

            #endregion

            #region A4: Standby Queue - Add passengers to full flight

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

            #endregion

            #region Part C: Q7 - SkyStack & Booking History Undo

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

            #endregion

            #region Part C: Q8 - Sorting Algorithms & Binary Search

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

            #endregion

            #endregion

            // ============================================================
            // ============================================================
            // PART D - INTEGRATION CHALLENGE (Q10)
            // Full Simulation Run - Exercise all features from Parts A-C
            // ============================================================
            // ============================================================

            Console.WriteLine("\n");
            Console.WriteLine("#######################################################");
            Console.WriteLine("###                                               ###");
            Console.WriteLine("###   PART D - INTEGRATION CHALLENGE (Q10)        ###");
            Console.WriteLine("###   Full System Simulation Run                  ###");
            Console.WriteLine("###                                               ###");
            Console.WriteLine("#######################################################");
            Console.WriteLine();
            Console.WriteLine("Part D Label Mapping (matching exam requirements):");
            Console.WriteLine("  ss) Setup System Data (3 Airlines, 8 Flights, 15 Bookings)");
            Console.WriteLine("  tt) LINQ Reports (Q5 & Q6)");
            Console.WriteLine("  qq) Sorting Algorithms (Bubble Sort, Merge Sort, Binary Search)");
            Console.WriteLine("  rr) Standby Queue Demonstration");
            Console.WriteLine("  ss) Undo Demonstration (Note: label appears twice in exam)");
            Console.WriteLine("  tt) Polymorphism Demonstration (Note: label appears twice in exam)");
            Console.WriteLine("  uu) System-Wide Summary");
            Console.WriteLine();

            // Reset manager for clean Part D demonstration
            aManager = new SkyLinkManager();

            // ================================================================
            // REGION D1: SETUP SYSTEM DATA
            // Create 3 Airlines, 8 Flights (6 Regular + 2 Business), 15+ Bookings
            // ================================================================

            #region REGION D1: Setup System Data (ss)

            Console.WriteLine("\n=== ss) SETUP SYSTEM DATA ===\n");

            // D1.1: Add 3 Airlines
            Console.WriteLine("--- D1.1: Adding 3 Airlines ---");
            Airline dAirlineVn = aManager.AddAirline("Vietnam Airlines", "VN", "Vietnam", 1956);
            Airline dAirlineVj = aManager.AddAirline("VietJet Air", "VJ", "Vietnam", 2007);
            Airline dAirlineQh = aManager.AddAirline("Bamboo Airways", "QH", "Vietnam", 2017);
            Console.WriteLine($"Added {aManager.Airlines.Count} airlines.\n");

            // D1.2: Add 8 Flights (6 Regular + 2 Business)
            Console.WriteLine("--- D1.2: Adding 8 Flights (6 Regular + 2 Business) ---");
            DateTime dFutureDate = DateTime.Now.AddDays(10);

            // Regular Flights (6)
            Flight dFlightVn1 = aManager.AddFlight(dAirlineVn.Id, "HAN", "SGN", dFutureDate, 120, 100, 1500000m);
            Flight dFlightVj1 = aManager.AddFlight(dAirlineVj.Id, "HAN", "CXR", dFutureDate.AddDays(1), 100, 80, 1200000m);
            Flight dFlightVj2 = aManager.AddFlight(dAirlineVj.Id, "CXR", "HAN", dFutureDate.AddDays(1).AddHours(3), 100, 80, 1200000m);
            Flight dFlightQh1 = aManager.AddFlight(dAirlineQh.Id, "HAN", "UIH", dFutureDate.AddDays(2), 90, 120, 1300000m);
            Flight dFlightQh2 = aManager.AddFlight(dAirlineQh.Id, "UIH", "HAN", dFutureDate.AddDays(2).AddHours(4), 90, 120, 1300000m);
            Flight dFlightVn3 = aManager.AddFlight(dAirlineVn.Id, "SGN", "DAD", dFutureDate.AddDays(3), 90, 100, 1400000m);

            // Business Flights (2)
            Flight dFlightVn2 = aManager.AddBusinessFlight(dAirlineVn.Id, "SGN", "PQC", dFutureDate.AddHours(2), 75, 20, 3000000m, true, true, 500000m);
            Flight dFlightVj3 = aManager.AddBusinessFlight(dAirlineVj.Id, "SGN", "HAN", dFutureDate.AddDays(4), 110, 15, 3500000m, true, true, 700000m);

            int dRegularCount = 6;
            int dBusinessCount = 2;
            Console.WriteLine($"Added {aManager.Flights.Count} flights ({dRegularCount} regular + {dBusinessCount} business).\n");

            // D1.3: Add 15 Bookings across all flights
            Console.WriteLine("--- D1.3: Adding 15 Bookings ---");

            // VN Flights Bookings
            aManager.AddBooking(dFlightVn1.Id, "Nguyen Van A", "PP001", "01A", BookingStatus.Confirmed);
            aManager.AddBooking(dFlightVn1.Id, "Tran Thi B", "PP002", "01B", BookingStatus.Confirmed);
            aManager.AddBooking(dFlightVn1.Id, "Le Van C", "PP003", "01C", BookingStatus.Pending);

            // VJ Flights Bookings
            aManager.AddBooking(dFlightVj1.Id, "Pham Van D", "PP004", "03A", BookingStatus.Confirmed);
            aManager.AddBooking(dFlightVj1.Id, "Hoang Thi E", "PP005", "03B", BookingStatus.Confirmed);
            aManager.AddBooking(dFlightVj2.Id, "Ngo Van F", "PP006", "04A", BookingStatus.Confirmed);
            aManager.AddBooking(dFlightVj2.Id, "Vu Thi G", "PP007", "04B", BookingStatus.Pending);

            // QH Flights Bookings
            aManager.AddBooking(dFlightQh1.Id, "Doan Van H", "PP008", "05A", BookingStatus.Confirmed);
            aManager.AddBooking(dFlightQh1.Id, "Bui Thi I", "PP009", "05B", BookingStatus.Confirmed);
            aManager.AddBooking(dFlightQh1.Id, "Dang Van J", "PP010", "05C", BookingStatus.Cancelled);
            aManager.AddBooking(dFlightQh2.Id, "Dinh Thi K", "PP011", "06A", BookingStatus.Confirmed);

            // VN3 + Business Flights Bookings
            aManager.AddBooking(dFlightVn3.Id, "Lam Van L", "PP012", "07A", BookingStatus.Confirmed);
            aManager.AddBooking(dFlightVn2.Id, "Truong Thi M", "PP013", "08A", BookingStatus.Confirmed);
            aManager.AddBooking(dFlightVj3.Id, "Bui Van N", "PP014", "09A", BookingStatus.Confirmed);
            aManager.AddBooking(dFlightVn3.Id, "Hoang Van O", "PP015", "07B", BookingStatus.Pending);

            Console.WriteLine($"Added {aManager.Bookings.Count} bookings.\n");

            #endregion

            // ================================================================
            // REGION D2: LINQ REPORTS (Part B - Q5 & Q6)
            // Execute all LINQ queries and display results
            // ================================================================

            #region REGION D2: LINQ Reports (tt)

            Console.WriteLine("=== tt) LINQ REPORTS (Q5 & Q6) ===\n");

            // Q5: LINQ Queries (q, r, s, t, u)

            #region Q5: LINQ Queries

            Console.WriteLine("--- Q5: Complex LINQ Queries ---\n");

            // q) Confirmed bookings for a specific flight (sorted by SeatNumber)
            Console.WriteLine("q) Confirmed Bookings for Flight VNHAN001:");
            var q5qResults = aManager.ConfirmedBookings(dFlightVn1.Id);
            Console.WriteLine($"   Found {q5qResults.Count} confirmed booking(s):");
            foreach (dynamic item in q5qResults)
            {
                Console.WriteLine($"     - {item.PassengerName} | Seat: {item.SeatNumber} | Fee: {item.BookingFee:N0}");
            }
            Console.WriteLine();

            // r) Top 5 most expensive flights with available seats
            Console.WriteLine("r) Top 5 Most Expensive Flights with Available Seats:");
            var q5rResults = aManager.GetTop5ExpensiveAvailableFlights();
            foreach (dynamic item in q5rResults)
            {
                Console.WriteLine($"     {item.AirlineName} | {item.FlightCode} | {item.Origin}->{item.Destination} | " +
                                  $"Price: {item.PricePerSeat:N0} | Available: {item.AvailableSeats} seats");
            }
            Console.WriteLine();

            // s) Airline financial statistics (sorted by Revenue desc)
            Console.WriteLine("s) Airline Financial Statistics (by Revenue desc):");
            var q5sResults = aManager.GetAirlineFinancialStatistics();
            Console.WriteLine($"{"Airline",-20} {"Flights",-10} {"Bookings",-10} {"Confirmed",-10} {"Revenue",-15}");
            Console.WriteLine(new string('-', 70));
            foreach (dynamic item in q5sResults)
            {
                Console.WriteLine($"  {item.AirlineName,-18} {item.TotalFlights,-10} {item.TotalBookings,-10} " +
                                  $"{item.ConfirmedBookings,-10} {item.TotalRevenue,15:N0}");
            }
            Console.WriteLine();

            // t) Frequent passengers (booked more than 1 flight)
            Console.WriteLine("t) Frequent Passengers (booked > 1 flight):");
            var q5tResults = aManager.GetFrequentPassengers();
            if (q5tResults.Count == 0)
            {
                Console.WriteLine("   No passengers have booked more than one flight.");
            }
            else
            {
                foreach (dynamic item in q5tResults)
                {
                    Console.WriteLine($"   {item.PassengerName} | Passport: {item.PassportNumber} | Flights: {item.FlightCount}");
                }
            }
            Console.WriteLine();

            // u) Flight schedule for a route
            Console.WriteLine("u) Flight Schedule for Route HAN -> SGN:");
            var q5uResults = aManager.GetFlightSchedule("HAN", "SGN");
            foreach (dynamic item in q5uResults)
            {
                Console.WriteLine($"   {item.FlightCode} | Departure: {item.DepartureTime:yyyy-MM-dd HH:mm} | " +
                                  $"Duration: {item.DurationMinutes} mins | Price: {item.PricePerSeat:N0}");
            }
            Console.WriteLine();

            #endregion

            // Q6: Advanced Aggregation (v, w, x, y)

            #region Q6: Advanced Aggregation

            Console.WriteLine("--- Q6: Advanced Aggregation & Projection ---\n");

            // v) Busiest day of the week
            Console.WriteLine("v) Busiest Day of the Week (by departures):");
            dynamic q6vResult = aManager.FindBusiestDayOfWeek();
            if (q6vResult != null)
            {
                Console.WriteLine($"   Day: {q6vResult.DayName} | Total departures: {q6vResult.Count}");
            }
            Console.WriteLine();

            // w) Average load factor per airline
            Console.WriteLine("w) Average Load Factor per Airline:");
            var q6wResults = aManager.GetAverageLoadFactorPerAirline();
            Console.WriteLine($"{"Airline",-20} {"Avg Load Factor (%)",-20}");
            Console.WriteLine(new string('-', 45));
            foreach (dynamic item in q6wResults)
            {
                Console.WriteLine($"  {item.AirlineName,-18} {item.AvgLoadFactor,18:N2}%");
            }
            Console.WriteLine();

            // x) Route statistics
            Console.WriteLine("x) Route Statistics:");
            var q6xResults = aManager.GetRouteStatistics();
            Console.WriteLine($"{"Route",-15} {"Flights",-8} {"Cheapest",-12} {"Most Expensive",-15} {"Average",-12}");
            Console.WriteLine(new string('-', 70));
            foreach (dynamic item in q6xResults)
            {
                Console.WriteLine($"  {item.Route,-13} {item.FlightCount,-8} {item.CheapestPrice,12:N0} " +
                                  $"{item.MostExpensivePrice,15:N0} {item.AveragePrice,12:N0}");
            }
            Console.WriteLine();

            // y) Passengers with all cancelled bookings
            Console.WriteLine("y) Passengers with All Cancelled Bookings:");
            var q6yResults = aManager.GetPassengersWithAllCancelledBookings();
            if (q6yResults.Count == 0)
            {
                Console.WriteLine("   No passengers have all their bookings cancelled.");
            }
            else
            {
                foreach (dynamic item in q6yResults)
                {
                    Console.WriteLine($"   {item.PassengerName} | Passport: {item.PassportNumber} | " +
                                      $"Cancelled Count: {item.CancelledCount}");
                }
            }
            Console.WriteLine();

            #endregion

            #endregion

            // ================================================================
            // REGION D3: SORTING ALGORITHMS (Part C - Q8)
            // Bubble Sort, Merge Sort, and Binary Search demonstration
            // ================================================================

            #region REGION D3: Sorting Algorithms (qq)

            Console.WriteLine("=== qq) SORTING ALGORITHMS ===\n");

            // Create copies of flight list for sorting
            List<Flight> dFlightsByPrice = new List<Flight>(aManager.Flights);
            List<Flight> dFlightsByDeparture = new List<Flight>(aManager.Flights);

            Console.WriteLine($"Sorting {dFlightsByPrice.Count} flights...\n");

            // Bubble Sort by PricePerSeat (ascending)
            Console.WriteLine("Bubble Sort by PricePerSeat (ascending):");
            aManager.BubbleSortByPrice(dFlightsByPrice);
            foreach (Flight f in dFlightsByPrice)
            {
                Console.WriteLine($"  {f.FlightCode}: {f.PricePerSeat:N0} VND");
            }
            Console.WriteLine($"\n  Comparisons: {aManager.BubbleSortComparisonCount}\n");

            // Merge Sort by DepartureTime (ascending)
            Console.WriteLine("Merge Sort by DepartureTime (ascending):");
            aManager.MergeSortByDeparture(dFlightsByDeparture);
            foreach (Flight f in dFlightsByDeparture)
            {
                Console.WriteLine($"  {f.FlightCode}: {f.DepartureTime:yyyy-MM-dd HH:mm}");
            }
            Console.WriteLine($"\n  Comparisons: {aManager.MergeSortComparisonCount}\n");

            // Algorithm comparison
            Console.WriteLine("Sorting Algorithm Comparison:");
            Console.WriteLine($"  Bubble Sort: {aManager.BubbleSortComparisonCount} comparisons");
            Console.WriteLine($"  Merge Sort: {aManager.MergeSortComparisonCount} comparisons");
            int diff = Math.Abs(aManager.BubbleSortComparisonCount - aManager.MergeSortComparisonCount);
            Console.WriteLine($"  Difference: {diff} comparisons");
            Console.WriteLine($"  => {(aManager.MergeSortComparisonCount < aManager.BubbleSortComparisonCount ? "Merge Sort is more efficient" : "Both perform similarly")}\n");

            // Binary Search after Merge Sort
            Console.WriteLine("Binary Search by DepartureTime:");
            Flight dTarget = dFlightsByDeparture[dFlightsByDeparture.Count / 2];
            DateTime dSearchTarget = dTarget.DepartureTime;
            int dFoundIndex = aManager.BinarySearchByDeparture(dFlightsByDeparture, dSearchTarget);

            Console.WriteLine($"  Search target: {dSearchTarget:yyyy-MM-dd HH:mm:ss}");
            if (dFoundIndex >= 0)
            {
                Flight dFound = dFlightsByDeparture[dFoundIndex];
                Console.WriteLine($"  Result: FOUND at index {dFoundIndex}");
                Console.WriteLine($"  Flight: {dFound.FlightCode} ({dFound.Origin}->{dFound.Destination})");
            }
            else
            {
                Console.WriteLine("  Result: NOT FOUND");
            }
            Console.WriteLine();

            #endregion

            // ================================================================
            // REGION D4: STANDBY QUEUE DEMONSTRATION (Part C - Q9)
            // Fill 2 flights to capacity, add standby passengers, promote
            // ================================================================

            #region REGION D4: Standby Queue (rr)

            Console.WriteLine("=== rr) STANDBY QUEUE DEMONSTRATION ===\n");

            // Create 2 flights with small capacity for testing
            Console.WriteLine("Creating 2 flights with small capacity (3 seats each) for standby testing:");
            Flight dStandbyFlight1 = aManager.AddFlight(dAirlineVn.Id, "PQC", "SGN", dFutureDate.AddDays(5), 60, 3, 1800000m);
            Flight dStandbyFlight2 = aManager.AddFlight(dAirlineVj.Id, "DAD", "SGN", dFutureDate.AddDays(5).AddHours(2), 65, 3, 1900000m);
            Console.WriteLine($"  Created: {dStandbyFlight1.FlightCode} (Capacity: 3)");
            Console.WriteLine($"  Created: {dStandbyFlight2.FlightCode} (Capacity: 3)\n");

            // Fill both flights to capacity
            Console.WriteLine("Filling both flights to capacity:");

            // Flight 1 - Fill to capacity
            aManager.AddBooking(dStandbyFlight1.Id, "Passenger S1-1", "PS101", "S1A", BookingStatus.Confirmed);
            aManager.AddBooking(dStandbyFlight1.Id, "Passenger S1-2", "PS102", "S1B", BookingStatus.Confirmed);
            aManager.AddBooking(dStandbyFlight1.Id, "Passenger S1-3", "PS103", "S1C", BookingStatus.Confirmed);
            Console.WriteLine($"  {dStandbyFlight1.FlightCode}: FULL (3/3 confirmed)");

            // Flight 2 - Fill to capacity
            aManager.AddBooking(dStandbyFlight2.Id, "Passenger S2-1", "PS201", "S2A", BookingStatus.Confirmed);
            aManager.AddBooking(dStandbyFlight2.Id, "Passenger S2-2", "PS202", "S2B", BookingStatus.Confirmed);
            aManager.AddBooking(dStandbyFlight2.Id, "Passenger S2-3", "PS203", "S2C", BookingStatus.Confirmed);
            Console.WriteLine($"  {dStandbyFlight2.FlightCode}: FULL (3/3 confirmed)\n");

            // Add 3 standby passengers to each flight (with different priorities)
            Console.WriteLine("Adding 3 standby passengers to each full flight (VIP=1, Frequent=2, Regular=3):");

            // Flight 1 standby passengers
            aManager.AddBooking(dStandbyFlight1.Id, "Standby VIP 1", "PS302", "S1E", BookingStatus.Confirmed, 1);      // Priority 1
            System.Threading.Thread.Sleep(10);
            aManager.AddBooking(dStandbyFlight1.Id, "Standby Frequent 1", "PS303", "S1F", BookingStatus.Confirmed, 2);  // Priority 2
            System.Threading.Thread.Sleep(10);
            aManager.AddBooking(dStandbyFlight1.Id, "Standby Regular 1", "PS301", "S1D", BookingStatus.Confirmed, 3);   // Priority 3

            // Flight 2 standby passengers
            aManager.AddBooking(dStandbyFlight2.Id, "Standby VIP 2", "PS402", "S2E", BookingStatus.Confirmed, 1);      // Priority 1
            System.Threading.Thread.Sleep(10);
            aManager.AddBooking(dStandbyFlight2.Id, "Standby Frequent 2", "PS403", "S2F", BookingStatus.Confirmed, 2);  // Priority 2
            System.Threading.Thread.Sleep(10);
            aManager.AddBooking(dStandbyFlight2.Id, "Standby Regular 2", "PS401", "S2D", BookingStatus.Confirmed, 3);   // Priority 3

            Console.WriteLine($"  {dStandbyFlight1.FlightCode} standby queue: {dStandbyFlight1.StandbyQueue.Count()}");
            Console.WriteLine($"  {dStandbyFlight2.FlightCode} standby queue: {dStandbyFlight2.StandbyQueue.Count()}\n");

            // Display top of each standby queue (should be VIP with priority 1)
            Console.WriteLine("Top of Standby Queues (should be VIP with Priority 1):");
            Console.WriteLine($"  {dStandbyFlight1.FlightCode}: {dStandbyFlight1.StandbyQueue.Peek().PassengerName} " +
                              $"(Priority {dStandbyFlight1.StandbyQueue.Peek().Priority})");
            Console.WriteLine($"  {dStandbyFlight2.FlightCode}: {dStandbyFlight2.StandbyQueue.Peek().PassengerName} " +
                              $"(Priority {dStandbyFlight2.StandbyQueue.Peek().Priority})\n");

            // Cancel one booking and promote from standby
            Console.WriteLine("Cancelling one booking and promoting from standby...");
            var dToCancel = aManager.Bookings.FirstOrDefault(b => b.FlightId == dStandbyFlight1.Id &&
                                                                  b.Status == BookingStatus.Confirmed);
            if (dToCancel != null)
            {
                Console.WriteLine($"  Cancelled: {dToCancel.PassengerName} on {dStandbyFlight1.FlightCode}");
                aManager.CancelBooking(dToCancel.Id);
            }

            Console.WriteLine($"\n  Promoting from standby for {dStandbyFlight1.FlightCode}...");
            aManager.PromoteFromStandby(dStandbyFlight1.Id);
            Console.WriteLine($"  Standby queue remaining: {dStandbyFlight1.StandbyQueue.Count()}\n");

            #endregion

            // ================================================================
            // REGION D5: UNDO DEMONSTRATION (Part C - Q7)
            // Add booking, cancel it, then undo the action
            // ================================================================

            #region REGION D5: Undo Demonstration (ss)

            Console.WriteLine("=== ss) UNDO DEMONSTRATION ===\n");

            Console.WriteLine("Adding a new booking for undo demonstration:");
            Booking dUndoBooking = aManager.AddBooking(dFlightVn3.Id, "Undo Test Passenger", "PUT001", "UNDO1", BookingStatus.Confirmed);
            Console.WriteLine($"  Added booking ID {dUndoBooking.Id} for {dUndoBooking.PassengerName}");
            Console.WriteLine($"  Total bookings: {aManager.Bookings.Count}\n");

            Console.WriteLine("Cancelling the booking:");
            Console.WriteLine($"  Before cancel: Status = {dUndoBooking.Status}");
            aManager.CancelBooking(dUndoBooking.Id);
            Console.WriteLine($"  After cancel: Status = {dUndoBooking.Status}\n");

            Console.WriteLine("Calling UndoLastBookingAction():");
            Console.WriteLine($"  Booking history size before undo: {aManager.BookingHistorySize}");
            Booking dUndoneBooking = aManager.UndoLastBookingAction();
            Console.WriteLine($"  Undone booking ID: {dUndoneBooking.Id}");
            Console.WriteLine($"  Status after undo: {dUndoneBooking.Status}");
            Console.WriteLine($"  Booking history size after undo: {aManager.BookingHistorySize}\n");

            Console.WriteLine("Summary:");
            Console.WriteLine("  Before: Booking was CANCELLED after CancelBooking()");
            Console.WriteLine("  After:  Booking is back to CONFIRMED after UndoLastBookingAction()\n");

            #endregion

            // ================================================================
            // REGION D6: POLYMORPHISM DEMONSTRATION
            // List<SkyEntity> with all entity types demonstrating polymorphism
            // ================================================================

            #region REGION D6: Polymorphism (tt)

            Console.WriteLine("=== tt) POLYMORPHISM DEMONSTRATION ===\n");

            // Create list of SkyEntity (polymorphic collection)
            List<SkyEntity> dEntities = new List<SkyEntity>();

            // Add Airlines
            dEntities.Add(dAirlineVn);
            dEntities.Add(dAirlineVj);
            dEntities.Add(dAirlineQh);

            // Add Flights (including BusinessFlight)
            dEntities.Add(dFlightVn1);  // Regular Flight
            dEntities.Add(dFlightVn2);  // BusinessFlight
            dEntities.Add(dFlightVj1);  // Regular Flight
            dEntities.Add(dFlightVj3);  // BusinessFlight

            // Add Bookings
            dEntities.Add(aManager.Bookings[0]);
            dEntities.Add(aManager.Bookings[1]);
            dEntities.Add(aManager.Bookings[2]);

            Console.WriteLine($"Created List<SkyEntity> with {dEntities.Count} entities.\n");

            // Group by EntityType and display
            Console.WriteLine("Grouped by EntityType:\n");
            foreach (var dGroup in dEntities.GroupBy(e => e.EntityType))
            {
                Console.WriteLine($"  [{dGroup.Key}] ({dGroup.Count()} item(s))");
                Console.WriteLine(new string('-', 50));
                foreach (SkyEntity e in dGroup)
                {
                    Console.WriteLine($"    {e.GetInfo()}");
                }
                Console.WriteLine();
            }

            // Polymorphic calls - same method, different behavior
            Console.WriteLine("Polymorphic GetInfo() calls:");
            foreach (SkyEntity e in dEntities)
            {
                Console.WriteLine($"  [{e.EntityType}] {e.GetInfo()}");
            }
            Console.WriteLine();

            #endregion

            // ================================================================
            // REGION D7: SYSTEM-WIDE SUMMARY TABLE
            // Final statistics for the entire system
            // ================================================================

            #region REGION D7: System-Wide Summary (uu)

            Console.WriteLine("=== uu) SYSTEM-WIDE SUMMARY ===\n");

            // Calculate statistics
            int dTotalFlights = aManager.Flights.Count;
            int dTotalBookings = aManager.Bookings.Count;
            int dConfirmedBookings = aManager.Bookings.Count(b => b.Status == BookingStatus.Confirmed);
            decimal dTotalRevenue = aManager.Bookings.Where(b => b.Status == BookingStatus.Confirmed).Sum(b => b.BookingFee);
            int dStandbyCount = aManager.Flights.Sum(f => f.StandbyQueue?.Count() ?? 0);

            // Summary table
            Console.WriteLine("+-------------------------------------------------+");
            Console.WriteLine("|            SYSTEM-WIDE SUMMARY                   |");
            Console.WriteLine("+-------------------------------------------------+");
            Console.WriteLine($"| {"Metric",-30} | {"Value",-16} |");
            Console.WriteLine("+-------------------------------------------------+");
            Console.WriteLine($"| {"Total Airlines",-30} | {aManager.Airlines.Count,-16} |");
            Console.WriteLine($"| {"Total Flights",-30} | {dTotalFlights,-16} |");
            Console.WriteLine("+-------------------------------------------------+");
            Console.WriteLine($"| {"Total Bookings",-30} | {dTotalBookings,-16} |");
            Console.WriteLine($"| {"Confirmed Bookings",-30} | {dConfirmedBookings,-16} |");
            Console.WriteLine($"| {"Total Revenue (VND)",-30} | {dTotalRevenue,16:N0} |");
            Console.WriteLine("+-------------------------------------------------+");
            Console.WriteLine($"| {"Total Standby Passengers",-30} | {dStandbyCount,-16} |");
            Console.WriteLine("+-------------------------------------------------+\n");

            // Per-Airline breakdown
            Console.WriteLine("Per-Airline Breakdown:\n");
            Console.WriteLine($"{"Airline",-20} {"Flights",-10} {"Bookings",-10} {"Confirmed",-10} {"Revenue",-15} {"Standby",-8}");
            Console.WriteLine(new string('-', 80));

            foreach (Airline a in aManager.Airlines)
            {
                var aFlightIds = aManager.Flights.Where(f => f.AirlineId == a.Id).Select(f => f.Id).ToList();
                int aBookings = aManager.Bookings.Count(b => aFlightIds.Contains(b.FlightId));
                int aConfirmed = aManager.Bookings.Count(b => aFlightIds.Contains(b.FlightId) && b.Status == BookingStatus.Confirmed);
                decimal aRevenue = aManager.Bookings.Where(b => aFlightIds.Contains(b.FlightId) && b.Status == BookingStatus.Confirmed).Sum(b => b.BookingFee);
                int aStandby = aManager.Flights.Where(f => f.AirlineId == a.Id).Sum(f => f.StandbyQueue?.Count() ?? 0);

                Console.WriteLine($"  {a.AirlineName,-18} {aFlightIds.Count,-10} {aBookings,-10} " +
                                  $"{aConfirmed,-10} {aRevenue,15:N0} {aStandby,8}");
            }
            Console.WriteLine();

            #endregion

            // ================================================================
            // END OF PART D INTEGRATION
            // ================================================================

            Console.WriteLine("\n");
            Console.WriteLine("#######################################################");
            Console.WriteLine("###                                               ###");
            Console.WriteLine("###   PART D - INTEGRATION CHALLENGE COMPLETE      ###");
            Console.WriteLine("###   All components from Parts A-C demonstrated  ###");
            Console.WriteLine("###                                               ###");
            Console.WriteLine("#######################################################");
            Console.WriteLine();
        }
    }
}
