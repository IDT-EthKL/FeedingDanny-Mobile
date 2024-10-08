using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Solana.Unity;
using Solana.Unity.Programs.Abstract;
using Solana.Unity.Programs.Utilities;
using Solana.Unity.Rpc;
using Solana.Unity.Rpc.Builders;
using Solana.Unity.Rpc.Core.Http;
using Solana.Unity.Rpc.Core.Sockets;
using Solana.Unity.Rpc.Types;
using Solana.Unity.Wallet;
using Feedingdanny;
using Feedingdanny.Program;
using Feedingdanny.Errors;
using Feedingdanny.Accounts;
using Feedingdanny.Types;

namespace Feedingdanny
{
    namespace Accounts
    {
        public partial class Game
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 1331205435963103771UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[] { 27, 90, 166, 125, 74, 100, 121, 18 };
            public static string ACCOUNT_DISCRIMINATOR_B58 => "5aNQXizG8jB";
            public Player Player { get; set; }

            public Fish[] Fish { get; set; }

            public LeaderboardEntry[] Leaderboard { get; set; }

            public static Game Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                Game result = new Game();
                offset += Player.Deserialize(_data, offset, out var resultPlayer);
                result.Player = resultPlayer;
                int resultFishLength = (int)_data.GetU32(offset);
                offset += 4;
                result.Fish = new Fish[resultFishLength];
                for (uint resultFishIdx = 0; resultFishIdx < resultFishLength; resultFishIdx++)
                {
                    offset += Types.Fish.Deserialize(_data, offset, out var resultFishresultFishIdx);
                    result.Fish[resultFishIdx] = resultFishresultFishIdx;
                }

                int resultLeaderboardLength = (int)_data.GetU32(offset);
                offset += 4;
                result.Leaderboard = new LeaderboardEntry[resultLeaderboardLength];
                for (uint resultLeaderboardIdx = 0; resultLeaderboardIdx < resultLeaderboardLength; resultLeaderboardIdx++)
                {
                    offset += LeaderboardEntry.Deserialize(_data, offset, out var resultLeaderboardresultLeaderboardIdx);
                    result.Leaderboard[resultLeaderboardIdx] = resultLeaderboardresultLeaderboardIdx;
                }

                return result;
            }
        }
    }

    namespace Errors
    {
        public enum FeedingdannyErrorKind : uint
        {
            InvalidIndex = 6000U,
            FishTooLarge = 6001U
        }
    }

    namespace Types
    {
        public partial class Player
        {
            public string Name { get; set; }

            public byte Size { get; set; }

            public uint Score { get; set; }

            public uint Exp { get; set; }

            public byte Level { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                offset += _data.WriteBorshString(Name, offset);
                _data.WriteU8(Size, offset);
                offset += 1;
                _data.WriteU32(Score, offset);
                offset += 4;
                _data.WriteU32(Exp, offset);
                offset += 4;
                _data.WriteU8(Level, offset);
                offset += 1;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out Player result)
            {
                int offset = initialOffset;
                result = new Player();
                offset += _data.GetBorshString(offset, out var resultName);
                result.Name = resultName;
                result.Size = _data.GetU8(offset);
                offset += 1;
                result.Score = _data.GetU32(offset);
                offset += 4;
                result.Exp = _data.GetU32(offset);
                offset += 4;
                result.Level = _data.GetU8(offset);
                offset += 1;
                return offset - initialOffset;
            }
        }

        public partial class Fish
        {
            public byte Size { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU8(Size, offset);
                offset += 1;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out Fish result)
            {
                int offset = initialOffset;
                result = new Fish();
                result.Size = _data.GetU8(offset);
                offset += 1;
                return offset - initialOffset;
            }
        }

        public partial class LeaderboardEntry
        {
            public string Name { get; set; }

            public uint Score { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                offset += _data.WriteBorshString(Name, offset);
                _data.WriteU32(Score, offset);
                offset += 4;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out LeaderboardEntry result)
            {
                int offset = initialOffset;
                result = new LeaderboardEntry();
                offset += _data.GetBorshString(offset, out var resultName);
                result.Name = resultName;
                result.Score = _data.GetU32(offset);
                offset += 4;
                return offset - initialOffset;
            }
        }
    }

    public partial class FeedingdannyClient : TransactionalBaseClient<FeedingdannyErrorKind>
    {
        public FeedingdannyClient(IRpcClient rpcClient, IStreamingRpcClient streamingRpcClient, PublicKey programId) : base(rpcClient, streamingRpcClient, programId)
        {
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Game>>> GetGamesAsync(string programAddress, Commitment commitment = Commitment.Confirmed)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp> { new Solana.Unity.Rpc.Models.MemCmp { Bytes = Game.ACCOUNT_DISCRIMINATOR_B58, Offset = 0 } };
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Game>>(res);
            List<Game> resultingAccounts = new List<Game>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => Game.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Game>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<Game>> GetGameAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<Game>(res);
            var resultingAccount = Game.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<Game>(res, resultingAccount);
        }

        public async Task<SubscriptionState> SubscribeGameAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, Game> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                Game parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = Game.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        protected override Dictionary<uint, ProgramError<FeedingdannyErrorKind>> BuildErrorsDictionary()
        {
            return new Dictionary<uint, ProgramError<FeedingdannyErrorKind>> { { 6000U, new ProgramError<FeedingdannyErrorKind>(FeedingdannyErrorKind.InvalidIndex, "Invalid fish index") }, { 6001U, new ProgramError<FeedingdannyErrorKind>(FeedingdannyErrorKind.FishTooLarge, "Fish is too large to eat") }, };
        }
    }

    namespace Program
    {
        public class InitializeGameAccounts
        {
            public PublicKey Game { get; set; }

            public PublicKey User { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class SpawnFishAccounts
        {
            public PublicKey Game { get; set; }
        }

        public class EatFishAccounts
        {
            public PublicKey Game { get; set; }
        }

        public class GetPlayerStatsAccounts
        {
            public PublicKey Game { get; set; }
        }

        public class GetLeaderboardAccounts
        {
            public PublicKey Game { get; set; }
        }

        public static class FeedingdannyProgram
        {
            public const string ID = "11111111111111111111111111111111";
            public static Solana.Unity.Rpc.Models.TransactionInstruction InitializeGame(InitializeGameAccounts accounts, string playerName, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Game, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.User, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(15529203708862021164UL, offset);
                offset += 8;
                offset += _data.WriteBorshString(playerName, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction { Keys = keys, ProgramId = programId.KeyBytes, Data = resultData };
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction SpawnFish(SpawnFishAccounts accounts, byte size, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Game, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(4859453560636983858UL, offset);
                offset += 8;
                _data.WriteU8(size, offset);
                offset += 1;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction { Keys = keys, ProgramId = programId.KeyBytes, Data = resultData };
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction EatFish(EatFishAccounts accounts, ulong fishIndex, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Game, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(5602882658059744296UL, offset);
                offset += 8;
                _data.WriteU64(fishIndex, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction { Keys = keys, ProgramId = programId.KeyBytes, Data = resultData };
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction GetPlayerStats(GetPlayerStatsAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Game, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(13654177240798009246UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction { Keys = keys, ProgramId = programId.KeyBytes, Data = resultData };
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction GetLeaderboard(GetLeaderboardAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Game, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(2332459865518217080UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction { Keys = keys, ProgramId = programId.KeyBytes, Data = resultData };
            }
        }
    }
}