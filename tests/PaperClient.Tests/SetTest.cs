namespace PaperClient.Tests;

public class SetTest : PaperClientTest {
	[Fact]
	public void SetNoTtl() {
		var response = this.client.Set("key", "value");

		Assert.True(response.IsOk());
		Assert.NotNull(response.Data());
	}

	[Fact]
	public void SetTtl() {
		var response = this.client.Set("key", "value", 2);

		Assert.True(response.IsOk());
		Assert.NotNull(response.Data());
	}

	[Fact]
	public void SetExpiry() {
		var set_response = this.client.Set("key", "value", 1);
		Assert.True(set_response.IsOk());
		Assert.NotNull(set_response.Data());

		var get_response = this.client.Get("key");
		Assert.True(get_response.IsOk());
		Assert.NotNull(get_response.Data());
		Assert.Equal("value", get_response.Data());

		Thread.Sleep(2000);

		var expired_response = this.client.Get("key");
		Assert.False(expired_response.IsOk());
		Assert.NotNull(expired_response.ErrData());
	}
}
