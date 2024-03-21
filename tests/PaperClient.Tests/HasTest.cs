namespace PaperClient.Tests;

public class HasTest : PaperClientTest {
	[Fact]
	public void HasNonExistent() {
		var response = this.client.Has("key");

		Assert.True(response.IsOk());
		Assert.False(response.Data());
		Assert.Null(response.ErrData());
	}

	[Fact]
	public void HasExistent() {
		var set = this.client.Set("key", "value");
		Assert.True(set.IsOk());

		var response = this.client.Has("key");

		Assert.True(response.IsOk());
		Assert.True(response.Data());
		Assert.Null(response.ErrData());
	}
}
