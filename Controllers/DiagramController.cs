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
using Microsoft.AspNetCore.OData.Query;

namespace master_project.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DiagramController : ControllerBase
    {
        private readonly ILogger<UploadController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly XMIProjectContext _projectContext;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public DiagramController(ILogger<UploadController> logger, UserManager<ApplicationUser> userManager
        , SignInManager<ApplicationUser> signInManager,
        XMIProjectContext projectContext)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _projectContext = projectContext;
        }
        [HttpGet]
        [EnableQuery()]
        public async Task<IActionResult> Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var uploads = await _projectContext.Uploads.Where(uploadItem => uploadItem.AuthorEmail == user.Email)
            .Select(uploadItem => new DiagramResponseContract
            {
                AuthorEmail = uploadItem.AuthorEmail,
                CreatedDate = uploadItem.CreatedDate,
                DiagramName = uploadItem.DiagramName,
                Id = uploadItem.Id,
                UploadName = uploadItem.UploadName
            }).ToListAsync();

            return Ok(uploads);

        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var upload = await _projectContext.Uploads.Where(uploadItem => uploadItem.AuthorEmail == user.Email
             && uploadItem.Id == id).FirstOrDefaultAsync();
            if (upload != null)
            {
                string xmiContent = Encoding.ASCII.GetString(upload.FileContent, 0, upload.FileContent.Length);
                UploadRequest uploadRequest = new UploadRequest();
                uploadRequest.fileSource = xmiContent;
                uploadRequest.name = upload.UploadName;
                ReadXmiBL readXmiBL = new ReadXmiBL(user);
                ReadXmiResponse readXmiResponse = readXmiBL.readXmi(uploadRequest);

                DiagramDetailResponseContract detailResponseContract = new DiagramDetailResponseContract();
                detailResponseContract.UploadName = upload.UploadName;
                detailResponseContract.UpdatedDate = upload.UpdatedDate;
                detailResponseContract.Id = upload.Id;
                detailResponseContract.Elements = readXmiResponse.resultElements;
                detailResponseContract.DiagramName = upload.DiagramName;
                detailResponseContract.CreatedDate = upload.CreatedDate;
                detailResponseContract.AuthorEmail = upload.AuthorEmail;

                return Ok(detailResponseContract);
            }else{
                return BadRequest();
            }


        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var upload = await _projectContext.Uploads.Where(uploadItem => uploadItem.AuthorEmail == user.Email &&
             uploadItem.Id == id).FirstOrDefaultAsync();
            if (upload != null)
            {
                var remove = _projectContext.Uploads.Remove(upload);
                await _projectContext.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest();
            }


        }
    }

}