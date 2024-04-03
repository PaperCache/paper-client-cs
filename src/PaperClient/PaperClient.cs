using System.Net.Sockets;

namespace PaperClient;

public class PaperClient {
	private const int MAX_RECONNECT_ATTEMPTS = 3;

	private string host;
	private int port;

	private string auth_token;
	private int reconnect_attempts;

	private TcpClient tcp_client;

	public PaperClient(string host, int port) {
		try {
			this.host = host;
			this.port = port;

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

	public PaperResponse<string> Ping() {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Ping);

		return this.Process(writer);
	}

	public PaperResponse<string> Version() {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Version);

		return this.Process(writer);
	}

	public PaperResponse<string> Auth(string token) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Auth);
		writer.WriteString(token);

		this.auth_token = token;

		return this.Process(writer);
	}

	public PaperResponse<string> Get(string key) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Get);
		writer.WriteString(key);

		return this.Process(writer);
	}

	public PaperResponse<string> Set(string key, string value, UInt32 ttl = 0) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Set);
		writer.WriteString(key);
		writer.WriteString(value);
		writer.WriteU32(ttl);

		return this.Process(writer);
	}

	public PaperResponse<string> Del(string key) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Del);
		writer.WriteString(key);

		return this.Process(writer);
	}

	public PaperResponse<bool?> Has(string key) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Has);
		writer.WriteString(key);

		return this.ProcessHas(writer);
	}

	public PaperResponse<string> Peek(string key) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Peek);
		writer.WriteString(key);

		return this.Process(writer);
	}

	public PaperResponse<string> Ttl(string key, UInt32 ttl = 0) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Ttl);
		writer.WriteString(key);
		writer.WriteU32(ttl);

		return this.Process(writer);
	}

	public PaperResponse<UInt64?> Size(string key) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Size);
		writer.WriteString(key);

		return this.ProcessSize(writer);
	}

	public PaperResponse<string> Wipe() {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Wipe);

		return this.Process(writer);
	}

	public PaperResponse<string> Resize(UInt64 size = 0) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Resize);
		writer.WriteU64(size);

		return this.Process(writer);
	}

	public PaperResponse<string> Policy(PaperPolicy policy) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Policy);
		writer.WriteU8((byte)policy);

		return this.Process(writer);
	}

	public PaperResponse<PaperStats?> Stats() {
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

	private PaperResponse<string> Process(SheetWriter writer) {
		try {
			var stream = this.tcp_client.GetStream();
			writer.Send(ref stream);

			var reader = new SheetReader(stream);

			var is_ok = reader.ReadBool();
			var data = reader.ReadString();

			return is_ok ?
				new PaperResponse<string>(data, null) :
				new PaperResponse<string>(null, data);
		} catch {
			this.Reconnect();
			return this.Process(writer);
		}
	}

	private PaperResponse<bool?> ProcessHas(SheetWriter writer) {
		try {
			var stream = this.tcp_client.GetStream();
			writer.Send(ref stream);

			var reader = new SheetReader(stream);

			var is_ok = reader.ReadBool();

			return is_ok ?
				new PaperResponse<bool?>(reader.ReadBool(), null) :
				new PaperResponse<bool?>(null, reader.ReadString());
		} catch {
			this.Reconnect();
			return this.ProcessHas(writer);
		}
	}

	private PaperResponse<UInt64?> ProcessSize(SheetWriter writer) {
		try {
			var stream = this.tcp_client.GetStream();
			writer.Send(ref stream);

			var reader = new SheetReader(stream);

			var is_ok = reader.ReadBool();

			return is_ok ?
				new PaperResponse<UInt64?>(reader.ReadU64(), null) :
				new PaperResponse<UInt64?>(null, reader.ReadString());
		} catch {
			this.Reconnect();
			return this.ProcessSize(writer);
		}
	}

	private PaperResponse<PaperStats?> ProcessStats(SheetWriter writer) {
		try {
			var stream = this.tcp_client.GetStream();
			writer.Send(ref stream);

			var reader = new SheetReader(stream);

			var is_ok = reader.ReadBool();

			if (!is_ok) {
				return new PaperResponse<PaperStats?>(null, reader.ReadString());
			}

			var max_size = reader.ReadU64();
			var used_size = reader.ReadU64();

			var total_gets = reader.ReadU64();
			var total_sets = reader.ReadU64();
			var total_dels = reader.ReadU64();

			var miss_ratio = reader.ReadF64();

			var policy_byte = reader.ReadU8();
			var uptime = reader.ReadU64();

			var stats = new PaperStats(
				max_size,
				used_size,

				total_gets,
				total_sets,
				total_dels,

				miss_ratio,

				policy_byte,
				uptime
			);

			return new PaperResponse<PaperStats?>(stats, null);
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
