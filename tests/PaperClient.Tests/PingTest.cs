namespace PaperClient.Tests;

public class PingTest : PaperClientTest {
	[Fact]
	public void Ping() {
		var response = this.client.Ping();

		Assert.True(response.IsOk());
		Assert.Equal("pong", response.Data());
	}
}
