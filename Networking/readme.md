# Kiến thức về Networking
- Lệnh curl -i url trên terminal
- vd curl -i https://xuanthulab.net/contact/ thì sẽ nhận về được nội dung respone bao gồm cả respone header và respone body 
- Học về URI, HttpClient, HttpRequestMessage, HttpResponeMessage

# Cách để ghi nội dung Stream ra file
```
var myStream = await ....();
string fileName = "1.png";
// Ghi nội dung bytes ra file
using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
{
    // Dùng FileMode.CreateNew nếu tên file đã tồn tại và muốn tạo file mới
    stream.Write(bytes, 0, bytes.Length);
}
```