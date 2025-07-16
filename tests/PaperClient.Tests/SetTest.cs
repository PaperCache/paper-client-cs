namespace PaperClient.Tests;

public class SetTest : PaperClientTest {
	[Fact]
	public void SetNoTtl() {
		this.client.Set("key", "value");
	}

	[Fact]
	public void SetTtl() {
		this.client.Set("key", "value", 2);
	}

	[Fact]
	public void SetExpiry() {
		this.client.Set("key", "value", 1);

		var got = this.client.Get("key");
		Assert.Equal("value", got);

		Thread.Sleep(2000);

		var err = Assert.Throws<PaperError>(() => this.client.Get("key"));
		Assert.Equal(PaperError.Type.KeyNotFound, err.GetType());
	}
}
