namespace PaperClient.Tests;

public class TtlTest : PaperClientTest {
	[Fact]
	public void TtlNonExistent() {
		var response = this.client.Ttl("key");

		Assert.False(response.IsOk());
		Assert.NotNull(response.ErrData());
	}

	[Fact]
	public void TtlExistent() {
		var set = this.client.Set("key", "value");
		Assert.True(set.IsOk());

		var response = this.client.Ttl("key");

		Assert.True(response.IsOk());
		Assert.NotNull(response.Data());
	}
}
