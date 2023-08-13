namespace Yield_Intersect_Except
{
    public class Intersect_Except_Example
    {
        public static void Example()
        {
            List<Faculty> local = new()
            {
                new Faculty()
                {
                    Code = "102",
                    Name = "CNTT"
                },
                new Faculty()
                {
                    Code = "103",
                    Name = "DTVT"
                }
            };

            List<Faculty> iam = new()
            {
                new Faculty()
                {
                    Code = "102",
                    Name = "Công nghệ thông tin"
                },
                new Faculty()
                {
                    Code = "103",
                    Name = "Điện tử viễn thông"
                },
                new Faculty()
                {
                    Code = "104",
                    Name = "Cơ khí"
                },
                new Faculty()
                {
                    Code = "105",
                    Name = "Xây dựng"
                }
            };

            // Các thao tác Intersect, Except được xây dựng có kết hợp bảng băm HashSet nên thao tác tốn O(n)
            var deletedFaculty = local.Except(iam, new FacultyComparer()).ToList();
            var insertedFaculty = iam.Except(local, new FacultyComparer()).ToList();

            // Lấy ra các khoa sẽ có dữ liệu thay đổi và dùng iam.Intersect chứ không dùng local.Intersect
            // vì muốn lấy dữ liệu mới từ IAM
            var updatedFaculty = iam.Intersect(local, new FacultyComparer()).ToList();

            // Dùng bảng băm để tìm kiếm cho nhanh, cập nhật Name cho các khoa của local trong O(n)
            Dictionary<string, string> dict = updatedFaculty.ToDictionary(f => f.Code, f => f.Name);
            foreach (Faculty faculty in local)
            {
                if (dict.ContainsKey(faculty.Code))
                {
                    faculty.Name = dict[faculty.Code];
                }
            }
        }
    }
}