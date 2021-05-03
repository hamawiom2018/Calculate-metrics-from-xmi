using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using master_project.Models;
using System.Text;
using System.Text.Encodings;
using master_project.BusinessLogic;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using master_project.Data.XMI_Project;
using Microsoft.EntityFrameworkCore;

namespace master_project.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {


        private readonly ILogger<UploadController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly XMIProjectContext _projectContext;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public UploadController(ILogger<UploadController> logger, UserManager<ApplicationUser> userManager
        , SignInManager<ApplicationUser> signInManager,
        XMIProjectContext projectContext)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _projectContext = projectContext;
        }
        [HttpPost, DisableRequestSizeLimit]
        [Route("Post")]
        
        public async Task<ReadXmiResponse> Post()
        {
            var r = await Request.ReadFromJsonAsync<UploadRequest>();
            var file = Convert.FromBase64String(r.fileSource);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            var userName = User.FindFirstValue(ClaimTypes.Email); // will give the user's userName
            var user = await _userManager.FindByIdAsync(userId);
            string fileContent=r.fileSource;
            ReadXmiBL readXmiBL = new ReadXmiBL(user);
            // ASCII conversion - string from bytes  
            string xmiContent = Encoding.ASCII.GetString(file, 0, file.Length);
            
            r.fileSource = xmiContent;
            ReadXmiResponse readXmiResponse = readXmiBL.readXmi(r);
            readXmiResponse.XmlContent=fileContent;
            return readXmiResponse;
        }
        [HttpPost, DisableRequestSizeLimit]
        [Route("Save")]
        public async Task<IActionResult> Save()
        {
            var r = await Request.ReadFromJsonAsync<UploadRequest>();
            var file = Convert.FromBase64String(r.fileSource);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            var userName = User.FindFirstValue(ClaimTypes.Email); // will give the user's userName
            var user = await _userManager.FindByIdAsync(userId);

            ReadXmiBL readXmiBL = new ReadXmiBL(user);
            // ASCII conversion - string from bytes  
            string xmiContent = Encoding.ASCII.GetString(file, 0, file.Length);
            r.fileSource = xmiContent;
            ReadXmiResponse readXmiResponse = readXmiBL.readXmi(r);
            Upload uploadItem =new Upload();
            uploadItem.UploadName=r.name;
            uploadItem.DiagramName=readXmiResponse.DiagramName;
            uploadItem.AuthorEmail=user.Email;
            uploadItem.FileContent=file;
            uploadItem.CreatedDate=DateTime.Now;
            var resUpload= await _projectContext.Uploads.AddAsync(uploadItem);
            await _projectContext.SaveChangesAsync();
            if(resUpload.State==EntityState.Added){
                return Ok(readXmiResponse);
            }
            

            return Ok(readXmiResponse);
        }
        /*
           [HttpGet]
           public IEnumerable<WeatherForecast> Get()
           {
               var rng = new Random();
               return Enumerable.Range(1, 5).Select(index => new WeatherForecast
               {
                   Date = DateTime.Now.AddDays(index),
                   TemperatureC = rng.Next(-20, 55),
                   Summary = Summaries[rng.Next(Summaries.Length)]
               })
               .ToArray();
           }*/
    }
}
