using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class LinkedListNode<T>
    {
        public T Data { get; set; }
        public LinkedListNode<T> Next { get; set; }

        public LinkedListNode(T data)
        {
            Data = data;
            Next = null;
        }
    }

    public class LinkedList<T>
    {
        private LinkedListNode<T> _head;
        private LinkedListNode<T> _tail;
        private LinkedListNode<T> _current;
        private LinkedListNode<T> _firstAdded;
        private int _count;

        public int Count
        {
            get { return _count; }
        }
        public LinkedList()
        {
            _head = null;
            _tail = null;
            _current = null;
            _firstAdded = null;
            _count = 0;
        }
        public void Add(T data)
        {
            LinkedListNode<T> newNode = new LinkedListNode<T>(data);
            if (_head == null)
            {
                newNode.Next = newNode;
                _head = newNode;
                _tail = newNode;
                _firstAdded = newNode;
            }
            else
            {
                newNode.Next = _head;
                _tail.Next = newNode;
                _tail = newNode;
            }
            _current = _head;
            _count++;
        }
        public void MoveToNext()
        {
            if (_head == null)
            {
                throw new InvalidOperationException("The list is empty");
            }
            _current = _current.Next;
        }
        public T GetCurrent()
        {
            if (_current != null)
            {
                return _current.Data;
            }
            else
            {
                throw new InvalidOperationException("The list is empty.");
            }
        }
        public T GetNext()
        {
            MoveToNext();
            return GetCurrent();            
        }
        public void Reset()
        {
            _current = _firstAdded;
        }
    }
}
