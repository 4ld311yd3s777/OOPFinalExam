namespace OOPFinalExam
{
    /// <summary>
    /// Identifies the type of booking operation recorded in the history.
    /// </summary>
    internal enum BookingActionType
    {
        /// <summary>
        /// A booking was added to the system.
        /// </summary>
        Added,

        /// <summary>
        /// An existing booking was cancelled.
        /// </summary>
        Cancelled
    }

    /// <summary>
    /// Stores the metadata needed to reverse one booking operation.
    /// </summary>
    internal class BookingAction
    {
        /// <summary>
        /// The type of operation that was performed.
        /// </summary>
        private BookingActionType myoActionType;

        /// <summary>
        /// The booking status before the operation was performed.
        /// </summary>
        private BookingStatus myoPreviousStatus;

        /// <summary>
        /// Gets the recorded operation type.
        /// </summary>
        public BookingActionType ActionType
        {
            get { return myoActionType; }
        }

        /// <summary>
        /// Gets the booking status that existed before the operation.
        /// </summary>
        public BookingStatus PreviousStatus
        {
            get { return myoPreviousStatus; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingAction"/> class.
        /// </summary>
        /// <param name="theoActionType">The type of booking operation.</param>
        /// <param name="theoPreviousStatus">The booking status before the operation.</param>
        public BookingAction(BookingActionType theoActionType, BookingStatus theoPreviousStatus)
        {
            myoActionType = theoActionType;
            myoPreviousStatus = theoPreviousStatus;
        }
    }
}
