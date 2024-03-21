namespace PaperClient;

public class LockableClient {
	private PaperClient client;
	private Mutex mutex;

	public LockableClient(PaperClient client, Mutex mutex) {
		this.client = client;
		this.mutex = mutex;
	}

	public PaperClient Lock() {
		this.mutex.WaitOne();
		return this.client;
	}

	public void Unlock() {
		this.mutex.ReleaseMutex();
	}
}
