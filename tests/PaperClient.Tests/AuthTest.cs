namespace PaperClient.Tests;

public class AuthTest : PaperClientTest {
	public AuthTest() : base(false) {}

	[Fact]
	public void AuthIncorrect() {
		var set_err = Assert.Throws<PaperError>(() => this.client.Set("key", "value"));
		Assert.Equal(PaperError.Type.Unauthorized, set_err.GetType());

		var auth_err = Assert.Throws<PaperError>(() => this.client.Auth("incorrect_auth_token"));
		Assert.Equal(PaperError.Type.Unauthorized, auth_err.GetType());
	}

	[Fact]
	public void AuthCorrect() {
		var err = Assert.Throws<PaperError>(() => this.client.Set("key", "value"));
		Assert.Equal(PaperError.Type.Unauthorized, err.GetType());

		this.client.Auth("auth_token");
		this.client.Set("key", "value");
	}
}
