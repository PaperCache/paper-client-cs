namespace PaperClient.Tests;

public class GetTest : PaperClientTest {
	[Fact]
	public void GetNonExistent() {
		var response = this.client.Get("key");

		Assert.False(response.IsOk());
		Assert.NotNull(response.ErrData());
	}

	[Fact]
	public void GetExistent() {
		var set = this.client.Set("key", "value");
		Assert.True(set.IsOk());

		var response = this.client.Get("key");

		Assert.True(response.IsOk());
		Assert.NotNull(response.Data());
		Assert.Equal("value", response.Data());
	}
}
