namespace PaperClient.Tests;

public class VersionTest : PaperClientTest {
	[Fact]
	public void Version() {
		var response = this.client.Version();

		Assert.True(response.IsOk());
		Assert.NotNull(response.Data());
	}
}
