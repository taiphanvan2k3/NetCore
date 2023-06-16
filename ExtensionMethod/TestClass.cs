namespace App
{
    public class TestClass
    {
        public readonly string Name;
        public TestClass(string name)
        {
            // Trường giá trị chỉ đọc không được phép gán giá trị
            this.Name = name;
        }
    }

    public class Vector
    {
        double x;
        double y;
        public Vector(double _x, double _y)
        {
            x = _x;
            y = _y;
        }

        public void ShowInfo()
        {
            System.Console.WriteLine($"x = {x}, y = {y}");
        }

        // Định nghĩa phép toán + giữa 2 vector thì phải chỉ định phương thức này
        // là static
        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector operator +(Vector v, int x)
        {
            return new Vector(v.x + x, v.y + x);
        }

        // Indexer [chiso]
        public double this[int i]
        {
            set
            {
                if (i == 0)
                    x = value;
                else if (i == 1)
                    y = value;
                else
                    throw new Exception("Chỉ số sai");
            }

            get
            {
                if (i == 0)
                    return x;
                else if (i == 1)
                    return y;
                throw new Exception("Chỉ số sai");
            }
        }

        public double this[string i]
        {
            set
            {
                if (i == "x")
                    x = value;
                else if (i == "y")
                    y = value;
                else
                    throw new Exception("Chỉ số sai");
            }

            get
            {
                if (i == "x")
                    return x;
                else if (i == "y")
                    return y;
                throw new Exception("Chỉ số sai");
            }
        }
    }
}