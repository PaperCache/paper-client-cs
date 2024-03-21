namespace PaperClient;

public class PaperPool {
	private PaperClient[] clients;
	private Mutex[] locks;
	private int index;

	public PaperPool(string host, int port, int size) {
		this.clients = new PaperClient[size];
		this.locks = new Mutex[size];
		this.index = 0;

		for (int i=0; i<size; i++) {
			this.clients[i] = new PaperClient(host, port);
			this.locks[i] = new Mutex();
		}
	}

	public LockableClient Client() {
		int index = this.index;
		int new_index = (index + 1) % this.clients.Length;
		Interlocked.Exchange(ref this.index, new_index);

		return new LockableClient(this.clients[index], this.locks[index]);
	}
}
