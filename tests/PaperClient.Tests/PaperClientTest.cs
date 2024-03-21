namespace PaperClient.Tests;

public class PaperClientTest {
	protected PaperClient client;

    public PaperClientTest() {
		this.client = new PaperClient("127.0.0.1", 3145);
		this.client.Wipe();
    }

	~PaperClientTest() {
		this.client.Disconnect();
	}
}
