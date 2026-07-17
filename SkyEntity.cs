using System;

namespace OOPFinalExam
{
    /// <summary>
    /// Abstract base class for all system entities.
    /// </summary>
    public abstract class SkyEntity
    {
        /// <summary>
        /// The unique identifier of the entity.
        /// </summary>
        private int myiId;

        /// <summary>
        /// Gets the unique identifier of the entity.
        /// </summary>
        public int Id
        {
            get { return myiId; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkyEntity"/> class.
        /// </summary>
        /// <param name="theiId">The unique identifier to assign to the entity.</param>
        protected SkyEntity(int theiId)
        {
            myiId = theiId;
        }

        /// <summary>
        /// Gets detailed information about the entity.
        /// </summary>
        /// <returns>A string containing detailed entity info.</returns>
        public abstract string GetInfo();

        /// <summary>
        /// Gets the type of the entity as a string description.
        /// </summary>
        public abstract string EntityType { get; }

        /// <summary>
        /// Gets a basic summary of the entity.
        /// </summary>
        /// <returns>A string summary.</returns>
        public virtual string GetSummary()
        {
            return $"Type: {EntityType}, Id: {Id}";
        }
    }
}
