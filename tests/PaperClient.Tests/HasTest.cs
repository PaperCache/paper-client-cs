namespace PaperClient.Tests;

public class HasTest : PaperClientTest {
	[Fact]
	public void HasNonExistent() {
		var has = this.client.Has("key");
		Assert.False(has);
	}

	[Fact]
	public void HasExistent() {
		this.client.Set("key", "value");

		var has = this.client.Has("key");
		Assert.True(has);
	}
}
