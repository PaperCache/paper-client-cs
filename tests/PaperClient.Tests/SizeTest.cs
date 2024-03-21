namespace PaperClient.Tests;

public class SizeTest : PaperClientTest {
	[Fact]
	public void SizeNonExistent() {
		var response = this.client.Size("key");

		Assert.False(response.IsOk());
		Assert.NotNull(response.ErrData());
	}

	[Fact]
	public void SizeExistent() {
		var set = this.client.Set("key", "value");
		Assert.True(set.IsOk());

		var response = this.client.Size("key");

		Assert.True(response.IsOk());
		Assert.NotNull(response.Data());
		Assert.True(response.Data() == 5);
	}
}
