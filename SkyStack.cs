using System;

namespace OOPFinalExam
{
    /// <summary>
    /// Represents a generic last-in, first-out collection backed by a resizable array.
    /// </summary>
    /// <typeparam name="T">The type of elements stored in the stack.</typeparam>
    public class SkyStack<T>
    {
        /// <summary>
        /// The initial number of elements that can be stored without resizing.
        /// </summary>
        private const int myiInitialCapacity = 8;

        /// <summary>
        /// The array that stores the stack elements.
        /// </summary>
        private T[] myoData;

        /// <summary>
        /// The number of elements currently stored in the stack.
        /// </summary>
        private int myiCount;

        /// <summary>
        /// Initializes a new empty instance of the <see cref="SkyStack{T}"/> class.
        /// </summary>
        public SkyStack()
        {
            myoData = new T[myiInitialCapacity];
            myiCount = 0;
        }

        /// <summary>
        /// Adds an item to the top of the stack and doubles the array capacity when full.
        /// </summary>
        /// <param name="theoItem">The item to add.</param>
        public void Push(T theoItem)
        {
            if (myiCount == myoData.Length)
            {
                T[] aoExpandedData = new T[myoData.Length * 2];
                Array.Copy(myoData, aoExpandedData, myoData.Length);
                myoData = aoExpandedData;
            }

            myoData[myiCount] = theoItem;
            myiCount++;
        }

        /// <summary>
        /// Removes and returns the item at the top of the stack.
        /// </summary>
        /// <returns>The most recently pushed item.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the stack is empty.</exception>
        public T Pop()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Cannot pop an item from an empty SkyStack.");
            }

            myiCount--;
            T aoItem = myoData[myiCount];
            myoData[myiCount] = default(T);
            return aoItem;
        }

        /// <summary>
        /// Returns the item at the top of the stack without removing it.
        /// </summary>
        /// <returns>The most recently pushed item.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the stack is empty.</exception>
        public T Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Cannot peek at an empty SkyStack.");
            }

            return myoData[myiCount - 1];
        }

        /// <summary>
        /// Determines whether the stack contains no elements.
        /// </summary>
        /// <returns><see langword="true"/> when the stack is empty; otherwise, <see langword="false"/>.</returns>
        public bool IsEmpty()
        {
            return myiCount == 0;
        }

        /// <summary>
        /// Gets the number of elements currently stored in the stack.
        /// </summary>
        /// <returns>The number of stack elements.</returns>
        public int Size()
        {
            return myiCount;
        }
    }
}
