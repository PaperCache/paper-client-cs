using System.Net.Sockets;

namespace PaperClient;

public class PaperClient {
	private const int MAX_RECONNECT_ATTEMPTS = 3;

	private string host;
	private int port;

	private string auth_token;
	private int reconnect_attempts;

	private TcpClient tcp_client;

	public PaperClient(string paper_addr) {
		try {
			if (!paper_addr.StartsWith("paper://")) {
				throw new ArgumentException("Invalid Paper address");
			}

			var parsed = paper_addr
				.Substring("paper://".Length)
				.Split(':');

			if (parsed.Length != 2) {
				throw new ArgumentException("Invalid Paper address");
			}

			this.host = parsed[0];
			this.port = int.Parse(parsed[1]);

			this.auth_token = "";
			this.reconnect_attempts = 0;

			this.tcp_client = new TcpClient(host, port);
		} catch {
			throw new ArgumentException("Connection refused");
		}
	}

	public void Disconnect() {
		this.tcp_client.Close();
	}

	public string Ping() {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Ping);

		return this.ProcessData(writer);
	}

	public string Version() {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Version);

		return this.ProcessData(writer);
	}

	public void Auth(string token) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Auth);
		writer.WriteString(token);

		this.auth_token = token;

		this.Process(writer);
	}

	public string Get(string key) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Get);
		writer.WriteString(key);

		return this.ProcessData(writer);
	}

	public void Set(string key, string value, UInt32 ttl = 0) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Set);
		writer.WriteString(key);
		writer.WriteString(value);
		writer.WriteU32(ttl);

		this.Process(writer);
	}

	public void Del(string key) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Del);
		writer.WriteString(key);

		this.Process(writer);
	}

	public bool Has(string key) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Has);
		writer.WriteString(key);

		return this.ProcessHas(writer);
	}

	public string Peek(string key) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Peek);
		writer.WriteString(key);

		return this.ProcessData(writer);
	}

	public void Ttl(string key, UInt32 ttl = 0) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Ttl);
		writer.WriteString(key);
		writer.WriteU32(ttl);

		this.Process(writer);
	}

	public UInt32 Size(string key) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Size);
		writer.WriteString(key);

		return this.ProcessSize(writer);
	}

	public void Wipe() {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Wipe);

		this.Process(writer);
	}

	public void Resize(UInt64 size = 0) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Resize);
		writer.WriteU64(size);

		this.Process(writer);
	}

	public void Policy(string policy) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Policy);
		writer.WriteString(policy);

		this.Process(writer);
	}

	public PaperStats Stats() {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Stats);

		return this.ProcessStats(writer);
	}

	private bool Reconnect() {
		if (this.reconnect_attempts > MAX_RECONNECT_ATTEMPTS) {
			return false;
		}

		try {
			this.tcp_client = new TcpClient(this.host, this.port);

			if (this.auth_token.Length > 0) {
				this.Auth(this.auth_token);
			}

			return true;
		} catch {
			return false;
		}
	}

	private void Process(SheetWriter writer) {
		try {
			var stream = this.tcp_client.GetStream();
			writer.Send(ref stream);

			var reader = new SheetReader(stream);

			var is_ok = reader.ReadBool();
			if (!is_ok) throw PaperError.FromReader(reader);

			this.reconnect_attempts = 0;
		} catch {
			if (!this.Reconnect()) {
				throw new PaperError(PaperError.Type.Disconnected);
			}

			this.Process(writer);
		}
	}

	private string ProcessData(SheetWriter writer) {
		try {
			var stream = this.tcp_client.GetStream();
			writer.Send(ref stream);

			var reader = new SheetReader(stream);

			var is_ok = reader.ReadBool();
			if (!is_ok) throw PaperError.FromReader(reader);

			this.reconnect_attempts = 0;
			return reader.ReadString();
		} catch {
			if (!this.Reconnect()) {
				throw new PaperError(PaperError.Type.Disconnected);
			}

			return this.ProcessData(writer);
		}
	}

	private bool ProcessHas(SheetWriter writer) {
		try {
			var stream = this.tcp_client.GetStream();
			writer.Send(ref stream);

			var reader = new SheetReader(stream);

			var is_ok = reader.ReadBool();
			if (!is_ok) throw PaperError.FromReader(reader);

			this.reconnect_attempts = 0;
			return reader.ReadBool();
		} catch {
			if (!this.Reconnect()) {
				throw new PaperError(PaperError.Type.Disconnected);
			}

			return this.ProcessHas(writer);
		}
	}

	private UInt32 ProcessSize(SheetWriter writer) {
		try {
			var stream = this.tcp_client.GetStream();
			writer.Send(ref stream);

			var reader = new SheetReader(stream);

			var is_ok = reader.ReadBool();
			if (!is_ok) throw PaperError.FromReader(reader);

			this.reconnect_attempts = 0;
			return reader.ReadU32();
		} catch {
			if (!this.Reconnect()) {
				throw new PaperError(PaperError.Type.Disconnected);
			}

			return this.ProcessSize(writer);
		}
	}

	private PaperStats ProcessStats(SheetWriter writer) {
		try {
			var stream = this.tcp_client.GetStream();
			writer.Send(ref stream);

			var reader = new SheetReader(stream);

			var is_ok = reader.ReadBool();
			if (!is_ok) throw PaperError.FromReader(reader);

			this.reconnect_attempts = 0;

			var pid = reader.ReadU32();

			var max_size = reader.ReadU64();
			var used_size = reader.ReadU64();
			var num_objects = reader.ReadU64();

			var rss = reader.ReadU64();
			var hwm = reader.ReadU64();

			var total_gets = reader.ReadU64();
			var total_sets = reader.ReadU64();
			var total_dels = reader.ReadU64();

			var miss_ratio = reader.ReadF64();

			var num_policies = reader.ReadU32();
			var policies = new String[num_policies];

			for (int i=0; i<num_policies; i++) {
				policies[i] = reader.ReadString();
			}

			var policy = reader.ReadString();
			var is_auto_policy = reader.ReadBool();

			var uptime = reader.ReadU64();

			return new PaperStats(
				pid,

				max_size,
				used_size,
				num_objects,

				rss,
				hwm,

				total_gets,
				total_sets,
				total_dels,

				miss_ratio,

				policies,
				policy,
				is_auto_policy,

				uptime
			);
		} catch {
			this.Reconnect();
			return this.ProcessStats(writer);
		}
	}
}

enum CommandByte : byte {
	Ping = 0,
	Version = 1,

	Auth = 2,

	Get = 3,
	Set = 4,
	Del = 5,

	Has = 6,
	Peek = 7,
	Ttl = 8,
	Size = 9,

	Wipe = 10,

	Resize = 11,
	Policy = 12,

	Stats = 13,
}
