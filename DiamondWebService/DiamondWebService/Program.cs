using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiamondWebService.WebReference;
using System.IO;
using System.Net;

namespace DiamondWebService
{
    class Program
    {
        static void Main(string[] args)
        {
            ReportCheckWS wp = new ReportCheckWS();
            string[] requestParameters = null;
            try
            {
                requestParameters = File.ReadAllLines(@"C:\eureka\requestgia.ini");
            }
            catch (Exception e) { return; }
            string ipAddress = requestParameters[0];
            string reportNo = "<REPORT_NO>" + requestParameters[2] + "</REPORT_NO>";
            string reportWeight = "<REPORT_WEIGHT>" + requestParameters[1] + "</REPORT_WEIGHT>";

            Console.WriteLine("Getting xml response...");
            var xmlResponse = wp.processRequest(getInputXml(ipAddress, reportNo, reportWeight));
            if (xmlResponse != null)
                File.WriteAllText(@"C:\eureka\xmlResponse.xml", xmlResponse);
            if (xmlResponse.Contains("<IS_PDF_AVAILABLE>TRUE</IS_PDF_AVAILABLE>"))
            {
                Console.WriteLine("Getting pdf response...");
                var pdfResponse = wp.downloadPDFReport(getInputXml(ipAddress, reportNo, reportWeight));
                if (pdfResponse != null)
                    File.WriteAllBytes(@"C:\eureka\pdfResponse.pdf", pdfResponse);

                //using (WebClient webClient = new WebClient())
                //{
                //    Console.WriteLine("Getting image response...");
                //    webClient.DownloadFile("https://myapps.gia.edu/DigitalImageClient/getDigitalImage.do?imgType=PROPIMG&reportNo=" + requestParameters[2] + "&IPAddress=" + ipAddress, @"C:\eureka\imageResponse.png");

                //    Console.WriteLine("Getting plot response...");
                //    webClient.DownloadFile("https://myapps.gia.edu/DigitalImageClient/getDigitalImage.do?imgType=PLOTIMG&reportNo=" + requestParameters[2] + "&IPAddress=" + ipAddress, @"C:\eureka\plotResponse.png");
                //}
            }
        }

        private static string getInputXml(string ipAddress, string reportNo, string reportWeight)
        {
            return @"<?xml version=""1.0"" encoding=""UTF8""?>
	                <REPORT_CHECK_REQUEST>
		            <HEADER>
                        <IP_ADDRESS>" + ipAddress + @"</IP_ADDRESS></HEADER>
		            <BODY>
			            <REPORT_DTLS>
				            <REPORT_DTL>" + reportNo + reportWeight + @"</REPORT_DTL>
			            </REPORT_DTLS>
		            </BODY>
	            </REPORT_CHECK_REQUEST>";
        }
    }
}
