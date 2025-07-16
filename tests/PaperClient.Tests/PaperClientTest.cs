namespace PaperClient.Tests;

public class PaperClientTest {
	protected PaperClient client;

    public PaperClientTest(bool auth = true) {
		this.client = new PaperClient("paper://127.0.0.1:3145");

		if (auth) {
			this.client.Auth("auth_token");
			this.client.Wipe();
		}
    }

	~PaperClientTest() {
		this.client.Disconnect();
	}
}
