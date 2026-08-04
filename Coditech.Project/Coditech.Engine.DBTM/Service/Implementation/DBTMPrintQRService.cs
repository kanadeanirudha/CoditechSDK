using Coditech.API.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper;
using Coditech.Common.Helper.Utilities;
using Coditech.Common.Logger;
using Coditech.Common.Service;
using Coditech.Resources;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using System.Collections.Specialized;
using System.Data;

using static Coditech.Common.Helper.HelperUtility;
namespace Coditech.API.Service
{
    public class DBTMPrintQRService : BaseService, IDBTMPrintQRService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICoditechLogging _coditechLogging;
        private readonly ICoditechRepository<GeneralPerson> _generalPersonRepository;
        private readonly ICoditechRepository<DBTMTraineeDetails> _dBTMTraineeDetailsRepository;
        public DBTMPrintQRService(ICoditechLogging coditechLogging, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _coditechLogging = coditechLogging;
            _generalPersonRepository = new CoditechRepository<GeneralPerson>(_serviceProvider.GetService<Coditech_Entities>());
            _dBTMTraineeDetailsRepository = new CoditechRepository<DBTMTraineeDetails>(_serviceProvider.GetService<CoditechCustom_Entities>());
        }

        public virtual DBTMPrintQRListModel GetDBTMPrintQRTraineeList(int generalBatchMasterId, string userType, FilterCollection filters, NameValueCollection sorts, NameValueCollection expands, int pagingStart, int pagingLength)
        {
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<DBTMPrintQRModel> objStoredProc = new CoditechViewRepository<DBTMPrintQRModel>(_serviceProvider.GetService<CoditechCustom_Entities>());
            objStoredProc.SetParameter("@GeneralBatchMasterId", generalBatchMasterId, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@UserType", userType, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@WhereClause", pageListModel.SPWhereClause, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_BY", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowsCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);
            List<DBTMPrintQRModel> list = objStoredProc.ExecuteStoredProcedureList("Coditech_GetDBTMPrintQRTraineeList @GeneralBatchMasterId,@UserType,@WhereClause,@Rows,@PageNo,@Order_BY,@RowsCount OUT", 6, out pageListModel.TotalRowCount)?.ToList();
            DBTMPrintQRListModel model = new DBTMPrintQRListModel();
            model.DBTMPrintQRList = list ?? new List<DBTMPrintQRModel>();
            model.BindPageListModel(pageListModel);
            return model;
        }

        public virtual DBTMPrintQRListModel DownloadPrintQR(ParameterModel parameterModel)
        {
            DBTMPrintQRListModel qrModel = GeneratePrintQRHTMLTemplate(parameterModel);
            string html = qrModel.PrintableHTML;
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "PrintQRPdf");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            string fileName = $"AthleteQR_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            string filePath = Path.Combine(folderPath, fileName);
            new BrowserFetcher().DownloadAsync().GetAwaiter().GetResult();
            using var browser = Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = new[] { "--no-sandbox", "--disable-setuid-sandbox", "--allow-file-access-from-files" }
            }).GetAwaiter().GetResult();
            using var page = browser.NewPageAsync().GetAwaiter().GetResult();
            page.SetContentAsync(html).GetAwaiter().GetResult();
            page.PdfAsync(filePath, new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                PreferCSSPageSize = true
            }).GetAwaiter().GetResult();
            browser.CloseAsync().GetAwaiter().GetResult();
            qrModel.FileName = fileName;
            qrModel.FilePath = filePath;
            return qrModel;
        }
        private DBTMPrintQRListModel GeneratePrintQRHTMLTemplate(ParameterModel parameterModel)
        {
            if (parameterModel == null || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);
            List<long> personIds = parameterModel.Ids.Split(',').Select(x => Convert.ToInt64(x)).ToList();
            List<GeneralPerson> persons = _generalPersonRepository.Table.Where(x => personIds.Contains(x.PersonId)).ToList();
            List<DBTMTraineeDetails> traineeDetails = _dBTMTraineeDetailsRepository.Table.Where(x => personIds.Contains(x.PersonId)).ToList();
            DBTMPrintQRListModel listModel = new DBTMPrintQRListModel();
            listModel.DBTMPrintQRList = new List<DBTMPrintQRModel>();
            string templateCode = EmailTemplateCodeCustomEnum.DBTMAutoActivityQRCodeFormatVertical.ToString();
            string finalHtml = "";
            var centreCode = traineeDetails.FirstOrDefault()?.CentreCode;
            var emailTemplate = GetEmailTemplateByCode(centreCode, templateCode);
            if (emailTemplate == null || string.IsNullOrWhiteSpace(emailTemplate.EmailTemplate))
                throw new CoditechException(ErrorCodes.NullModel, "QR Template not found.");

            foreach (GeneralPerson person in persons)
            {
                DBTMTraineeDetails trainee = traineeDetails.FirstOrDefault(x => x.PersonId == person.PersonId);
                string personCode = trainee?.PersonCode;
                string qrImage = DBTMCustomHelper.GenerateQRCode(personCode, string.Empty);
                string printableHtml = ReplacePrintableHTMLQRTemplate(emailTemplate.EmailTemplate, person, personCode, qrImage);
                finalHtml += $@"
                <div style='page-break-inside: avoid;
                            break-inside: avoid;
                            margin-bottom: 20px;'>
                    {printableHtml}
                </div>";
                listModel.DBTMPrintQRList.Add(new DBTMPrintQRModel
                {
                    PersonId = person.PersonId,
                    FirstName = person.FirstName,
                    MiddleName = person.MiddleName,
                    LastName = person.LastName,
                    PersonCode = personCode,
                    QRCode = qrImage,
                    PrintableHTML = printableHtml
                });
            }
            listModel.PrintableHTML = finalHtml;
            return listModel;
        }

        #region private
        private string ReplacePrintableHTMLQRTemplate(string html, GeneralPerson person, string personCode, string qrImage)
        {
            html = ReplaceTokenWithMessageText("#FirstName#", person.FirstName ?? "", html);
            html = ReplaceTokenWithMessageText("#MiddleName#", person.MiddleName ?? "", html);
            html = ReplaceTokenWithMessageText("#LastName#", person.LastName ?? "", html);
            html = ReplaceTokenWithMessageText("#PersonCode#", personCode ?? "", html);
            html = ReplaceTokenWithMessageText("#MobileNumber#", person.MobileNumber ?? "", html);
            html = ReplaceTokenWithMessageText("#QRImage#", qrImage ?? "", html);

            html = ReplaceTokenWithMessageText("#htmlopen#", "<html>", html);
            html = ReplaceTokenWithMessageText("#headopen#", "<head>", html);
            html = ReplaceTokenWithMessageText("#headclose#", "</head>", html);
            html = ReplaceTokenWithMessageText("#bodyopen#", "<body>", html);
            html = ReplaceTokenWithMessageText("#bodyclose#", "</body>", html);
            html = ReplaceTokenWithMessageText("#htmlclose#", "</html>", html);

            return html;
        }
        #endregion
    }
}
