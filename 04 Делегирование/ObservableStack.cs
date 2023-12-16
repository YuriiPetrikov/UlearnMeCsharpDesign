// Вставьте сюда финальное содержимое файла ObservableStack.cs
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Delegates.Observers.StackOperationsLogger;

namespace Delegates.Observers
{
    public class StackOperationsLogger
	{
        public delegate void StackHandler(object eventData);
        
		private readonly Observer observer = new Observer();
		
		public void SubscribeOn<T>(ObservableStack<T> stack)
		{
			stack.Add(observer);
		}

		public string GetLog()
		{
			return observer.Log.ToString();
		}
	}

	public class Observer 
	{
		public StringBuilder Log = new StringBuilder();

		public void HandleEvent(object eventData)
		{
			Log.Append(eventData);
		}
	}


	public class ObservableStack<T>
	{
		public event StackHandler NotifyEvent;

		public void Add(Observer observer)
		{
			NotifyEvent += observer.HandleEvent;
		}

		public void Notify(object eventData)
		{
			NotifyEvent?.Invoke(eventData);
		}

		public void Remove(Observer observer)
		{
			NotifyEvent -= observer.HandleEvent;
		}

		List<T> data = new List<T>();

		public void Push(T obj)
		{
			data.Add(obj);
			Notify(new StackEventData<T> { IsPushed = true, Value = obj });
		}

		public T Pop()
		{
			if (data.Count == 0)
				throw new InvalidOperationException();
			var result = data[data.Count - 1];
			Notify(new StackEventData<T> { IsPushed = false, Value = result });
			return result;
		}
	}
}
