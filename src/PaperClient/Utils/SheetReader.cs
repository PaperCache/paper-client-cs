using System.Net.Sockets;
using System.Text;

namespace PaperClient;

public class SheetReader {
	private NetworkStream tcp_stream;

	public SheetReader(NetworkStream tcp_stream) {
		this.tcp_stream = tcp_stream;
	}

	public byte ReadU8() {
		return (byte)this.tcp_stream.ReadByte();
	}

	public bool ReadBool() {
		return this.ReadU8() == 33;
	}

	public UInt32 ReadU32() {
		byte[] buf = new byte[4];
		this.tcp_stream.Read(buf, 0, buf.Length);

		if (!BitConverter.IsLittleEndian) {
			Array.Reverse(buf);
		}

		return BitConverter.ToUInt32(buf);
	}

	public UInt64 ReadU64() {
		byte[] buf = new byte[8];
		this.tcp_stream.Read(buf, 0, buf.Length);

		if (!BitConverter.IsLittleEndian) {
			Array.Reverse(buf);
		}

		return BitConverter.ToUInt64(buf);
	}

	public double ReadF64() {
		byte[] buf = new byte[8];
		this.tcp_stream.Read(buf, 0, buf.Length);

		if (!BitConverter.IsLittleEndian) {
			Array.Reverse(buf);
		}

		return BitConverter.ToDouble(buf);
	}

	public string ReadString() {
		var size = this.ReadU32();

		byte[] buf = new byte[size];
		this.tcp_stream.Read(buf, 0, buf.Length);

		return Encoding.UTF8.GetString(buf);
	}
}
