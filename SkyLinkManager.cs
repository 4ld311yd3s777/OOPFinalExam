using System;
using System.Collections.Generic;

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
        /// The standby queue belonging to each flight, keyed by Flight ID.
        /// </summary>
        private Dictionary<int, StandbyQueue> myoStandbyQueues;

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
        /// The number of price comparisons made by the most recent bubble sort.
        /// </summary>
        private long mylBubbleSortComparisons;

        /// <summary>
        /// The number of departure-time comparisons made by the most recent merge sort.
        /// </summary>
        private long mylMergeSortComparisons;

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
        /// Gets the number of comparisons made by the most recent bubble sort.
        /// </summary>
        public long BubbleSortComparisonCount
        {
            get { return mylBubbleSortComparisons; }
        }

        /// <summary>
        /// Gets the number of comparisons made by the most recent merge sort.
        /// </summary>
        public long MergeSortComparisonCount
        {
            get { return mylMergeSortComparisons; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkyLinkManager"/> class.
        /// </summary>
        public SkyLinkManager()
        {
            myoAirlines = new List<Airline>();
            myoFlights = new List<Flight>();
            myoBookings = new List<Booking>();
            myoStandbyQueues = new Dictionary<int, StandbyQueue>();
            myiNextAirlineId = 1;
            myiNextFlightId = 1;
            myiNextBookingId = 1;
            mylBubbleSortComparisons = 0;
            mylMergeSortComparisons = 0;
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
            myoStandbyQueues.Add(aNewFlight.Id, new StandbyQueue());
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
            myoStandbyQueues.Add(aNewFlight.Id, new StandbyQueue());
            return aNewFlight;
        }

        /// <summary>
        /// Adds a regular-priority booking or places the passenger on standby when the flight is full.
        /// </summary>
        /// <param name="theiFlightId">The ID of the flight.</param>
        /// <param name="thesPassengerName">The name of the passenger.</param>
        /// <param name="thesPassportNumber">The unique passport number of the passenger.</param>
        /// <param name="thesSeatNumber">The seat number.</param>
        /// <param name="theoStatus">The status of the booking.</param>
        /// <returns>The created booking, or null when the passenger is placed on standby.</returns>
        public Booking AddBooking(int theiFlightId, string thesPassengerName, string thesPassportNumber, string thesSeatNumber, BookingStatus theoStatus)
        {
            return AddBooking(
                theiFlightId,
                thesPassengerName,
                thesPassportNumber,
                thesSeatNumber,
                theoStatus,
                3);
        }

        /// <summary>
        /// Adds a booking with a standby priority or queues the passenger when no confirmed seat is available.
        /// </summary>
        /// <param name="theiFlightId">The ID of the flight.</param>
        /// <param name="thesPassengerName">The name of the passenger.</param>
        /// <param name="thesPassportNumber">The unique passport number of the passenger.</param>
        /// <param name="thesSeatNumber">The requested seat number.</param>
        /// <param name="theoStatus">The requested booking status.</param>
        /// <param name="theiPriority">The standby priority where a lower number has higher priority.</param>
        /// <returns>The created booking, or null when the passenger is placed on standby.</returns>
        public Booking AddBooking(
            int theiFlightId,
            string thesPassengerName,
            string thesPassportNumber,
            string thesSeatNumber,
            BookingStatus theoStatus,
            int theiPriority)
        {
            Flight aFlight = FindFlightById(theiFlightId);
            if (aFlight == null)
            {
                throw new ArgumentException($"Flight with ID {theiFlightId} does not exist.");
            }

            if (theiPriority <= 0)
            {
                throw new ArgumentException("Standby priority must be greater than zero.");
            }

            foreach (Booking aBooking in myoBookings)
            {
                if (aBooking.PassportNumber.Equals(thesPassportNumber, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException($"Passport number '{thesPassportNumber}' is already registered on another booking.");
                }
            }

            foreach (KeyValuePair<int, StandbyQueue> aQueueEntry in myoStandbyQueues)
            {
                List<StandbyPassenger> aWaitingPassengers =
                    aQueueEntry.Value.GetPassengersInPriorityOrder();
                foreach (StandbyPassenger aPassenger in aWaitingPassengers)
                {
                    if (aPassenger.PassportNumber.Equals(
                        thesPassportNumber,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        throw new ArgumentException(
                            $"Passport number '{thesPassportNumber}' is already registered on a standby queue.");
                    }
                }
            }

            if (theoStatus == BookingStatus.Confirmed &&
                CountConfirmedBookings(theiFlightId) >= aFlight.TotalSeats)
            {
                StandbyPassenger aStandbyPassenger = new StandbyPassenger(
                    thesPassengerName,
                    thesPassportNumber,
                    theiPriority,
                    DateTime.Now);
                myoStandbyQueues[theiFlightId].Enqueue(aStandbyPassenger);
                return null;
            }

            if (theoStatus != BookingStatus.Cancelled)
            {
                foreach (Booking aBooking in myoBookings)
                {
                    if (aBooking.FlightId == theiFlightId &&
                        aBooking.Status != BookingStatus.Cancelled &&
                        aBooking.SeatNumber.Equals(thesSeatNumber, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new ArgumentException(
                            $"Seat '{thesSeatNumber}' is already taken on Flight {theiFlightId}.");
                    }
                }
            }

            Booking aNewBooking = new Booking(
                myiNextBookingId++,
                theiFlightId,
                thesPassengerName,
                thesPassportNumber,
                thesSeatNumber,
                theoStatus,
                aFlight.PricePerSeat);
            myoBookings.Add(aNewBooking);
            return aNewBooking;
        }

        /// <summary>
        /// Sorts flights in ascending order by price using bubble sort.
        /// </summary>
        /// <param name="theoList">The flight list to sort in place.</param>
        public void BubbleSortByPrice(List<Flight> theoList)
        {
            if (theoList == null)
            {
                throw new ArgumentNullException(nameof(theoList));
            }

            mylBubbleSortComparisons = 0;

            for (int aiLastIndex = theoList.Count - 1; aiLastIndex > 0; aiLastIndex--)
            {
                bool abWasSwapped = false;

                for (int aiIndex = 0; aiIndex < aiLastIndex; aiIndex++)
                {
                    mylBubbleSortComparisons++;
                    if (theoList[aiIndex].PricePerSeat > theoList[aiIndex + 1].PricePerSeat)
                    {
                        Flight aTemporaryFlight = theoList[aiIndex];
                        theoList[aiIndex] = theoList[aiIndex + 1];
                        theoList[aiIndex + 1] = aTemporaryFlight;
                        abWasSwapped = true;
                    }
                }

                if (!abWasSwapped)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Sorts flights in ascending departure-time order using recursive merge sort.
        /// </summary>
        /// <param name="theoList">The flight list to sort in place.</param>
        public void MergeSortByDeparture(List<Flight> theoList)
        {
            if (theoList == null)
            {
                throw new ArgumentNullException(nameof(theoList));
            }

            mylMergeSortComparisons = 0;
            if (theoList.Count < 2)
            {
                return;
            }

            Flight[] aoMergeBuffer = new Flight[theoList.Count];
            MergeSortRange(theoList, aoMergeBuffer, 0, theoList.Count - 1);
        }

        /// <summary>
        /// Finds a flight with an exact departure time in a departure-sorted list.
        /// </summary>
        /// <param name="theoSorted">The flight list sorted by departure time.</param>
        /// <param name="theTarget">The exact departure time to find.</param>
        /// <returns>The matching flight index, or -1 when no match exists.</returns>
        public int BinarySearchByDeparture(List<Flight> theoSorted, DateTime theTarget)
        {
            if (theoSorted == null)
            {
                throw new ArgumentNullException(nameof(theoSorted));
            }

            int aiLeftIndex = 0;
            int aiRightIndex = theoSorted.Count - 1;

            while (aiLeftIndex <= aiRightIndex)
            {
                int aiMiddleIndex = aiLeftIndex + ((aiRightIndex - aiLeftIndex) / 2);
                int aiComparison =
                    theoSorted[aiMiddleIndex].DepartureTime.CompareTo(theTarget);

                if (aiComparison == 0)
                {
                    return aiMiddleIndex;
                }

                if (aiComparison < 0)
                {
                    aiLeftIndex = aiMiddleIndex + 1;
                }
                else
                {
                    aiRightIndex = aiMiddleIndex - 1;
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets the standby queue attached to a flight.
        /// </summary>
        /// <param name="theiFlightId">The flight ID whose queue is required.</param>
        /// <returns>The flight's standby queue.</returns>
        public StandbyQueue GetStandbyQueue(int theiFlightId)
        {
            if (!myoStandbyQueues.ContainsKey(theiFlightId))
            {
                throw new ArgumentException($"Flight with ID {theiFlightId} does not exist.");
            }

            return myoStandbyQueues[theiFlightId];
        }

        /// <summary>
        /// Cancels an active booking so that its confirmed seat becomes available.
        /// </summary>
        /// <param name="theiBookingId">The booking ID to cancel.</param>
        /// <returns>The cancelled booking.</returns>
        public Booking CancelBooking(int theiBookingId)
        {
            Booking aBookingToCancel = FindBookingById(theiBookingId);
            if (aBookingToCancel == null)
            {
                throw new ArgumentException($"Booking with ID {theiBookingId} does not exist.");
            }

            if (aBookingToCancel.Status == BookingStatus.Cancelled)
            {
                throw new InvalidOperationException(
                    $"Booking {theiBookingId} is already cancelled.");
            }

            aBookingToCancel.Status = BookingStatus.Cancelled;
            return aBookingToCancel;
        }

        /// <summary>
        /// Promotes the highest-priority standby passenger after a confirmed seat becomes available.
        /// </summary>
        /// <param name="theiFlightId">The flight ID from which to promote a passenger.</param>
        /// <returns>The confirmed booking created for the promoted passenger.</returns>
        public Booking PromoteFromStandby(int theiFlightId)
        {
            Flight aFlight = FindFlightById(theiFlightId);
            if (aFlight == null)
            {
                throw new ArgumentException($"Flight with ID {theiFlightId} does not exist.");
            }

            StandbyQueue aQueue = myoStandbyQueues[theiFlightId];
            if (aQueue.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Flight {aFlight.FlightCode} has no standby passengers.");
            }

            if (CountConfirmedBookings(theiFlightId) >= aFlight.TotalSeats)
            {
                throw new InvalidOperationException(
                    $"Flight {aFlight.FlightCode} is still full; no standby passenger can be promoted.");
            }

            string asAvailableSeat = FindAvailableSeatNumber(aFlight);
            StandbyPassenger aPassenger = aQueue.Dequeue();

            try
            {
                return AddBooking(
                    theiFlightId,
                    aPassenger.PassengerName,
                    aPassenger.PassportNumber,
                    asAvailableSeat,
                    BookingStatus.Confirmed,
                    aPassenger.Priority);
            }
            catch
            {
                aQueue.Enqueue(aPassenger);
                throw;
            }
        }

        /// <summary>
        /// Recursively sorts one inclusive range of a flight list by departure time.
        /// </summary>
        /// <param name="theoList">The flight list being sorted.</param>
        /// <param name="theoBuffer">The reusable merge buffer.</param>
        /// <param name="theiLeftIndex">The inclusive left index.</param>
        /// <param name="theiRightIndex">The inclusive right index.</param>
        private void MergeSortRange(
            List<Flight> theoList,
            Flight[] theoBuffer,
            int theiLeftIndex,
            int theiRightIndex)
        {
            if (theiLeftIndex >= theiRightIndex)
            {
                return;
            }

            int aiMiddleIndex = theiLeftIndex + ((theiRightIndex - theiLeftIndex) / 2);
            MergeSortRange(theoList, theoBuffer, theiLeftIndex, aiMiddleIndex);
            MergeSortRange(theoList, theoBuffer, aiMiddleIndex + 1, theiRightIndex);
            MergeRanges(
                theoList,
                theoBuffer,
                theiLeftIndex,
                aiMiddleIndex,
                theiRightIndex);
        }

        /// <summary>
        /// Merges two adjacent departure-sorted ranges into one sorted range.
        /// </summary>
        /// <param name="theoList">The flight list being sorted.</param>
        /// <param name="theoBuffer">The reusable merge buffer.</param>
        /// <param name="theiLeftIndex">The first index of the left range.</param>
        /// <param name="theiMiddleIndex">The final index of the left range.</param>
        /// <param name="theiRightIndex">The final index of the right range.</param>
        private void MergeRanges(
            List<Flight> theoList,
            Flight[] theoBuffer,
            int theiLeftIndex,
            int theiMiddleIndex,
            int theiRightIndex)
        {
            int aiLeftCursor = theiLeftIndex;
            int aiRightCursor = theiMiddleIndex + 1;
            int aiBufferCursor = theiLeftIndex;

            while (aiLeftCursor <= theiMiddleIndex && aiRightCursor <= theiRightIndex)
            {
                mylMergeSortComparisons++;
                if (theoList[aiLeftCursor].DepartureTime <=
                    theoList[aiRightCursor].DepartureTime)
                {
                    theoBuffer[aiBufferCursor++] = theoList[aiLeftCursor++];
                }
                else
                {
                    theoBuffer[aiBufferCursor++] = theoList[aiRightCursor++];
                }
            }

            while (aiLeftCursor <= theiMiddleIndex)
            {
                theoBuffer[aiBufferCursor++] = theoList[aiLeftCursor++];
            }

            while (aiRightCursor <= theiRightIndex)
            {
                theoBuffer[aiBufferCursor++] = theoList[aiRightCursor++];
            }

            for (int aiIndex = theiLeftIndex; aiIndex <= theiRightIndex; aiIndex++)
            {
                theoList[aiIndex] = theoBuffer[aiIndex];
            }
        }

        /// <summary>
        /// Counts confirmed bookings belonging to one flight.
        /// </summary>
        /// <param name="theiFlightId">The flight ID to count.</param>
        /// <returns>The number of confirmed bookings.</returns>
        private int CountConfirmedBookings(int theiFlightId)
        {
            int aiConfirmedCount = 0;

            foreach (Booking aBooking in myoBookings)
            {
                if (aBooking.FlightId == theiFlightId &&
                    aBooking.Status == BookingStatus.Confirmed)
                {
                    aiConfirmedCount++;
                }
            }

            return aiConfirmedCount;
        }

        /// <summary>
        /// Finds a free seat number, preferring a seat released by a cancelled booking.
        /// </summary>
        /// <param name="theoFlight">The flight requiring a free seat.</param>
        /// <returns>An available seat number.</returns>
        private string FindAvailableSeatNumber(Flight theoFlight)
        {
            foreach (Booking aBooking in myoBookings)
            {
                if (aBooking.FlightId == theoFlight.Id &&
                    aBooking.Status == BookingStatus.Cancelled &&
                    IsSeatAvailable(theoFlight.Id, aBooking.SeatNumber))
                {
                    return aBooking.SeatNumber;
                }
            }

            for (int aiSeatIndex = 0; aiSeatIndex < theoFlight.TotalSeats; aiSeatIndex++)
            {
                int aiRowNumber = (aiSeatIndex / 6) + 1;
                char acSeatLetter = (char)('A' + (aiSeatIndex % 6));
                string asSeatNumber = $"{aiRowNumber:D2}{acSeatLetter}";

                if (IsSeatAvailable(theoFlight.Id, asSeatNumber))
                {
                    return asSeatNumber;
                }
            }

            throw new InvalidOperationException(
                $"No available seat number could be assigned on Flight {theoFlight.FlightCode}.");
        }

        /// <summary>
        /// Determines whether an active booking already occupies a seat.
        /// </summary>
        /// <param name="theiFlightId">The flight ID.</param>
        /// <param name="thesSeatNumber">The seat number to check.</param>
        /// <returns>True when no active booking occupies the seat.</returns>
        private bool IsSeatAvailable(int theiFlightId, string thesSeatNumber)
        {
            foreach (Booking aBooking in myoBookings)
            {
                if (aBooking.FlightId == theiFlightId &&
                    aBooking.Status != BookingStatus.Cancelled &&
                    aBooking.SeatNumber.Equals(
                        thesSeatNumber,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
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
        /// Helper to find a booking by its ID.
        /// </summary>
        /// <param name="theiId">The booking ID to find.</param>
        /// <returns>The matching booking, or null when no booking has that ID.</returns>
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
    }
}
