namespace PaperClient.Tests;

public class DelTest : PaperClientTest {
	[Fact]
	public void DelNonExistent() {
		var response = this.client.Del("key");

		Assert.False(response.IsOk());
		Assert.NotNull(response.ErrData());
	}

	[Fact]
	public void DelExistent() {
		var set = this.client.Set("key", "value");
		Assert.True(set.IsOk());

		var response = this.client.Del("key");

		Assert.True(response.IsOk());
		Assert.NotNull(response.Data());
	}
}
