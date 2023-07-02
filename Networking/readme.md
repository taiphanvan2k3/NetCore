# Kiến thức về Networking
- Lệnh curl -i url trên terminal
- vd curl -i https://xuanthulab.net/contact/ thì sẽ nhận về được nội dung respone bao gồm cả respone header và respone body 
- Học về URI,DNS, Ping, HttpClient, HttpRequestMessage, HttpResponeMessage

# Cách để ghi nội dung của mảng byte[] ra file
```
byte[] bytes = await ....();
string fileName = "1.png";

// Ghi nội dung bytes ra file
using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
{
    // Dùng FileMode.CreateNew nếu tên file đã tồn tại và muốn tạo file mới
    stream.Write(bytes, 0, bytes.Length);
}
```

# Cách để ghi nội dung Stream ra file
```
var httpResponseMessage = await httpClient.GetAsync(url);

using var stream = await httpResponseMessage.Content.ReadAsStreamAsync();

// Có thể đọc từng byte/ khối byte từ stream này
int SIZEBUFFER = 500;

// Tạo ra vùng đệm gồm 500 bytes
var buffer = new byte[SIZEBUFFER];

using var streamWrite = File.OpenWrite(fileName);
bool eof = false;
do
{
    int numBytes = await stream.ReadAsync(buffer, 0, SIZEBUFFER);
    if (numBytes == 0)
    {
        eof = true;
    }
    else
    {
        await streamWrite.WriteAsync(buffer, 0, numBytes);
    }
} while (!eof);
```