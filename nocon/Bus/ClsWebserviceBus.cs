using System;
using System.Collections.Generic;
using System.Text;
using nocon.Data;
using System.Net;

namespace nocon.Bus
{
    public class ClsWebserviceBus
    {
        public bool getKeepALiveWS(ClsWebservice enderecoWs)
        {
            string address = "";

            try
            {
                // Create a request for the URL. 
                address = enderecoWs.Endereco;
                WebRequest request = WebRequest.Create(address);
                
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                
                // Get the response.
                WebResponse response = request.GetResponse();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<ClsWebservice> getWebservices()
        {
            List<ClsWebservice> lst = new List<ClsWebservice>();
            ClsWebservice ws;

            ws = new ClsWebservice();
            ws.Nome = "RIOCARD RS";
            ws.Endereco = @"http://200.248.179.6:55443/WSRioCard/Service.asmx";
            ws.Status = false;
            lst.Add(ws);

            ws = new ClsWebservice();
            ws.Nome = "RIOCARD SP";
            ws.Endereco = @"http://200.142.86.169:55443/WSRioCard/Service.asmx";
            ws.Status = false;
            //lst.Add(ws);

            ws = new ClsWebservice();
            ws.Nome = "REEMBOLSO RS";
            ws.Endereco = @"http://200.248.179.6:55444/WSReembolso/Service.asmx";
            ws.Status = false;
            lst.Add(ws);

            ws = new ClsWebservice();
            ws.Nome = "REEMBOLSO SP";
            ws.Endereco = @"http://200.142.86.169:55444/WSReembolso/Service.asmx";
            ws.Status = false;
            //lst.Add(ws);

            return lst;
        }
    }
}
