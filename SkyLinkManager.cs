using System;
using System.Collections.Generic;
using System.Linq; 

namespace OOPFinalExam
{
    /// <summary>
    /// Manages the Airlines, Flights, Bookings, and business logic of the system.
    /// </summary>
    public class SkyLinkManager
    {
        /// <summary>
        /// The list of registered airlines.
        /// </summary>
        private List<Airline> myoAirlines;

        /// <summary>
        /// The list of registered flights.
        /// </summary>
        private List<Flight> myoFlights;

        /// <summary>
        /// The list of registered bookings.
        /// </summary>
        private List<Booking> myoBookings;

        /// <summary>
        /// The booking objects recorded in last-in, first-out action order.
        /// </summary>
        private SkyStack<Booking> myoBookingHistory;

        /// <summary>
        /// The metadata used to reverse each corresponding booking history entry.
        /// </summary>
        private SkyStack<BookingAction> myoBookingActions;

        /// <summary>
        /// The number of comparisons made by the most recent bubble sort.
        /// </summary>
        private int myiBubbleSortComparisonCount;

        /// <summary>
        /// The number of comparisons made by the most recent merge sort.
        /// </summary>
        private int myiMergeSortComparisonCount;

        /// <summary>
        /// Counter for auto-incrementing Airline IDs.
        /// </summary>
        private int myiNextAirlineId;

        /// <summary>
        /// Counter for auto-incrementing Flight IDs.
        /// </summary>
        private int myiNextFlightId;

        /// <summary>
        /// Counter for auto-incrementing Booking IDs.
        /// </summary>
        private int myiNextBookingId;

        /// <summary>
        /// The number of reversible booking actions currently stored.
        /// </summary>
        public int BookingHistorySize
        {
            get { return myoBookingHistory.Size(); }
        }

        /// <summary>
        /// Gets the number of price comparisons made by the most recent bubble sort.
        /// </summary>
        public int BubbleSortComparisonCount
        {
            get { return myiBubbleSortComparisonCount; }
        }

        /// <summary>
        /// Gets the number of departure-time comparisons made by the most recent merge sort.
        /// </summary>
        public int MergeSortComparisonCount
        {
            get { return myiMergeSortComparisonCount; }
        }

        /// <summary>
        /// Gets the list of airlines.
        /// </summary>
        public List<Airline> Airlines
        {
            get { return myoAirlines; }
        }

        /// <summary>
        /// Gets the list of flights.
        /// </summary>
        public List<Flight> Flights
        {
            get { return myoFlights; }
        }

        /// <summary>
        /// Gets the list of bookings.
        /// </summary>
        public List<Booking> Bookings
        {
            get { return myoBookings; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkyLinkManager"/> class.
        /// </summary>
        public SkyLinkManager()
        {
            myoAirlines = new List<Airline>();
            myoFlights = new List<Flight>();
            myoBookings = new List<Booking>();
            myoBookingHistory = new SkyStack<Booking>();
            myoBookingActions = new SkyStack<BookingAction>();
            myiNextAirlineId = 1;
            myiNextFlightId = 1;
            myiNextBookingId = 1;
            myiBubbleSortComparisonCount = 0;
            myiMergeSortComparisonCount = 0;
        }

        /// <summary>
        /// Adds a new airline to the system with automatic ID assignment and uniqueness validation.
        /// </summary>
        /// <param name="thesAirlineName">The name of the airline.</param>
        /// <param name="thesIATACode">The unique 2-letter IATA code of the airline.</param>
        /// <param name="thesCountry">The country of registration.</param>
        /// <param name="theiFoundedYear">The year it was founded.</param>
        /// <returns>The created <see cref="Airline"/> instance.</returns>
        public Airline AddAirline(string thesAirlineName, string thesIATACode, string thesCountry, int theiFoundedYear)
        {
            if (string.IsNullOrWhiteSpace(thesAirlineName))
            {
                throw new ArgumentException("Airline name cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(thesIATACode) || thesIATACode.Length != 2)
            {
                throw new ArgumentException("IATA code must be exactly 2 characters.");
            }

            // Enforce uniqueness: AirlineName and IATACode must each be unique
            foreach (Airline aAirline in myoAirlines)
            {
                if (aAirline.AirlineName.Equals(thesAirlineName, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException($"Airline name '{thesAirlineName}' is already registered.");
                }
                if (aAirline.IATACode.Equals(thesIATACode, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException($"IATA Code '{thesIATACode}' is already registered.");
                }
            }

            Airline aNewAirline = new Airline(myiNextAirlineId++, thesAirlineName, thesIATACode.ToUpper(), thesCountry, theiFoundedYear);
            myoAirlines.Add(aNewAirline);
            return aNewAirline;
        }

        /// <summary>
        /// Adds a new flight to the system, validating the airline exists and generating a unique FlightCode.
        /// </summary>
        /// <param name="theiAirlineId">The ID of the airline operating the flight.</param>
        /// <param name="thesOrigin">The origin airport code.</param>
        /// <param name="thesDestination">The destination airport code.</param>
        /// <param name="thedtDepartureTime">The departure time.</param>
        /// <param name="theiDurationMinutes">The duration of the flight in minutes.</param>
        /// <param name="theiTotalSeats">The total seats available.</param>
        /// <param name="theddPricePerSeat">The price per seat.</param>
        /// <returns>The created <see cref="Flight"/> instance.</returns>
        public Flight AddFlight(int theiAirlineId, string thesOrigin, string thesDestination, DateTime thedtDepartureTime, int theiDurationMinutes, int theiTotalSeats, decimal theddPricePerSeat)
        {
            // Validate AirlineId exists
            Airline aAirline = FindAirlineById(theiAirlineId);
            if (aAirline == null)
            {
                throw new ArgumentException($"Airline with ID {theiAirlineId} does not exist.");
            }

            string asFlightCode = GenerateFlightCode(aAirline.IATACode, thesOrigin, myiNextFlightId);

            Flight aNewFlight = new Flight(myiNextFlightId++, theiAirlineId, asFlightCode, thesOrigin, thesDestination, thedtDepartureTime, theiDurationMinutes, theiTotalSeats, theddPricePerSeat);
            myoFlights.Add(aNewFlight);
            return aNewFlight;
        }

        /// <summary>
        /// Adds a new business flight to the system.
        /// </summary>
        /// <param name="theiAirlineId">The ID of the airline operating the flight.</param>
        /// <param name="thesOrigin">The origin airport code.</param>
        /// <param name="thesDestination">The destination airport code.</param>
        /// <param name="thedtDepartureTime">The departure time.</param>
        /// <param name="theiDurationMinutes">The duration of the flight in minutes.</param>
        /// <param name="theiTotalSeats">The total seats available.</param>
        /// <param name="theddPricePerSeat">The base price per seat.</param>
        /// <param name="thebLoungeAccess">True if lounge access is included.</param>
        /// <param name="thebMealIncluded">True if a meal is included.</param>
        /// <param name="theddPremiumSurcharge">The premium surcharge.</param>
        /// <returns>The created <see cref="BusinessFlight"/> instance.</returns>
        public BusinessFlight AddBusinessFlight(int theiAirlineId, string thesOrigin, string thesDestination, DateTime thedtDepartureTime, int theiDurationMinutes, int theiTotalSeats, decimal theddPricePerSeat, bool thebLoungeAccess, bool thebMealIncluded, decimal theddPremiumSurcharge)
        {
            // Validate AirlineId exists
            Airline aAirline = FindAirlineById(theiAirlineId);
            if (aAirline == null)
            {
                throw new ArgumentException($"Airline with ID {theiAirlineId} does not exist.");
            }

            string asFlightCode = GenerateFlightCode(aAirline.IATACode, thesOrigin, myiNextFlightId);

            BusinessFlight aNewFlight = new BusinessFlight(myiNextFlightId++, theiAirlineId, asFlightCode, thesOrigin, thesDestination, thedtDepartureTime, theiDurationMinutes, theiTotalSeats, theddPricePerSeat, thebLoungeAccess, thebMealIncluded, theddPremiumSurcharge);
            myoFlights.Add(aNewFlight);
            return aNewFlight;
        }

        /// <summary>
        /// Adds a booking, performing validations for flight existence, passport uniqueness, seat uniqueness, and capacity.
        /// </summary>
        /// <param name="theiFlightId">The ID of the flight.</param>
        /// <param name="thesPassengerName">The name of the passenger.</param>
        /// <param name="thesPassportNumber">The unique passport number of the passenger.</param>
        /// <param name="thesSeatNumber">The seat number.</param>
        /// <param name="theoStatus">The status of the booking.</param>
        /// <param name="theiPriority">The priority of the passenger if added to standby (default is 3).</param>
        /// <returns>The created <see cref="Booking"/> instance, or null if passenger is added to standby.</returns>
        public Booking AddBooking(int theiFlightId, string thesPassengerName, string thesPassportNumber, string thesSeatNumber, BookingStatus theoStatus, int theiPriority = 3)
        {
            // Validate FlightId exists
            Flight aFlight = FindFlightById(theiFlightId);
            if (aFlight == null)
            {
                throw new ArgumentException($"Flight with ID {theiFlightId} does not exist.");
            }

            // Enforce uniqueness: PassportNumber must be globally unique
            foreach (Booking aBooking in myoBookings)
            {
                if (aBooking.PassportNumber.Equals(thesPassportNumber, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException($"Passport number '{thesPassportNumber}' is already registered on another booking.");
                }
            }

            // Enforce uniqueness: SeatNumber must be unique per flight
            foreach (Booking aBooking in myoBookings)
            {
                if (aBooking.FlightId == theiFlightId && aBooking.SeatNumber.Equals(thesSeatNumber, StringComparison.OrdinalIgnoreCase) && aBooking.Status != BookingStatus.Cancelled)
                {
                    throw new ArgumentException($"Seat '{thesSeatNumber}' is already taken on Flight {theiFlightId}.");
                }
            }

            // Enforce capacity: check confirmed bookings limit
            if (theoStatus == BookingStatus.Confirmed)
            {
                int aiConfirmedCount = 0;
                foreach (Booking aBooking in myoBookings)
                {
                    if (aBooking.FlightId == theiFlightId && aBooking.Status == BookingStatus.Confirmed)
                    {
                        aiConfirmedCount++;
                    }
                }

                if (aiConfirmedCount >= aFlight.TotalSeats)
                {
                    aFlight.StandbyQueue.Enqueue(new StandbyPassenger(thesPassengerName, thesPassportNumber, theiPriority, DateTime.Now));
                    Console.WriteLine($"Flight {aFlight.FlightCode} is full. Passenger '{thesPassengerName}' added to standby queue with priority {theiPriority}.");
                    return null;
                }
            }

            Booking aNewBooking = new Booking(myiNextBookingId++, theiFlightId, thesPassengerName, thesPassportNumber, thesSeatNumber, theoStatus, aFlight.PricePerSeat);
            myoBookings.Add(aNewBooking);
            myoBookingHistory.Push(aNewBooking);
            myoBookingActions.Push(new BookingAction(BookingActionType.Added, theoStatus));
            return aNewBooking;
        }

        /// <summary>
        /// Cancels an existing booking and records its previous status for undo.
        /// </summary>
        /// <param name="theiBookingId">The ID of the booking to cancel.</param>
        /// <returns>The cancelled booking.</returns>
        /// <exception cref="ArgumentException">Thrown when the booking does not exist.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the booking is already cancelled.</exception>
        public Booking CancelBooking(int theiBookingId)
        {
            Booking aBooking = FindBookingById(theiBookingId);
            if (aBooking == null)
            {
                throw new ArgumentException($"Booking with ID {theiBookingId} does not exist.");
            }

            if (aBooking.Status == BookingStatus.Cancelled)
            {
                throw new InvalidOperationException($"Booking with ID {theiBookingId} is already cancelled.");
            }

            BookingStatus aoPreviousStatus = aBooking.Status;
            aBooking.Status = BookingStatus.Cancelled;
            myoBookingHistory.Push(aBooking);
            myoBookingActions.Push(new BookingAction(BookingActionType.Cancelled, aoPreviousStatus));
            return aBooking;
        }

        /// <summary>
        /// Reverses the most recent booking addition or cancellation.
        /// </summary>
        /// <returns>The booking affected by the undo operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when there is no booking action to undo.</exception>
        public Booking UndoLastBookingAction()
        {
            if (myoBookingHistory.IsEmpty())
            {
                throw new InvalidOperationException("There is no booking action to undo.");
            }

            Booking aBooking = myoBookingHistory.Pop();
            BookingAction aAction = myoBookingActions.Pop();

            if (aAction.ActionType == BookingActionType.Added)
            {
                if (!myoBookings.Remove(aBooking))
                {
                    throw new InvalidOperationException($"Booking with ID {aBooking.Id} can no longer be removed.");
                }
            }
            else
            {
                aBooking.Status = aAction.PreviousStatus;
            }

            return aBooking;
        }

        /// <summary>
        /// Promotes the top standby passenger for the given flight to confirmed status if a seat is available.
        /// </summary>
        /// <param name="theiFlightId">The flight ID.</param>
        public void PromoteFromStandby(int theiFlightId)
        {
            Flight aFlight = FindFlightById(theiFlightId);
            if (aFlight == null)
            {
                throw new ArgumentException($"Flight with ID {theiFlightId} does not exist.");
            }

            if (aFlight.StandbyQueue.Count() == 0)
            {
                Console.WriteLine($"No passengers in standby queue for flight {aFlight.FlightCode} to promote.");
                return;
            }

            int aiConfirmedCount = 0;
            foreach (Booking aBooking in myoBookings)
            {
                if (aBooking.FlightId == theiFlightId && aBooking.Status == BookingStatus.Confirmed)
                {
                    aiConfirmedCount++;
                }
            }

            if (aiConfirmedCount < aFlight.TotalSeats)
            {
                string asAssignedSeat = "SBY-01";
                foreach (Booking aBooking in myoBookings)
                {
                    if (aBooking.FlightId == theiFlightId && aBooking.Status == BookingStatus.Cancelled)
                    {
                        asAssignedSeat = aBooking.SeatNumber;
                        break;
                    }
                }

                StandbyPassenger aTopPassenger = aFlight.StandbyQueue.Dequeue();
                Console.WriteLine($"Promoting {aTopPassenger.PassengerName} from standby to confirmed for flight {aFlight.FlightCode} with seat {asAssignedSeat}.");

                AddBooking(theiFlightId, aTopPassenger.PassengerName, aTopPassenger.PassportNumber, asAssignedSeat, BookingStatus.Confirmed, aTopPassenger.Priority);
            }
            else
            {
                Console.WriteLine($"Flight {aFlight.FlightCode} is still full, cannot promote from standby.");
            }
        }

        /// <summary>
        /// Generates a flight code based on airline code, origin, and flight ID, handling collisions by appending numeric suffixes.
        /// </summary>
        /// <param name="theIATACode">The airline's IATA code.</param>
        /// <param name="theOrigin">The origin airport code.</param>
        /// <param name="theFlightId">The flight ID.</param>
        /// <returns>A unique flight code string.</returns>
        public string GenerateFlightCode(string theIATACode, string theOrigin, int theFlightId)
        {
            string asBaseCode = $"{theIATACode}{theOrigin}{theFlightId:D3}".ToUpper();
            string asGeneratedCode = asBaseCode;
            int aiSuffix = 1;

            while (FlightCodeExists(asGeneratedCode))
            {
                asGeneratedCode = asBaseCode + aiSuffix;
                aiSuffix++;
            }

            return asGeneratedCode;
        }

        /// <summary>
        /// Checks if a flight code is already used by any flight in the system.
        /// </summary>
        /// <param name="thesFlightCode">The flight code to check.</param>
        /// <returns>True if it exists; otherwise, false.</returns>
        private bool FlightCodeExists(string thesFlightCode)
        {
            foreach (Flight aFlight in myoFlights)
            {
                if (aFlight.FlightCode.Equals(thesFlightCode, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Helper to find an Airline by its ID.
        /// </summary>
        private Airline FindAirlineById(int theiId)
        {
            foreach (Airline aAirline in myoAirlines)
            {
                if (aAirline.Id == theiId)
                {
                    return aAirline;
                }
            }
            return null;
        }

        /// <summary>
        /// Helper to find a Flight by its ID.
        /// </summary>
        private Flight FindFlightById(int theiId)
        {
            foreach (Flight aFlight in myoFlights)
            {
                if (aFlight.Id == theiId)
                {
                    return aFlight;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds a booking by its ID.
        /// </summary>
        /// <param name="theiId">The booking ID to find.</param>
        /// <returns>The matching booking, or <see langword="null"/> when it does not exist.</returns>
        private Booking FindBookingById(int theiId)
        {
            foreach (Booking aBooking in myoBookings)
            {
                if (aBooking.Id == theiId)
                {
                    return aBooking;
                }
            }

            return null;
        }

        /// <summary>
        /// Sorts flights in place by price per seat in ascending order using bubble sort.
        /// </summary>
        /// <param name="theoList">The flight list to sort.</param>
        /// <exception cref="ArgumentNullException">Thrown when the flight list is null.</exception>
        public void BubbleSortByPrice(List<Flight> theoList)
        {
            if (theoList == null)
            {
                throw new ArgumentNullException(nameof(theoList));
            }

            myiBubbleSortComparisonCount = 0;

            for (int aiEnd = theoList.Count - 1; aiEnd > 0; aiEnd--)
            {
                bool abSwapped = false;

                for (int aiIndex = 0; aiIndex < aiEnd; aiIndex++)
                {
                    myiBubbleSortComparisonCount++;
                    if (theoList[aiIndex].PricePerSeat > theoList[aiIndex + 1].PricePerSeat)
                    {
                        Flight aFlight = theoList[aiIndex];
                        theoList[aiIndex] = theoList[aiIndex + 1];
                        theoList[aiIndex + 1] = aFlight;
                        abSwapped = true;
                    }
                }

                if (!abSwapped)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Sorts flights in place by departure time in ascending order using recursive merge sort.
        /// </summary>
        /// <param name="theoList">The flight list to sort.</param>
        /// <exception cref="ArgumentNullException">Thrown when the flight list is null.</exception>
        public void MergeSortByDeparture(List<Flight> theoList)
        {
            if (theoList == null)
            {
                throw new ArgumentNullException(nameof(theoList));
            }

            myiMergeSortComparisonCount = 0;
            if (theoList.Count < 2)
            {
                return;
            }

            Flight[] aoTemporary = new Flight[theoList.Count];
            MergeSortByDeparture(theoList, aoTemporary, 0, theoList.Count - 1);
        }

        /// <summary>
        /// Recursively divides a flight list range and merges the sorted halves.
        /// </summary>
        /// <param name="theoList">The flight list being sorted.</param>
        /// <param name="theoTemporary">The reusable merge buffer.</param>
        /// <param name="theiLeft">The inclusive left index.</param>
        /// <param name="theiRight">The inclusive right index.</param>
        private void MergeSortByDeparture(List<Flight> theoList, Flight[] theoTemporary, int theiLeft, int theiRight)
        {
            if (theiLeft >= theiRight)
            {
                return;
            }

            int aiMiddle = theiLeft + (theiRight - theiLeft) / 2;
            MergeSortByDeparture(theoList, theoTemporary, theiLeft, aiMiddle);
            MergeSortByDeparture(theoList, theoTemporary, aiMiddle + 1, theiRight);
            MergeByDeparture(theoList, theoTemporary, theiLeft, aiMiddle, theiRight);
        }

        /// <summary>
        /// Merges two adjacent departure-time-sorted ranges into one sorted range.
        /// </summary>
        /// <param name="theoList">The flight list being sorted.</param>
        /// <param name="theoTemporary">The reusable merge buffer.</param>
        /// <param name="theiLeft">The inclusive left index.</param>
        /// <param name="theiMiddle">The final index of the left range.</param>
        /// <param name="theiRight">The inclusive right index.</param>
        private void MergeByDeparture(List<Flight> theoList, Flight[] theoTemporary, int theiLeft, int theiMiddle, int theiRight)
        {
            int aiLeftIndex = theiLeft;
            int aiRightIndex = theiMiddle + 1;
            int aiMergeIndex = theiLeft;

            while (aiLeftIndex <= theiMiddle && aiRightIndex <= theiRight)
            {
                myiMergeSortComparisonCount++;
                if (theoList[aiLeftIndex].DepartureTime <= theoList[aiRightIndex].DepartureTime)
                {
                    theoTemporary[aiMergeIndex] = theoList[aiLeftIndex];
                    aiLeftIndex++;
                }
                else
                {
                    theoTemporary[aiMergeIndex] = theoList[aiRightIndex];
                    aiRightIndex++;
                }

                aiMergeIndex++;
            }

            while (aiLeftIndex <= theiMiddle)
            {
                theoTemporary[aiMergeIndex] = theoList[aiLeftIndex];
                aiLeftIndex++;
                aiMergeIndex++;
            }

            while (aiRightIndex <= theiRight)
            {
                theoTemporary[aiMergeIndex] = theoList[aiRightIndex];
                aiRightIndex++;
                aiMergeIndex++;
            }

            for (int aiIndex = theiLeft; aiIndex <= theiRight; aiIndex++)
            {
                theoList[aiIndex] = theoTemporary[aiIndex];
            }
        }

        /// <summary>
        /// Finds a flight with an exact departure time in a departure-sorted list.
        /// </summary>
        /// <param name="theoSorted">The flight list sorted by departure time ascending.</param>
        /// <param name="thedtTarget">The exact departure time to find.</param>
        /// <returns>The index of a matching flight, or -1 when no match exists.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the flight list is null.</exception>
        public int BinarySearchByDeparture(List<Flight> theoSorted, DateTime thedtTarget)
        {
            if (theoSorted == null)
            {
                throw new ArgumentNullException(nameof(theoSorted));
            }

            int aiLeft = 0;
            int aiRight = theoSorted.Count - 1;

            while (aiLeft <= aiRight)
            {
                int aiMiddle = aiLeft + (aiRight - aiLeft) / 2;
                int aiComparison = theoSorted[aiMiddle].DepartureTime.CompareTo(thedtTarget);

                if (aiComparison == 0)
                {
                    return aiMiddle;
                }

                if (aiComparison < 0)
                {
                    aiLeft = aiMiddle + 1;
                }
                else
                {
                    aiRight = aiMiddle - 1;
                }
            }

            return -1;
        }

        // =====================================================================
        // Q5. COMPLEX LINQ QUERIES (q -> u)
        // =====================================================================

        // q) Returns all confirmed bookings for a given flight ID, sorted by seat number ascending.
        public List<object> ConfirmedBookings (int theiFlightId)
        {
            var confirmedBookings = from b in Bookings
                                   
                                    where b.FlightId == theiFlightId && b.Status == BookingStatus.Confirmed 
                                    orderby b.SeatNumber ascending
                                    select new
                                    {
                                        b.PassengerName,
                                        b.PassportNumber,
                                        b.SeatNumber,
                                        b.BookingFee,
                                    };
            return confirmedBookings.Cast<object>().ToList(); 
        }
        // r) Returns the top 5 most expensive flights that still have available seats, sorted by price descending.
        public List<object> GetTop5ExpensiveAvailableFlights()
        {
            var result = (from f in myoFlights
                          join a in myoAirlines on f.AirlineId equals a.Id
                          join b in myoBookings on f.Id equals b.FlightId into flightBookings
                          let confirmedCount = flightBookings.Count(b => b.Status == BookingStatus.Confirmed)
                          let availableSeats = f.TotalSeats - confirmedCount
                          where availableSeats > 0
                          orderby f.PricePerSeat descending
                          select new
                          {
                              AirlineName = a.AirlineName,
                              f.FlightCode,
                              f.Origin,
                              f.Destination,
                              f.PricePerSeat,
                              AvailableSeats = availableSeats
                          })
                          .Take(5);

            return result.Cast<object>().ToList();
        }
       
        // s) Computes total flights, total bookings, confirmed bookings, and total revenue per airline, sorted by revenue descending.
        public List<object> GetAirlineFinancialStatistics()
        {
            var result = from a in myoAirlines
                         join f in myoFlights on a.Id equals f.AirlineId into airlineFlights
                         let flightIds = airlineFlights.Select(x => x.Id)
                         join b in myoBookings on 1 equals 1 into allBookings
                         let airlineBookings = allBookings.Where(x => flightIds.Contains(x.FlightId))
                         let confirmedBookings = airlineBookings.Where(x => x.Status == BookingStatus.Confirmed)
                         let totalRevenue = confirmedBookings.Sum(x => x.BookingFee)
                         select new
                         {
                             AirlineName = a.AirlineName,
                             TotalFlights = airlineFlights.Count(),
                             TotalBookings = airlineBookings.Count(),
                             ConfirmedBookings = confirmedBookings.Count(),
                             TotalRevenue = totalRevenue
                         } into stats
                         orderby stats.TotalRevenue descending
                         select stats;

            return result.Cast<object>().ToList();
        }

        // t) Finds all frequent passengers who have booked more than one flight across all airlines.
        public List<object> GetFrequentPassengers()
        {
            var result = from b in myoBookings
                         group b by new { b.PassportNumber, b.PassengerName } into g
                         where g.Count() > 1
                         select new
                         {
                             PassengerName = g.Key.PassengerName,
                             PassportNumber = g.Key.PassportNumber,
                             FlightCount = g.Count()
                         };

            return result.Cast<object>().ToList();
        }

        // u) Returns the flight schedule for a specific route (origin and destination pair), sorted by departure time ascending.
        public List<object> GetFlightSchedule(string thesOrigin, string thesDestination)
        {
            var result = from f in myoFlights
                         where f.Origin.Equals(thesOrigin, StringComparison.OrdinalIgnoreCase)
                               && f.Destination.Equals(thesDestination, StringComparison.OrdinalIgnoreCase)
                         join b in myoBookings on f.Id equals b.FlightId into flightBookings
                         let confirmedCount = flightBookings.Count(x => x.Status == BookingStatus.Confirmed)
                         let availableSeats = f.TotalSeats - confirmedCount
                         orderby f.DepartureTime ascending
                         select new
                         {
                             f.FlightCode,
                             f.DepartureTime,
                             f.DurationMinutes,
                             f.PricePerSeat,
                             AvailableSeats = availableSeats
                         };

            return result.Cast<object>().ToList();
        }

        // =====================================================================
        // Q6. ADVANCED AGGREGATION & PROJECTION (v -> y)
        // =====================================================================

        // v) Finds the busiest day of the week by evaluating the total number of departures.
        public object FindBusiestDayOfWeek()
        {
            var busiestDay = (from f in myoFlights
                              group f by f.DepartureTime.DayOfWeek into g
                              orderby g.Count() descending
                              select new
                              {
                                  DayName = g.Key.ToString(),
                                  Count = g.Count()
                              })
                              .FirstOrDefault();

            return busiestDay;
        }

        // w) Calculates the average load factor percentage rounded to 2 decimal places for active airlines.
        public List<object> GetAverageLoadFactorPerAirline()
        {
            var result = from a in myoAirlines
                         join f in myoFlights on a.Id equals f.AirlineId into airlineFlights
                         where airlineFlights.Any()
                         select new
                         {
                             AirlineName = a.AirlineName,
                             AvgLoadFactor = Math.Round(
                                 airlineFlights.Average(f => {
                                     int confirmed = myoBookings.Count(b => b.FlightId == f.Id && b.Status == BookingStatus.Confirmed);
                                     return ((double)confirmed / f.TotalSeats) * 100;
                                 }), 2)
                         };

            return result.Cast<object>().ToList();
        }

        // x) Groups all flights by route and returns flight counts along with min, max, and average seat pricing.
        public List<object> GetRouteStatistics()
        {
            var result = from f in myoFlights
                         group f by new { f.Origin, f.Destination } into g
                         select new
                         {
                             Route = $"{g.Key.Origin}-{g.Key.Destination}".ToUpper(),
                             FlightCount = g.Count(),
                             CheapestPrice = g.Min(x => x.PricePerSeat),
                             MostExpensivePrice = g.Max(x => x.PricePerSeat),
                             AveragePrice = g.Average(x => x.PricePerSeat)
                         } into routeStats
                         orderby routeStats.FlightCount descending
                         select routeStats;

            return result.Cast<object>().ToList();
        }

        // y) Identifies passengers who have completely cancelled every single booking they have ever placed.
        public List<object> GetPassengersWithAllCancelledBookings()
        {
            var result = from b in myoBookings
                         group b by new { b.PassportNumber, b.PassengerName } into g
                         where g.All(x => x.Status == BookingStatus.Cancelled)
                         select new
                         {
                             PassengerName = g.Key.PassengerName,
                             PassportNumber = g.Key.PassportNumber,
                             CancelledCount = g.Count()
                         };

            return result.Cast<object>().ToList();
        }
    }
}
