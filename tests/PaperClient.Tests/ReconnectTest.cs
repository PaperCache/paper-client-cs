namespace PaperClient.Tests;

public class ReconnectTest : PaperClientTest {
	[Fact]
	public void Reconnect() {
		var pre_disconnect = this.client.Has("key");
		Assert.True(pre_disconnect.IsOk());

		this.client.Disconnect();

		var post_disconnect = this.client.Has("key");
		Assert.True(post_disconnect.IsOk());
	}
}
