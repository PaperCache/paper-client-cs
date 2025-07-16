namespace PaperClient.Tests;

public class SizeTest : PaperClientTest {
	[Fact]
	public void SizeNonExistent() {
		var err = Assert.Throws<PaperError>(() => this.client.Size("key"));
		Assert.Equal(PaperError.Type.KeyNotFound, err.GetType());
	}

	[Fact]
	public void SizeExistent() {
		this.client.Set("key", "value");
		var size = this.client.Size("key");

		Assert.True(size > 0);
	}
}
