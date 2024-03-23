namespace PaperClient.Tests;

public class AuthTest : PaperClientTest {
	public AuthTest() : base(false) {}

	[Fact]
	public void AuthIncorrect() {
		Assert.False(this.client.Set("key", "value", 0).IsOk());

		var response = this.client.Auth("incorrect_auth_token");
		Assert.False(response.IsOk());

		Assert.False(this.client.Set("key", "value", 0).IsOk());
	}

	[Fact]
	public void AuthCorrect() {
		Assert.False(this.client.Set("key", "value", 0).IsOk());

		var response = this.client.Auth("auth_token");
		Assert.True(response.IsOk());

		Assert.True(this.client.Set("key", "value", 0).IsOk());

	}
}
