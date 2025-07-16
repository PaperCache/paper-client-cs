namespace PaperClient.Tests;

public class GetTest : PaperClientTest {
	[Fact]
	public void GetNonExistent() {
		var err = Assert.Throws<PaperError>(() => this.client.Get("key"));
		Assert.Equal(PaperError.Type.KeyNotFound, err.GetType());
	}

	[Fact]
	public void GetExistent() {
		this.client.Set("key", "value");
		var got = this.client.Get("key");

		Assert.Equal("value", got);
	}
}
