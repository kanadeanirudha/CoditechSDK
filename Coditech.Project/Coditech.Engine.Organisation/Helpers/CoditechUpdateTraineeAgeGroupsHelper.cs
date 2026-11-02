using Coditech.Common.API.Model;
using Coditech.Common.Logger;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Text;


namespace Coditech.Hangfire
{
    public class CoditechUpdateTraineeAgeGroupsHelper : BaseScheduler, ISchedulerProviders
    {
        protected readonly ICoditechLogging coditechLogging;
        private readonly IServiceProvider _serviceProvider;
        public CoditechUpdateTraineeAgeGroupsHelper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            coditechLogging = _serviceProvider.GetService<ICoditechLogging>();
        }
        public void InvokeMethod(TaskSchedulerModel model)
        {
            if (!string.IsNullOrEmpty(model?.ExeParameters))
            {
                string[] args = model.ExeParameters.Split(',');

                // Expected args: [0]=baseUrl, [1]=authToken, [2]=userHeader, [3]=SchedulerCallFor, [4]=optionalTimeout
                if (args.Length < 4)
                {
                    coditechLogging.LogMessage("Insufficient parameters passed to CoditechCustomSchedulerHelper.", LoggingComponent, TraceLevel.Warning);
                    return;
                }

                try
                {
                    // Request timeout is optionally contained in args[4]
                    if (args.Length > 4 && !string.IsNullOrEmpty(args[4]) && int.TryParse(args[4], out int timeout))
                        base.RequestTimeout = timeout;
                }
                catch (Exception ex)
                {
                    coditechLogging.LogMessage(ex.Message, LoggingComponent, TraceLevel.Warning);
                }

                CallAPI(args);
            }
        }

        private void CallAPI(string[] args)
        {
            string data = JsonConvert.SerializeObject(new ParameterModel());
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            string jsonString = string.Empty;
            string message = string.Empty;
            string requestPath = $"{args[0].TrimEnd('/')}/DBTMUser/UpdateTraineeAgeGroups";
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(requestPath);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = base.RequestTimeout;
                request.ContentLength = dataBytes.Length;

                // Set headers (use indexed args validated by caller)
                try
                {
                    if (!string.IsNullOrEmpty(args[1]))
                        request.Headers[AuthorizationHeader] = "Basic " + args[1];
                    if (!string.IsNullOrEmpty(args[2]))
                        request.Headers[UserHeader] = args[2];
                }
                catch { /* ignore header set failures */ }

                using (var reqStream = request.GetRequestStream())
                {
                    reqStream.Write(dataBytes, 0, dataBytes.Length);
                }
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var dataStream = response.GetResponseStream())
                using (var reader = new StreamReader(dataStream ?? Stream.Null))
                {
                    jsonString = reader.ReadToEnd();
                    coditechLogging.LogMessage($"{args[3]} API Call Successfully.", LoggingComponent, TraceLevel.Warning);
                }
            }
            catch (WebException webException)
            {
                coditechLogging.LogMessage(webException, LoggingComponent, TraceLevel.Error);
            }
            catch (Exception ex)
            {
                coditechLogging.LogMessage(ex, LoggingComponent, TraceLevel.Error);
            }
        }
    }
}

