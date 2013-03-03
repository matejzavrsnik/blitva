/*
Copyright (C) 2013 Matej Zavrsnik <matejzavrsnik@gmail.com> (matejzavrsnik.com)

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
associated documentation files (the "Software"), to deal in the Software without restriction,
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial
portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN
NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using System;
using System.Collections;

namespace Blitva
{
    public class Heap<T> : Ahnentafel
        where T : IComparable<T>
    {
        public enum HeapType
        {
            minHeap,
            maxHeap
        };

        private HeapType heapType;

        public Heap(HeapType type)
        {
            heapType = type;
        }

        public HeapType Type
        {
            get { return heapType; }
        }

        public T getBest()
        {
            if (isEmpty())
                throw new System.InvalidOperationException("Heap is empty.");
            return (T)m_content[0];
        }

        public T extractBest()
        {
            T result = getBest();
            deleteBest();
            return result;
        }

        public void deleteBest()
        {
            if (isEmpty())
                throw new System.InvalidOperationException("Heap is empty.");
            switchItems(0, getCount() - 1);
            m_content.RemoveAt(getCount() - 1);
            if (!isEmpty())
                bubbleDown(0);
        }

        public virtual int insert(T item)
        {
            int index = m_content.Add(item);
            index = bubbleUp(index);
            return index;
        }

        protected bool isFirstBigger(int first, int second)
        {
            return (((IComparable<T>)m_content[first]).CompareTo(((T)m_content[second])) > 0);
        }

        private int bubbleUp(int index)
        {
            if (index == 0)
                return 0;
            int parent = getParentIndex(index);
            // while parent is smaller and item not on root already
            while ((heapType == HeapType.minHeap && index != 0 && isFirstBigger(parent,index))
                || (heapType == HeapType.maxHeap && index != 0 && isFirstBigger(index,parent)))
            {
                switchItems(index, parent);
                index = parent;
                parent = getParentIndex(parent);
            }
            return index;
        }

        private int bubbleDown(int index)
        {
            int leftChild, rightChild, targetChild;
            bool finished = false;
            do
            {
                leftChild = getLeftChildIndex(index);
                rightChild = getRightChildIndex(index);
                // if left child is bigger then right child
                if (leftChild == index && rightChild == index) // when no children, get child will return element itself
                {
                    finished = true; // bubbled down to the end
                }
                else // bubble further
                {
                    if ((heapType == HeapType.minHeap && isFirstBigger(leftChild, rightChild)) || 
                        (heapType == HeapType.maxHeap && isFirstBigger(rightChild, leftChild)))
                        targetChild = rightChild;
                    else
                        targetChild = leftChild;
                    // if smaller item at index is bigger than smaller child
                    if (   (heapType == HeapType.minHeap && isFirstBigger(index, targetChild))
                        || (heapType == HeapType.maxHeap && isFirstBigger(targetChild, index)))
                    {
                        switchItems(targetChild, index);
                        index = targetChild;
                    }
                    else
                        finished = true;
                }
            }
            while (!finished);
            return index;
        }

        private void switchItems(int index1, int index2)
        {
            T temp = (T)m_content[index1];
            m_content[index1] = m_content[index2];
            m_content[index2] = temp;
        }
    }

    public class MinHeap<T> : Heap<T>
        where T : IComparable<T>
    {
        public MinHeap() 
            : base(HeapType.minHeap)
        {
        }

        public T getMin() { return base.getBest(); }
        public T extractMin() { return base.extractBest(); }
        public void deleteMin() { base.deleteBest(); }
    }

    public class MaxHeap<T> : Heap<T>
        where T : IComparable<T>
    {
        public MaxHeap()
            : base(HeapType.maxHeap)
        {
        }

        public T getMax() { return base.getBest(); }
        public T extractMax() { return base.extractBest(); }
        public void deleteMax() { base.deleteBest(); }
    }

    public class MidHeap<T> : MinHeap<T>
        where T : IComparable<T>
    {
        private MaxHeap<T> m_maxHeap;

        public MidHeap()
            : base()
        {
            m_maxHeap = new MaxHeap<T>();
        }

        public new bool isEmpty()
        {
            return (base.isEmpty() && m_maxHeap.isEmpty());
        }

        public new int getCount()
        {
            return (base.getCount() + m_maxHeap.getCount());
        }

        public new void clear()
        {
            base.clear();
            m_maxHeap.clear();
        }

        public override int insert(T item) 
        {
            int value;
            if (base.isEmpty() || base.getMin().CompareTo(item) < 0)
                value = base.insert(item);
            else
                value = m_maxHeap.insert(item);
            if (m_maxHeap.getCount() > base.getCount()+1)
                base.insert(m_maxHeap.extractMax());
            else if (base.getCount() > m_maxHeap.getCount()+1)
                m_maxHeap.insert(base.extractMin());

            return value; 
        }

        public T getMid() { return correctHeap().getBest(); }
        public T extractMid() { return correctHeap().extractBest(); }
        public void deleteMid() { correctHeap().deleteBest(); }

        private Heap<T> correctHeap()
        {
            if (base.getCount() > m_maxHeap.getCount())
                return this;
            else
                return m_maxHeap;
        }
    }
}
