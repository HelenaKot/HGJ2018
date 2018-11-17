interface IObservable<T>
{
	void Subscribe(IObserver<T> observer);
	void Unsubscribe(IObserver<T> observer);
	void UpdateObservers();
}