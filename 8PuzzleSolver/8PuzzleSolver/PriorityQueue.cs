using System;
using System.Collections.Generic;
using System.Text;

namespace _8PuzzleSolver
{
    class PriorityQueue<T, T1> where T : IComparable<T> where T1 : IComparable<T1>
    {
        List<(T distance, T1 value)> array = new List<(T, T1)>();
        


        public PriorityQueue()
        {
           
        }


        public void Insert(T priority, T1 value)
        {
            array.Add((priority, value));
            //expandArray();

            //array[length] = priority;
            //objArray[length] = value;

            HeapifyUp(array.Count-1);
        }

        public (T distance,T1 value) Pop()
        {
            var temp = array[0];
            array[0] = array[array.Count - 1];
            array.RemoveAt(array.Count - 1);

            HeapifyDown(0);

            return temp;
        }

        public void HeapifyDown(int index)
        {

            int leftChild = index * 2 + 1;
            if (leftChild >= array.Count)
            {
                return;
            }

            int rightChild = index * 2 + 2;
            int swapIndex = 0;

            if (rightChild >= array.Count)
            {
                swapIndex = leftChild;
            }
            else
            {
                if (array[rightChild].distance.CompareTo(array[leftChild].distance) <= 0)
                {
                    swapIndex = rightChild;

                }
                else
                {
                    swapIndex = leftChild;
                }
            }

            if (array[swapIndex].distance.CompareTo(array[index].distance) <= 0)
            {
                var temp = array[index];
                array[index] = array[swapIndex];
                array[swapIndex] = temp;

                
            }

            HeapifyDown(swapIndex);
        }



        private void HeapifyUp(int index)
        {
            int parentIndex = (index - 1) / 2;

            if (index == 0)
            {
                return;
            }

            if (array[parentIndex].distance.CompareTo(array[index].distance) >= 0)
            {
                var temp = array[index];
                array[index] = array[parentIndex];
                array[parentIndex] = temp;

            }

            HeapifyUp(parentIndex);
        }


      
    }


}
