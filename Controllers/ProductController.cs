using System.IO;
using JwtUser.Helper;
using JwtUser.Repos;
using JwtUser.Repos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtUser.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly LearnDataContext _context;

        public ProductController(LearnDataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPut("UploadProduct")]
        public async Task<IActionResult> UploadImage(IFormFile file, string productCode)
        {
            APIResponse<string> response = new APIResponse<string>();
            string FilePath = GetFilePath(productCode);
            try
            {
                if (!System.IO.Directory.Exists(FilePath))
                {
                    System.IO.Directory.CreateDirectory(FilePath);
                }

                string imagePath = FilePath + "\\" + productCode + ".png";
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                using (FileStream stream = System.IO.File.Create(imagePath))
                {
                    await file.CopyToAsync(stream);
                    response.Message = "Success";
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.Message = ex.Message;
                return Ok(response);
            }
        }

        [HttpPut("UploadMultipleProduct")]
        public async Task<IActionResult> UploadMultipleImage(IFormFileCollection fileCollection, string productCode)
        {
            int passcount = 0;
            int errorcount = 0;
            APIResponse<string> response = new APIResponse<string>();
            string FilePath = GetFilePath(productCode);
            try
            {
                if (!System.IO.Directory.Exists(FilePath))
                {
                    System.IO.Directory.CreateDirectory(FilePath);
                }

                foreach (var files in fileCollection)
                {
                    string imagePath = FilePath + "\\" + files.FileName;
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    using (FileStream stream = System.IO.File.Create(imagePath))
                    {
                        await files.CopyToAsync(stream);
                        passcount++;
                    }
                }
            }
            catch (Exception ex)
            {
                errorcount++;
                response.ResponseCode = 400;
                response.Message = ex.Message;
                return BadRequest(response);
            }
            response.Message = $"{passcount} files uploaded Successfully and {errorcount} files failed to upload.";
            return Ok(response);
        }

        [HttpGet("GetImage")]
        public async Task<IActionResult> GetImage(string productCode)
        {
            APIResponse<string> response = new APIResponse<string>();
            string hostUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string FilePath = GetFilePath(productCode);
                string ImagePath = FilePath + "\\" + productCode + ".png";
                if (!System.IO.File.Exists(ImagePath))
                {
                    response.Message = "Not Found";
                    return NotFound(response);
                }
                response.Message = "Success!";
                response.Result = hostUrl + "/upload/products/" + productCode + "/" + productCode + ".png";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return NotFound(response);
            }
        }

        [HttpGet("GetMultipleImage")]
        public async Task<IActionResult> GetMultipleImage(string productCode)
        {
            APIResponse<List<string>> response = new APIResponse<List<string>>();
            string hostUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                List<string> imageUrls = new List<string>();
                string FilePath = GetFilePath(productCode);

                if (System.IO.Directory.Exists(FilePath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(FilePath); // Checkout
                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        string filename = fileInfo.Name;
                        string imagepath = FilePath + "\\" + filename;
                        if (System.IO.File.Exists(imagepath))
                        {
                            string imageurl = hostUrl + "/upload/products/" + productCode + "/" + filename;
                            imageUrls.Add(imageurl);
                        }
                    }
                    response.Result = imageUrls;
                    response.Message = "Success!";
                    return Ok(response);
                }
                else
                {
                    response.Message = "Not Found!";
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return NotFound(response);
            }
        }

        [HttpGet("Download")]
        public async Task<IActionResult> download(string productCode)
        {
            try
            {
                string FilePath = GetFilePath(productCode);

                if (System.IO.Directory.Exists(FilePath))
                {
                    string imagepath = FilePath + "\\" + productCode + ".png";
                    if (System.IO.File.Exists(imagepath))
                    {
                        MemoryStream stream = new MemoryStream();
                        using (FileStream fileStream = new FileStream(imagepath, FileMode.Open))
                        {
                            await fileStream.CopyToAsync(stream);
                        }
                        stream.Position = 0;
                        return File(stream, "image/png", productCode + ".png");
                    }
                }
                else
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("Remove")]
        public async Task<IActionResult> Remove(string productCode)
        {
            APIResponse<string> apiResponse = new APIResponse<string>();
            try
            {
                string FilePath = GetFilePath(productCode);
                if (System.IO.Directory.Exists(FilePath))
                {
                    string imagepath = FilePath + "\\" + productCode + ".png";
                    if (System.IO.File.Exists(imagepath))
                    {
                        System.IO.File.Delete(imagepath);
                    }
                }
                else
                {
                    apiResponse.Message = "File or directory not found!";
                    apiResponse.ResponseCode = 404;
                    apiResponse.Result = "";
                    return NotFound(apiResponse);
                }
                apiResponse.Message = "Success";
                return NotFound(apiResponse);
            }
            catch (Exception ex)
            {
                apiResponse.Message = "An error occurred while deleting the file.";
                apiResponse.ResponseCode = 500;
                apiResponse.Result = ex.Message;
                return StatusCode(500, apiResponse);
            }
        }

        [HttpGet("RemoveMultiple")]
        public async Task<IActionResult> RemoveMultiple(string productCode)
        {
            APIResponse<string> apiResponse = new APIResponse<string>();
            try
            {
                string FilePath = GetFilePath(productCode);
                if (System.IO.Directory.Exists(FilePath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(FilePath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();

                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        string filename = fileInfo.Name;
                        string imagepath = FilePath + "\\" + filename;
                        if (System.IO.File.Exists(imagepath))
                        {
                            System.IO.File.Delete(imagepath);
                        }
                    }
                }
                else
                {
                    apiResponse.Message = "Not Found!";
                    apiResponse.ResponseCode = 404;
                    apiResponse.Result = "";
                    return NotFound(apiResponse);
                }
                apiResponse.Message = "Success";
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("SaveMultipleImageDB")]
        public async Task<IActionResult> SaveMultipleImageDB(IFormFileCollection files, string productcode)
        {
            int passcount = 0;
            int errorcount = 0;
            APIResponse<string> apiResponse = new APIResponse<string>();
            try
            {
                foreach (var file in files)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        _context.TblImages.Add(new TblImage { ProductId = productcode, ImageData = stream.ToArray(), ImageName = file.FileName });
                        await _context.SaveChangesAsync();
                    }
                    passcount++;
                }
                apiResponse.Message = $"{passcount} files uploaded successfully";
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                apiResponse.ResponseCode = 400;
                apiResponse.Message = ex.Message;
                return Ok(apiResponse);
            }
        }

        [HttpGet("GetMultipleImageDB")]
        public async Task<IActionResult> GetMultipleImageDB(string productcode)
        {
            APIResponse<List<string>> apiResponse = new APIResponse<List<string>>();
            List<string> imagesBase64 = new List<string>();
            try
            {
                var imageUrls = await _context.TblImages.Where(x => x.ProductId == productcode).ToListAsync();
                if (imageUrls != null && imageUrls.Count() > 0)
                {
                    foreach (var image in imageUrls)
                    {
                        imagesBase64.Add(Convert.ToBase64String(image.ImageData));
                    }
                }
                else
                {
                    apiResponse.ResponseCode = 400;
                    apiResponse.Message = "Not Found!!";
                    return Ok(apiResponse);
                }
                apiResponse.Result = imagesBase64;
                apiResponse.Message = "Success";
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                apiResponse.ResponseCode = 400;
                apiResponse.Message = ex.Message;
                return Ok(apiResponse);
            }
        }

        [HttpGet("dbdownload")]
        public async Task<IActionResult> dbdownload(string productcode)
        {
            APIResponse<string> apiResponse = new APIResponse<string>();
            try
            {
                var imageUrl = await _context.TblImages.FirstOrDefaultAsync(x => x.ProductId == productcode);
                if (imageUrl != null)
                {
                    return File(imageUrl.ImageData, "image/png", productcode + ".png");
                }
                else
                {
                    apiResponse.ResponseCode = 400;
                    apiResponse.Message = "Not Found!!";
                    return Ok(apiResponse);
                }
            }
            catch (Exception ex)
            {
                apiResponse.ResponseCode = 400;
                apiResponse.Message = ex.Message;
                return Ok(apiResponse);
            }
        }

        [NonAction]
        public string GetFilePath(string productCode)
        {
            string FilePath = _webHostEnvironment.WebRootPath + "\\upload\\products\\" + productCode;
            return FilePath;
        }
    }
}