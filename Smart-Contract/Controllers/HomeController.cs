using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.Compilation;
using Nethereum.Web3;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Smart_Contract.Controllers
{
	public class HomeController : Controller
	{
		public async Task<ActionResult> Index()
		{
			var result = 0;
			var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";

			var password = "password";
			var web3 = new Web3();
			var abi = @"[{""constant"":false,""inputs"":[{""name"":""val"",""type"":""int256""}],""name"":""multiply"",""outputs"":[{""name"":""d"",""type"":""int256""}],""payable"":false,""type"":""function""},{""inputs"":[{""name"":""multiplier"",""type"":""int256""}],""payable"":false,""type"":""constructor""}]";
			var code = "0x6060604052341561000f57600080fd5b6040516020806100d4833981016040528080519150505b60008190555b505b60988061003c6000396000f300606060405263ffffffff7c01000000000000000000000000000000000000000000000000000000006000350416631df4f1448114603c575b600080fd5b3415604657600080fd5b604f6004356061565b60405190815260200160405180910390f35b60005481025b9190505600a165627a7a723058208f4531f242c475edbea7363dee2f7ba970d27f1029f1c968ef00391fb56cd4430029";
			var multiply = 7;
			var unlockAccount = await web3.Personal.UnlockAccount.SendRequestAsync(senderAddress, password, new HexBigInteger(120));
			if (unlockAccount)
			{


				var transactionHash = await web3.Eth.DeployContract.SendRequestAsync(abi, code, senderAddress, multiply);


				/***************** Miner *********************/
				var minerOut = await web3.Miner.Start.SendRequestAsync(5);
				if (minerOut)
				{
					var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
					while(receipt == null)
					{
						Thread.Sleep(5000);
						receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
					}

					var contractAdd = receipt.ContractAddress;
					var contract = web3.Eth.GetContract(abi, contractAdd);
					var multiplyFunction = contract.GetFunction("multiply");

					result = await multiplyFunction.CallAsync<int>(7);
				}
			}
			return View(result);	
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}