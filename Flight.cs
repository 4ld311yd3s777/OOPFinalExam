using System;

namespace OOPFinalExam
{
    /// <summary>
    /// Represents a flight entity in the system.
    /// </summary>
    public class Flight : SkyEntity
    {
        /// <summary>
        /// The unique ID of the airline this flight belongs to.
        /// </summary>
        private int myiAirlineId;

        /// <summary>
        /// The auto-generated flight code.
        /// </summary>
        private string mysFlightCode;

        /// <summary>
        /// The origin airport code (3 uppercase letters).
        /// </summary>
        private string mysOrigin;

        /// <summary>
        /// The destination airport code (3 uppercase letters).
        /// </summary>
        private string mysDestination;

        /// <summary>
        /// The departure time of the flight.
        /// </summary>
        private DateTime mydtDepartureTime;

        /// <summary>
        /// The duration of the flight in minutes.
        /// </summary>
        private int myiDurationMinutes;

        /// <summary>
        /// The total number of seats.
        /// </summary>
        private int myiTotalSeats;

        /// <summary>
        /// The price per seat.
        /// </summary>
        private decimal myddPricePerSeat;

        /// <summary>
        /// The flight's standby queue.
        /// </summary>
        private StandbyQueue myoStandbyQueue;

        /// <summary>
        /// Gets the flight's standby queue.
        /// </summary>
        public StandbyQueue StandbyQueue
        {
            get { return myoStandbyQueue; }
        }

        /// <summary>
        /// Gets the airline ID (read-only).
        /// </summary>
        public int AirlineId
        {
            get { return myiAirlineId; }
        }

        /// <summary>
        /// Gets the flight code (read-only).
        /// </summary>
        public string FlightCode
        {
            get { return mysFlightCode; }
        }

        /// <summary>
        /// Gets or sets the origin airport.
        /// </summary>
        public string Origin
        {
            get { return mysOrigin; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length != 3)
                {
                    throw new ArgumentException("Origin must be exactly 3 characters.");
                }
                string aUpper = value.ToUpper();
                if (aUpper == (mysDestination ?? "").ToUpper())
                {
                    throw new ArgumentException("Origin cannot be equal to Destination.");
                }
                mysOrigin = aUpper;
            }
        }

        /// <summary>
        /// Gets or sets the destination airport.
        /// </summary>
        public string Destination
        {
            get { return mysDestination; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length != 3)
                {
                    throw new ArgumentException("Destination must be exactly 3 characters.");
                }
                string aUpper = value.ToUpper();
                if (aUpper == (mysOrigin ?? "").ToUpper())
                {
                    throw new ArgumentException("Destination cannot be equal to Origin.");
                }
                mysDestination = aUpper;
            }
        }

        /// <summary>
        /// Gets or sets the departure time. Must be in the future.
        /// </summary>
        public DateTime DepartureTime
        {
            get { return mydtDepartureTime; }
            set
            {
                if (value <= DateTime.Now)
                {
                    throw new ArgumentException("Departure time must be in the future.");
                }
                mydtDepartureTime = value;
            }
        }

        /// <summary>
        /// Gets or sets the duration of the flight in minutes. Must be > 0.
        /// </summary>
        public int DurationMinutes
        {
            get { return myiDurationMinutes; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Duration minutes must be greater than zero.");
                }
                myiDurationMinutes = value;
            }
        }

        /// <summary>
        /// Gets or sets the total seats on the flight. Must be > 0.
        /// </summary>
        public int TotalSeats
        {
            get { return myiTotalSeats; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Total seats must be greater than zero.");
                }
                myiTotalSeats = value;
            }
        }

        /// <summary>
        /// Gets or sets the price per seat. Must be > 0.
        /// </summary>
        public decimal PricePerSeat
        {
            get { return myddPricePerSeat; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Price per seat must be greater than zero.");
                }
                myddPricePerSeat = value;
            }
        }

        /// <summary>
        /// Gets the type of this entity.
        /// </summary>
        public override string EntityType
        {
            get { return "Flight"; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Flight"/> class.
        /// </summary>
        /// <param name="theiId">The ID of the flight.</param>
        /// <param name="theiAirlineId">The ID of the airline.</param>
        /// <param name="thesFlightCode">The generated flight code.</param>
        /// <param name="thesOrigin">The origin airport.</param>
        /// <param name="thesDestination">The destination airport.</param>
        /// <param name="thedtDepartureTime">The departure date and time.</param>
        /// <param name="theiDurationMinutes">The duration of the flight in minutes.</param>
        /// <param name="theiTotalSeats">The total seats available.</param>
        /// <param name="theddPricePerSeat">The price per seat.</param>
        public Flight(int theiId, int theiAirlineId, string thesFlightCode, string thesOrigin, string thesDestination, DateTime thedtDepartureTime, int theiDurationMinutes, int theiTotalSeats, decimal theddPricePerSeat)
            : base(theiId)
        {
            myiAirlineId = theiAirlineId;
            myoStandbyQueue = new StandbyQueue();
            mysFlightCode = thesFlightCode;

            // Set fields directly or through properties to trigger validation.
            // Since we need to validate, let's use the properties.
            // Note: Destination and Origin check each other, so set destination/origin carefully.
            mysDestination = thesDestination.ToUpper();
            Origin = thesOrigin; // This will check against Destination
            DepartureTime = thedtDepartureTime;
            DurationMinutes = theiDurationMinutes;
            TotalSeats = theiTotalSeats;
            PricePerSeat = theddPricePerSeat;
        }

        /// <summary>
        /// Gets detailed information about the flight.
        /// </summary>
        /// <returns>A formatted string with flight details.</returns>
        public override string GetInfo()
        {
            return $"Flight [ID: {Id}, Code: {FlightCode}, AirlineID: {AirlineId}, Route: {Origin}->{Destination}, Departure: {DepartureTime:yyyy-MM-dd HH:mm}, Duration: {DurationMinutes} mins, Seats: {TotalSeats}, Price: {PricePerSeat:C}]";
        }
    }
}
