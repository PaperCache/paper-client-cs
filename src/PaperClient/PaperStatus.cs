namespace PaperClient;

public struct PaperStatus {
	public UInt32 pid { get; }

	public UInt64 max_size { get; }
	public UInt64 used_size { get; }
	public UInt64 num_objects { get; }

	public UInt64 rss { get; }
	public UInt64 hwm { get; }

	public UInt64 total_gets { get; }
	public UInt64 total_sets { get; }
	public UInt64 total_dels { get; }

	public double miss_ratio { get; }

	public string[] policies { get; }
	public string policy { get; }
	public bool is_auto_policy { get; }

	public UInt64 uptime { get; }

	public PaperStatus(
		UInt32 pid,

		UInt64 max_size,
		UInt64 used_size,
		UInt64 num_objects,

		UInt64 rss,
		UInt64 hwm,

		UInt64 total_gets,
		UInt64 total_sets,
		UInt64 total_dels,

		double miss_ratio,

		string[] policies,
		string policy,
		bool is_auto_policy,

		UInt64 uptime
	) {
		this.pid= pid;

		this.max_size = max_size;
		this.used_size = used_size;
		this.num_objects = num_objects;

		this.rss = rss;
		this.hwm = hwm;

		this.total_gets = total_gets;
		this.total_sets = total_sets;
		this.total_dels = total_dels;

		this.miss_ratio = miss_ratio;

		this.policies = policies;
		this.policy = policy;
		this.is_auto_policy = is_auto_policy;

		this.uptime = uptime;
	}
}
