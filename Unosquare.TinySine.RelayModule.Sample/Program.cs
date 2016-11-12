﻿using System;
using System.Collections.Generic;

namespace Unosquare.TinySine.RelayModule.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var relayBoard = new RelayController())
            {
                relayBoard.Open("COM4", RelayController.DefaultPassword);

                while (true)
                {
                    var selectedOption = Log.ReadPrompt("Select an option", ActionOptions, "Exit this program");
                    if (selectedOption.Key == ConsoleKey.Q)
                    {
                        Log.Info($"Board: Model: {relayBoard.BoardModel}, Version: {relayBoard.BoardVersion}, FW: {relayBoard.FirmwareVersion}, Channels: {relayBoard.RelayChannelCount}, Mode: {relayBoard.RelayOperatingMode}");
                    }
                    else if (selectedOption.Key == ConsoleKey.W)
                    {
                        DumpRelaysState(relayBoard);
                    }
                    else if (selectedOption.Key == ConsoleKey.E)
                    {
                        var newOpModeKey = Log.ReadKey("Enter Operating Mode (L for Latching, anything else, Momentary): ", false);
                        if (newOpModeKey.Key == ConsoleKey.L)
                        {
                            relayBoard.RelayOperatingMode = RelayOperatingMode.Latching;
                        }
                        else
                        {
                            relayBoard.RelayOperatingMode = RelayOperatingMode.Momentary;
                        }

                        Log.Info($"Relay Operating Mode is now: {relayBoard.RelayOperatingMode}");
                    }
                    else if (selectedOption.Key == ConsoleKey.R)
                    {
                        Log.Info($"Relay Board Working Voltage: {relayBoard.WorkingVoltage}, Temperature: {relayBoard.Temperature}");
                    }
                    else if (selectedOption.Key == ConsoleKey.A)
                    {
                        relayBoard.SetRelayStateAll(true);
                    }
                    else if (selectedOption.Key == ConsoleKey.S)
                    {
                        relayBoard.SetRelayStateAll(false);
                    }
                    else if (selectedOption.Key == ConsoleKey.D)
                    {
                        var relayNumber = (RelayNumber)Log.ReadNumber("Enter a relay number from 1 to 8", 1);
                        if ((int)relayNumber >= 1 && (int)relayNumber <= 8)
                        {
                            relayBoard[relayNumber] = true;
                        }
                        else
                        {
                            Log.Error("Bad relay number. Argument must be between 1 and 8");
                        }
                    }
                    else if (selectedOption.Key == ConsoleKey.F)
                    {
                        var relayNumber = (RelayNumber)Log.ReadNumber("Enter a relay number from 1 to 8", 1);
                        if ((int)relayNumber >= 1 && (int)relayNumber <= 8)
                        {
                            relayBoard[relayNumber] = false;
                        }
                        else
                        {
                            Log.Error("Bad relay number. Argument must be between 1 and 8");
                        }
                    }
                    else
                    {
                        break;
                    }
                }

            }

            Log.ReadKey("Press any key to continue . . .", true);
        }

        static private void DumpRelaysState(RelayController relayBoard)
        {
            //return;
            var states = relayBoard.GetRelaysStateDictionary();

            Log.Info("STATUS DUMP");
            Log.Info($"RELAY:\t8\t7\t6\t5\t4\t3\t2\t1");
            Log.Info($"STATE:\t" + 
                $"{(states[RelayNumber.Relay08] ? 1 : 0)}\t" +
                $"{(states[RelayNumber.Relay07] ? 1 : 0)}\t" +
                $"{(states[RelayNumber.Relay06] ? 1 : 0)}\t" +
                $"{(states[RelayNumber.Relay05] ? 1 : 0)}\t" +
                $"{(states[RelayNumber.Relay04] ? 1 : 0)}\t" +
                $"{(states[RelayNumber.Relay03] ? 1 : 0)}\t" +
                $"{(states[RelayNumber.Relay02] ? 1 : 0)}\t" +
                $"{(states[RelayNumber.Relay01] ? 1 : 0)}\t");
        }

        #region Action Options

        static private readonly Dictionary<ConsoleKey, string> ActionOptions = new Dictionary<ConsoleKey, string>
        {
            // Module COntrol Items
            { ConsoleKey.Q, "MODULE   - Show Board Info" },
            { ConsoleKey.W, "MODULE   - Show Relays States" },
            { ConsoleKey.E, "MODULE   - Change Relay Operating Mode" },
            { ConsoleKey.R, "MODULE   - Show Voltage and Temperature" },

            { ConsoleKey.A, "RELAYS   - Set All Relays to High" },
            { ConsoleKey.S, "RELAYS   - Set All Relays to Low" },
            { ConsoleKey.D, "RELAYS   - Set the State of a Single Relay to High" },
            { ConsoleKey.F, "RELAYS   - Set the State of a Single Relay to Low" },
        };

        #endregion
    }
}
