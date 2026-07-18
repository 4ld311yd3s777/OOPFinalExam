using System;

namespace OOPFinalExam
{
    /// <summary>
    /// Represents a passenger in the standby queue.
    /// </summary>
    public class StandbyPassenger
    {
        public string PassengerName { get; set; }
        public string PassportNumber { get; set; }
        public int Priority { get; set; }
        public DateTime RegistrationTime { get; set; }

        public StandbyPassenger(string theName, string thePassport, int thePriority, DateTime theRegTime)
        {
            PassengerName = theName;
            PassportNumber = thePassport;
            Priority = thePriority;
            RegistrationTime = theRegTime;
        }
    }
}