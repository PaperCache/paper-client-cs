namespace PaperClient.Tests;

public class StatsTest : PaperClientTest {
	[Fact]
	public void Stats() {
		var response = this.client.Stats();

		Assert.True(response.IsOk());
		Assert.NotNull(response.Data());
	}
}
