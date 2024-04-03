namespace PaperClient.Tests;

public class PolicyTest : PaperClientTest {
	[Fact]
	public void Policy() {
		const PaperPolicy INITIAL_POLICY = PaperPolicy.Lfu;
		const PaperPolicy UPDATED_POLICY = PaperPolicy.Fifo;

		var initial_response = this.client.Policy(INITIAL_POLICY);

		Assert.True(initial_response.IsOk());
		Assert.NotNull(initial_response.Data());
		Assert.Equal(INITIAL_POLICY, this.CachePolicy());

		var updated_response = this.client.Policy(UPDATED_POLICY);

		Assert.True(updated_response.IsOk());
		Assert.NotNull(updated_response.Data());
		Assert.Equal(UPDATED_POLICY, this.CachePolicy());
	}

	private PaperPolicy CachePolicy() {
		var response = this.client.Stats();
		var data = response.Data();

		Assert.NotNull(data);

		var stats = data.Value;
		return stats.policy;
	}
}
