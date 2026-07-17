using System;

namespace OOPFinalExam
{
    /// <summary>
    /// Represents an Airline entity in the system.
    /// </summary>
    public class Airline : SkyEntity
    {
        /// <summary>
        /// The name of the airline.
        /// </summary>
        private string mysAirlineName;

        /// <summary>
        /// The IATA code of the airline.
        /// </summary>
        private string mysIATACode;

        /// <summary>
        /// The country where the airline is registered.
        /// </summary>
        private string mysCountry;

        /// <summary>
        /// The year the airline was founded.
        /// </summary>
        private int myiFoundedYear;

        /// <summary>
        /// Gets or sets the name of the airline.
        /// </summary>
        public string AirlineName
        {
            get { return mysAirlineName; }
            set { mysAirlineName = value; }
        }

        /// <summary>
        /// Gets or sets the IATA code of the airline.
        /// </summary>
        public string IATACode
        {
            get { return mysIATACode; }
            set { mysIATACode = value; }
        }

        /// <summary>
        /// Gets or sets the country of the airline.
        /// </summary>
        public string Country
        {
            get { return mysCountry; }
            set { mysCountry = value; }
        }

        /// <summary>
        /// Gets or sets the year the airline was founded.
        /// </summary>
        public int FoundedYear
        {
            get { return myiFoundedYear; }
            set { myiFoundedYear = value; }
        }

        /// <summary>
        /// Gets the type of this entity.
        /// </summary>
        public override string EntityType
        {
            get { return "Airline"; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Airline"/> class.
        /// </summary>
        /// <param name="theiId">The ID of the airline.</param>
        /// <param name="thesAirlineName">The name of the airline.</param>
        /// <param name="thesIATACode">The IATA code of the airline.</param>
        /// <param name="thesCountry">The country of the airline.</param>
        /// <param name="theiFoundedYear">The founded year of the airline.</param>
        public Airline(int theiId, string thesAirlineName, string thesIATACode, string thesCountry, int theiFoundedYear)
            : base(theiId)
        {
            mysAirlineName = thesAirlineName;
            mysIATACode = thesIATACode;
            mysCountry = thesCountry;
            myiFoundedYear = theiFoundedYear;
        }

        /// <summary>
        /// Gets detailed information about the airline.
        /// </summary>
        /// <returns>A formatted string with airline details.</returns>
        public override string GetInfo()
        {
            return $"Airline [ID: {Id}, Name: {AirlineName}, IATA: {IATACode}, Country: {Country}, Founded: {FoundedYear}]";
        }
    }
}
