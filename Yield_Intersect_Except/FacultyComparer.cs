using System.Diagnostics.CodeAnalysis;

namespace Yield_Intersect_Except
{
    public class FacultyComparer : IEqualityComparer<Faculty>
    {
        public bool Equals(Faculty x, Faculty y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            // Nếu 2 đối tượng cùng tham chiếu đến 1 vùng nhớ
            if (x == y)
            {
                return true;
            }

            return x.Code == y.Code;
        }

        public int GetHashCode(Faculty obj)
        {
            if (obj == null)
            {
                return 0;
            }

            return obj.Id.GetHashCode();
        }
    }
}