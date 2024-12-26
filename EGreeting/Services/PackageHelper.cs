using System.Text.RegularExpressions;

namespace EGreeting.Services
{
    public static class PackageHelper
    {
        public static DateTime ToDate(this string Duration)
        {
            if (string.IsNullOrWhiteSpace(Duration))
            {
                return DateTime.Now;
            }
            // Xử lý chuỗi Duration 
            var match = System.Text.RegularExpressions.Regex.Match(Duration, @"(\d+)\s*(ngày|tháng|năm)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                int number = int.Parse(match.Groups[1].Value); // Số lượng (ví dụ: 1 trong "1 ngày")
                string unit = match.Groups[2].Value.ToLower(); // Đơn vị (ngày, tháng, năm)

                // Tính toán ngày dựa trên đơn vị
                switch (unit)
                {
                    case "day":
                        return DateTime.Now.AddDays(number);
                    case "month":
                        return DateTime.Now.AddMonths(number);
                    case "year":
                        return DateTime.Now.AddYears(number);
                }
            }

            return DateTime.Now;

        }
    }
}
