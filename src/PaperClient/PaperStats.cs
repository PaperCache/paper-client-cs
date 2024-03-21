namespace PaperClient;

public struct PaperStats {
	public UInt64 max_size { get; }
	public UInt64 used_size { get; }

	public UInt64 total_gets { get; }
	public UInt64 total_sets { get; }
	public UInt64 total_dels { get; }

	public float miss_ratio { get; }

	public PaperPolicy policy { get; }
	public UInt64 uptime { get; }

	public PaperStats(
		UInt64 max_size,
		UInt64 used_size,

		UInt64 total_gets,
		UInt64 total_sets,
		UInt64 total_dels,

		float miss_ratio,

		byte policy_byte,
		UInt64 uptime
	) {
		this.max_size = max_size;
		this.used_size = used_size;

		this.total_gets = total_gets;
		this.total_sets = total_sets;
		this.total_dels = total_dels;

		this.miss_ratio = miss_ratio;

		this.policy = PaperStats.GetPolicyFromByte(policy_byte);
		this.uptime = uptime;
	}

	public UInt64 MaxSize() {
		return this.max_size;
	}

	private static PaperPolicy GetPolicyFromByte(byte value) {
		switch (value) {
			case 0: return PaperPolicy.Lfu;
			case 1: return PaperPolicy.Fifo;
			case 2: return PaperPolicy.Lru;
			case 3: return PaperPolicy.Mru;
		}

		throw new ArgumentException("Invalid policy byte.");
	}
}
