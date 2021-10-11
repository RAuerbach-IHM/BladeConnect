using Microsoft.EntityFrameworkCore;
using SwitchBladeInterface.API.DBContext;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories
{
    public class ChannelInfoRepository : IChannelInfoRepository
    {
        private readonly SwitchBladeInterfaceContext _context;
        public ChannelInfoRepository(SwitchBladeInterfaceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ChannelInfo> GetChannelInfo(long deviceID, int channelNumber)
        {
            try
            {
                return await _context.ChannelInfo
                            .FirstOrDefaultAsync(c => c.Device_ID == deviceID && c.Channel_Number == channelNumber);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving ChannelInfo - " + ex);
                return new ChannelInfo();
            }
        }

        public async Task<List<ChannelInfo>> GetChannelInfo()
        {
            try
            {
                return await _context.ChannelInfo.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving ChannelInfo - " + ex);
                return new List<ChannelInfo>();
            }
        }

        public async Task<List<ChannelInfo>> GetChannelInfoByDevice(long deviceID)
        {
            try
            {
                return await _context.ChannelInfo.Where(c => c.Device_ID == deviceID).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving ChannelInfo - " + ex);
                return new List<ChannelInfo>();
            }
        }

        public async Task<List<ChannelInfo>> GetChannelInfoByDevices(long[] deviceIDs)
        {
            try
            {
                return await _context.ChannelInfo.Where(c => deviceIDs.Contains(c.Device_ID)).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Retrieving ChannelInfo - " + ex);
                return new List<ChannelInfo>();
            }
        }


        public async Task<bool> SaveChannelInfoStatus(long deviceID, int channel, int status)
        { 
           
            try
            {
                var result = await _context.ChannelInfo.FirstOrDefaultAsync(i => i.Device_ID == deviceID && i.Channel_Number == channel);
                if (result != null)  //Update
                {
                    try
                    {
                        //result.Device_ID = deviceID;
                        //result.Channel_Number = channel;
                        result.Status = status;
                        
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating channel info. " + ex);
                        return false;
                    }
                }
                else  //Insert
                {
                    try
                    {
                        ChannelInfo channelInfo = new ChannelInfo();
                        channelInfo.Device_ID = deviceID;
                        channelInfo.Channel_Number = channel;
                        channelInfo.Status = status;
                        await _context.AddAsync(channelInfo);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving channel info. " + ex);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from channel info. " + ex);
                return false;
            }
            return true;

        }

        public async Task<bool> SaveChannelInfoName(long deviceID, int channel, string name)
        {
            if (name == null)
            {
                return false;
            }
            try
            {
                var result = await _context.ChannelInfo.FirstOrDefaultAsync(i => i.Device_ID == deviceID && i.Channel_Number == channel);
                if (result != null)  //Update
                {
                    try
                    {
                        //result.Device_ID = deviceID;
                        //result.Channel_Number = channel;
                        result.Name = name;

                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating channel info. " + ex);
                        return false;
                    }
                }
                else  //Insert
                {
                    try
                    {
                        ChannelInfo channelInfo = new ChannelInfo();
                        channelInfo.Device_ID = deviceID;
                        channelInfo.Channel_Number = channel;
                        channelInfo.Name = name;
                        await _context.AddAsync(channelInfo);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving channel info. " + ex);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from channel info. " + ex);
                return false;
            }
            return true;

        }

        public async Task<bool> SaveChannelInfoDetails (long deviceID, int channel, string[] details)
        {
            if (details == null)
            {
                return false;
            }
            try
            {
                var result = await _context.ChannelInfo.FirstOrDefaultAsync(i => i.Device_ID == deviceID && i.Channel_Number == channel);
                if (result != null)  //Update
                {
                    try
                    {
                        foreach (string detail in details)
                        {
                            string[] input = detail.Split(':');
                            if (input.Count() < 2)
                                continue;

                            if (string.IsNullOrEmpty(input[1]) || string.IsNullOrEmpty(input[1]))
                            {
                                continue;
                            }

                            switch (input[0])
                            {
                                case "CallFrom":
                                    result.Call_From = input[1].TrimEnd();
                                    break;
                                case "CallTo":
                                    result.Call_To = input[1].TrimEnd();
                                    break;
                                case "CallerName":
                                    result.Caller_Name = input[1].TrimEnd();
                                    break;
                                case "Originating":
                                    result.Originating = input[1].TrimEnd();
                                    break;
                                case "Receiving":
                                    result.Receiving = input[1].TrimEnd();
                                    break;
                                case "Codec":
                                    result.Codec = input[1].Trim('>').TrimEnd();
                                    break;
                                default:
                                    break;
                            }
                        }

                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating channel info. " + ex);
                        return false;
                    }
                }
                else  //Insert
                {
                    try
                    {
                        ChannelInfo channelInfo = new ChannelInfo();
                        channelInfo.Device_ID = deviceID;

                        foreach (string detail in details)
                        {
                            string[] input = detail.Split(':');
                            if (input.Count() < 2)
                                continue;

                            if (string.IsNullOrEmpty(input[1]) || string.IsNullOrEmpty(input[1]))
                            {
                                continue;
                            }

                            switch (input[0])
                            {
                                case "CallFrom":
                                    channelInfo.Call_From = input[1].TrimEnd();
                                    break;
                                case "CallTo":
                                    channelInfo.Call_To = input[1].TrimEnd();
                                    break;
                                case "CallerName":
                                    channelInfo.Caller_Name = input[1].TrimEnd();
                                    break;
                                case "Originating":
                                    channelInfo.Originating = input[1].TrimEnd();
                                    break;
                                case "Receiving":
                                    channelInfo.Receiving = input[1].TrimEnd();
                                    break;
                                case "Codec":
                                    channelInfo.Codec = input[1].Trim('>').TrimEnd();
                                    break;
                                default:
                                    break;
                            }
                        }


                        await _context.AddAsync(channelInfo);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving channel info. " + ex);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from channel info. " + ex);
                return false;
            }
            return true;
        }
        /*
        public bool InsterOrUpdateIntoChannelInfo(List<ChannelInfo> channelInfo, string parameter)
        {
            bool combinedResult = true;
            bool result;

            try
            {
                Connect();
                if (channelInfo == null)
                    return false;

                foreach (ChannelInfo info in channelInfo)
                {
                    if (GetNumberOfRows(info) > 0)
                    {
                        result = UpdateIntoChannelInfo(info, parameter); //Update
                    }

                    else
                    {
                        result = InsertIntoChannelInfo(info, parameter); //Insert
                    }

                    if (result == false)
                        combinedResult = false;
                }

                Disconnect();
            }
            catch (Exception)
            {
                return combinedResult;
            }
            return combinedResult;
        }
        */
        /*
        public bool DeleteFromChannelInfo(ChannelInfo channelInfo)
        {
            SQLiteCommand command = new SQLiteCommand();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            String sql;
            bool result;
            try
            {
                sql = "Delete FROM ChannelInfo where device_id = '" + channelInfo.DeviceID + "' and channel_number = " + channelInfo.ChannelNumber;
                command = new SQLiteCommand(sql, cnn);

                adapter.DeleteCommand = command;
                adapter.DeleteCommand.ExecuteNonQuery();
                result = true;
            }

            catch (Exception)
            {
                result = false;
            }
            finally
            {
                if (adapter != null)
                    adapter.Dispose();

                if (command != null)
                    command.Dispose();
            }
            return result;
        }
        */
        /*
        private int GetNumberOfRows(ChannelInfo channelInfo)
        {
            int numberOfRowsReturned = 0;
            SQLiteCommand command = new SQLiteCommand();

            try
            {
                String sql = "";

                sql = "SELECT COUNT(device_id) FROM ChannelInfo WHERE device_id = " + channelInfo.DeviceID + " and channel_number = " + channelInfo.ChannelNumber;
                command = new SQLiteCommand(sql, cnn);

                numberOfRowsReturned = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception)
            {
                numberOfRowsReturned = -1;
            }
            finally
            {
                if (command != null)
                    command.Dispose();
            }
            return numberOfRowsReturned;
        }
        */

        /*
        private bool InsertIntoChannelInfo(ChannelInfo channelInfo, string parameter)
        {
            SQLiteCommand command = new SQLiteCommand();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            String sql = "";
            bool result;
            try
            {
                if (parameter == "Status")
                    sql = "Insert into ChannelInfo (channel_number, device_id, status) values (" + channelInfo.ChannelNumber + ", " + channelInfo.DeviceID + ", " + channelInfo.Status + ");";
                else if (parameter == "UserName")
                    sql = "Insert into ChannelInfo (name, channel_number, device_id) values ('" + channelInfo.Name + "', " + channelInfo.ChannelNumber + ", " + channelInfo.DeviceID + ");";
                else if (parameter == "SIPCallDetails")
                    sql = "Insert into ChannelInfo (channel_number, device_id, call_from, call_to, caller_name, originating, receiving, codec) values (" + channelInfo.ChannelNumber + ", " + channelInfo.DeviceID + ", '" + channelInfo.CallFrom + "', '" + channelInfo.CallTo + "', '" + channelInfo.CallerName + "', '" + channelInfo.Originating + "', '" + channelInfo.Receiving + "', '" + channelInfo.Codec + "');";

                command = new SQLiteCommand(sql, cnn);

                adapter.InsertCommand = command;
                adapter.InsertCommand.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                if (adapter != null)
                    adapter.Dispose();

                if (command != null)
                    command.Dispose();
            }

            return result;
        }
        */

        /*
        private bool UpdateIntoChannelInfo(ChannelInfo channelInfo, string parameter)
        {
            using (SQLiteCommand command = new SQLiteCommand())
            {
                command.Connection = cnn;
                //command.CommandType = CommandType.Text;

                if (parameter == "Status")
                    command.CommandText = @"Update ChannelInfo SET device_id = @deviceID, channel_number = @channelNumber, status = @Status
                    where device_id = @deviceID and channel_number = @channelNumber;";
                else if (parameter == "UserName")
                    command.CommandText = @"Update ChannelInfo SET device_id = @deviceID, name = @name, channel_number = @channelNumber where device_id = @deviceID and channel_number = @channelNumber;";
                else if (parameter == "SIPCallDetails")
                    command.CommandText = @"Update ChannelInfo SET device_id = @deviceID, channel_number = @channelNumber, call_from = @callFrom, call_to = @callTo, caller_name = @callerName, originating = @originating, receiving = @receiving, codec = @codec where device_id = @deviceID and channel_number = @channelNumber;";


                command.Parameters.AddWithValue("@deviceID", channelInfo.DeviceID);
                command.Parameters.AddWithValue("@name", channelInfo.Name);
                command.Parameters.AddWithValue("@channelNumber", channelInfo.ChannelNumber);
                command.Parameters.AddWithValue("@status", channelInfo.Status);

                command.Parameters.AddWithValue("@callFrom", channelInfo.CallFrom);
                command.Parameters.AddWithValue("@callTo", channelInfo.CallTo);
                command.Parameters.AddWithValue("@callerName", channelInfo.CallerName);
                command.Parameters.AddWithValue("@originating", channelInfo.Originating);
                command.Parameters.AddWithValue("@receiving", channelInfo.Receiving);
                command.Parameters.AddWithValue("@codec", channelInfo.Codec);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error Saving Channel Info");
                    return false;
                }

                return true;
            }
        }
        */

    }
}