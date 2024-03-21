namespace PaperClient;

public class PaperResponse<T> {
	private T? data;
	private string? err_data;

	public PaperResponse(T? data, string? err_data) {
		this.data = data;
		this.err_data = err_data;
	}

	public bool IsOk() {
		return this.data != null;
	}

	public T? Data() {
		return this.data;
	}

	public string? ErrData() {
		return this.err_data;
	}
}
