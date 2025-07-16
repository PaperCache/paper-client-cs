namespace PaperClient;

public class PaperError : Exception {
	private Type type;

	public PaperError(Type type) {
		this.type = type;
	}

	public new Type GetType() {
		return this.type;
	}

	public static PaperError FromReader(SheetReader reader) {
		try {
			byte code = reader.ReadU8();

			if (code == 0) {
				byte cache_code = reader.ReadU8();
				return PaperError.FromCacheCode(cache_code);
			}

			return PaperError.FromCode(code);
		} catch {
			return new PaperError(Type.Internal);
		}
	}

	private static PaperError FromCode(byte code) {
		switch (code) {
			case 2: return new PaperError(Type.MaxConnectionsExceeded);
			case 3: return new PaperError(Type.Unauthorized);
			default: return new PaperError(Type.Internal);
		}
	}

	private static PaperError FromCacheCode(byte code) {
		switch (code) {
			case 1: return new PaperError(Type.KeyNotFound);

			case 2: return new PaperError(Type.ZeroValueSize);
			case 3: return new PaperError(Type.ExceedingValueSize);

			case 4: return new PaperError(Type.ZeroCacheSize);

			case 5: return new PaperError(Type.UnconfiguredPolicy);
			case 6: return new PaperError(Type.InvalidPolicy);

			default: return new PaperError(Type.Internal);
		}
	}

	public enum Type {
		Internal,

		InvalidAddress,

		ConnectionRefused,
		MaxConnectionsExceeded,
		Unauthorized,
		Disconnected,

		KeyNotFound,

		ZeroValueSize,
		ExceedingValueSize,

		ZeroCacheSize,

		UnconfiguredPolicy,
		InvalidPolicy,
	}
}
