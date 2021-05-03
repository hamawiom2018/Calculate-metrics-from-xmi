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
using master_project.BusinessLogic.Metrics;
using Newtonsoft.Json;
using master_project.Models.UML;

namespace master_project.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MetricController : ControllerBase
    {


        private readonly ILogger<UploadController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly XMIProjectContext _projectContext;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public MetricController(ILogger<UploadController> logger, UserManager<ApplicationUser> userManager
        , SignInManager<ApplicationUser> signInManager,
        XMIProjectContext projectContext)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _projectContext = projectContext;
        }
        [HttpPost]
        [Route("GetMetric")]
        public async Task<IActionResult> GetMetric(MetricRequestContract model)
        {
            if (model.TargetType == 0)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                var upload = await _projectContext.Uploads.Where(uploadItem => uploadItem.AuthorEmail == user.Email
                 && uploadItem.Id == model.DiagramId).FirstOrDefaultAsync();
                if (upload != null)
                {
                    string xmiContent = Encoding.ASCII.GetString(upload.FileContent, 0, upload.FileContent.Length);
                    UploadRequest uploadRequest = new UploadRequest();
                    uploadRequest.fileSource = xmiContent;
                    uploadRequest.name = upload.UploadName;
                    ReadXmiBL readXmiBL = new ReadXmiBL(user);
                    ReadXmiResponse readXmiResponse = readXmiBL.readXmi(uploadRequest);
                    IdentifyDiagramBL identifyDiagramBL = new IdentifyDiagramBL(user);

                    List<int> elementTypes = new List<int>();
                    elementTypes = identifyDiagramBL.getElementsTypes(JsonConvert.DeserializeObject<List<UMLElement>>(readXmiResponse.resultElements), elementTypes);
                    var resultMetrics = await _projectContext.Metrics.Where(metric => metric.TargetType == model.TargetType
                    && _projectContext.MetricElements.Where(metricElement => metricElement.MetricId == metric.Id && elementTypes.Any(elementType => metricElement.Element == elementType)).Any()).ToListAsync();
                    return Ok(resultMetrics);
                }
            }
            else
            {
                var resultMetrics = await _projectContext.Metrics.Where(metric => metric.TargetType == model.TargetType).ToListAsync();
                return Ok(resultMetrics);
            }

            return Ok();


        }
        [HttpPost]
        [Route("Evaluate")]
        public async Task<IActionResult> Evaluate(EvaluateRequestContract evaluateRequestContract)
        {
            double result = -1;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var upload = await _projectContext.Uploads.Where(uploadItem => uploadItem.AuthorEmail == user.Email
             && uploadItem.Id == evaluateRequestContract.DiagramId).FirstOrDefaultAsync();
            if (upload != null)
            {
                string xmiContent = Encoding.ASCII.GetString(upload.FileContent, 0, upload.FileContent.Length);
                UploadRequest uploadRequest = new UploadRequest();
                uploadRequest.fileSource = xmiContent;
                uploadRequest.name = upload.UploadName;
                ReadXmiBL readXmiBL = new ReadXmiBL(user);
                ReadXmiResponse readXmiResponse = readXmiBL.readXmi(uploadRequest);

                //Calculate Metric
                MetricsBL metricsBL = new MetricsBL();

                result = metricsBL.CalculateMetric(readXmiResponse.resultElements, evaluateRequestContract.MetricCode, evaluateRequestContract.ElementRef);
            }

            return Ok(result);
        }

    }

}
