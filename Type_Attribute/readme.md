# Regex cho Sđt ở VN
- Nếu dùng attribute ```[Phone]``` thì với sđt input là 0905... vẫn thỏa mãn -> Không phù hợp
- => Phải dùng:
```[RegularExpression(@"^(?!0+$)((\+\d{1,2}[- ]?)|0)[35789](?!0+$)\d{8}$", ErrorMessage = "So dien thoai khong hop le")]```