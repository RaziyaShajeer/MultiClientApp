using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Security;
using SNR_ClientApp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    internal class SyncOperationService
    {
        HttpClient httpClient;
        public SyncOperationService()
        {
           
        }

        internal SyncOperationDTO getAssignedSyncOperations()
        {
            LogManager.WriteLog(" request for assigned syncOperations. ");
            try
            {
                httpClient = RestClientUtil.getClient();
                var responseTask = httpClient.GetAsync(ApiConstants.ASSIGNED_SYNC_OPERATIONS);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for assigned syncOperations Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                    SyncOperationDTO cardsList = JsonConvert.DeserializeObject<SyncOperationDTO>(response);
                    return cardsList;
                }
                return null;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex.ToString());
                LogManager.HandleException(ex);
                throw new ServiceException(ex.Message);
            }
        }

        public List<SyncOperationTimeDTO> getSyncOperations()
        {
            LogManager.WriteLog(" request for syncOperationStatus. ");
            try
            {
                httpClient = RestClientUtil.getClient();
                var responseTask = httpClient.GetAsync(ApiConstants.SYNC_OPERATION_STATUS);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for  syncOperationStatus Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                    List<SyncOperationTimeDTO> cardsList = JsonConvert.DeserializeObject<List<SyncOperationTimeDTO>>(response);
                    return cardsList;
                }
                LogManager.WriteLog("request for syncOperationStatus Failed..");
                return null;

            }
            catch(Exception ex)
            {
                LogManager.WriteLog(ex.ToString());
                LogManager.HandleException(ex);
                throw new ServiceException(ex.Message);
            }

        }
    }
}
