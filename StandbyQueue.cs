using System;
using System.Collections.Generic;

namespace OOPFinalExam
{
    /// <summary>
    /// Implements a min-heap priority queue for standby passengers.
    /// </summary>
    public class StandbyQueue
    {
        /// <summary>
        /// The array-backed list used to store the min-heap.
        /// </summary>
        private List<StandbyPassenger> myoHeap;

        /// <summary>
        /// Gets the number of passengers currently waiting.
        /// </summary>
        public int Count
        {
            get { return myoHeap.Count; }
        }

        /// <summary>
        /// Initializes a new empty standby queue.
        /// </summary>
        public StandbyQueue()
        {
            myoHeap = new List<StandbyPassenger>();
        }

        /// <summary>
        /// Adds a passenger to the queue and restores the min-heap property.
        /// </summary>
        /// <param name="theoPassenger">The standby passenger to enqueue.</param>
        public void Enqueue(StandbyPassenger theoPassenger)
        {
            if (theoPassenger == null)
            {
                throw new ArgumentNullException(nameof(theoPassenger));
            }

            myoHeap.Add(theoPassenger);
            SiftUp(myoHeap.Count - 1);
        }

        /// <summary>
        /// Removes and returns the highest-priority passenger.
        /// </summary>
        /// <returns>The passenger with the lowest priority number and earliest registration time.</returns>
        public StandbyPassenger Dequeue()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The standby queue is empty.");
            }

            StandbyPassenger aTopPassenger = myoHeap[0];
            int aiLastIndex = myoHeap.Count - 1;
            myoHeap[0] = myoHeap[aiLastIndex];
            myoHeap.RemoveAt(aiLastIndex);

            if (myoHeap.Count > 0)
            {
                SiftDown(0);
            }

            return aTopPassenger;
        }

        /// <summary>
        /// Returns the highest-priority passenger without removing that passenger.
        /// </summary>
        /// <returns>The passenger at the root of the min-heap.</returns>
        public StandbyPassenger Peek()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The standby queue is empty.");
            }

            return myoHeap[0];
        }

        /// <summary>
        /// Returns a snapshot of all passengers in priority order without changing this queue.
        /// </summary>
        /// <returns>A list ordered by priority and then registration time.</returns>
        public List<StandbyPassenger> GetPassengersInPriorityOrder()
        {
            StandbyQueue aQueueCopy = new StandbyQueue();
            List<StandbyPassenger> aOrderedPassengers = new List<StandbyPassenger>();

            foreach (StandbyPassenger aPassenger in myoHeap)
            {
                aQueueCopy.Enqueue(aPassenger);
            }

            while (aQueueCopy.Count > 0)
            {
                aOrderedPassengers.Add(aQueueCopy.Dequeue());
            }

            return aOrderedPassengers;
        }

        /// <summary>
        /// Moves an item upward until its parent has equal or higher priority.
        /// </summary>
        /// <param name="theiIndex">The heap index to move upward.</param>
        private void SiftUp(int theiIndex)
        {
            int aiCurrentIndex = theiIndex;

            while (aiCurrentIndex > 0)
            {
                int aiParentIndex = (aiCurrentIndex - 1) / 2;
                if (!HasHigherPriority(myoHeap[aiCurrentIndex], myoHeap[aiParentIndex]))
                {
                    break;
                }

                Swap(aiCurrentIndex, aiParentIndex);
                aiCurrentIndex = aiParentIndex;
            }
        }

        /// <summary>
        /// Moves an item downward until both children have equal or lower priority.
        /// </summary>
        /// <param name="theiIndex">The heap index to move downward.</param>
        private void SiftDown(int theiIndex)
        {
            int aiCurrentIndex = theiIndex;

            while (true)
            {
                int aiLeftIndex = (aiCurrentIndex * 2) + 1;
                int aiRightIndex = aiLeftIndex + 1;
                int aiHighestPriorityIndex = aiCurrentIndex;

                if (aiLeftIndex < myoHeap.Count &&
                    HasHigherPriority(myoHeap[aiLeftIndex], myoHeap[aiHighestPriorityIndex]))
                {
                    aiHighestPriorityIndex = aiLeftIndex;
                }

                if (aiRightIndex < myoHeap.Count &&
                    HasHigherPriority(myoHeap[aiRightIndex], myoHeap[aiHighestPriorityIndex]))
                {
                    aiHighestPriorityIndex = aiRightIndex;
                }

                if (aiHighestPriorityIndex == aiCurrentIndex)
                {
                    break;
                }

                Swap(aiCurrentIndex, aiHighestPriorityIndex);
                aiCurrentIndex = aiHighestPriorityIndex;
            }
        }

        /// <summary>
        /// Determines whether one passenger must appear before another in the min-heap.
        /// </summary>
        /// <param name="theoFirst">The first passenger to compare.</param>
        /// <param name="theoSecond">The second passenger to compare.</param>
        /// <returns>True when the first passenger has a higher queue priority.</returns>
        private bool HasHigherPriority(StandbyPassenger theoFirst, StandbyPassenger theoSecond)
        {
            if (theoFirst.Priority != theoSecond.Priority)
            {
                return theoFirst.Priority < theoSecond.Priority;
            }

            return theoFirst.RegistrationTime < theoSecond.RegistrationTime;
        }

        /// <summary>
        /// Exchanges two passengers in the heap.
        /// </summary>
        /// <param name="theiFirstIndex">The first heap index.</param>
        /// <param name="theiSecondIndex">The second heap index.</param>
        private void Swap(int theiFirstIndex, int theiSecondIndex)
        {
            StandbyPassenger aTemporaryPassenger = myoHeap[theiFirstIndex];
            myoHeap[theiFirstIndex] = myoHeap[theiSecondIndex];
            myoHeap[theiSecondIndex] = aTemporaryPassenger;
        }
    }
}
