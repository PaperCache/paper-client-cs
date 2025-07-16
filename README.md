# paper-client-cs

The C# [PaperCache](https://papercache.io) client. The client supports all commands described in the wire protocol on the homepage.

## Example
```cs
using PaperClient;

var client = new PaperClient("paper://127.0.0.1:3145");

client.Set("hello", "world");
var got = client.Get("hello");
```
