namespace PaperClient.Tests;

public class DelTest : PaperClientTest {
	[Fact]
	public void DelNonExistent() {
		var err = Assert.Throws<PaperError>(() => this.client.Del("key"));
		Assert.Equal(PaperError.Type.KeyNotFound, err.GetType());
	}

	[Fact]
	public void DelExistent() {
		this.client.Set("key", "value");
		this.client.Del("key");
	}
}
