using System.Net.Sockets;
using System.Text;

namespace PaperClient;

public class SheetWriter {
	private List<byte> buf;

	public SheetWriter() {
		this.buf = new List<byte>();
	}

	public void WriteU8(byte value) {
		this.buf.Add(value);
	}

	public void WriteU32(UInt32 value) {
		byte[] buf = BitConverter.GetBytes(value);

		if (!BitConverter.IsLittleEndian) {
			Array.Reverse(buf);
		}

		this.buf.AddRange(buf);
	}

	public void WriteU64(UInt64 value) {
		byte[] buf = BitConverter.GetBytes(value);

		if (!BitConverter.IsLittleEndian) {
			Array.Reverse(buf);
		}

		this.buf.AddRange(buf);
	}

	public void WriteString(string value) {
		this.WriteU32((UInt32)value.Length);
		byte[] buf = Encoding.UTF8.GetBytes(value);

		if (!BitConverter.IsLittleEndian) {
			Array.Reverse(buf);
		}

		this.buf.AddRange(buf);
	}

	public void Send(ref NetworkStream tcp_stream) {
		var data = this.buf.ToArray();
		tcp_stream.Write(data, 0, data.Length);
	}
}
