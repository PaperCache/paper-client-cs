namespace PaperClient.Tests;

public class PeekTest : PaperClientTest {
	[Fact]
	public void PeekNonExistent() {
		var err = Assert.Throws<PaperError>(() => this.client.Peek("key"));
		Assert.Equal(PaperError.Type.KeyNotFound, err.GetType());
	}

	[Fact]
	public void PeekExistent() {
		this.client.Set("key", "value");
		var got = this.client.Peek("key");

		Assert.Equal("value", got);
	}
}
