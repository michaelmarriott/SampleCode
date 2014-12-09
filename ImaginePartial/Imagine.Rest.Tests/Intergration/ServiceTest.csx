using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ScriptCs.Contracts;
using ScriptCs.Request;

public class ServiceTest : IDisposable {
	public HttpClient client;
	public string resource;
	public string accountId = "27100013726";//"28990022007";//
    public string customer = "RVDP-TES014";//"RVTP-TES014"; // 
    public string url = "http://api.voxtelecom.co.za";

	public ServiceTest(String url, String resource, String customer, String accountId) {
	  this.url = url;
	  this.customer = customer;
	  this.accountId = accountId;
	  client = new HttpClient();
	  client.BaseAddress = new Uri(url);
	  this.resource = resource;
	  client.DefaultRequestHeaders.Add("SYSTEM_TOKEN", "5c1a65fd-8511-44c8-ad93-f775b789ec8a");
	  client.DefaultRequestHeaders.Accept.Clear();
	  client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	}



	public void Dispose() {
	  client.Dispose();
	}

	public void HttpGetVoiceAccountsSuccesfully() {
	  HttpResponseMessage response = client.GetAsync(resource+"/VoiceAccounts/" + accountId).Result;
	  response.EnsureSuccessStatusCode(); //   if (response.IsSuccessStatusCode) {var result = response.Content.ReadAsAsync<AccountViewModel>().Result;}
	}

	public void HttpGetVoiceAccountsAliasesSuccesfully() {
	  HttpResponseMessage response = client.GetAsync(resource+"/VoiceAccounts/" + accountId + "/aliases/").Result;
	  response.EnsureSuccessStatusCode(); 
	}

	public void HttpGetVoiceAccountsFollowMeNumbersSuccesfully() {
	  HttpResponseMessage response = client.GetAsync(resource+"/VoiceAccounts/" + accountId + "/FollowMeNumbers/").Result;
	  response.EnsureSuccessStatusCode();
	}

	public void HttpGetAllVoiceAccountsSuccesfully() {}
		
	public void HttpGetAllVoiceCustomerSuccesfully() {
      HttpResponseMessage response = client.GetAsync(resource+"/VoiceCustomers/").Result;
      response.EnsureSuccessStatusCode();  
    }

    public void HttpGetByCustomerNameSuccesfully() {
      HttpResponseMessage response = client.GetAsync(resource+"/VoiceCustomers/" + customer).Result;
      response.EnsureSuccessStatusCode(); 
    }
	
    public void HttpGetAccountsByCustomerNameSuccesfully() {
      HttpResponseMessage response = client.GetAsync(resource+"/VoiceCustomers/" + customer + "/VoiceAccounts/").Result;
      response.EnsureSuccessStatusCode();
    }

	public void HttpGetFlagsByRatingSuccesfully() {
      HttpResponseMessage response = client.GetAsync(resource+"/Flags/?name=Rating").Result;
      response.EnsureSuccessStatusCode();
    }
	
	//Bill Run Controller
	public void HttpGetBillRunSuccesfully() {
      HttpResponseMessage response = client.GetAsync(resource+"/BillRun?code=201402").Result;
      response.EnsureSuccessStatusCode();
    }

	//Cloud Controller
	public void HttpGetCloudSuccesfully() {
      HttpResponseMessage response = client.GetAsync(resource+"/Clouds").Result;
      response.EnsureSuccessStatusCode();
    }
	
	//Voice Jobs
	public void HttpGetVoiceJobsSuccesfully() {
      HttpResponseMessage response = client.GetAsync(resource+"/VoiceJobs?name=DrReport").Result;
      response.EnsureSuccessStatusCode();
    }

	//Inbound Call Data Record
	public void HttpGetInboundCallDataRecordSuccesfully() {
      HttpResponseMessage response = client.GetAsync(resource+"/InboundCallDataRecord?lobId=2&dateStart=20140101&dateEnd=20140131&accountcode=EKU00001").Result;
      response.EnsureSuccessStatusCode();
    }

	//Inbound Call Data Record Summary
	public void HttpGetInboundCallDataRecordSummarySuccesfully() {
      HttpResponseMessage response = client.GetAsync(resource+"/InboundCallDataRecordSummary?lobId=2&dateStart=20140101&dateEnd=20140131&accountcode=EKU00001").Result;
      response.EnsureSuccessStatusCode();
    }

	//Outbound Call Data Record
	public void HttpGetOutboundCallDataRecordSuccesfully() {
      HttpResponseMessage response = client.GetAsync(resource+"/OutboundCallDataRecord?lobId=2&dateStart=20140101&dateEnd=20140131&accountcode=EKU00001").Result;
      response.EnsureSuccessStatusCode();
    }

	//Outbound Call Data Record Summary
	public void HttpGetOutboundCallDataRecordSummarySuccesfully() {
      HttpResponseMessage response = client.GetAsync(resource+"/OutboundCallDataRecordSummary?lobId=2&dateStart=20140101&dateEnd=20140131&accountcode=EKU00001").Result;
      response.EnsureSuccessStatusCode();
    }
	
	//Outbound Call Data Record Summary
	public void HttpGetVoiceDidNumbersReserveNumbersSuccesfully() {
      HttpResponseMessage response = client.GetAsync(resource+"/VoiceDidNumbers/Reserve?resellerIdentifier=RVAT-VoxAtlantic&amount=1&prefix=2787").Result;
      response.EnsureSuccessStatusCode();
    }
}

string url = "http://api.voxtelecom.co.za/";
string resource = "2/voice";
string customer = "RVDP-TES014";
string accountId = "27100013726";

if (Env.ScriptArgs.Count > 0){ url = Env.ScriptArgs[0]; }
if (Env.ScriptArgs.Count > 1){ resource = Env.ScriptArgs[1]; }
if (Env.ScriptArgs.Count > 2){ customer = Env.ScriptArgs[2]; }
if (Env.ScriptArgs.Count > 3){ accountId = Env.ScriptArgs[3]; }

Console.Write(url);
Console.WriteLine("Starting...");
using (var service = new ServiceTest(url, resource, customer, accountId)){
	service.HttpGetVoiceAccountsSuccesfully();
	Console.WriteLine("Get Voice Accounts Done !");

	service.HttpGetVoiceAccountsAliasesSuccesfully();
	Console.WriteLine("Get Voice Accounts Aliases Done !");

	service.HttpGetVoiceAccountsFollowMeNumbersSuccesfully();
	Console.WriteLine("Get Voice Accounts Follow Me Numbers Done !");

	service.HttpGetAllVoiceCustomerSuccesfully();
	Console.WriteLine("Get All Voice Customer Done !");

	service.HttpGetByCustomerNameSuccesfully();
	Console.WriteLine("Get By Customer Name Done !");

	service.HttpGetAccountsByCustomerNameSuccesfully();
	Console.WriteLine("Get Accounts By Customer Name Done !");

	service.HttpGetBillRunSuccesfully();
	Console.WriteLine("Get Bill Run Done !");

	service.HttpGetCloudSuccesfully();
	Console.WriteLine("Get Cloud Done !");

	service.HttpGetFlagsByRatingSuccesfully();
	Console.WriteLine("Get Flag Done !");

	service.HttpGetVoiceJobsSuccesfully();
	Console.WriteLine("Get Voice Jobs Done !");

	service.HttpGetInboundCallDataRecordSuccesfully();
	Console.WriteLine("Inbound Call Data Record Done !");

	service.HttpGetInboundCallDataRecordSummarySuccesfully();
	Console.WriteLine("Inbound Call Data Record Summary Done !");

	service.HttpGetOutboundCallDataRecordSuccesfully();
	Console.WriteLine("Outbound Call Data Record Done !");

	service.HttpGetOutboundCallDataRecordSummarySuccesfully();
	Console.WriteLine("Outbound Call Data Record Summary Done !");

	service.HttpGetVoiceDidNumbersReserveNumbersSuccesfully();
	Console.WriteLine("Get VoiceDidNumbers Reserve Numbers!");
}
Console.WriteLine("Done!");