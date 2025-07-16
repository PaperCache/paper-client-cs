namespace PaperClient.Tests;

public class PaperPoolTest {
	[Fact]
	public void Pool() {
		var pool = new PaperPool("paper://127.0.0.1:3145", 2);

		for (int i=0; i<10; i++) {
			var lockable_client = pool.Client();
			var client = lockable_client.Lock();

			var response = client.Ping();
			Assert.Equal("pong", response);

			lockable_client.Unlock();
		}
	}

	[Fact]
	public void Auth() {
		var pool = new PaperPool("paper://127.0.0.1:3145", 2);

		Assert.False(this.CanSet(pool));
		pool.Auth("auth_token");
		Assert.True(this.CanSet(pool));
	}

	private bool CanSet(PaperPool pool) {
		var lockable_client = pool.Client();
		var client = lockable_client.Lock();

		try {
			client.Set("key", "value");
			lockable_client.Unlock();

			return true;
		} catch {
			lockable_client.Unlock();
			return false;
		}
	}
}
