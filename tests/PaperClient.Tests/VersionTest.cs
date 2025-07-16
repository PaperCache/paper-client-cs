namespace PaperClient.Tests;

public class VersionTest : PaperClientTest {
	[Fact]
	public void Version() {
		var version = this.client.Version();
		Assert.NotNull(version);
	}
}
