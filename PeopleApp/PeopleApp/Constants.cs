using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleApp
{
    public class Constants
    {
        public static string BaseApiAddress => "https://peopleapp3.azurewebsites.net/";
        public const string StorageConnection = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://10.3.231.178:8088/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;";
        //public const string StorageConnection = "UseDevelopmentStorage=true"; 
    }
}
