namespace PaperClient.Tests;

public class StatusTest : PaperClientTest {
	[Fact]
	public void Status() {
		this.client.Status();
	}
}
