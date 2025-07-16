namespace PaperClient.Tests;

public class WipeTest : PaperClientTest {
	[Fact]
	public void Wipe() {
		this.client.Set("key", "value");
		this.client.Wipe();

		var err = Assert.Throws<PaperError>(() => this.client.Get("key"));
		Assert.Equal(PaperError.Type.KeyNotFound, err.GetType());
	}
}
