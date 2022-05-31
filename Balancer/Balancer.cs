using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Linq;
using System.Text;
using System.Threading;

namespace Balancer
{
    interface IEntry<T>: IComparable<IEntry<T>>
    {
        //Retrieve balanced object
        T Get();
        //Aquire balanced object
        IEntry<T> Acquire();
        //Updates performance information for last action. Should be update by application base on performance metrics
        void UpdateStatistic(int weight);
        //Resets total load information. Generally used internally to prevent data overflow
        void ResetTotal();
        //Calculated average performance
        int Weight { get; }
        //Calculated total load for balanced object
        int Total { get; }
    }

    class Balancer<T>: IReadOnlyList<IEntry<T>>
    {
        private class Entry : IEntry<T>
        {
            public Entry(Balancer<T> owner, T item)
            {
                 Initialize(owner, item, int.MaxValue);
            }
            public Entry(Balancer<T> owner, T item, int max_weight)
            {
                Initialize(owner, item, max_weight);
            }
            private void Initialize(Balancer<T> owner, T item, int max_weight)
            {
                value = item;
                _max_weight = max_weight;
                _owner = owner;
                Weight = 1;
            }

            public T Get()
            {
                return value;
            }

            public void UpdateStatistic(int weight)
            {
                //NOTE: Read and write to int, float is atomic. Reads/writes to double, long is not guarantee to be atomic
                //https://msdn.microsoft.com/en-us/library/aa691278(v=vs.71).aspx
                //But calculation is not atomic
                if (weight > _max_weight)
                {
                    weight = _max_weight;
                }
                else if (weight < 1)
                {
                    weight = 1;
                }

                int calc = (Weight + weight) / 2;
                Weight = (calc != 0) ? calc : 1;
            }

            public void ResetTotal()
            {
                Total = 0;
            }

            public IEntry<T> Acquire()
            {
                Total += Weight;
                if (Total > _max_total)
                {
                    _owner.ToList().ForEach(iterator => iterator.ResetTotal());
                }
                return this;
            }

            public int CompareTo(IEntry<T> other)
            {
                if (other == null) return 1;

                return (Weight + Total).CompareTo(other.Weight + other.Total);
            }

            //properties
            private const int _max_total = int.MaxValue / 2;
            Balancer<T> _owner;
            private T value;
            private int _max_weight;
            public int Weight { get; private set; }
            public int Total { get; private set; }
        }

        private ReadOnlyCollection<IEntry<T>> _items;

        public int Count
        {
            get
            {
                return _items.Count();
            }
        }

        public IEntry<T> this[int index]
        {
            get
            {
                return _items[index];
            }
        }

        //Note: balanced collection do not changes durign Balancer lifetime
        public Balancer(IEnumerable<T> collection, int max_weight = int.MaxValue)
        {
            

            var items = new List<IEntry<T>>();
            foreach (var item in collection)
            {
                items.Add(new Entry(this, item, max_weight));
            }
            if (items.Count == 0)
            {
                throw new ArgumentException("Nothing to balance");
            }
            _items = new ReadOnlyCollection<IEntry<T>>(items);
        }

        static public IEntry<T> AcquireNext(IEnumerable<IEntry<T>> items)
        {
            return items.Min().Acquire();
        }

        public IEntry<T> AcquireNext()
        {
            return Balancer<T>.AcquireNext(this);
        }

        public IEnumerator<IEntry<T>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
