using System.Net.Sockets;

namespace PaperClient;

public class PaperClient {
	private TcpClient tcp_client;

	public PaperClient(string host, int port) {
		try {
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

		var stream = this.tcp_client.GetStream();
		writer.Send(ref stream);

		var reader = new SheetReader(stream);

		var is_ok = reader.ReadBool();
		var data = reader.ReadString();

		return is_ok ?
			new PaperResponse<string>(data, null) :
			new PaperResponse<string>(null, data);
	}

	public PaperResponse<string> Version() {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Version);

		var stream = this.tcp_client.GetStream();
		writer.Send(ref stream);

		var reader = new SheetReader(stream);

		var is_ok = reader.ReadBool();
		var data = reader.ReadString();

		return is_ok ?
			new PaperResponse<string>(data, null) :
			new PaperResponse<string>(null, data);
	}

	public PaperResponse<string> Get(string key) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Get);
		writer.WriteString(key);

		var stream = this.tcp_client.GetStream();
		writer.Send(ref stream);

		var reader = new SheetReader(stream);

		var is_ok = reader.ReadBool();
		var data = reader.ReadString();

		return is_ok ?
			new PaperResponse<string>(data, null) :
			new PaperResponse<string>(null, data);
	}

	public PaperResponse<string> Set(string key, string value, UInt32 ttl = 0) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Set);
		writer.WriteString(key);
		writer.WriteString(value);
		writer.WriteU32(ttl);

		var stream = this.tcp_client.GetStream();
		writer.Send(ref stream);

		var reader = new SheetReader(stream);

		var is_ok = reader.ReadBool();
		var data = reader.ReadString();

		return is_ok ?
			new PaperResponse<string>(data, null) :
			new PaperResponse<string>(null, data);
	}

	public PaperResponse<string> Del(string key) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Del);
		writer.WriteString(key);

		var stream = this.tcp_client.GetStream();
		writer.Send(ref stream);

		var reader = new SheetReader(stream);

		var is_ok = reader.ReadBool();
		var data = reader.ReadString();

		return is_ok ?
			new PaperResponse<string>(data, null) :
			new PaperResponse<string>(null, data);
	}

	public PaperResponse<bool?> Has(string key) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Has);
		writer.WriteString(key);

		var stream = this.tcp_client.GetStream();
		writer.Send(ref stream);

		var reader = new SheetReader(stream);

		var is_ok = reader.ReadBool();

		return is_ok ?
			new PaperResponse<bool?>(reader.ReadBool(), null) :
			new PaperResponse<bool?>(null, reader.ReadString());
	}

	public PaperResponse<string> Peek(string key) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Peek);
		writer.WriteString(key);

		var stream = this.tcp_client.GetStream();
		writer.Send(ref stream);

		var reader = new SheetReader(stream);

		var is_ok = reader.ReadBool();
		var data = reader.ReadString();

		return is_ok ?
			new PaperResponse<string>(data, null) :
			new PaperResponse<string>(null, data);
	}

	public PaperResponse<string> Ttl(string key, UInt32 ttl = 0) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Ttl);
		writer.WriteString(key);
		writer.WriteU32(ttl);

		var stream = this.tcp_client.GetStream();
		writer.Send(ref stream);

		var reader = new SheetReader(stream);

		var is_ok = reader.ReadBool();
		var data = reader.ReadString();

		return is_ok ?
			new PaperResponse<string>(data, null) :
			new PaperResponse<string>(null, data);
	}

	public PaperResponse<UInt64?> Size(string key) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Size);
		writer.WriteString(key);

		var stream = this.tcp_client.GetStream();
		writer.Send(ref stream);

		var reader = new SheetReader(stream);

		var is_ok = reader.ReadBool();

		return is_ok ?
			new PaperResponse<UInt64?>(reader.ReadU64(), null) :
			new PaperResponse<UInt64?>(null, reader.ReadString());
	}

	public PaperResponse<string> Wipe() {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Wipe);

		var stream = this.tcp_client.GetStream();
		writer.Send(ref stream);

		var reader = new SheetReader(stream);

		var is_ok = reader.ReadBool();
		var data = reader.ReadString();

		return is_ok ?
			new PaperResponse<string>(data, null) :
			new PaperResponse<string>(null, data);
	}

	public PaperResponse<string> Resize(UInt64 size = 0) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Resize);
		writer.WriteU64(size);

		var stream = this.tcp_client.GetStream();
		writer.Send(ref stream);

		var reader = new SheetReader(stream);

		var is_ok = reader.ReadBool();
		var data = reader.ReadString();

		return is_ok ?
			new PaperResponse<string>(data, null) :
			new PaperResponse<string>(null, data);
	}

	public PaperResponse<string> Policy(PaperPolicy policy) {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Policy);
		writer.WriteU64((byte)policy);

		var stream = this.tcp_client.GetStream();
		writer.Send(ref stream);

		var reader = new SheetReader(stream);

		var is_ok = reader.ReadBool();
		var data = reader.ReadString();

		return is_ok ?
			new PaperResponse<string>(data, null) :
			new PaperResponse<string>(null, data);
	}

	public PaperResponse<PaperStats?> Stats() {
		var writer = new SheetWriter();
		writer.WriteU8((byte)CommandByte.Stats);

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
	}
}

enum CommandByte : byte {
	Ping = 0,
	Version = 1,

	Get = 2,
	Set = 3,
	Del = 4,

	Has = 5,
	Peek = 6,
	Ttl = 7,
	Size = 8,

	Wipe = 9,

	Resize = 10,
	Policy = 11,

	Stats = 12,
}
