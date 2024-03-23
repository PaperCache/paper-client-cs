namespace PaperClient.Tests;

public class PaperPoolTest {
	[Fact]
	public void Pool() {
		var pool = new PaperPool("127.0.0.1", 3145, 2);

		for (int i=0; i<10; i++) {
			var lockable_client = pool.Client();
			var client = lockable_client.Lock();

			var response = client.Ping();

			Assert.True(response.IsOk());
			Assert.Equal("pong", response.Data());

			lockable_client.Unlock();
		}
	}

	[Fact]
	public void Auth() {
		var pool = new PaperPool("127.0.0.1", 3145, 2);

		Assert.False(this.CanSet(pool));
		pool.Auth("auth_token");
		Assert.True(this.CanSet(pool));
	}

	private bool CanSet(PaperPool pool) {
		var lockable_client = pool.Client();
		var client = lockable_client.Lock();

		var response = client.Set("key", "value");

		lockable_client.Unlock();

		return response.IsOk();
	}
}
