namespace PaperClient.Tests;

public class TtlTest : PaperClientTest {
	[Fact]
	public void TtlNonExistent() {
		var err = Assert.Throws<PaperError>(() => this.client.Ttl("key", 1));
		Assert.Equal(PaperError.Type.KeyNotFound, err.GetType());
	}

	[Fact]
	public void TtlExistent() {
		this.client.Set("key", "value");
		this.client.Ttl("key", 1);
	}
}
