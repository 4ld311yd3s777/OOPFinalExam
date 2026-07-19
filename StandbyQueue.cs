using System;
using System.Collections.Generic;

namespace OOPFinalExam
{
    /// <summary>
    /// Implements a min-heap priority queue for StandbyPassenger.
    /// </summary>
    public class StandbyQueue
    {
        private List<StandbyPassenger> heap = new List<StandbyPassenger>();

        public void Enqueue(StandbyPassenger p)
        {
            heap.Add(p);
            HeapifyUp(heap.Count - 1);
        }

        public StandbyPassenger Dequeue()
        {
            if (heap.Count == 0) throw new InvalidOperationException("Queue is empty.");

            var root = heap[0];
            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);

            if (heap.Count > 0)
            {
                HeapifyDown(0);
            }

            return root;
        }

        public StandbyPassenger Peek()
        {
            if (heap.Count == 0) throw new InvalidOperationException("Queue is empty.");
            return heap[0];
        }

        public int Count()
        {
            return heap.Count;
        }

        /// <summary>
        /// Determines whether the standby queue is empty.
        /// </summary>
        /// <returns>True if empty; otherwise, false.</returns>
        public bool IsEmpty()
        {
            return heap.Count == 0;
        }

        private void HeapifyUp(int index)
        {
            int parent = (index - 1) / 2;
            while (index > 0 && Compare(heap[index], heap[parent]) < 0)
            {
                Swap(index, parent);
                index = parent;
                parent = (index - 1) / 2;
            }
        }

        private void HeapifyDown(int index)
        {
            int lastIndex = heap.Count - 1;
            while (true)
            {
                int leftChild = 2 * index + 1;
                int rightChild = 2 * index + 2;
                int smallest = index;

                if (leftChild <= lastIndex && Compare(heap[leftChild], heap[smallest]) < 0)
                {
                    smallest = leftChild;
                }
                if (rightChild <= lastIndex && Compare(heap[rightChild], heap[smallest]) < 0)
                {
                    smallest = rightChild;
                }

                if (smallest != index)
                {
                    Swap(index, smallest);
                    index = smallest;
                }
                else
                {
                    break;
                }
            }
        }

        private int Compare(StandbyPassenger a, StandbyPassenger b)
        {
            int pCmp = a.Priority.CompareTo(b.Priority);
            if (pCmp == 0)
            {
                return a.RegistrationTime.CompareTo(b.RegistrationTime);
            }
            return pCmp;
        }

        private void Swap(int i, int j)
        {
            var temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }
    }
}