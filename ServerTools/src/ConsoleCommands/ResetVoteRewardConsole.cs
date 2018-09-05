﻿using System;
using System.Collections.Generic;
using System.Data;

namespace ServerTools
{
    public class ResetVoteRewardConsole : ConsoleCmdAbstract
    {
        public override string GetDescription()
        {
            return "[ServerTools]-Reset a player's vote reward status so they can receive another reward.";
        }

        public override string GetHelp()
        {
            return "Usage:\n" +
                   "  1. votereward reset <steamId/entityId/playerName>\n" +
                   "1. Reset the vote reward delay status of a player Id\n";
        }

        public override string[] GetCommands()
        {
            return new string[] { "st-VoteReward", "votereward", "vr" };
        }

        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
        {
            try
            {
                if (_params.Count != 2)
                {
                    SdtdConsole.Instance.Output(string.Format("Wrong number of arguments, expected 2, found {0}.", _params.Count));
                    return;
                }
                if (_params[0].ToLower().Equals("reset"))
                {
                    ClientInfo _cInfo = ConsoleHelper.ParseParamIdOrName(_params[1]);
                    if (_cInfo != null)
                    {
                        string _sql = string.Format("SELECT lastVoteReward FROM Players WHERE steamid = '{0}'", _cInfo.playerId);
                        DataTable _result = SQL.TQuery(_sql);
                        if (_result.Rows.Count != 0)
                        {
                            DateTime _lastVoteReward;
                            DateTime.TryParse(_result.Rows[0].ItemArray.GetValue(0).ToString(), out _lastVoteReward);
                            if (_lastVoteReward.ToString() != "10/29/2000 7:30:00 AM")
                            {
                                _sql = string.Format("UPDATE Players SET lastVoteReward = '10/29/2000 7:30:00 AM' WHERE steamid = '{0}'", _cInfo.playerId);
                                SQL.FastQuery(_sql);
                                SdtdConsole.Instance.Output("Vote reward delay reset.");
                            }
                            else
                            {
                                SdtdConsole.Instance.Output(string.Format("Player with id {0} does not have a Vote reward delay to reset.", _params[1]));
                            }
                        }
                        _result.Dispose();
                    }
                    else
                    {
                        if (_params[1].Length != 17)
                        {
                            SdtdConsole.Instance.Output(string.Format("Can not reset Id: Invalid Id {0}", _params[1]));
                            return;
                        }
                        string _id = SQL.EscapeString(_params[1]);
                        string _sql = string.Format("SELECT lastVoteReward FROM Players WHERE steamid = '{0}'", _id);
                        DataTable _result = SQL.TQuery(_sql);
                        if (_result.Rows.Count != 0)
                        {
                            DateTime _lastVoteReward;
                            DateTime.TryParse(_result.Rows[0].ItemArray.GetValue(0).ToString(), out _lastVoteReward);
                            if (_lastVoteReward.ToString() != "10/29/2000 7:30:00 AM")
                            {
                                _sql = string.Format("UPDATE Players SET lastVoteReward = '10/29/2000 7:30:00 AM' WHERE steamid = '{0}'", _id);
                                SQL.FastQuery(_sql);
                                SdtdConsole.Instance.Output("Vote reward delay reset.");
                            }
                            else
                            {
                                SdtdConsole.Instance.Output(string.Format("Player with id {0} does not have a Vote reward delay to reset.", _params[1]));
                            }
                        }
                        else
                        {
                            SdtdConsole.Instance.Output(string.Format("Player with id {0} does not have a Vote reward delay to reset.", _params[1]));
                        }
                        _result.Dispose();
                    }
                }
                else
                {
                    SdtdConsole.Instance.Output(string.Format("Invalid argument {0}.", _params[0]));
                }
            }
            catch (Exception e)
            {
                Log.Out(string.Format("[SERVERTOOLS] Error in ResetVoteRewardConsole.Run: {0}.", e));
            }
        }
    }
}