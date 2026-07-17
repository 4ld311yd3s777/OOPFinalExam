using System;

namespace OOPFinalExam
{
    /// <summary>
    /// Represents a business flight with premium features.
    /// </summary>
    public class BusinessFlight : Flight
    {
        /// <summary>
        /// Indicates if lounge access is included.
        /// </summary>
        private bool mybLoungeAccess;

        /// <summary>
        /// Indicates if a meal is included.
        /// </summary>
        private bool mybMealIncluded;

        /// <summary>
        /// The premium surcharge for the business flight.
        /// </summary>
        private decimal myddPremiumSurcharge;

        /// <summary>
        /// Gets or sets a value indicating whether lounge access is included.
        /// </summary>
        public bool LoungeAccess
        {
            get { return mybLoungeAccess; }
            set { mybLoungeAccess = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a meal is included.
        /// </summary>
        public bool MealIncluded
        {
            get { return mybMealIncluded; }
            set { mybMealIncluded = value; }
        }

        /// <summary>
        /// Gets or sets the premium surcharge. Must be >= 0.
        /// </summary>
        public decimal PremiumSurcharge
        {
            get { return myddPremiumSurcharge; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Premium surcharge cannot be negative.");
                }
                myddPremiumSurcharge = value;
            }
        }

        /// <summary>
        /// Gets the type of this entity.
        /// </summary>
        public override string EntityType
        {
            get { return "BusinessFlight"; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessFlight"/> class.
        /// </summary>
        /// <param name="theiId">The ID of the flight.</param>
        /// <param name="theiAirlineId">The ID of the airline.</param>
        /// <param name="thesFlightCode">The generated flight code.</param>
        /// <param name="thesOrigin">The origin airport.</param>
        /// <param name="thesDestination">The destination airport.</param>
        /// <param name="thedtDepartureTime">The departure date and time.</param>
        /// <param name="theiDurationMinutes">The duration of the flight in minutes.</param>
        /// <param name="theiTotalSeats">The total seats available.</param>
        /// <param name="theddPricePerSeat">The base price per seat.</param>
        /// <param name="thebLoungeAccess">True if lounge access is included; otherwise, false.</param>
        /// <param name="thebMealIncluded">True if a meal is included; otherwise, false.</param>
        /// <param name="theddPremiumSurcharge">The premium surcharge amount.</param>
        public BusinessFlight(int theiId, int theiAirlineId, string thesFlightCode, string thesOrigin, string thesDestination, DateTime thedtDepartureTime, int theiDurationMinutes, int theiTotalSeats, decimal theddPricePerSeat, bool thebLoungeAccess, bool thebMealIncluded, decimal theddPremiumSurcharge)
            : base(theiId, theiAirlineId, thesFlightCode, thesOrigin, thesDestination, thedtDepartureTime, theiDurationMinutes, theiTotalSeats, theddPricePerSeat)
        {
            mybLoungeAccess = thebLoungeAccess;
            mybMealIncluded = thebMealIncluded;
            PremiumSurcharge = theddPremiumSurcharge;
        }

        /// <summary>
        /// Gets detailed information about the business flight.
        /// </summary>
        /// <returns>A formatted string with business flight details.</returns>
        public override string GetInfo()
        {
            return base.GetInfo() + $" [Business Class - Lounge: {LoungeAccess}, Meal: {MealIncluded}, Surcharge: {PremiumSurcharge:C}]";
        }
    }
}
