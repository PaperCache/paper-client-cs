namespace PaperClient.Tests;

public class ResizeTest : PaperClientTest {
	[Fact]
	public void Resize() {
		const UInt64 INITIAL_SIZE = 40 * 1024 * 1024;
		const UInt64 UPDATED_SIZE = 20 * 1024 * 1024;

		var initial_response = this.client.Resize(INITIAL_SIZE);

		Assert.True(initial_response.IsOk());
		Assert.NotNull(initial_response.Data());
		Assert.True(this.CacheSize() == INITIAL_SIZE);

		var updated_response = this.client.Resize(UPDATED_SIZE);

		Assert.True(updated_response.IsOk());
		Assert.NotNull(updated_response.Data());
		Assert.True(this.CacheSize() == UPDATED_SIZE);
	}

	private UInt64 CacheSize() {
		var response = this.client.Stats();
		var data = response.Data();

		Assert.NotNull(data);

		var stats = data.Value;
		return stats.max_size;
	}
}
