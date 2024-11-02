using ElectronicShop.Model;
using ElectronicShop.Model.Domain;
using ElectronicShop.Resource;
using ElectronicShop.Services.Interfaces;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace ElectronicShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly string bucketName;
        private readonly IEmailService _emailService;

        public UploadController(
            UserManager<Users> userManager,
            RoleManager<Role> roleManager,
            SignInManager<Users> signInManager,
            IAPIServices apiServices,
            IConfiguration configuration,
            IEmailService emailService) : base(userManager, roleManager, signInManager, apiServices)
        {
            _configuration = configuration;
            _emailService = emailService;

            var relativePath = "Resources/serviceAccountKey.json";
            var absolutePath = Path.Combine(AppContext.BaseDirectory, relativePath);
            var json = JObject.Parse(System.IO.File.ReadAllText(absolutePath));
            bucketName = $"{json["project_id"]?.ToString()}.appspot.com";
        }

        [HttpPost("upload-imagae")]
        public async Task<IActionResult> UploadImage()
        {
            IFormFile? file = Request.Form.Files.FirstOrDefault();
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            // Kiểm tra dung lượng file (giới hạn 5 MB)
            var maxFileSize = 5 * 1024 * 1024; // 5 MB
            if (file.Length > maxFileSize)
            {
                return BadRequest("Hinh ảnh phải có dung lượng nhỏ hơn 5MB. Xin vui lòng thử lại!");
            }

            // Kiểm tra chỉ nhận file là ảnh
            var supportedTypes = new[]
            {
                "image/jpeg"
               ,"image/png"
               ,"image/heic"
               ,"image/webp", // Định dạng ảnh WebP
            };

            if (!supportedTypes.Contains(file.ContentType.ToLower()))
            {
                return BadRequest("Hình ảnh không hợp lệ! Hình ảnh phải là những file: JPEG/JPG, PNG, HEIC, webp. Xin vui lòng thử lại!");
            }

            try
            {
                var storage = StorageClient.Create();
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                stream.Position = 0;

                // Upload vào folder 'images'
                var objectName = $"image/{Guid.NewGuid()}_{file.FileName}";

                // Upload file lên Firebase Storage
                var storageObject = await storage.UploadObjectAsync(bucketName, objectName, null, stream, new UploadObjectOptions
                {
                    PredefinedAcl = PredefinedObjectAcl.PublicRead
                });

                // Cập nhật metadata sau khi upload
                storageObject.ContentType = file.ContentType;
                await storage.UpdateObjectAsync(storageObject);

                var imageUrl = $"https://storage.googleapis.com/{bucketName}/{objectName}";
                return Ok(new
                {
                    Url = imageUrl
                   ,
                    Server = $"https://storage.googleapis.com/{bucketName}"
                   ,
                    Slug = objectName
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("send-mail-contact")]
        public async Task<IActionResult> SendEmail(EmailRequestReq req)
        {
            try
            {
                var ToEmail = _configuration["SendMail:ToEmail"] ?? "namthanh2082001@gmail.com";
                var Subject = _configuration["SendMail:Subject"] ?? "";
                var Message = _configuration["SendMail:Message"] ?? "";

                var emailRequest = new EmailRequest();
                emailRequest.ToEmail = ToEmail;
                emailRequest.Subject = Subject;
                emailRequest.Message = Message.Replace("{Name}", req.Name)
                                              .Replace("{Phone}", req.Phone)
                                              .Replace("{Email}", req.Email ?? "-")
                                              .Replace("{Note}", req.Note ?? "-")
                                              .Replace("{Date}", DateTime.Now.ToString("hh:mm dd/MM/yyyy"));

                await _emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Message);
                return Ok(new BaseResponse(null, ErrorCode.Success, "Cảm ơn Quý Khách đã điền thông tin. Chúng tôi sẽ liên hệ đến Quý khách sớm nhất!"));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponse(null, ErrorCode.Error, "Hệ thống gửi email đang bảo trì. Liên hệ số điện thoại 0911 315 315 để được liên lạc sớm nhất!"));
            }
        }

        public class EmailRequestReq
        {
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public string Note { get; set; } = string.Empty;
        }

        public class EmailRequest
        {
            public string ToEmail { get; set; } = string.Empty;
            public string Subject { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
        }
    }
}