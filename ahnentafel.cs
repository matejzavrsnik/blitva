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
    public class Ahnentafel
    {
        protected ArrayList m_content;

        protected Ahnentafel()
        {
            m_content = new ArrayList();
        }

        public bool isEmpty()
        {
            if (getCount() == 0)
                return true;
            return false;
        }

        public int getCount()
        {
            return m_content.Count;
        }

        public void clear()
        {
            m_content.Clear();
        }

        protected int getParentIndex(int index)
        {
            if (index < 0 || index > getCount() - 1)
                throw new System.InvalidOperationException("Invalid index.");
            int result = (int)Math.Floor(((double)index - 1) / 2);
            return result;
        }

        protected int getLeftChildIndex(int index)
        {
            if (index < 0 || index > getCount() - 1)
                throw new System.InvalidOperationException("Invalid index.");
            int result = (2 * index) + 1;
            if (result > getCount() - 1)
                result = index; // return itself if no children
            return result;
        }

        protected int getRightChildIndex(int index)
        {
            if (index < 0 || index > getCount() - 1)
                throw new System.InvalidOperationException("Invalid index.");
            int result = (2 * index) + 2;
            if (result > getCount() - 1)
                result = index; // return itself if no children
            return result;
        }
    }
}
