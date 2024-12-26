using EGreeting.Data;
using EGreeting.Models;
using EGreeting.Models.EmailListViewModels;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Security.Cryptography;
using System.Text;
using Path = System.IO.Path;

namespace EGreeting.Services
{
    public class Common : ICommon
    {
        private readonly IWebHostEnvironment _iHostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public Common(IWebHostEnvironment iHostingEnvironment, ApplicationDbContext context, IConfiguration configuration, HttpClient httpClient)
        {
            _iHostingEnvironment = iHostingEnvironment;
            _context = context;
            _configuration = configuration;
            _httpClient = httpClient;
        }


        public string GetContentRootPath()
        {
            return _iHostingEnvironment.ContentRootPath;
        }
        public string GetFolderUploadPath()
        {
            return Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload");
        }


        public string GetSHA256(string str)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
        public string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }


        public string GenerateRandomString(int length)
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = characters[random.Next(characters.Length)];
            }

            return new string(result);
        }
        public async Task<string> UploadedFile(IFormFile ProfilePicture)
        {
            string ProfilePictureFileName = null;

            if (ProfilePicture != null)
            {
                string fileExtension = Path.GetExtension(ProfilePicture.FileName).ToLower();

                if (fileExtension != ".png" && fileExtension != ".jpg" && fileExtension != ".jpeg")
                {
                    return "Chỉ cho phép tải lên tệp .png, .jpg hoặc .jpeg";
                }

                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/Upload");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }


                string guidString = Guid.NewGuid().ToString("N");
                int maxGuidLength = 20 - fileExtension.Length;
                ProfilePictureFileName = guidString.Substring(0, Math.Min(maxGuidLength, guidString.Length)) + fileExtension;

                string filePath = Path.Combine(uploadsFolder, ProfilePictureFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfilePicture.CopyToAsync(fileStream);
                }
            }

            return ProfilePictureFileName;
        }

        public async Task<List<CategoriesCard>> MenuNav()
        {
            return await _context.CategoriesCard.ToListAsync();
        }
        public async Task SendEmail(Cards request, EmailListCRUD model)
        {
            // Khởi tạo message và body
            var messageToUser = new MimeMessage();
            messageToUser.From.Add(new MailboxAddress(request.NameCard, _configuration.GetSection("EmailUserName").Value));

            foreach (var email in model.EmailList)
            {
                messageToUser.To.Add(MailboxAddress.Parse(email));
            }

            messageToUser.Subject = model.EmailSubject;


            var filePath = _configuration.GetSection("BaseURL").Value + request.ImageCardPreview;
            var builder = new BodyBuilder();
            var emailTemplatePathUser = Path.Combine(_iHostingEnvironment.WebRootPath, "EmailImage.html");
            var emailTemplateUser = File.ReadAllText(emailTemplatePathUser);

            emailTemplateUser = emailTemplateUser.Replace("{linkImage}", filePath ?? "");
            emailTemplateUser = emailTemplateUser.Replace("{alt}", request.NameCard ?? "");

            builder.HtmlBody = emailTemplateUser;
            messageToUser.Body = builder.ToMessageBody();

            // Gửi email qua SMTP server
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                // Kết nối tới SMTP server
                client.Connect(_configuration.GetSection("EmailHost").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate(_configuration.GetSection("EmailUserName").Value, _configuration.GetSection("EmailPassword").Value);

                // Gửi email
                await client.SendAsync(messageToUser);

                // Đóng kết nối
                client.Disconnect(true);
            }
        }

    }


}