namespace PaperClient.Tests;

public class ReconnectTest : PaperClientTest {
	[Fact]
	public void Reconnect() {
		this.client.Has("key");
		this.client.Disconnect();
		this.client.Has("key");
	}
}
