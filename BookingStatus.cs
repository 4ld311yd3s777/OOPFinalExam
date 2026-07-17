namespace OOPFinalExam
{
    /// <summary>
    /// Represents the status of a passenger booking.
    /// </summary>
    public enum BookingStatus
    {
        /// <summary>
        /// The booking has been confirmed and a seat is reserved.
        /// </summary>
        Confirmed,

        /// <summary>
        /// The booking is pending confirmation.
        /// </summary>
        Pending,

        /// <summary>
        /// The booking is cancelled.
        /// </summary>
        Cancelled
    }
}
