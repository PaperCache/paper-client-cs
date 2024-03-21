namespace PaperClient.Tests;

public class PeekTest : PaperClientTest {
	[Fact]
	public void PeekNonExistent() {
		var response = this.client.Peek("key");

		Assert.False(response.IsOk());
		Assert.NotNull(response.ErrData());
	}

	[Fact]
	public void PeekExistent() {
		var set = this.client.Set("key", "value");
		Assert.True(set.IsOk());

		var response = this.client.Peek("key");

		Assert.True(response.IsOk());
		Assert.NotNull(response.Data());
		Assert.Equal("value", response.Data());
	}
}
