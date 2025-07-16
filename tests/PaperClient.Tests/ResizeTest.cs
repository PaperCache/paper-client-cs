namespace PaperClient.Tests;

public class ResizeTest : PaperClientTest {
	[Fact]
	public void Resize() {
		const UInt64 INITIAL_SIZE = 40 * 1024 * 1024;
		const UInt64 UPDATED_SIZE = 20 * 1024 * 1024;

		this.client.Resize(INITIAL_SIZE);
		Assert.Equal(INITIAL_SIZE, this.CacheSize());

		this.client.Resize(UPDATED_SIZE);
		Assert.Equal(UPDATED_SIZE, this.CacheSize());
	}

	private UInt64 CacheSize() {
		var status = this.client.Status();
		return status.max_size;
	}
}
