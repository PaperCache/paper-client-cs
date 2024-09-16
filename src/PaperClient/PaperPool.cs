namespace PaperClient;

public class PaperPool {
	private PaperClient[] clients;
	private Mutex[] locks;
	private int index;

	public PaperPool(string paper_addr, int size) {
		this.clients = new PaperClient[size];
		this.locks = new Mutex[size];
		this.index = 0;

		for (int i=0; i<size; i++) {
			this.clients[i] = new PaperClient(paper_addr);
			this.locks[i] = new Mutex();
		}
	}

	public void Auth(string token) {
		for (int i=0; i<this.clients.Length; i++) {
			this.locks[i].WaitOne();
			this.clients[i].Auth(token);
			this.locks[i].ReleaseMutex();
		}
	}

	public LockableClient Client() {
		int index = this.index;
		int new_index = (index + 1) % this.clients.Length;
		Interlocked.Exchange(ref this.index, new_index);

		return new LockableClient(this.clients[index], this.locks[index]);
	}
}
