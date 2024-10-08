using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Solana.Unity.Programs;
using Solana.Unity.Rpc.Core.Http;
using Solana.Unity.Rpc.Models;
using Solana.Unity.Rpc.Types;
using Solana.Unity.Wallet;
using SolPlay.Scripts.Services;
using Feedingdanny.Accounts;
using static Feedingdanny.Program.FeedingdannyProgram;
using Feedingdanny.Program;
using UnityEngine;
using Frictionless;

public class ContractHandler : MonoBehaviour
{
    public static PublicKey ProgramId = new PublicKey("ERZeBKsnZ19HqiRCJ4npkVkwf2hvUKN5GSsDW5Uy84bm");

    public PublicKey gameDataAccount;
    public GameState gameState;

    public async void createOrLaunchGame(string name)
    {
        //PublicKey.TryFindProgramAddress(new[]
        //{
        //    Encoding.UTF8.GetBytes("level1")
        //},
        //ProgramId, out gameDataAccount, out var bump);

        PublicKey.TryCreateWithSeed(gameState.web3.WalletBase.Account, "1", ProgramId, out gameDataAccount);

        InitializeGameAccounts account = new InitializeGameAccounts();
        account.Game = gameDataAccount;
        account.User = gameState.web3.WalletBase.Account;
        account.SystemProgram = (PublicKey)"11111111111111111111111111111111";

        TransactionInstruction initializeGameInstruction = InitializeGame(account, name, ProgramId);

        //var walletHolderService = ServiceFactory.Resolve<WalletHolderService>();
        //var result = await walletHolderService.BaseWallet.ActiveRpcClient.GetLatestBlockHashAsync(Commitment.Confirmed);

        Transaction transaction = new Transaction();
        transaction.FeePayer = account.User;
        //transaction.RecentBlockHash = result.Result.Value.Blockhash;
        Debug.Log("0");
        transaction.RecentBlockHash = await gameState.web3.WalletBase.GetBlockHash();

        Debug.Log("1");
        transaction.Signatures = new List<SignaturePubKeyPair>();
        transaction.Instructions = new List<TransactionInstruction>();
        transaction.Instructions.Add(initializeGameInstruction);

        //Debug.Log(transaction.);

        Debug.Log("2");

        //gameState.web3.LoginWithWalletAdapter();
        Transaction signedTransaction = await gameState.web3.WalletBase.SignTransaction(transaction);

        Debug.Log(signedTransaction);

        RequestResult<string> signature = await gameState.web3.WalletBase.ActiveRpcClient.SendTransactionAsync(
            Convert.ToBase64String(signedTransaction.Serialize()),
            true, Commitment.Confirmed);

        Debug.Log("4");

        Debug.Log(ProgramId + " " + gameDataAccount);
        Debug.Log(account.Game + " " + account.User + " " + gameState.web3.WalletBase.Account);
    }

    public async void spawnFish()
    {
        //PublicKey.TryFindProgramAddress(new[]
        //{
        //    Encoding.UTF8.GetBytes("level1")
        //},
        //ProgramId, out gameDataAccount, out var bump);

        SpawnFishAccounts account = new SpawnFishAccounts();
        account.Game = gameDataAccount;

        TransactionInstruction initializeGameInstruction = SpawnFish(account, 0, ProgramId);

        //var walletHolderService = ServiceFactory.Resolve<WalletHolderService>();
        //var result = await walletHolderService.BaseWallet.ActiveRpcClient.GetLatestBlockHashAsync(Commitment.Confirmed);

        Transaction transaction = new Transaction();
        transaction.FeePayer = gameState.web3.WalletBase.Account;
        //transaction.RecentBlockHash = result.Result.Value.Blockhash;
        transaction.RecentBlockHash = await gameState.web3.WalletBase.GetBlockHash();
        transaction.Signatures = new List<SignaturePubKeyPair>();
        transaction.Instructions = new List<TransactionInstruction>();
        transaction.Instructions.Add(initializeGameInstruction);

        Transaction signedTransaction = await gameState.web3.WalletBase.SignTransaction(transaction);

        RequestResult<string> signature = await gameState.web3.WalletBase.ActiveRpcClient.SendTransactionAsync(
            Convert.ToBase64String(signedTransaction.Serialize()),
            true, Commitment.Confirmed);
    }

    public async void eatFish()
    {
        //PublicKey.TryFindProgramAddress(new[]
        //{
        //    Encoding.UTF8.GetBytes("level1")
        //},
        //ProgramId, out gameDataAccount, out var bump);

        EatFishAccounts account = new EatFishAccounts();
        account.Game = gameDataAccount;

        TransactionInstruction initializeGameInstruction = EatFish(account, 0, ProgramId);

        //var walletHolderService = ServiceFactory.Resolve<WalletHolderService>();
        //var result = await walletHolderService.BaseWallet.ActiveRpcClient.GetLatestBlockHashAsync(Commitment.Confirmed);

        Transaction transaction = new Transaction();
        transaction.FeePayer = gameState.web3.WalletBase.Account;
        //transaction.RecentBlockHash = result.Result.Value.Blockhash;
        transaction.RecentBlockHash = await gameState.web3.WalletBase.GetBlockHash();
        transaction.Signatures = new List<SignaturePubKeyPair>();
        transaction.Instructions = new List<TransactionInstruction>();
        transaction.Instructions.Add(initializeGameInstruction);

        Transaction signedTransaction = await gameState.web3.WalletBase.SignTransaction(transaction);

        RequestResult<string> signature = await gameState.web3.WalletBase.ActiveRpcClient.SendTransactionAsync(
            Convert.ToBase64String(signedTransaction.Serialize()),
            true, Commitment.Confirmed);
    }
}