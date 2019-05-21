using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Business.Services
{
    public class ExportService : IExportService
    {
        private readonly IUnitOfWork unitOfWork;

        public ExportService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> Export(Type type, string path)
        {
            var successfullyExported = false;
            if (type == typeof(RsopPot))
            {
                var allRSoPPots = await unitOfWork.RsopPotRepository.GetAllEntities();
                successfullyExported = ExportToJson(path, allRSoPPots);
            }
            if (type == typeof(Rsop))
            {
                var allRSoPs = await unitOfWork.RsopRepository.GetAllEntities();
                successfullyExported = ExportToJson(path, allRSoPs);
            }

            return successfullyExported;
        }

        private bool ExportToJson<T>(string savePath, List<T> collectionToExport)
        {
            if (collectionToExport.Count > 0)
            {
                string json = JsonConvert.SerializeObject(collectionToExport, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                File.WriteAllText(savePath, json);

                return true;
            }

            return false;
        }
    }
}
