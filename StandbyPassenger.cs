using System;

namespace OOPFinalExam
{
    /// <summary>
    /// Represents a passenger waiting for a seat on a full flight.
    /// </summary>
    public class StandbyPassenger
    {
        /// <summary>
        /// The passenger's full name.
        /// </summary>
        private string mysPassengerName;

        /// <summary>
        /// The passenger's passport number.
        /// </summary>
        private string mysPassportNumber;

        /// <summary>
        /// The passenger priority where a lower number means a higher priority.
        /// </summary>
        private int myiPriority;

        /// <summary>
        /// The time at which the passenger joined the standby queue.
        /// </summary>
        private DateTime mydtRegistrationTime;

        /// <summary>
        /// Gets the passenger's full name.
        /// </summary>
        public string PassengerName
        {
            get { return mysPassengerName; }
        }

        /// <summary>
        /// Gets the passenger's passport number.
        /// </summary>
        public string PassportNumber
        {
            get { return mysPassportNumber; }
        }

        /// <summary>
        /// Gets the passenger priority where 1 is VIP, 2 is Frequent Flyer, and 3 is Regular.
        /// </summary>
        public int Priority
        {
            get { return myiPriority; }
        }

        /// <summary>
        /// Gets the time at which the passenger joined the standby queue.
        /// </summary>
        public DateTime RegistrationTime
        {
            get { return mydtRegistrationTime; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StandbyPassenger"/> class.
        /// </summary>
        /// <param name="thesPassengerName">The passenger's full name.</param>
        /// <param name="thesPassportNumber">The passenger's passport number.</param>
        /// <param name="theiPriority">The passenger priority; a lower number has higher priority.</param>
        /// <param name="thedtRegistrationTime">The standby registration time.</param>
        public StandbyPassenger(
            string thesPassengerName,
            string thesPassportNumber,
            int theiPriority,
            DateTime thedtRegistrationTime)
        {
            if (string.IsNullOrWhiteSpace(thesPassengerName))
            {
                throw new ArgumentException("Passenger name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(thesPassportNumber))
            {
                throw new ArgumentException("Passport number cannot be empty.");
            }

            if (theiPriority <= 0)
            {
                throw new ArgumentException("Priority must be greater than zero.");
            }

            mysPassengerName = thesPassengerName;
            mysPassportNumber = thesPassportNumber;
            myiPriority = theiPriority;
            mydtRegistrationTime = thedtRegistrationTime;
        }

        /// <summary>
        /// Returns a readable description of the standby passenger.
        /// </summary>
        /// <returns>A formatted standby passenger description.</returns>
        public override string ToString()
        {
            return $"{PassengerName} ({PassportNumber}) - Priority {Priority}, Registered {RegistrationTime:yyyy-MM-dd HH:mm:ss}";
        }
    }
}
