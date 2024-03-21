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
}
