using System;

namespace OOPFinalExam
{
    /// <summary>
    /// Represents a passenger booking for a flight.
    /// </summary>
    public class Booking : SkyEntity
    {
        /// <summary>
        /// The ID of the flight this booking is for.
        /// </summary>
        private int myiFlightId;

        /// <summary>
        /// The name of the passenger.
        /// </summary>
        private string mysPassengerName;

        /// <summary>
        /// The passport number of the passenger.
        /// </summary>
        private string mysPassportNumber;

        /// <summary>
        /// The seat number reserved (e.g. "12A").
        /// </summary>
        private string mysSeatNumber;

        /// <summary>
        /// The status of the booking.
        /// </summary>
        private BookingStatus myoStatus;

        /// <summary>
        /// The base price of the flight to compute the booking fee.
        /// </summary>
        private decimal myddBaseFlightPrice;

        /// <summary>
        /// Gets the flight ID (read-only).
        /// </summary>
        public int FlightId
        {
            get { return myiFlightId; }
        }

        /// <summary>
        /// Gets or sets the passenger name. Cannot be empty.
        /// </summary>
        public string PassengerName
        {
            get { return mysPassengerName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Passenger name cannot be empty.");
                }
                mysPassengerName = value;
            }
        }

        /// <summary>
        /// Gets or sets the passport number.
        /// </summary>
        public string PassportNumber
        {
            get { return mysPassportNumber; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Passport number cannot be empty.");
                }
                mysPassportNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets the seat number.
        /// </summary>
        public string SeatNumber
        {
            get { return mysSeatNumber; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Seat number cannot be empty.");
                }
                mysSeatNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets the booking status.
        /// </summary>
        public BookingStatus Status
        {
            get { return myoStatus; }
            set { myoStatus = value; }
        }

        /// <summary>
        /// Gets the booking fee (read-only), calculated as 5% of the base flight price.
        /// </summary>
        public decimal BookingFee
        {
            get { return myddBaseFlightPrice * 0.05m; }
        }

        /// <summary>
        /// Gets the type of this entity.
        /// </summary>
        public override string EntityType
        {
            get { return "Booking"; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Booking"/> class.
        /// </summary>
        /// <param name="theiId">The ID of the booking.</param>
        /// <param name="theiFlightId">The ID of the flight.</param>
        /// <param name="thesPassengerName">The passenger's name.</param>
        /// <param name="thesPassportNumber">The passenger's passport number.</param>
        /// <param name="thesSeatNumber">The assigned seat number.</param>
        /// <param name="theoStatus">The booking status.</param>
        /// <param name="theddBaseFlightPrice">The base price of the flight.</param>
        public Booking(int theiId, int theiFlightId, string thesPassengerName, string thesPassportNumber, string thesSeatNumber, BookingStatus theoStatus, decimal theddBaseFlightPrice)
            : base(theiId)
        {
            myiFlightId = theiFlightId;
            PassengerName = thesPassengerName;
            PassportNumber = thesPassportNumber;
            SeatNumber = thesSeatNumber;
            myoStatus = theoStatus;
            myddBaseFlightPrice = theddBaseFlightPrice;
        }

        /// <summary>
        /// Gets detailed information about the booking.
        /// </summary>
        /// <returns>A formatted string with booking details.</returns>
        public override string GetInfo()
        {
            return $"Booking [ID: {Id}, FlightID: {FlightId}, Passenger: {PassengerName}, Passport: {PassportNumber}, Seat: {SeatNumber}, Status: {Status}, Fee: {BookingFee:C}]";
        }
    }
}
