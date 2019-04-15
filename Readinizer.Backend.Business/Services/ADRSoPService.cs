using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;
using Microsoft.GroupPolicy;


namespace Readinizer.Backend.Business.Services
{
    public class ADRSoPService : IADRSoPService
    {
        public void getRSoP()
        {

            GPRsop test = new GPRsop(RsopMode.Logging, "");
            test.LoggingComputer = "sysadmWS01";
            test.LoggingUser = "readinizer.ch\\lkellenb";
            test.LoggingMode = LoggingMode.Computer;
            test.CreateQueryResults();
            test.GenerateReportToFile(ReportType.Xml, "C:\\Users\\lkellenb\\Desktop\\test.xml");

        }

    }
}
