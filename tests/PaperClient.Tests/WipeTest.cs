namespace PaperClient.Tests;

public class WipeTest : PaperClientTest {
	[Fact]
	public void Wipe() {
		Assert.True(this.client.Set("key", "value").IsOk());

		var response = this.client.Wipe();
		Assert.True(response.IsOk());
		Assert.NotNull(response.Data());

		var got = this.client.Get("key");
		Assert.False(got.IsOk());
		Assert.NotNull(got.ErrData());
	}
}
