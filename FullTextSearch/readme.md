**Full-Text Search (FTS)** là một kỹ thuật tìm kiếm được sử dụng để tìm kiếm các từ trong văn bản một cách hiệu quả. Trong PostgreSQL, FTS hỗ trợ tìm kiếm trên các cột văn bản mà không yêu cầu phải so khớp chính xác từ khóa, cho phép tìm kiếm văn bản với các tính năng mạnh mẽ như loại bỏ stop words, tìm kiếm gần đúng, và tìm kiếm dựa trên trọng số (ranking).
### Nguyên lý hoạt động của Full-Text Search:

- **Chuyển đổi văn bản thành tsvector**:
Trước khi tiến hành tìm kiếm, PostgreSQL sử dụng hàm `to_tsvector` để chuyển đổi văn bản thành một **vector từ** (tsvector). Tsvector chứa các **lexeme** (đơn vị từ chuẩn) đại diện cho các từ trong văn bản, giúp hệ thống nhận diện được các từ giống nhau về nghĩa, mặc dù chúng có hình thức khác nhau (như số nhiều hay biến thể động từ).
- **Loại bỏ stop words**:
Các **stop words** là những từ không mang nhiều thông tin (như "a", "the", "an",...) và thường bị loại bỏ trong quá trình chuẩn bị dữ liệu. Điều này giúp giảm khối lượng dữ liệu cần tìm kiếm và cải thiện hiệu suất.
- **Chuyển đổi các từ số nhiều**:
PostgreSQL sử dụng các **stemming algorithms** để nhận diện và chuyển các từ về **dạng gốc** (lemma). Ví dụ:
    - Các từ như "searches", "searching" sẽ được chuyển thành "search".
    - "ultimating", "ultimations", "ultimated" sẽ được chuẩn hóa thành "ultimate".
    
    Điều này có nghĩa là khi bạn tìm kiếm một từ như "search", PostgreSQL sẽ tự động khớp với các từ có dạng khác như "searches", "searched", hoặc "searching".
	### Cách thức loại bỏ số nhiều và chuẩn hóa từ trong PostgreSQL

PostgreSQL sử dụng các quy tắc chuẩn hóa (stemming) trong quá trình chuyển đổi văn bản sang `tsvector`. Các quy tắc này được thực hiện tự động khi bạn sử dụng các hàm như `to_tsvector`, và nó có thể áp dụng cho các tình huống sau:

1. **Chuyển từ số nhiều thành số ít**:
    - **Ví dụ**: Từ "dogs" sẽ được chuẩn hóa thành "dog".
    - **Quy tắc**: Các hậu tố số nhiều như **"-s", "-es", "-ies"** thường được loại bỏ, chuyển từ về dạng số ít.
2. **Chuẩn hóa động từ**:
    - **Ví dụ**: "running", "ran", "runs" sẽ được chuẩn hóa thành "run".
    - **Quy tắc**: Các dạng động từ khác nhau (như quá khứ, hiện tại, hoặc phân từ quá khứ) sẽ được chuyển về dạng gốc của động từ.
3. **Chuẩn hóa các từ biến thể (từ đồng nghĩa)**:
    - **Ví dụ**: "ultimating", "ultimations", "ultimated" sẽ được chuyển thành "ultimate".
    - **Quy tắc**: Các từ có các biến thể khác nhau (động từ, danh từ, tính từ) sẽ được nhận diện và chuẩn hóa.
	### Cách hoạt động của các hàm `phraseto_tsquery` và `websearch_to_tsquery`

1. **`phraseto_tsquery`**:
    - Đây là một hàm để chuyển đổi một cụm từ thành một `tsquery` mà yêu cầu các từ phải đứng gần nhau trong văn bản.
    - **Điều kiện**: Mặc dù các từ cần được tìm kiếm gần nhau, nhưng nó cũng có thể bỏ qua các stop words và các ký tự đặc biệt trong quá trình tìm kiếm.
    - **Thứ tự từ**: `phraseto_tsquery` yêu cầu các từ phải xuất hiện theo thứ tự trong văn bản và khớp chính xác với thứ tự này. Tuy nhiên, nếu có stop words xen vào giữa các từ quan trọng, chúng có thể ảnh hưởng đến kết quả tìm kiếm.
    - **Ví dụ**:Truy vấn trên tìm các khóa học có tên chứa các từ `spring`, `java`, `mvc`, `ultimation` gần nhau.
        
        ```sql
        SELECT * FROM "Courses" c
        WHERE to_tsvector('english', c."Name") @@ phraseto_tsquery('spring java mvc ultimation');
        ```
        
2. **`websearch_to_tsquery`**:
    - Đây là một hàm chuyển đổi một chuỗi tìm kiếm thành `tsquery` để tìm kiếm với **tính linh hoạt cao hơn** so với `phraseto_tsquery`.
    - **Điều kiện**: `websearch_to_tsquery` không yêu cầu các từ phải đứng gần nhau, và nó không quan tâm đến thứ tự của các từ trong chuỗi truy vấn. Nó thực hiện tìm kiếm rộng hơn và mềm dẻo hơn, bỏ qua các ký tự đặc biệt và không yêu cầu các từ phải xuất hiện theo đúng thứ tự.
    - **Ví dụ**:Truy vấn trên sẽ khớp với các từ `spring`, `mvc`, `ultimation` trong bất kỳ thứ tự nào.
        
        ```sql
        SELECT * FROM "Courses" c
        WHERE to_tsvector('english', c."Name") @@ websearch_to_tsquery('spring mvc ultimation')
        ```
        

### So sánh `phraseto_tsquery` và `websearch_to_tsquery`

| **Tiêu chí** | **`phraseto_tsquery`** | **`websearch_to_tsquery`** |
| --- | --- | --- |
| **Thứ tự của từ** | Yêu cầu các từ phải đứng gần nhau và theo thứ tự. | Không yêu cầu thứ tự của các từ, có thể khớp với từ bất kỳ nơi nào trong văn bản. |
| **Stop words** | Loại bỏ stop words, nhưng vẫn có thể ảnh hưởng nếu các từ quan trọng bị ngắt quãng bởi stop words. | Loại bỏ stop words nhưng không ảnh hưởng đến cấu trúc của câu. |
| **Khoảng cách giữa các từ** | Các từ phải nằm gần nhau. | Các từ có thể xuất hiện ở các vị trí khác nhau trong văn bản. |
| **Tính mềm dẻo** | Khó khăn khi có các ký tự đặc biệt hoặc stop words xen vào giữa các từ. | Tìm kiếm linh hoạt, bỏ qua ký tự đặc biệt và stop words. |
| **Ứng dụng** | Tìm kiếm chính xác với các cụm từ có thứ tự nhất định. | Tìm kiếm rộng rãi, phù hợp cho các truy vấn không yêu cầu chính xác thứ tự. |

### Ví dụ và kết quả của các truy vấn

1. **Truy vấn với `phraseto_tsquery`**:
    - Khi bạn chạy `phraseto_tsquery` với chuỗi tìm kiếm như `'spring a an ultimated'`, PostgreSQL sẽ tạo ra một `tsquery` với các từ `'spring'`, `'ultimated'` gần nhau và yêu cầu chúng phải xuất hiện theo thứ tự trong văn bản.
    - **Kết quả**: Nếu có stop words như `"a"`, `"an"`, chúng sẽ bị loại bỏ và có thể làm thay đổi thứ tự của các từ trong truy vấn, dẫn đến việc không khớp chính xác.
2. **Truy vấn với `websearch_to_tsquery`**:
    - Khi bạn sử dụng `websearch_to_tsquery`, các stop words và ký tự đặc biệt sẽ được loại bỏ mà không ảnh hưởng đến kết quả tìm kiếm. Truy vấn này sẽ tìm kiếm các từ trong văn bản mà không cần quan tâm đến thứ tự hoặc khoảng cách giữa chúng.
    - **Kết quả**: Truy vấn sẽ khớp với các từ trong bất kỳ thứ tự nào, vì vậy nếu bạn nhập `'Z tới A spring mvc ultimation'`, truy vấn vẫn sẽ khớp với tên khóa học có các từ này.

### Kết luận

- **`phraseto_tsquery`** thích hợp cho các tình huống tìm kiếm với các từ phải xuất hiện gần nhau và theo thứ tự trong văn bản, đồng thời hỗ trợ loại bỏ stop words.
- **`websearch_to_tsquery`** phù hợp hơn cho các tình huống cần tìm kiếm rộng rãi và linh hoạt, không quan tâm đến thứ tự của các từ hoặc khoảng cách giữa chúng.
### Code ví dụ

```sql
select * from "Courses" c
where to_tsvector('english', c."Name") @@ phraseto_tsquery('spring java mvc ultimation') -- khớp Spring Java MVC Ultimated từ Z tới A

-- phraseto_tsquery('spring a an ultimated') thì giữa spring, ultimated có 3 khoảng trống (cụ thể 'spring' <3> 'ultim')
-- nên sẽ khớp 'spring' <-> 'java' <-> 'mvc' <-> 'ultim' <-> 'từ' <-> 'z' <-> 'tới'
select * from "Courses" c
where to_tsvector('english', c."Name") @@ phraseto_tsquery('spring a an ultimated') -- khớp Spring Java MVC Ultimated từ Z tới A

-- vì phraseto_tsquery('spring a an từ') có kết quả là 'spring' <3> 'từ'
-- và select phraseto_tsquery('spring a an từ') có kết quả 'spring' <3> 'từ'
select * from "Courses" c
where to_tsvector('english', c."Name") @@ phraseto_tsquery('spring a an từ') -- khớp Java Spring REST Ultimate từ A tới Z

select * from "Courses" c
where to_tsvector('english', c."Name") @@ phraseto_tsquery('mvc ultimating') -- khớp Spring Java MVC Ultimated từ Z tới A

select * from "Courses" c
where to_tsvector('english', c."Name") @@ phraseto_tsquery('  ultimating') -- loại bỏ khoảng trống và vẫn khớp

select * from "Courses" c
where to_tsvector('english', c."Name") @@ phraseto_tsquery('and , .    ultimating từ ; Z ; tới  ;  A') -- loại bỏ and ; . , và vẫn khớp Spring Java MVC Ultimated từ Z tới A

select * from "Courses" c
where to_tsvector('english', c."Name") @@ phraseto_tsquery('and , .    ultimating từ ;    Z tới A') -- khớp Spring Java MVC Ultimated từ Z tới A

select * from "Courses" c
where to_tsvector('english', c."Name") @@ phraseto_tsquery('and , .    ultimating từ an ;    Z tới A') -- không khớp Spring Java MVC Ultimated từ Z tới A

-- Query 1: 'ultim' <-> 'từ' <-> 'z' <-> 'tới'     'ultim' <-> 'từ' <2> 'z' <-> 'tới'
-- Query 2: 'ultim' <-> 'từ' <-> 'z' <-> 'tới'     'ultim' <-> 'từ' <7> 'z' <-> 'tới'
-- Query 3: 'ultim' <-> 'từ' <-> 'z' <-> 'tới'     'ultim' <-> 'từ' <4> 'z' <-> 'tới'
select phraseto_tsquery('and , .    ultimating từ ;    Z tới A'), phraseto_tsquery('and , .    ultimating từ an ;    Z tới A');
select phraseto_tsquery('and , .    ultimating từ ; ; :   Z tới A'), phraseto_tsquery(', .    ultimating từ an an a a the a;    Z tới A');
select phraseto_tsquery('and , .    ultimating từ ; ; :   Z tới A'), phraseto_tsquery('the a an the or and , .    ultimating từ an the a;    Z tới A');

-- 2 câu query sau đều có cùng kết quả: 'ultim' & 'từ' & 'z' & 'tới'
select websearch_to_tsquery('and , .    ultimating từ ;    Z tới A'), websearch_to_tsquery('and , .    ultimating từ an ;    Z tới A');
select websearch_to_tsquery('and , .    ultimating từ ; ; :   Z tới A'), websearch_to_tsquery('and , .    ultimating từ an the a;    Z tới A');

-- websearch_to_tsquery tìm kiếm mềm mại hơn so với phraseto_tsquery vì nó không yêu cầu thứ tự
select * from "Courses" c
where to_tsvector('english', c."Name") @@ websearch_to_tsquery('and , .    ultimating từ and the or Z tới A') -- 

select *
from "Courses" c
where to_tsvector('english', c."Name") @@ websearch_to_tsquery('A      tới Z spring mvc ultimation') -- vẫn khớp Spring Java MVC Ultimated từ Z tới A

select *
from "Courses" c
where to_tsvector('english', c."Name") @@ websearch_to_tsquery('Z tới A spring mvc ultimation') -- khớp Spring Java MVC Ultimated từ Z tới A

select *
from "Courses" c
where to_tsvector('english', c."Name") @@ websearch_to_tsquery('từ Z spring mvc ultimation') -- vẫn khớp Spring Java MVC Ultimated từ Z tới A
```