# Extension method
Extension method mở rộng chức năng của thư viện, lớp mà không nhất thiết phải tạo những lớp mới kế thừa từ lớp có sẵn
- Để làm được tạo được các phương thức mở rộng thì các phương thức đó phải là phương thức tĩnh và được đặt trong các lớp tĩnh
- Muốn mở rộng phương thức cho lớp nào thì tham số đầu tiên phương thức phải là đối tượng của lớp đó và có từ khóa this

# Readonly Property
- Trường giá trị chỉ đọc không được phép gán giá trị cho nó, nhưng trong
constructor thì có thể làm điều này.

# Nạp chồng toán tử +,-,..
- Phương thức phải xác định kiểu dữ liệu trả về phù hợp và đặc biệt rằng phương thức này phải là phương thức static
```
  public static Vector operator +(Vector v1, Vector v2)
  {
      return new Vector(v1.x + v2.x, v1.y + v2.y);
  }
```
# Indexer
Ví dụ:
```
public double this[int i]
{
    get{ }

    set{ }
}

public double this[string i]
{
    get{ }

    set{ }
}
```