namespace PaperClient.Tests;

public class PolicyTest : PaperClientTest {
	[Fact]
	public void Policy() {
		const string INITIAL_POLICY = "lfu";
		const string UPDATED_POLICY = "fifo";

		this.client.Policy(INITIAL_POLICY);
		Assert.Equal(INITIAL_POLICY, this.CachePolicy());

		this.client.Policy(UPDATED_POLICY);
		Assert.Equal(UPDATED_POLICY, this.CachePolicy());
	}

	private string CachePolicy() {
		var status = this.client.Status();
		return status.policy;
	}
}
